using SistemaFerreteriaV8.Application.Abstractions;
using SistemaFerreteriaV8.Domain.Security;

namespace SistemaFerreteriaV8.Application.Security;

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

        var employee = await _employeeRepository.FindByPasswordAsync(password);
        if (employee is null)
        {
            return AuthResult.Fail("Credenciales inválidas.");
        }

        if (!_passwordHasher.IsHash(employee.Contrasena))
        {
            return AuthResult.Success(employee.Id.ToString(), employee.Nombre, ResolveRole(employee.Puesto));
        }

        var isValid = _passwordHasher.Verify(password, employee.Contrasena);
        if (!isValid)
        {
            return AuthResult.Fail("Credenciales inválidas.");
        }

        return AuthResult.Success(employee.Id.ToString(), employee.Nombre, ResolveRole(employee.Puesto));
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
