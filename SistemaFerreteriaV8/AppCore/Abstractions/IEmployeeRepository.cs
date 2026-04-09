using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface IEmployeeRepository
{
    Task<Empleado?> FindByIdAsync(string employeeId);
    Task<Empleado?> FindByPlainPasswordAsync(string password);
    Task<IReadOnlyList<Empleado>> ListAsync();
    Task UpdateAsync(Empleado employee);
    Task SetUserPermissionOverridesAsync(string employeeId, IReadOnlyCollection<string> allowPermissions, IReadOnlyCollection<string> denyPermissions);
}
