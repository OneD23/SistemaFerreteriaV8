using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.Infrastructure.Services;

public sealed class ProductService : IProductService
{
    public async Task<ProductLookupResult> FindByIdAsync(string productId)
    {
        if (string.IsNullOrWhiteSpace(productId))
            return new ProductLookupResult(false, "Id de producto vacío.");

        var product = await Productos.BuscarAsync(productId);
        return product == null
            ? new ProductLookupResult(false, "Producto no encontrado.")
            : new ProductLookupResult(true, "Producto encontrado.", product);
    }

    public async Task<ProductLookupResult> FindByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new ProductLookupResult(false, "Nombre de producto vacío.");

        var product = await Productos.BuscarPorClaveAsync("nombre", name);
        return product == null
            ? new ProductLookupResult(false, "Producto no encontrado.")
            : new ProductLookupResult(true, "Producto encontrado.", product);
    }

    public async Task<StockCheckResult> CheckStockAsync(string productId, double requestedQuantity)
    {
        var lookup = await FindByIdAsync(productId);
        if (!lookup.Found || lookup.Product == null)
            return new StockCheckResult(false, lookup.Message, 0, requestedQuantity);

        if (requestedQuantity <= 0)
            return new StockCheckResult(false, "Cantidad solicitada inválida.", lookup.Product.Cantidad, requestedQuantity);

        if (lookup.Product.Cantidad < requestedQuantity)
            return new StockCheckResult(false, "Stock insuficiente.", lookup.Product.Cantidad, requestedQuantity);

        return new StockCheckResult(true, "Stock disponible.", lookup.Product.Cantidad, requestedQuantity);
    }
}
