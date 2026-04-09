using MongoDB.Driver;
using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.Infrastructure.Services;

public sealed class ProductService : IProductService
{
    private const int MaxAttempts = 2;

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

    public async Task<StockMovementResult> ApplySaleStockAsync(IEnumerable<StockMovementItem> items, string operationId)
    {
        var attempt = 0;
        Exception? lastTransient = null;

        while (attempt < MaxAttempts)
        {
            attempt++;
            var applied = new List<StockMovementAppliedItem>();

            try
            {
                foreach (var item in items)
                {
                    if (item.Quantity <= 0) continue;
                    if (string.Equals(item.ProductName, "Generico", StringComparison.OrdinalIgnoreCase)) continue;

                    var lookup = !string.IsNullOrWhiteSpace(item.ProductId)
                        ? await FindByIdAsync(item.ProductId)
                        : await FindByNameAsync(item.ProductName);

                    if (!lookup.Found || lookup.Product == null)
                        return new StockMovementResult(false, $"No se encontró el producto '{item.ProductName}' para la operación {operationId}.", applied, applied.Count > 0, attempt);

                    var product = lookup.Product;
                    if (product.Cantidad < item.Quantity)
                        return new StockMovementResult(false, $"Stock insuficiente para '{product.Nombre}'. Disponible={product.Cantidad}, requerido={item.Quantity}.", applied, applied.Count > 0, attempt);

                    var originalQty = product.Cantidad;
                    var originalSold = product.Vendido;

                    product.Cantidad -= item.Quantity;
                    product.Vendido += item.Quantity;
                    await product.ActualizarProductosAsync();

                    applied.Add(new StockMovementAppliedItem(product.Id, product.Nombre, item.Quantity, originalQty, originalSold));
                }

                return new StockMovementResult(true, string.Empty, applied, false, attempt);
            }
            catch (MongoException ex)
            {
                lastTransient = ex;
                if (attempt < MaxAttempts)
                    await Task.Delay(100 * attempt);
            }
            catch (TimeoutException ex)
            {
                lastTransient = ex;
                if (attempt < MaxAttempts)
                    await Task.Delay(100 * attempt);
            }
            catch (Exception ex)
            {
                return new StockMovementResult(false, $"Error inesperado al descontar stock ({operationId}): {ex.Message}", applied, applied.Count > 0, attempt);
            }
        }

        return new StockMovementResult(false, $"Falló la actualización de stock por error transitorio ({operationId}): {lastTransient?.Message}", Array.Empty<StockMovementAppliedItem>(), false, attempt);
    }

    public async Task<StockMovementResult> CompensateStockAsync(IEnumerable<StockMovementAppliedItem> appliedItems, string operationId)
    {
        var failed = false;
        var attempted = new List<StockMovementAppliedItem>();

        foreach (var item in appliedItems)
        {
            attempted.Add(item);

            try
            {
                var lookup = !string.IsNullOrWhiteSpace(item.ProductId)
                    ? await FindByIdAsync(item.ProductId)
                    : await FindByNameAsync(item.ProductName);

                if (!lookup.Found || lookup.Product == null)
                {
                    failed = true;
                    continue;
                }

                lookup.Product.Cantidad = item.OriginalQuantity;
                lookup.Product.Vendido = item.OriginalSold;
                await lookup.Product.ActualizarProductosAsync();
            }
            catch
            {
                failed = true;
            }
        }

        return failed
            ? new StockMovementResult(false, $"Compensación parcial/fallida para operación {operationId}.", attempted, true)
            : new StockMovementResult(true, $"Compensación completada para operación {operationId}.", attempted);
    }
}
