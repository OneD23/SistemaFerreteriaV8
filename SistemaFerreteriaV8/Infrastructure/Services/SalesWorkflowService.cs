using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.AppCore.Sales;
using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.Infrastructure.Services;

public sealed class SalesWorkflowService : ISalesWorkflowService
{
    public async Task<SalesWorkflowResult> ConfirmSaleAsync(SalesWorkflowRequest request)
    {
        var operationId = Guid.NewGuid().ToString("N");
        var startedAt = DateTime.UtcNow;

        try
        {
            var mapResult = await BuildPersistableListProductsAsync(request.Draft.Lines, request.ApplyStockMovement);
            if (!mapResult.Success)
            {
                await AppServices.Audit.WriteAsync(
                    "venta_confirmacion",
                    "ventas",
                    "validation_error",
                    mapResult.Message,
                    request.Draft.EmployeeId,
                    request.Draft.CustomerName,
                    new { operationId, request.Draft.InvoiceId });

                return new SalesWorkflowResult(
                    false,
                    mapResult.Message,
                    null,
                    SalesWorkflowErrorType.Validation,
                    operationId,
                    startedAt,
                    DateTime.UtcNow);
            }

            var listProducts = mapResult.Products;

            var invoice = new Factura
            {
                Id = request.Draft.InvoiceId,
                NombreCliente = request.Draft.CustomerName,
                NombreEmpresa = request.Draft.CompanyId,
                RNC = request.Draft.Rnc,
                IdCliente = request.Draft.CustomerId,
                Fecha = request.Draft.CreatedAt,
                IdEmpleado = request.Draft.EmployeeId,
                Productos = listProducts,
                Total = request.Draft.Total,
                Descuentos = request.Draft.Discount,
                Description = request.Draft.Description,
                Direccion = request.Draft.Address,
                Paga = request.Draft.Paid,
                Enviar = request.Draft.SendByDelivery,
                TipoFactura = request.InvoiceType,
                Cotizacion = request.IsQuotation
            };

            var existing = await Factura.BuscarAsync(invoice.Id);
            if (existing != null)
                await invoice.ActualizarFacturaAsync();
            else
                await invoice.InsertarFacturaAsync();

            if (request.ApplyStockMovement)
            {
                var stockResult = await ApplyStockMovementAsync(listProducts);
                if (!stockResult.Success)
                {
                    invoice.Estado = "stock_error";
                    invoice.Informacion = stockResult.Message;
                    await invoice.ActualizarFacturaAsync();

                    var errorType = stockResult.CompensationFailed
                        ? SalesWorkflowErrorType.Compensation
                        : SalesWorkflowErrorType.Stock;

                    await AppServices.Audit.WriteAsync(
                        "venta_confirmacion",
                        "ventas",
                        "stock_error",
                        stockResult.Message,
                        request.Draft.EmployeeId,
                        request.Draft.CustomerName,
                        new { operationId, request.Draft.InvoiceId, errorType = errorType.ToString() });

                    return new SalesWorkflowResult(
                        false,
                        stockResult.Message,
                        invoice,
                        errorType,
                        operationId,
                        startedAt,
                        DateTime.UtcNow);
                }
            }

            await AppServices.Audit.WriteAsync(
                "venta_confirmacion",
                "ventas",
                "ok",
                "Venta confirmada correctamente.",
                request.Draft.EmployeeId,
                request.Draft.CustomerName,
                new { operationId, request.Draft.InvoiceId, request.Draft.Total });

            return new SalesWorkflowResult(
                true,
                "Venta confirmada correctamente.",
                invoice,
                SalesWorkflowErrorType.None,
                operationId,
                startedAt,
                DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            await AppServices.Audit.WriteAsync(
                "venta_confirmacion",
                "ventas",
                "unexpected_error",
                ex.Message,
                request.Draft.EmployeeId,
                request.Draft.CustomerName,
                new { operationId, request.Draft.InvoiceId });

            return new SalesWorkflowResult(
                false,
                $"Error al confirmar venta: {ex.Message}",
                null,
                SalesWorkflowErrorType.Unexpected,
                operationId,
                startedAt,
                DateTime.UtcNow);
        }
    }

    private static async Task<(bool Success, string Message, List<ListProduct> Products)> BuildPersistableListProductsAsync(
        IReadOnlyCollection<InvoiceDraftLine> lines,
        bool validateStock)
    {
        var result = new List<ListProduct>();

        foreach (var line in lines)
        {
            Productos? product;
            if (line.IsGeneric)
            {
                product = new Productos
                {
                    Nombre = "Generico",
                    Precio = new List<double> { line.UnitPrice, line.UnitPrice, line.UnitPrice, line.UnitPrice }
                };
            }
            else
            {
                product = !string.IsNullOrWhiteSpace(line.ProductId)
                    ? await Productos.BuscarAsync(line.ProductId)
                    : await Productos.BuscarPorClaveAsync("nombre", line.ProductName);

                if (validateStock && (product == null || product.Cantidad < line.Quantity))
                    return (false, $"Stock insuficiente para '{line.ProductName}' al mapear líneas persistibles.", result);
            }

            if (product == null)
                return (false, $"Producto '{line.ProductName}' no encontrado al mapear líneas persistibles.", result);

            result.Add(new ListProduct
            {
                Producto = product,
                Cantidad = line.Quantity,
                Precio = line.UnitPrice
            });
        }

        return (true, string.Empty, result);
    }

    private static async Task<(bool Success, string Message, bool CompensationFailed)> ApplyStockMovementAsync(IEnumerable<ListProduct> items)
    {
        var touched = new List<(Productos Product, double OriginalQty, double OriginalSold)>();
        var compensationFailed = false;

        try
        {
            foreach (var item in items)
            {
                if (item.Producto == null || item.Producto.Nombre == "Generico")
                    continue;

                var product = !string.IsNullOrWhiteSpace(item.Producto.Id)
                    ? await Productos.BuscarAsync(item.Producto.Id)
                    : await Productos.BuscarPorClaveAsync("nombre", item.Producto.Nombre);

                if (product == null)
                    return (false, $"No se encontró el producto '{item.Producto.Nombre}' al descontar stock.", false);

                if (product.Cantidad < item.Cantidad)
                    return (false, $"Stock insuficiente para '{product.Nombre}'. Disponible={product.Cantidad}, requerido={item.Cantidad}.", false);

                touched.Add((product, product.Cantidad, product.Vendido));

                product.Cantidad -= item.Cantidad;
                product.Vendido += item.Cantidad;
                await product.ActualizarProductosAsync();
            }

            return (true, string.Empty, false);
        }
        catch (Exception ex)
        {
            // compensación best-effort
            foreach (var t in touched)
            {
                try
                {
                    t.Product.Cantidad = t.OriginalQty;
                    t.Product.Vendido = t.OriginalSold;
                    await t.Product.ActualizarProductosAsync();
                }
                catch
                {
                    compensationFailed = true;
                }
            }

            return (false, $"Falló la actualización de stock: {ex.Message}", compensationFailed);
        }
    }
}
