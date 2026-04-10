using SistemaFerreteriaV8.Domain.Security;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface IAuthorizationService
{
    bool IsInRole(AuthResult authResult, SystemRole role);
    bool IsAdmin(AuthResult authResult);
    bool HasPermission(AuthResult authResult, string permission);
    IReadOnlyCollection<string> GetPermissions(SystemRole role);
}
