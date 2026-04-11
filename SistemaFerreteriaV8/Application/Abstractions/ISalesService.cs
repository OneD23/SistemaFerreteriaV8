using SistemaFerreteriaV8.Application.Sales;

namespace SistemaFerreteriaV8.Application.Abstractions;

public interface ISalesService
{
    SalesTotals CalculateTotals(IEnumerable<SaleLineInput> lines, string discountInput, bool discountIsPercentage);
    bool CanCreateSale(IEnumerable<SaleLineInput> lines);
}
