using SistemaFerreteriaV8.Domain.Security;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface IAuthenticationService
{
    Task<AuthResult> AuthenticateAsync(string password);
}
