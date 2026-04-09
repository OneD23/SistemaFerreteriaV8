using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface IProductService
{
    Task<ProductLookupResult> FindByIdAsync(string productId);
    Task<ProductLookupResult> FindByNameAsync(string name);
    Task<StockCheckResult> CheckStockAsync(string productId, double requestedQuantity);
}

public sealed record ProductLookupResult(bool Found, string Message, Productos? Product = null);
public sealed record StockCheckResult(bool Success, string Message, double Available = 0, double Requested = 0);
