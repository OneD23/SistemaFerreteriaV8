namespace SistemaFerreteriaV8.Domain.Sales;

public static class SaleStatus
{
    public const string Pending = "pending";
    public const string Confirmed = "confirmed";
    public const string StockError = "stock_error";
    public const string Failed = "failed";
    public const string Compensated = "compensated";
}
