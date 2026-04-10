using SistemaFerreteriaV8.Domain.Audit;

namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface IAuditService
{
    Task WriteAsync(AuditEvent auditEvent);
    Task WriteAsync(string eventType, string module, string result, string message, string actorId = "", string actorName = "", object? metadata = null);
}
