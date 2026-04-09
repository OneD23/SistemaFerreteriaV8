using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface ICashRegisterService
{
    Task<CashRegisterResult> ValidateOpenStateAsync();
    Task<CashRegisterResult> OpenAsync(CashRegisterOpenRequest request);
    Task<CashRegisterResult> CloseAsync(CashRegisterCloseRequest request);
}

public sealed record CashRegisterOpenRequest(string Turno, double BalanceInicial, string Usuario);

public sealed record CashRegisterCloseRequest(string Usuario, double BalanceFinal, DateTime FechaCierre);

public sealed record CashRegisterResult(bool Success, string Message, Caja? CajaActiva = null);
