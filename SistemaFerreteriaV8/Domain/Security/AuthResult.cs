namespace SistemaFerreteriaV8.Domain.Security;

public sealed class AuthResult
{
    public bool IsAuthenticated { get; init; }
    public string Message { get; init; } = string.Empty;
    public string EmployeeId { get; init; } = string.Empty;
    public string EmployeeName { get; init; } = string.Empty;
    public SystemRole Role { get; init; } = SystemRole.Unknown;
    public IReadOnlyCollection<string> UserAllowedPermissions { get; init; } = Array.Empty<string>();
    public IReadOnlyCollection<string> UserDeniedPermissions { get; init; } = Array.Empty<string>();

    public static AuthResult Success(
        string employeeId,
        string employeeName,
        SystemRole role,
        IReadOnlyCollection<string>? userAllowedPermissions = null,
        IReadOnlyCollection<string>? userDeniedPermissions = null) =>
        new()
        {
            IsAuthenticated = true,
            EmployeeId = employeeId,
            EmployeeName = employeeName,
            Role = role,
            UserAllowedPermissions = userAllowedPermissions ?? Array.Empty<string>(),
            UserDeniedPermissions = userDeniedPermissions ?? Array.Empty<string>(),
            Message = "Autenticación exitosa"
        };

    public static AuthResult Fail(string message) =>
        new()
        {
            IsAuthenticated = false,
            Message = message,
            Role = SystemRole.Unknown
        };
}
