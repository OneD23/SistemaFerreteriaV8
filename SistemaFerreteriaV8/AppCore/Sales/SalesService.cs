using SistemaFerreteriaV8.AppCore.Abstractions;

namespace SistemaFerreteriaV8.AppCore.Sales;

public sealed class SalesService : ISalesService
{
    public SalesTotals CalculateTotals(IEnumerable<SaleLineInput> lines, string discountInput, bool discountIsPercentage, double taxRatePercent = 0)
    {
        var subtotal = lines.Where(l => l.Quantity > 0 && l.UnitPrice >= 0).Sum(l => l.Quantity * l.UnitPrice);

        var discountValue = ParseDiscountValue(discountInput, subtotal, discountIsPercentage);
        var taxableBase = Math.Max(0, subtotal - discountValue);
        var taxAmount = taxRatePercent > 0 ? taxableBase * (taxRatePercent / 100.0) : 0;
        var total = taxableBase + taxAmount;

        return new SalesTotals(subtotal, discountValue, taxAmount, total);
    }

    public SalePreparationResult PrepareSale(SalePreparationRequest request)
    {
        var issues = new List<SaleValidationIssue>();
        var normalizedLines = request.Lines
            .Where(l => l is not null)
            .Select(NormalizeLine)
            .ToList();

        if (!normalizedLines.Any())
        {
            issues.Add(new SaleValidationIssue("ventas.lineas", "La venta no contiene líneas."));
        }

        foreach (var line in normalizedLines)
        {
            if (line.Quantity <= 0)
                issues.Add(new SaleValidationIssue($"linea:{line.ProductName}", "Cantidad inválida."));

            if (line.UnitPrice <= 0)
                issues.Add(new SaleValidationIssue($"linea:{line.ProductName}", "Precio inválido."));

            if (!line.IsGeneric)
            {
                if (!line.ProductFound)
                    issues.Add(new SaleValidationIssue($"linea:{line.ProductName}", "Producto no encontrado para validar stock."));
                else if (line.Quantity > line.AvailableStock)
                    issues.Add(new SaleValidationIssue($"linea:{line.ProductName}", "Stock insuficiente."));
            }
        }

        var totals = CalculateTotals(
            normalizedLines,
            request.DiscountInput,
            request.DiscountIsPercentage,
            request.TaxRatePercent);

        return new SalePreparationResult(
            issues.Count == 0,
            issues,
            normalizedLines,
            totals);
    }

    public InvoiceDraft BuildInvoiceDraft(SalePreparationResult preparation, InvoiceDraftMetadata metadata)
    {
        if (!preparation.IsValid)
        {
            var detail = string.Join("; ", preparation.Issues.Select(i => i.Message));
            throw new InvalidOperationException($"No se puede construir factura persistible: {detail}");
        }

        if (metadata.InvoiceId <= 0)
            throw new InvalidOperationException("InvoiceId inválido para persistencia.");

        var lines = preparation.Lines.Select(line => new InvoiceDraftLine(
            line.ProductId,
            line.ProductName,
            line.Quantity,
            line.UnitPrice,
            line.Quantity * line.UnitPrice,
            line.IsGeneric)).ToList();

        return new InvoiceDraft(
            metadata.InvoiceId,
            metadata.CustomerId,
            metadata.CustomerName,
            metadata.Rnc,
            metadata.EmployeeId,
            metadata.CompanyId,
            metadata.InvoiceType,
            metadata.Description,
            metadata.Address,
            metadata.SendByDelivery,
            metadata.Paid,
            metadata.CreatedAt,
            preparation.Totals.Subtotal,
            preparation.Totals.Discount,
            preparation.Totals.Tax,
            preparation.Totals.Total,
            lines);
    }

    public bool CanCreateSale(IEnumerable<SaleLineInput> lines)
    {
        return lines.Any(l => l.Quantity > 0 && l.UnitPrice > 0);
    }

    private static SaleLineInput NormalizeLine(SaleLineInput line)
    {
        var qty = Math.Max(0, line.Quantity);
        var unitPrice = Math.Max(0, line.UnitPrice);
        var available = line.IsGeneric ? double.MaxValue : Math.Max(0, line.AvailableStock);

        return line with
        {
            ProductName = string.IsNullOrWhiteSpace(line.ProductName) ? "SinNombre" : line.ProductName.Trim(),
            Quantity = qty,
            UnitPrice = unitPrice,
            AvailableStock = available
        };
    }

    private static double ParseDiscountValue(string discountInput, double subtotal, bool discountIsPercentage)
    {
        if (!double.TryParse((discountInput ?? string.Empty).Replace("$", string.Empty).Trim(), out var parsedDiscount))
            return 0;

        var discountValue = discountIsPercentage
            ? subtotal * (parsedDiscount / 100.0)
            : parsedDiscount;

        return Math.Max(0, Math.Min(discountValue, subtotal));
    }
}

public sealed record SaleLineInput(
    string ProductId,
    string ProductName,
    double Quantity,
    double UnitPrice,
    double AvailableStock,
    bool ProductFound,
    bool IsGeneric);

public sealed record SalesTotals(double Subtotal, double Discount, double Tax, double Total);

public sealed record SalePreparationRequest(
    IReadOnlyCollection<SaleLineInput> Lines,
    string DiscountInput,
    bool DiscountIsPercentage,
    double TaxRatePercent = 0);

public sealed record SaleValidationIssue(string Code, string Message);

public sealed record SalePreparationResult(
    bool IsValid,
    IReadOnlyCollection<SaleValidationIssue> Issues,
    IReadOnlyCollection<SaleLineInput> Lines,
    SalesTotals Totals);

public sealed record InvoiceDraft(
    int InvoiceId,
    string CustomerId,
    string CustomerName,
    string Rnc,
    string EmployeeId,
    string CompanyId,
    string InvoiceType,
    string Description,
    string Address,
    bool SendByDelivery,
    bool Paid,
    DateTime CreatedAt,
    double Subtotal,
    double Discount,
    double Tax,
    double Total,
    IReadOnlyCollection<InvoiceDraftLine> Lines);

public sealed record InvoiceDraftLine(
    string ProductId,
    string ProductName,
    double Quantity,
    double UnitPrice,
    double Subtotal,
    bool IsGeneric);

public sealed record InvoiceDraftMetadata(
    int InvoiceId,
    string CustomerId,
    string CustomerName,
    string Rnc,
    string EmployeeId,
    string CompanyId,
    string InvoiceType,
    string Description,
    string Address,
    bool SendByDelivery,
    bool Paid,
    DateTime CreatedAt);
