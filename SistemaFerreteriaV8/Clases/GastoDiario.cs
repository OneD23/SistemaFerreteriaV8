using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaFerreteriaV8.Clases
{
    public class GastoDiario
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [BsonElement("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [BsonElement("monto")]
        public double Monto { get; set; }

        private static IMongoCollection<GastoDiario> Collection =>
            new MongoClient(new OneKeys().URI)
                .GetDatabase(new OneKeys().DatabaseName)
                .GetCollection<GastoDiario>("gastosDiarios");

        public async Task CrearAsync()
        {
            await Collection.InsertOneAsync(this);
        }

        public static async Task<List<GastoDiario>> ListarPorRangoAsync(DateTime desde, DateTime hasta)
        {
            var filtro = Builders<GastoDiario>.Filter.Gte(g => g.Fecha, desde) &
                         Builders<GastoDiario>.Filter.Lte(g => g.Fecha, hasta);
            return await Collection.Find(filtro).ToListAsync();
        }
    }
}
