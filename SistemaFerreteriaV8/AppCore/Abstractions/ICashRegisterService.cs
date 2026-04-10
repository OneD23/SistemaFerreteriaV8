using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface ICashRegisterService
{
    Task<CashRegisterResult> ValidateOpenStateAsync();
    Task<CashRegisterResult> GetActiveAsync(string? usuario = null);
    Task<CashRegisterResult> OpenAsync(CashRegisterOpenRequest request);
    Task<CashRegisterResult> BuildClosePreviewAsync(CashRegisterClosePreviewRequest request);
    Task<CashRegisterResult> CloseAsync(CashRegisterCloseRequest request);
}

public sealed record CashRegisterOpenRequest(string Turno, double BalanceInicial, string Usuario);

public sealed record CashRegisterClosePreviewRequest(string Usuario, double BalanceFinal, DateTime FechaCierre);

public sealed record CashRegisterCloseRequest(string Usuario, double BalanceFinal, DateTime FechaCierre);

public sealed record CashRegisterResult(
    bool Success,
    string Message,
    Caja? CajaActiva = null,
    CashRegisterErrorType ErrorType = CashRegisterErrorType.None,
    double ExpectedBalance = 0,
    double ReportedBalance = 0,
    double Difference = 0,
    string OperationId = "",
    IReadOnlyCollection<Factura.FacturaResumen>? RelatedInvoices = null);

public enum CashRegisterErrorType
{
    None = 0,
    Validation = 1,
    DuplicateOpen = 2,
    NotFound = 3,
    InconsistentState = 4,
    Persistence = 5,
    Unexpected = 6
}
