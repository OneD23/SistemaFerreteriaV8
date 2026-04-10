using MongoDB.Bson;
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

    public async Task<IReadOnlyCollection<AuditEvent>> QueryAsync(AuditQuery request)
    {
        var filters = new List<FilterDefinition<AuditEvent>>();

        if (request.FromUtc.HasValue)
            filters.Add(Builders<AuditEvent>.Filter.Gte(x => x.TimestampUtc, request.FromUtc.Value));

        if (request.ToUtc.HasValue)
            filters.Add(Builders<AuditEvent>.Filter.Lte(x => x.TimestampUtc, request.ToUtc.Value));

        if (!string.IsNullOrWhiteSpace(request.Actor))
        {
            var actorRegex = new BsonRegularExpression(request.Actor.Trim(), "i");
            filters.Add(Builders<AuditEvent>.Filter.Or(
                Builders<AuditEvent>.Filter.Regex(x => x.ActorName, actorRegex),
                Builders<AuditEvent>.Filter.Regex(x => x.ActorId, actorRegex)));
        }

        if (!string.IsNullOrWhiteSpace(request.Module))
            filters.Add(Builders<AuditEvent>.Filter.Regex(x => x.Module, new BsonRegularExpression(request.Module.Trim(), "i")));

        if (!string.IsNullOrWhiteSpace(request.EventType))
            filters.Add(Builders<AuditEvent>.Filter.Regex(x => x.EventType, new BsonRegularExpression(request.EventType.Trim(), "i")));

        if (!string.IsNullOrWhiteSpace(request.OperationId))
            filters.Add(Builders<AuditEvent>.Filter.Regex(x => x.MetadataJson, new BsonRegularExpression(request.OperationId.Trim(), "i")));

        var filter = filters.Count == 0
            ? Builders<AuditEvent>.Filter.Empty
            : Builders<AuditEvent>.Filter.And(filters);

        var limit = request.Limit <= 0 ? 100 : Math.Min(request.Limit, 1000);

        return await _collection
            .Find(filter)
            .SortByDescending(x => x.TimestampUtc)
            .Limit(limit)
            .ToListAsync();
    }
}
