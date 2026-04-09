using SistemaFerreteriaV8.AppCore.Abstractions;

namespace SistemaFerreteriaV8.AppCore.Sales;

public sealed class SalesService : ISalesService
{
    public SalesTotals CalculateTotals(IEnumerable<SaleLineInput> lines, string discountInput, bool discountIsPercentage)
    {
        var subtotal = lines.Where(l => l.Quantity > 0 && l.UnitPrice >= 0).Sum(l => l.Quantity * l.UnitPrice);

        var discountValue = 0.0;
        if (double.TryParse((discountInput ?? string.Empty).Replace("$", string.Empty).Trim(), out var parsedDiscount))
        {
            discountValue = discountIsPercentage
                ? subtotal * (parsedDiscount / 100.0)
                : parsedDiscount;
        }

        discountValue = Math.Max(0, Math.Min(discountValue, subtotal));
        var total = subtotal - discountValue;

        return new SalesTotals(subtotal, discountValue, total);
    }

    public bool CanCreateSale(IEnumerable<SaleLineInput> lines)
    {
        return lines.Any(l => l.Quantity > 0 && l.UnitPrice > 0);
    }
}

public sealed record SaleLineInput(double Quantity, double UnitPrice);
public sealed record SalesTotals(double Subtotal, double Discount, double Total);
