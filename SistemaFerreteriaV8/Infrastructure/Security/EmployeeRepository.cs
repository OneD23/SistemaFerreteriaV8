using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.Infrastructure.Security;

public sealed class EmployeeRepository : IEmployeeRepository
{
    public async Task<Empleado?> FindByIdAsync(string employeeId)
    {
        return await Empleado.BuscarAsync(employeeId);
    }

    public async Task<Empleado?> FindByPlainPasswordAsync(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        return await Empleado.BuscarPorClaveAsync("contrasena", password);
    }

    public async Task<IReadOnlyList<Empleado>> ListAsync()
    {
        return await Empleado.ListarAsync();
    }

    public async Task UpdateAsync(Empleado employee)
    {
        await employee.EditarAsync();
    }

    public async Task SetUserPermissionOverridesAsync(
        string employeeId,
        IReadOnlyCollection<string> allowPermissions,
        IReadOnlyCollection<string> denyPermissions)
    {
        var employee = await FindByIdAsync(employeeId);
        if (employee == null) return;

        employee.PermisosAllow = allowPermissions
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        employee.PermisosDeny = denyPermissions
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        await UpdateAsync(employee);
    }
}
