using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.Infrastructure.Services;

public sealed class CashRegisterService : ICashRegisterService
{
    public async Task<CashRegisterResult> ValidateOpenStateAsync()
    {
        var active = await Caja.BuscarPorClaveAsync("estado", "true");
        if (active == null)
            return new CashRegisterResult(false, "No hay caja abierta.");

        return new CashRegisterResult(true, "Caja activa encontrada.", active);
    }

    public async Task<CashRegisterResult> OpenAsync(CashRegisterOpenRequest request)
    {
        var active = await Caja.BuscarPorClaveAsync("estado", "true");
        if (active != null)
            return new CashRegisterResult(false, "Ya existe una caja abierta.", active);

        var caja = new Caja
        {
            Turno = request.Turno,
            Estado = "true",
            FechaApertura = DateTime.Now,
            BalanceInicial = request.BalanceInicial,
            Usuario = request.Usuario
        };

        await caja.CrearAsync();
        return new CashRegisterResult(true, "Caja abierta correctamente.", caja);
    }

    public async Task<CashRegisterResult> CloseAsync(CashRegisterCloseRequest request)
    {
        var active = await Caja.BuscarPorClaveAsync("estado", "true");
        if (active == null)
            return new CashRegisterResult(false, "No hay caja activa para cerrar.");

        active.Estado = "false";
        active.FechaCierre = request.FechaCierre;
        active.BalanceFinal = request.BalanceFinal;
        active.Usuario = string.IsNullOrWhiteSpace(request.Usuario) ? active.Usuario : request.Usuario;

        await active.EditarAsync();
        return new CashRegisterResult(true, "Caja cerrada correctamente.", active);
    }
}
