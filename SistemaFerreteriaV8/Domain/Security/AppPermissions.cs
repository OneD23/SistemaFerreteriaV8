namespace SistemaFerreteriaV8.Domain.Security;

public static class AppPermissions
{
    public const string VentasCrear = "ventas.crear";
    public const string VentasDescuento = "ventas.descuento";
    public const string VentasCambiarPrecio = "ventas.cambiar_precio";
    public const string VentasCancelar = "ventas.cancelar";
    public const string CajaAbrir = "caja.abrir";
    public const string CajaCerrar = "caja.cerrar";
    public const string InventarioEditar = "inventario.editar";
    public const string InventarioAjustarStock = "inventario.ajustar_stock";
    public const string ClientesEditar = "clientes.editar";
    public const string ConfiguracionEditar = "configuracion.editar";
    public const string EmpleadosGestionar = "empleados.gestionar";
    public const string ReportesVer = "reportes.ver";

    public static readonly IReadOnlyCollection<string> All = new[]
    {
        VentasCrear,
        VentasDescuento,
        VentasCambiarPrecio,
        VentasCancelar,
        CajaAbrir,
        CajaCerrar,
        InventarioEditar,
        InventarioAjustarStock,
        ClientesEditar,
        ConfiguracionEditar,
        EmpleadosGestionar,
        ReportesVer
    };
}
