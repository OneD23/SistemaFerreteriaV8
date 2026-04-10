using MongoDB.Driver;
using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.Infrastructure.Services;

public sealed class CashRegisterService : ICashRegisterService
{
    public Task<CashRegisterResult> ValidateOpenStateAsync()
        => GetActiveAsync();

    public async Task<CashRegisterResult> GetActiveAsync(string? usuario = null)
    {
        var operationId = Guid.NewGuid().ToString("N");

        try
        {
            var openBoxes = await Caja.ListarPorClaveAsync("estado", "true");

            if (openBoxes.Count == 0)
                return new CashRegisterResult(false, "No hay caja abierta.", null, CashRegisterErrorType.NotFound, OperationId: operationId);

            if (openBoxes.Count > 1)
            {
                var selected = PickMostRecent(openBoxes, usuario);
                await WriteAuditAsync("cash_register.inconsistent_open", "warning", "Se detectaron múltiples cajas abiertas.", usuario, operationId, new { openBoxes = openBoxes.Count, selected = selected?.Id });
                return new CashRegisterResult(
                    false,
                    "Estado inconsistente: existen múltiples cajas abiertas. Se requiere revisión administrativa.",
                    selected,
                    CashRegisterErrorType.InconsistentState,
                    OperationId: operationId);
            }

            var open = openBoxes[0];
            if (!string.IsNullOrWhiteSpace(usuario) && !string.Equals(open.Usuario, usuario, StringComparison.OrdinalIgnoreCase))
            {
                return new CashRegisterResult(
                    false,
                    $"Hay una caja abierta por '{open.Usuario}'.",
                    open,
                    CashRegisterErrorType.DuplicateOpen,
                    OperationId: operationId);
            }

            return new CashRegisterResult(true, "Caja activa encontrada.", open, OperationId: operationId);
        }
        catch (Exception ex)
        {
            await WriteAuditAsync("cash_register.get_active_error", "error", ex.Message, usuario, operationId);
            return new CashRegisterResult(false, $"Error al consultar caja activa: {ex.Message}", null, CashRegisterErrorType.Unexpected, OperationId: operationId);
        }
    }

    public async Task<CashRegisterResult> OpenAsync(CashRegisterOpenRequest request)
    {
        var operationId = Guid.NewGuid().ToString("N");

        if (string.IsNullOrWhiteSpace(request.Usuario))
            return new CashRegisterResult(false, "Debe indicar un usuario para abrir caja.", null, CashRegisterErrorType.Validation, OperationId: operationId);

        if (string.IsNullOrWhiteSpace(request.Turno))
            return new CashRegisterResult(false, "Debe indicar el turno para abrir caja.", null, CashRegisterErrorType.Validation, OperationId: operationId);

        if (!IsValidAmount(request.BalanceInicial))
            return new CashRegisterResult(false, "El balance inicial es inválido.", null, CashRegisterErrorType.Validation, OperationId: operationId);

        var active = await GetActiveAsync();
        if (active.Success && active.CajaActiva != null)
            return new CashRegisterResult(false, "Ya existe una caja abierta.", active.CajaActiva, CashRegisterErrorType.DuplicateOpen, OperationId: operationId);

        if (active.ErrorType == CashRegisterErrorType.InconsistentState)
            return new CashRegisterResult(false, active.Message, active.CajaActiva, CashRegisterErrorType.InconsistentState, OperationId: operationId);

        try
        {
            var caja = new Caja
            {
                Turno = request.Turno.Trim(),
                Estado = "true",
                FechaApertura = DateTime.Now,
                BalanceInicial = request.BalanceInicial,
                Usuario = request.Usuario.Trim(),
                BalanceFinal = 0
            };

            await caja.CrearAsync();

            await WriteAuditAsync(
                "cash_register.opened",
                "ok",
                "Caja abierta correctamente.",
                request.Usuario,
                operationId,
                new { caja.Turno, caja.BalanceInicial, caja.Id });

            return new CashRegisterResult(true, "Caja abierta correctamente.", caja, OperationId: operationId);
        }
        catch (MongoWriteException ex) when (ex.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            await WriteAuditAsync("cash_register.open_duplicate_key", "warning", ex.Message, request.Usuario, operationId);
            var activeAfterDuplicate = await GetActiveAsync();
            return new CashRegisterResult(
                false,
                "No se pudo abrir la caja porque ya existe una caja abierta (restricción de base de datos).",
                activeAfterDuplicate.CajaActiva,
                CashRegisterErrorType.DuplicateOpen,
                OperationId: operationId);
        }
        catch (Exception ex)
        {
            await WriteAuditAsync("cash_register.open_error", "error", ex.Message, request.Usuario, operationId);
            return new CashRegisterResult(false, $"No se pudo abrir la caja: {ex.Message}", null, CashRegisterErrorType.Persistence, OperationId: operationId);
        }
    }

    public async Task<CashRegisterResult> BuildClosePreviewAsync(CashRegisterClosePreviewRequest request)
    {
        var operationId = Guid.NewGuid().ToString("N");

        if (!IsValidAmount(request.BalanceFinal))
            return new CashRegisterResult(false, "El balance final reportado es inválido.", null, CashRegisterErrorType.Validation, OperationId: operationId);

        var active = await GetActiveAsync(request.Usuario);
        if (!active.Success || active.CajaActiva == null)
            return new CashRegisterResult(false, active.Message, active.CajaActiva, active.ErrorType, OperationId: operationId);

        var caja = active.CajaActiva;

        try
        {
            var (facturas, vendidoTotal) = await Factura.ListarFacturasCierre("nombreEmpresa", caja.Id);
            var expectedBalance = caja.BalanceInicial + vendidoTotal;
            var difference = expectedBalance - request.BalanceFinal;

            return new CashRegisterResult(
                true,
                "Resumen de cierre generado.",
                caja,
                CashRegisterErrorType.None,
                expectedBalance,
                request.BalanceFinal,
                difference,
                operationId,
                facturas);
        }
        catch (Exception ex)
        {
            await WriteAuditAsync("cash_register.close_preview_error", "error", ex.Message, request.Usuario, operationId, new { cajaId = caja.Id });
            return new CashRegisterResult(false, $"No se pudo generar el resumen de cierre: {ex.Message}", caja, CashRegisterErrorType.Unexpected, OperationId: operationId);
        }
    }

    public async Task<CashRegisterResult> CloseAsync(CashRegisterCloseRequest request)
    {
        var operationId = Guid.NewGuid().ToString("N");

        if (!IsValidAmount(request.BalanceFinal))
            return new CashRegisterResult(false, "El balance final es inválido.", null, CashRegisterErrorType.Validation, OperationId: operationId);

        var preview = await BuildClosePreviewAsync(new CashRegisterClosePreviewRequest(request.Usuario, request.BalanceFinal, request.FechaCierre));
        if (!preview.Success || preview.CajaActiva == null)
            return new CashRegisterResult(false, preview.Message, preview.CajaActiva, preview.ErrorType, OperationId: operationId);

        var active = preview.CajaActiva;

        try
        {
            active.Estado = "false";
            active.FechaCierre = request.FechaCierre;
            active.BalanceFinal = request.BalanceFinal;
            active.Usuario = string.IsNullOrWhiteSpace(request.Usuario) ? active.Usuario : request.Usuario.Trim();

            await active.EditarAsync();

            await WriteAuditAsync(
                "cash_register.closed",
                "ok",
                "Caja cerrada correctamente.",
                request.Usuario,
                operationId,
                new
                {
                    active.Id,
                    preview.ExpectedBalance,
                    preview.ReportedBalance,
                    preview.Difference,
                    request.FechaCierre
                });

            return new CashRegisterResult(
                true,
                "Caja cerrada correctamente.",
                active,
                CashRegisterErrorType.None,
                preview.ExpectedBalance,
                preview.ReportedBalance,
                preview.Difference,
                operationId,
                preview.RelatedInvoices);
        }
        catch (Exception ex)
        {
            await WriteAuditAsync("cash_register.close_error", "error", ex.Message, request.Usuario, operationId, new { active.Id });
            return new CashRegisterResult(false, $"No se pudo cerrar la caja: {ex.Message}", active, CashRegisterErrorType.Persistence, OperationId: operationId);
        }
    }

    private static Caja? PickMostRecent(IEnumerable<Caja> boxes, string? usuario)
    {
        if (!string.IsNullOrWhiteSpace(usuario))
        {
            var own = boxes
                .Where(c => string.Equals(c.Usuario, usuario, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(c => c.FechaApertura)
                .FirstOrDefault();

            if (own != null)
                return own;
        }

        return boxes.OrderByDescending(c => c.FechaApertura).FirstOrDefault();
    }

    private static bool IsValidAmount(double amount)
        => !double.IsNaN(amount) && !double.IsInfinity(amount) && amount >= 0;

    private static async Task WriteAuditAsync(string eventType, string result, string message, string? actorName, string operationId, object? metadata = null)
    {
        try
        {
            await AppServices.Audit.WriteAsync(
                eventType,
                "caja",
                result,
                message,
                actorName ?? string.Empty,
                actorName ?? string.Empty,
                new { operationId, metadata });
        }
        catch
        {
            // no-op: la auditoría no debe romper operación de caja
        }
    }
}
