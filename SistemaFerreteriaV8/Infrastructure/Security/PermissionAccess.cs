using SistemaFerreteriaV8.Clases;
using SistemaFerreteriaV8.Domain.Security;
using System.Windows.Forms;

namespace SistemaFerreteriaV8.Infrastructure.Security;

public static class PermissionAccess
{
    public static Empleado? GetActiveEmployee()
    {
        var main = WinFormsApp.OpenForms["Form1"] as Form1;
        return main?.EmpleadoActivo;
    }

    public static AuthResult BuildSession(Empleado? empleado)
    {
        if (empleado is null)
            return AuthResult.Fail("Usuario no autenticado.");

        var role = empleado.Puesto?.Trim().ToLowerInvariant() switch
        {
            "administrador" => SystemRole.Administrador,
            "cajera" => SystemRole.Cajera,
            "inventario" => SystemRole.Inventario,
            "contabilidad" => SystemRole.Contabilidad,
            _ => SystemRole.Unknown
        };

        return AuthResult.Success(empleado.Id.ToString(), empleado.Nombre ?? string.Empty, role);
    }

    public static async Task<bool> EnsurePermissionAsync(
        Empleado? empleadoActivo,
        string permission,
        IWin32Window owner,
        string actionLabel)
    {
        var session = BuildSession(empleadoActivo);
        if (SecurityServices.AuthorizationService.HasPermission(session, permission))
            return true;

        string code = SecurityPrompt.PromptPassword(
            $"Se requiere permiso '{permission}' para {actionLabel}.\n\nDigite una clave autorizada.",
            "Permiso requerido");

        if (string.IsNullOrWhiteSpace(code))
            return false;

        var elevated = await SecurityServices.AuthenticationService.AuthenticateAsync(code);
        if (SecurityServices.AuthorizationService.HasPermission(elevated, permission))
            return true;

        MessageBox.Show(owner, "No posee permiso para esta acción.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }
}
