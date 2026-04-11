using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Domain.Security;

namespace SistemaFerreteriaV8.AppCore.Security;

public sealed class UserPermissionService : IUserPermissionService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IAuthorizationService _authorizationService;

    public UserPermissionService(IEmployeeRepository employeeRepository, IAuthorizationService authorizationService)
    {
        _employeeRepository = employeeRepository;
        _authorizationService = authorizationService;
    }

    public async Task<UserPermissionSnapshot?> GetSnapshotAsync(string employeeId)
    {
        var employee = await _employeeRepository.FindByIdAsync(employeeId);
        if (employee == null) return null;

        var role = employee.Puesto?.Trim().ToLowerInvariant() switch
        {
            "administrador" => SystemRole.Administrador,
            "cajera" => SystemRole.Cajera,
            "inventario" => SystemRole.Inventario,
            "contabilidad" => SystemRole.Contabilidad,
            _ => SystemRole.Unknown
        };

        var rolePermissions = _authorizationService.GetPermissions(role)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var allow = (employee.PermisosAllow ?? new List<string>())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var deny = (employee.PermisosDeny ?? new List<string>())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var effective = rolePermissions
            .Concat(allow)
            .Where(p => !deny.Contains(p, StringComparer.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        return new UserPermissionSnapshot(
            employee.Id.ToString(),
            employee.Nombre,
            rolePermissions,
            allow,
            deny,
            effective);
    }

    public async Task SetOverridesAsync(string employeeId, IReadOnlyCollection<string> allowPermissions, IReadOnlyCollection<string> denyPermissions)
    {
        await _employeeRepository.SetUserPermissionOverridesAsync(employeeId, allowPermissions, denyPermissions);
    }
}
