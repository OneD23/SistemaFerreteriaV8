using MongoDB.Driver;
using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Clases;
using SistemaFerreteriaV8.Domain.Audit;
using System.Text.Json;

namespace SistemaFerreteriaV8.Infrastructure.Services;

public sealed class AuditService : IAuditService
{
    private readonly IMongoCollection<AuditEvent> _collection;

    public AuditService()
    {
        var client = new MongoClient(new OneKeys().URI);
        var db = client.GetDatabase(new OneKeys().DatabaseName);
        _collection = db.GetCollection<AuditEvent>("audit_events");
    }

    public async Task WriteAsync(AuditEvent auditEvent)
    {
        if (auditEvent == null) return;
        await _collection.InsertOneAsync(auditEvent);
    }

    public async Task WriteAsync(
        string eventType,
        string module,
        string result,
        string message,
        string actorId = "",
        string actorName = "",
        object? metadata = null)
    {
        var audit = new AuditEvent
        {
            EventType = eventType,
            Module = module,
            Result = result,
            Message = message,
            ActorId = actorId,
            ActorName = actorName,
            MetadataJson = metadata == null ? string.Empty : JsonSerializer.Serialize(metadata)
        };

        await WriteAsync(audit);
    }
}
