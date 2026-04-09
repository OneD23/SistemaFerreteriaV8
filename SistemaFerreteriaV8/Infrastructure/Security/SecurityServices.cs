using SistemaFerreteriaV8.Application.Abstractions;
using SistemaFerreteriaV8.Application.Security;

namespace SistemaFerreteriaV8.Infrastructure.Security;

public static class SecurityServices
{
    private static readonly Lazy<IPasswordHasher> PasswordHasherLazy = new(() => new Pbkdf2PasswordHasher());
    private static readonly Lazy<IEmployeeRepository> EmployeeRepositoryLazy = new(() => new EmployeeRepository());
    private static readonly Lazy<IAuthenticationService> AuthenticationServiceLazy =
        new(() => new AuthenticationService(EmployeeRepositoryLazy.Value, PasswordHasherLazy.Value));
    private static readonly Lazy<IAuthorizationService> AuthorizationServiceLazy = new(() => new AuthorizationService());

    public static IPasswordHasher PasswordHasher => PasswordHasherLazy.Value;
    public static IEmployeeRepository EmployeeRepository => EmployeeRepositoryLazy.Value;
    public static IAuthenticationService AuthenticationService => AuthenticationServiceLazy.Value;
    public static IAuthorizationService AuthorizationService => AuthorizationServiceLazy.Value;
}
