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
    Factura? PersistedInvoice = null);
