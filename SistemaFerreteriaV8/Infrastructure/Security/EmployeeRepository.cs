using SistemaFerreteriaV8.Application.Abstractions;
using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.Infrastructure.Security;

public sealed class EmployeeRepository : IEmployeeRepository
{
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
}
