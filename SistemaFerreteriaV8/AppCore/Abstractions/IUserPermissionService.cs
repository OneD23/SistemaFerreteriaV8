namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface IUserPermissionService
{
    Task<UserPermissionSnapshot?> GetSnapshotAsync(string employeeId);
    Task SetOverridesAsync(string employeeId, IReadOnlyCollection<string> allowPermissions, IReadOnlyCollection<string> denyPermissions);
}

public sealed record UserPermissionSnapshot(
    string EmployeeId,
    string EmployeeName,
    IReadOnlyCollection<string> RolePermissions,
    IReadOnlyCollection<string> AllowOverrides,
    IReadOnlyCollection<string> DenyOverrides,
    IReadOnlyCollection<string> EffectivePermissions);
