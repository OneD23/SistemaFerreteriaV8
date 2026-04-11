namespace SistemaFerreteriaV8.Domain.Audit;

public sealed class AuditEvent
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    public string ActorId { get; set; } = string.Empty;
    public string ActorName { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string MetadataJson { get; set; } = string.Empty;
}
