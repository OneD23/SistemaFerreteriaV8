using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Domain.Security;

namespace SistemaFerreteriaV8.AppCore.Security;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthenticationService(IEmployeeRepository employeeRepository, IPasswordHasher passwordHasher)
    {
        _employeeRepository = employeeRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResult> AuthenticateAsync(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return AuthResult.Fail("Debe ingresar una clave.");
        }

        var employee = await _employeeRepository.FindByPlainPasswordAsync(password);
        if (employee is not null)
        {
            if (!_passwordHasher.IsHash(employee.Contrasena))
            {
                employee.Contrasena = _passwordHasher.Hash(password);
                await _employeeRepository.UpdateAsync(employee);
            }

            return AuthResult.Success(employee.Id.ToString(), employee.Nombre, ResolveRole(employee.Puesto));
        }

        var allEmployees = await _employeeRepository.ListAsync();
        var hashedEmployee = allEmployees.FirstOrDefault(e =>
            !string.IsNullOrWhiteSpace(e.Contrasena) &&
            _passwordHasher.IsHash(e.Contrasena) &&
            _passwordHasher.Verify(password, e.Contrasena));

        if (hashedEmployee is null)
        {
            return AuthResult.Fail("Credenciales inválidas.");
        }

        return AuthResult.Success(hashedEmployee.Id.ToString(), hashedEmployee.Nombre, ResolveRole(hashedEmployee.Puesto));
    }

    private static SystemRole ResolveRole(string? puesto)
    {
        return puesto?.Trim().ToLowerInvariant() switch
        {
            "administrador" => SystemRole.Administrador,
            "cajera" => SystemRole.Cajera,
            "inventario" => SystemRole.Inventario,
            "contabilidad" => SystemRole.Contabilidad,
            _ => SystemRole.Unknown
        };
    }
}
