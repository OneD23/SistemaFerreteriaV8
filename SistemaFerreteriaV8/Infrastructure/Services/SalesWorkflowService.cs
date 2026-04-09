using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.AppCore.Sales;
using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.Infrastructure.Services;

public sealed class SalesWorkflowService : ISalesWorkflowService
{
    public async Task<SalesWorkflowResult> ConfirmSaleAsync(SalesWorkflowRequest request)
    {
        try
        {
            var listProducts = await BuildPersistableListProductsAsync(request.Draft.Lines, request.ApplyStockMovement);
            if (listProducts.Count == 0)
                return new SalesWorkflowResult(false, "No hay líneas válidas para persistir.");

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
                    return new SalesWorkflowResult(false, stockResult.Message, invoice);
                }
            }

            return new SalesWorkflowResult(true, "Venta confirmada correctamente.", invoice);
        }
        catch (Exception ex)
        {
            return new SalesWorkflowResult(false, $"Error al confirmar venta: {ex.Message}");
        }
    }

    private static async Task<List<ListProduct>> BuildPersistableListProductsAsync(
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
                    return new List<ListProduct>();
            }

            if (product == null) continue;

            result.Add(new ListProduct
            {
                Producto = product,
                Cantidad = line.Quantity,
                Precio = line.UnitPrice
            });
        }

        return result;
    }

    private static async Task<(bool Success, string Message)> ApplyStockMovementAsync(IEnumerable<ListProduct> items)
    {
        var touched = new List<(Productos Product, double OriginalQty, double OriginalSold)>();

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
                    return (false, $"No se encontró el producto '{item.Producto.Nombre}' al descontar stock.");

                if (product.Cantidad < item.Cantidad)
                    return (false, $"Stock insuficiente para '{product.Nombre}'. Disponible={product.Cantidad}, requerido={item.Cantidad}.");

                touched.Add((product, product.Cantidad, product.Vendido));

                product.Cantidad -= item.Cantidad;
                product.Vendido += item.Cantidad;
                await product.ActualizarProductosAsync();
            }

            return (true, string.Empty);
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
                    // best-effort
                }
            }

            return (false, $"Falló la actualización de stock: {ex.Message}");
        }
    }
}
