using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.AppCore.Sales;
using SistemaFerreteriaV8.Clases;
using SistemaFerreteriaV8.Domain.Sales;

namespace SistemaFerreteriaV8.Infrastructure.Services;

public sealed class SalesWorkflowService : ISalesWorkflowService
{
    public async Task<SalesWorkflowResult> ConfirmSaleAsync(SalesWorkflowRequest request)
    {
        var operationId = Guid.NewGuid().ToString("N");
        var startedAt = DateTime.UtcNow;

        Task WriteAuditAsync(string eventType, string result, string message, object? metadata = null)
            => SafeAuditAsync(eventType, result, message, request, operationId, metadata);

        try
        {
            var mapResult = await BuildPersistableListProductsAsync(request.Draft.Lines, request.ApplyStockMovement, operationId);
            if (!mapResult.Success)
            {
                await WriteAuditAsync(
                    "sales.validation_failed",
                    "validation_error",
                    mapResult.Message,
                    new { request.Draft.InvoiceId });

                return new SalesWorkflowResult(
                    false,
                    mapResult.Message,
                    null,
                    SalesWorkflowErrorType.Validation,
                    operationId,
                    startedAt,
                    DateTime.UtcNow);
            }

            var listProducts = mapResult.Products;
            var stockItems = listProducts
                .Where(p => p.Producto != null && !string.Equals(p.Producto.Nombre, "Generico", StringComparison.OrdinalIgnoreCase))
                .Select(p => new StockMovementItem(p.Producto.Id ?? string.Empty, p.Producto.Nombre ?? string.Empty, p.Cantidad))
                .ToList();

            var invoice = new Factura
            {
                Id = request.Draft.InvoiceId,
                NombreCliente = request.Draft.CustomerName,
                NombreEmpresa = request.Draft.CompanyId,
                RNC = request.Draft.Rnc,
                IdCliente = request.Draft.CustomerId,
                Fecha = request.Draft.CreatedAt,
                IdEmpleado = request.Draft.EmployeeId,
                Productos = listProducts,
                Total = request.Draft.Total,
                Descuentos = request.Draft.Discount,
                Description = request.Draft.Description,
                Direccion = request.Draft.Address,
                Paga = request.Draft.Paid,
                Enviar = request.Draft.SendByDelivery,
                TipoFactura = request.InvoiceType,
                Cotizacion = request.IsQuotation,
                Estado = SaleStatus.Pending,
                Informacion = $"Operación {operationId} iniciada"
            };

            var existing = await Factura.BuscarAsync(invoice.Id);
            if (existing != null)
                await invoice.ActualizarFacturaAsync();
            else
                await invoice.InsertarFacturaAsync();

            await WriteAuditAsync(
                "sales.invoice_persisted",
                "ok",
                "Factura persistida en estado pending.",
                new { request.Draft.InvoiceId, status = invoice.Estado });

            IReadOnlyCollection<StockMovementAppliedItem> appliedItems = Array.Empty<StockMovementAppliedItem>();

            if (request.ApplyStockMovement)
            {
                var stockResult = await AppServices.Product.ApplySaleStockAsync(stockItems, operationId);
                appliedItems = stockResult.AppliedItems ?? Array.Empty<StockMovementAppliedItem>();

                if (!stockResult.Success)
                {
                    var compensationNeeded = stockResult.IsPartial && appliedItems.Count > 0;
                    var compensationResult = compensationNeeded
                        ? await AppServices.Product.CompensateStockAsync(appliedItems, operationId)
                        : new StockMovementResult(true, "Compensación no requerida.", appliedItems);

                    invoice.Estado = compensationNeeded && compensationResult.Success
                        ? SaleStatus.Compensated
                        : SaleStatus.StockError;

                    invoice.Informacion = compensationNeeded
                        ? $"{stockResult.Message} | {compensationResult.Message}"
                        : stockResult.Message;

                    await invoice.ActualizarFacturaAsync();

                    await WriteAuditAsync(
                        "sales.stock_apply_failed",
                        compensationResult.Success ? "compensated" : "stock_error",
                        invoice.Informacion,
                        new
                        {
                            request.Draft.InvoiceId,
                            stockResult.Attempts,
                            stockResult.IsPartial,
                            compensated = compensationResult.Success,
                            status = invoice.Estado
                        });

                    return new SalesWorkflowResult(
                        false,
                        stockResult.Message,
                        invoice,
                        compensationNeeded ? SalesWorkflowErrorType.Compensation : SalesWorkflowErrorType.Stock,
                        operationId,
                        startedAt,
                        DateTime.UtcNow);
                }

                await WriteAuditAsync(
                    "sales.stock_applied",
                    "ok",
                    "Stock descontado correctamente.",
                    new { request.Draft.InvoiceId, stockResult.Attempts, items = appliedItems.Count });
            }

            try
            {
                invoice.Estado = SaleStatus.Confirmed;
                invoice.Informacion = $"Venta confirmada. operationId={operationId}";
                await invoice.ActualizarFacturaAsync();
            }
            catch (Exception exAfterStock)
            {
                var compensationResult = appliedItems.Count > 0
                    ? await AppServices.Product.CompensateStockAsync(appliedItems, operationId)
                    : new StockMovementResult(true, "Compensación no requerida.", Array.Empty<StockMovementAppliedItem>());

                invoice.Estado = compensationResult.Success ? SaleStatus.Compensated : SaleStatus.Failed;
                invoice.Informacion = $"Error al confirmar factura tras descontar stock: {exAfterStock.Message}. {compensationResult.Message}";

                try
                {
                    await invoice.ActualizarFacturaAsync();
                }
                catch
                {
                    // best effort: el rastro se conserva en auditoría
                }

                await WriteAuditAsync(
                    "sales.invoice_finalize_failed",
                    compensationResult.Success ? "compensated" : "failed",
                    invoice.Informacion,
                    new { request.Draft.InvoiceId, status = invoice.Estado });

                return new SalesWorkflowResult(
                    false,
                    invoice.Informacion,
                    invoice,
                    compensationResult.Success ? SalesWorkflowErrorType.Compensation : SalesWorkflowErrorType.Persistence,
                    operationId,
                    startedAt,
                    DateTime.UtcNow);
            }

            await WriteAuditAsync(
                "sales.confirmed",
                "ok",
                "Venta confirmada correctamente.",
                new { request.Draft.InvoiceId, request.Draft.Total, status = invoice.Estado });

            return new SalesWorkflowResult(
                true,
                "Venta confirmada correctamente.",
                invoice,
                SalesWorkflowErrorType.None,
                operationId,
                startedAt,
                DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            await WriteAuditAsync(
                "sales.unexpected_error",
                "unexpected_error",
                ex.Message,
                new { request.Draft.InvoiceId });

            return new SalesWorkflowResult(
                false,
                $"Error al confirmar venta: {ex.Message}",
                null,
                SalesWorkflowErrorType.Unexpected,
                operationId,
                startedAt,
                DateTime.UtcNow);
        }
    }

    private static async Task<(bool Success, string Message, List<ListProduct> Products)> BuildPersistableListProductsAsync(
        IReadOnlyCollection<InvoiceDraftLine> lines,
        bool validateStock,
        string operationId)
    {
        var result = new List<ListProduct>();

        foreach (var line in lines)
        {
            Productos? product;
            if (line.IsGeneric)
            {
                product = new Productos
                {
                    Nombre = "Generico",
                    Precio = new List<double> { line.UnitPrice, line.UnitPrice, line.UnitPrice, line.UnitPrice }
                };
            }
            else
            {
                ProductLookupResult lookup;
                if (!string.IsNullOrWhiteSpace(line.ProductId))
                {
                    lookup = await AppServices.Product.FindByIdAsync(line.ProductId);
                    if (validateStock)
                    {
                        var stock = await AppServices.Product.CheckStockAsync(line.ProductId, line.Quantity);
                        if (!stock.Success)
                            return (false, $"{stock.Message} '{line.ProductName}' (op={operationId}).", result);
                    }
                }
                else
                {
                    lookup = await AppServices.Product.FindByNameAsync(line.ProductName);
                    if (validateStock && lookup.Product != null && lookup.Product.Cantidad < line.Quantity)
                        return (false, $"Stock insuficiente para '{line.ProductName}' al mapear líneas persistibles (op={operationId}).", result);
                }

                if (!lookup.Found || lookup.Product == null)
                    return (false, $"Producto '{line.ProductName}' no encontrado al mapear líneas persistibles (op={operationId}).", result);

                product = lookup.Product;
            }

            result.Add(new ListProduct
            {
                Producto = product,
                Cantidad = line.Quantity,
                Precio = line.UnitPrice
            });
        }

        return (true, string.Empty, result);
    }

    private static async Task SafeAuditAsync(
        string eventType,
        string result,
        string message,
        SalesWorkflowRequest request,
        string operationId,
        object? metadata = null)
    {
        try
        {
            await AppServices.Audit.WriteAsync(
                eventType,
                "ventas",
                result,
                message,
                request.Draft.EmployeeId,
                request.Draft.CustomerName,
                new
                {
                    operationId,
                    request.Draft.InvoiceId,
                    metadata
                });
        }
        catch
        {
            // Nunca dejar caer ConfirmSaleAsync por falla de auditoría.
        }
    }
}
