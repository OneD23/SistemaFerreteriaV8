using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.AppCore.Sales;

namespace SistemaFerreteriaV8.Infrastructure.Services;

public static class AppServices
{
    private static readonly Lazy<ISalesService> SalesServiceLazy = new(() => new SalesService());
    private static readonly Lazy<ISalesWorkflowService> SalesWorkflowServiceLazy = new(() => new SalesWorkflowService());
    private static readonly Lazy<ICashRegisterService> CashRegisterServiceLazy = new(() => new CashRegisterService());

    public static ISalesService Sales => SalesServiceLazy.Value;
    public static ISalesWorkflowService SalesWorkflow => SalesWorkflowServiceLazy.Value;
    public static ICashRegisterService CashRegister => CashRegisterServiceLazy.Value;
}
