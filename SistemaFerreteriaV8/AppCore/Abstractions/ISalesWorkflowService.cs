using SistemaFerreteriaV8.AppCore.Sales;
using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface ISalesWorkflowService
{
    Task<SalesWorkflowResult> ConfirmSaleAsync(SalesWorkflowRequest request);
}

public sealed record SalesWorkflowRequest(
    InvoiceDraft Draft,
    bool ApplyStockMovement,
    string InvoiceType,
    bool IsQuotation = false);

public sealed record SalesWorkflowResult(
    bool Success,
    string Message,
    Factura? PersistedInvoice = null,
    SalesWorkflowErrorType ErrorType = SalesWorkflowErrorType.None,
    string OperationId = "",
    DateTime StartedAtUtc = default,
    DateTime FinishedAtUtc = default);

public enum SalesWorkflowErrorType
{
    None = 0,
    Validation = 1,
    Persistence = 2,
    Stock = 3,
    Compensation = 4,
    Unexpected = 5
}
