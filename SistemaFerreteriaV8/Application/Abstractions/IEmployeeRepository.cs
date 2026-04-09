using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8.Application.Abstractions;

public interface IEmployeeRepository
{
    Task<Empleado?> FindByPasswordAsync(string password);
}
