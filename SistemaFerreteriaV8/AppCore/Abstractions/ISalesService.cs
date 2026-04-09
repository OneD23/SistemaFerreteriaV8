using SistemaFerreteriaV8.AppCore.Sales;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface ISalesService
{
    SalesTotals CalculateTotals(IEnumerable<SaleLineInput> lines, string discountInput, bool discountIsPercentage);
    bool CanCreateSale(IEnumerable<SaleLineInput> lines);
}
