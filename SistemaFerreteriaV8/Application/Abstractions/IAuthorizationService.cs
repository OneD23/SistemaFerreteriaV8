using SistemaFerreteriaV8.Domain.Security;

namespace SistemaFerreteriaV8.Application.Abstractions;

public interface IAuthorizationService
{
    bool IsInRole(AuthResult authResult, SystemRole role);
    bool IsAdmin(AuthResult authResult);
    bool HasPermission(AuthResult authResult, string permission);
    IReadOnlyCollection<string> GetPermissions(SystemRole role);
}
