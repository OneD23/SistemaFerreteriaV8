using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Domain.Security;

namespace SistemaFerreteriaV8.AppCore.Security;

public sealed class AuthorizationService : IAuthorizationService
{
    private static readonly IReadOnlyDictionary<SystemRole, IReadOnlyCollection<string>> RolePermissions =
        new Dictionary<SystemRole, IReadOnlyCollection<string>>
        {
            [SystemRole.Administrador] = AppPermissions.All,
            [SystemRole.Cajera] = new[]
            {
                AppPermissions.VentasCrear,
                AppPermissions.VentasDescuento,
                AppPermissions.CajaAbrir,
                AppPermissions.CajaCerrar,
                AppPermissions.ReportesVer
            },
            [SystemRole.Inventario] = new[]
            {
                AppPermissions.InventarioEditar,
                AppPermissions.InventarioAjustarStock,
                AppPermissions.ReportesVer
            },
            [SystemRole.Contabilidad] = new[]
            {
                AppPermissions.ReportesVer,
                AppPermissions.VentasCancelar,
                AppPermissions.CajaCerrar
            },
            [SystemRole.Unknown] = Array.Empty<string>()
        };

    public bool IsInRole(AuthResult authResult, SystemRole role)
    {
        return authResult.IsAuthenticated && authResult.Role == role;
    }

    public bool IsAdmin(AuthResult authResult)
    {
        return IsInRole(authResult, SystemRole.Administrador);
    }

    public bool HasPermission(AuthResult authResult, string permission)
    {
        if (!authResult.IsAuthenticated || string.IsNullOrWhiteSpace(permission))
            return false;

        if (authResult.UserDeniedPermissions.Contains(permission, StringComparer.OrdinalIgnoreCase))
            return false;

        if (authResult.UserAllowedPermissions.Contains(permission, StringComparer.OrdinalIgnoreCase))
            return true;

        var granted = GetPermissions(authResult.Role);
        return granted.Contains(permission, StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlyCollection<string> GetPermissions(SystemRole role)
    {
        if (RolePermissions.TryGetValue(role, out var permissions))
            return permissions;

        return Array.Empty<string>();
    }
}
