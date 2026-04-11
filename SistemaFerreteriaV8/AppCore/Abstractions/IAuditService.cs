using SistemaFerreteriaV8.Domain.Audit;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface IAuditService
{
    Task WriteAsync(AuditEvent auditEvent);
    Task WriteAsync(string eventType, string module, string result, string message, string actorId = "", string actorName = "", object? metadata = null);
    Task<IReadOnlyCollection<AuditEvent>> QueryAsync(AuditQuery request);
}

public sealed record AuditQuery(
    DateTime? FromUtc = null,
    DateTime? ToUtc = null,
    string Actor = "",
    string Module = "",
    string EventType = "",
    string OperationId = "",
    int Limit = 300);
