using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaFerreteriaV8.Clases
{
    public class Caja
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // Usar ObjectId de MongoDB
        public string Id { get; set; }

        [BsonElement("fechaApertura")]
        public DateTime FechaApertura { get; set; }

        [BsonElement("fechaCierre")]
        public DateTime FechaCierre { get; set; }

        [BsonElement("usuario")]
        public string Usuario { get; set; }

        [BsonElement("turno")]
        public string Turno { get; set; }

        [BsonElement("balanceInicial")]
        public double BalanceInicial { get; set; }

        [BsonElement("estado")]
        public string Estado { get; set; }

        [BsonElement("balanceFinal")]
        public double BalanceFinal { get; set; }

        private static readonly IMongoCollection<Caja> _cajaCollection;

        private const string EstadoAbierta = "true";

        // Inicialización estática para usar siempre la misma instancia de colección
        static Caja()
        {
            _cajaCollection = new MongoClient(new OneKeys().URI)
                .GetDatabase(new OneKeys().DatabaseName)
                .GetCollection<Caja>("caja2");

            EnsureIndexes();
        }

        private static void EnsureIndexes()
        {
            try
            {
                var indexNames = _cajaCollection.Indexes
                    .List()
                    .ToList()
                    .Select(i => i.GetValue("name", "").AsString)
                    .ToHashSet();

                // Regla de consistencia elegida: solo puede existir una caja global con estado abierto=true.
                if (!indexNames.Contains("idx_caja_single_open_global"))
                {
                    var partialOpenFilter = Builders<Caja>.Filter.Eq(c => c.Estado, EstadoAbierta);
                    var indexModel = new CreateIndexModel<Caja>(
                        Builders<Caja>.IndexKeys.Ascending(c => c.Estado),
                        new CreateIndexOptions
                        {
                            Name = "idx_caja_single_open_global",
                            Unique = true,
                            PartialFilterExpression = partialOpenFilter
                        });

                    _cajaCollection.Indexes.CreateOne(indexModel);
                }
            }
            catch (MongoCommandException)
            {
                // Evita romper inicialización si existen datos legados que violan la restricción.
                // El servicio seguirá detectando inconsistencias para mitigación operativa.
            }
            catch (MongoException)
            {
                // Evita caída del sistema por errores transitorios de conectividad/índices.
            }
        }

        public Caja()
        {
            // El Id lo generará automáticamente MongoDB al insertar, si no se asigna aquí.
        }

        // ----------------- Métodos CRUD Async -----------------

        public async Task CrearAsync()
        {
            // Si Id es null, MongoDB crea un nuevo ObjectId.
            await _cajaCollection.InsertOneAsync(this);
        }

        public async Task EditarAsync()
        {
            await _cajaCollection.ReplaceOneAsync(c => c.Id == this.Id, this);
        }

        public async Task EliminarAsync()
        {
            await _cajaCollection.DeleteOneAsync(c => c.Id == this.Id);
        }

        public static async Task<Caja> BuscarAsync(string id)
        {
            return await _cajaCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public static async Task<Caja> BuscarPorClaveAsync(string clave, string valor)
        {
            var filter = Builders<Caja>.Filter.Eq(clave, valor);
            return await _cajaCollection.Find(filter).FirstOrDefaultAsync();
        }

        public static async Task<List<Caja>> ListarAsync()
        {
            return await _cajaCollection.Find(_ => true).ToListAsync();
        }

        public static async Task<List<Caja>> ListarPorClaveAsync(string clave, string valor)
        {
            var filter = Builders<Caja>.Filter.Eq(clave, valor);
            return await _cajaCollection.Find(filter).ToListAsync();
        }

        public static async Task<List<Caja>> ListarFacturasAsync(DateTime fecha1, DateTime fecha2)
        {
            var filter = Builders<Caja>.Filter.And(
                Builders<Caja>.Filter.Gte(c => c.FechaApertura, fecha1),
                Builders<Caja>.Filter.Lte(c => c.FechaApertura, fecha2)
            );
            return await _cajaCollection.Find(filter).ToListAsync();
        }

        // --------------------------------------
        // Ya NO es necesario un GeneradorManualDeId.
        // El Id será generado como ObjectId (24 caracteres) automáticamente al hacer InsertOneAsync().
        // --------------------------------------
    }
}
