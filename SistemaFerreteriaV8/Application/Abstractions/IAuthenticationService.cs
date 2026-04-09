using SistemaFerreteriaV8.Domain.Security;

namespace SistemaFerreteriaV8.Application.Abstractions;

public interface IAuthenticationService
{
    Task<AuthResult> AuthenticateAsync(string password);
}
