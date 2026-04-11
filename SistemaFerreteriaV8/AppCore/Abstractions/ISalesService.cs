using SistemaFerreteriaV8.AppCore.Sales;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface ISalesService
{
    SalesTotals CalculateTotals(IEnumerable<SaleLineInput> lines, string discountInput, bool discountIsPercentage, double taxRatePercent = 0);
    SalePreparationResult PrepareSale(SalePreparationRequest request);
    InvoiceDraft BuildInvoiceDraft(SalePreparationResult preparation, InvoiceDraftMetadata metadata);
    bool CanCreateSale(IEnumerable<SaleLineInput> lines);
}
