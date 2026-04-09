using SistemaFerreteriaV8.Application.Abstractions;
using SistemaFerreteriaV8.Domain.Security;

namespace SistemaFerreteriaV8.Application.Security;

public sealed class AuthorizationService : IAuthorizationService
{
    public bool IsInRole(AuthResult authResult, SystemRole role)
    {
        return authResult.IsAuthenticated && authResult.Role == role;
    }

    public bool IsAdmin(AuthResult authResult)
    {
        return IsInRole(authResult, SystemRole.Administrador);
    }
}
