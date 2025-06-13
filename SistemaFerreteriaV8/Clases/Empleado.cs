using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaFerreteriaV8.Clases
{
    public class Empleado
    {
        [BsonId]
        public ObjectId Id { get; set; } // Usa ObjectId para MongoDB performance

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("puesto")]
        public string Puesto { get; set; }

        [BsonElement("cedula")]
        public string Cedula { get; set; }

        [BsonElement("contrasena")]
        public string Contrasena { get; set; }

        [BsonElement("direccion")]
        public string Direccion { get; set; }

        [BsonElement("telefono")]
        public string Telefono { get; set; }

        [BsonElement("correo")]
        public string Correo { get; set; }

        [BsonElement("cuenta")]
        public string Cuenta { get; set; }

        [BsonElement("fecha")]
        public DateTime Fecha { get; set; }

        private static IMongoCollection<Empleado> EmpleadoCollection =>
            new MongoClient(new OneKeys().URI)
                .GetDatabase("Ferreteria")
                .GetCollection<Empleado>("Empleado");

        public Empleado() { }

        // Métodos Async Modernos

        public async Task CrearAsync()
        {
            await EmpleadoCollection.InsertOneAsync(this);
        }

        public async Task EditarAsync()
        {
            await EmpleadoCollection.ReplaceOneAsync(e => e.Id == this.Id, this);
        }

        public async Task EliminarAsync()
        {
            await EmpleadoCollection.DeleteOneAsync(e => e.Id == this.Id);
        }

        public static async Task<Empleado> BuscarAsync(string id)
        {
            // Permite buscar por string id (se convierte a ObjectId si es válido)
            ObjectId oid;
            var filter = ObjectId.TryParse(id, out oid)
                ? Builders<Empleado>.Filter.Eq(e => e.Id, oid)
                : Builders<Empleado>.Filter.Eq("cedula", id); // Búsqueda alternativa por cédula
            return await EmpleadoCollection.Find(filter).FirstOrDefaultAsync();
        }

        public static async Task<List<Empleado>> ListarAsync()
        {
            return await EmpleadoCollection.Find(_ => true).ToListAsync();
        }

        public static async Task<Empleado> BuscarPorClaveAsync(string clave, string valor)
        {
            var filter = Builders<Empleado>.Filter.Eq(clave, valor);
            return await EmpleadoCollection.Find(filter).FirstOrDefaultAsync();
        }

        public static async Task<List<Empleado>> ListarPorClaveAsync(string clave, string valor)
        {
            var filter = Builders<Empleado>.Filter.Eq(clave, valor);
            return await EmpleadoCollection.Find(filter).ToListAsync();
        }

        // Si necesitas crear manualmente el ObjectId, usa esto:
        public static ObjectId GenerarNuevoObjectId()
        {
            return ObjectId.GenerateNewId();
        }
    }
}
