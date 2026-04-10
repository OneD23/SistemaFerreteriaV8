using MongoDB.Bson;
using MongoDB.Driver;
using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.Infrastructure.Services;

public sealed class ProductService : IProductService
{
    private const int MaxAttempts = 2;

    public Task<ProductLookupResult> FindByCodeAsync(string code)
        => FindByIdAsync(code);

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

    public async Task<ProductSearchResult> SearchByNameAsync(string term, int limit = 25, bool excludeGeneric = true)
    {
        if (string.IsNullOrWhiteSpace(term))
            return new ProductSearchResult(false, "Término de búsqueda vacío.", Array.Empty<Productos>());

        var client = new MongoClient(new OneKeys().URI);
        var db = client.GetDatabase(new OneKeys().DatabaseName);
        var col = db.GetCollection<Productos>("Productos");

        var filter = Builders<Productos>.Filter.Regex("nombre", new BsonRegularExpression(term.Trim(), "i"));
        if (excludeGeneric)
        {
            filter = Builders<Productos>.Filter.And(
                filter,
                Builders<Productos>.Filter.Ne("nombre", "Generico"));
        }

        var results = await col.Find(filter)
            .Limit(Math.Max(1, limit))
            .ToListAsync();

        return new ProductSearchResult(true, "Búsqueda completada.", results);
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

    public async Task<StockSnapshotResult> GetStockSnapshotAsync(string productId)
    {
        var lookup = await FindByIdAsync(productId);
        if (!lookup.Found || lookup.Product == null)
            return new StockSnapshotResult(false, lookup.Message);

        return new StockSnapshotResult(
            true,
            "Stock consultado correctamente.",
            lookup.Product.Id,
            lookup.Product.Nombre,
            lookup.Product.Cantidad,
            lookup.Product.Vendido);
    }

    public async Task<StockAdjustmentResult> AdjustStockAsync(StockAdjustmentRequest request)
    {
        var operationId = string.IsNullOrWhiteSpace(request.OperationId)
            ? Guid.NewGuid().ToString("N")
            : request.OperationId;

        var lookup = !string.IsNullOrWhiteSpace(request.ProductId)
            ? await FindByIdAsync(request.ProductId)
            : await FindByNameAsync(request.ProductName);

        if (!lookup.Found || lookup.Product == null)
            return new StockAdjustmentResult(false, "Producto no encontrado para ajuste de inventario.", null, OperationId: operationId);

        var product = lookup.Product;
        var previousQty = product.Cantidad;
        var nextQty = previousQty + request.QuantityDelta;
        if (nextQty < 0)
            return new StockAdjustmentResult(false, "Ajuste inválido: la cantidad resultante no puede ser negativa.", product, previousQty, previousQty, operationId);

        var nextSold = product.Vendido + request.SoldDelta;
        if (nextSold < 0)
            return new StockAdjustmentResult(false, "Ajuste inválido: el total vendido no puede ser negativo.", product, previousQty, previousQty, operationId);

        product.Cantidad = nextQty;
        product.Vendido = nextSold;
        await product.ActualizarProductosAsync();

        return new StockAdjustmentResult(true, "Ajuste aplicado correctamente.", product, previousQty, nextQty, operationId);
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

                    var adjustment = await AdjustStockAsync(new StockAdjustmentRequest(
                        item.ProductId,
                        item.ProductName,
                        QuantityDelta: -item.Quantity,
                        SoldDelta: item.Quantity,
                        Reason: "sale_confirm",
                        OperationId: operationId));

                    if (!adjustment.Success || adjustment.Product == null)
                        return new StockMovementResult(false, adjustment.Message, applied, applied.Count > 0, attempt);

                    applied.Add(new StockMovementAppliedItem(
                        adjustment.Product.Id,
                        adjustment.Product.Nombre,
                        item.Quantity,
                        adjustment.PreviousQuantity,
                        adjustment.Product.Vendido - item.Quantity));
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
