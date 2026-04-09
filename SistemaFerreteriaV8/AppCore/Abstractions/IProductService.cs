using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface IProductService
{
    Task<ProductLookupResult> FindByIdAsync(string productId);
    Task<ProductLookupResult> FindByNameAsync(string name);
    Task<StockCheckResult> CheckStockAsync(string productId, double requestedQuantity);
    Task<StockMovementResult> ApplySaleStockAsync(IEnumerable<StockMovementItem> items, string operationId);
    Task<StockMovementResult> CompensateStockAsync(IEnumerable<StockMovementAppliedItem> appliedItems, string operationId);
}

public sealed record ProductLookupResult(bool Found, string Message, Productos? Product = null);
public sealed record StockCheckResult(bool Success, string Message, double Available = 0, double Requested = 0);
public sealed record StockMovementItem(string ProductId, string ProductName, double Quantity);
public sealed record StockMovementAppliedItem(string ProductId, string ProductName, double Quantity, double OriginalQuantity, double OriginalSold);
public sealed record StockMovementResult(
    bool Success,
    string Message,
    IReadOnlyCollection<StockMovementAppliedItem>? AppliedItems = null,
    bool IsPartial = false,
    int Attempts = 1);
