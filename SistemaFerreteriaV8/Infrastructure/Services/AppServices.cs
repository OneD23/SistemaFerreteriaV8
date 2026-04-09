using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.AppCore.Sales;

namespace SistemaFerreteriaV8.Infrastructure.Services;

public static class AppServices
{
    private static readonly Lazy<ISalesService> SalesServiceLazy = new(() => new SalesService());

    public static ISalesService Sales => SalesServiceLazy.Value;
}
