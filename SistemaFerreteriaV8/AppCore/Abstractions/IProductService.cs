using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface IProductService
{
    Task<ProductLookupResult> FindByIdAsync(string productId);
    Task<ProductLookupResult> FindByCodeAsync(string code);
    Task<ProductLookupResult> FindByNameAsync(string name);
    Task<ProductSearchResult> SearchByNameAsync(string term, int limit = 25, bool excludeGeneric = true);
    Task<StockCheckResult> CheckStockAsync(string productId, double requestedQuantity);
    Task<StockSnapshotResult> GetStockSnapshotAsync(string productId);
    Task<StockAdjustmentResult> AdjustStockAsync(StockAdjustmentRequest request);
    Task<StockMovementResult> ApplySaleStockAsync(IEnumerable<StockMovementItem> items, string operationId);
    Task<StockMovementResult> CompensateStockAsync(IEnumerable<StockMovementAppliedItem> appliedItems, string operationId);
}

public sealed record ProductLookupResult(bool Found, string Message, Productos? Product = null);
public sealed record ProductSearchResult(bool Success, string Message, IReadOnlyCollection<Productos> Products);
public sealed record StockCheckResult(bool Success, string Message, double Available = 0, double Requested = 0);
public sealed record StockSnapshotResult(bool Success, string Message, string ProductId = "", string ProductName = "", double Quantity = 0, double Sold = 0);
public sealed record StockAdjustmentRequest(string ProductId, string ProductName, double QuantityDelta, double SoldDelta = 0, string Reason = "manual_adjustment", string OperationId = "");
public sealed record StockAdjustmentResult(bool Success, string Message, Productos? Product = null, double PreviousQuantity = 0, double NewQuantity = 0, string OperationId = "");
public sealed record StockMovementItem(string ProductId, string ProductName, double Quantity);
public sealed record StockMovementAppliedItem(string ProductId, string ProductName, double Quantity, double OriginalQuantity, double OriginalSold);
public sealed record StockMovementResult(
    bool Success,
    string Message,
    IReadOnlyCollection<StockMovementAppliedItem>? AppliedItems = null,
    bool IsPartial = false,
    int Attempts = 1);
