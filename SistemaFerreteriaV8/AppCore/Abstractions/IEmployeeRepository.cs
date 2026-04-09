using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface IEmployeeRepository
{
    Task<Empleado?> FindByPlainPasswordAsync(string password);
    Task<IReadOnlyList<Empleado>> ListAsync();
    Task UpdateAsync(Empleado employee);
}
