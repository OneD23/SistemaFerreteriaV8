using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaFerreteriaV8.Clases
{
    public class Caja
    {
        [BsonId]
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

        private readonly IMongoCollection<Caja> _cajaCollection;

        public Caja()
        {
            _cajaCollection = new MongoClient(new OneKeys().URI)
                .GetDatabase("Ferreteria")
                .GetCollection<Caja>("caja2");
        }

        // --- Métodos síncronos legacy ---
        

        // --- Métodos Async óptimos ---

        public async Task CrearAsync()
        {
            await _cajaCollection.InsertOneAsync(this);
        }

        public async Task EditarAsync()
        {
            await _cajaCollection.ReplaceOneAsync(m => m.Id == this.Id, this);
        }

        public async Task EliminarAsync()
        {
            await _cajaCollection.DeleteOneAsync(m => m.Id == this.Id);
        }

        public async Task<Caja> BuscarAsync(string id)
        {
            return await _cajaCollection.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Caja>> ListarAsync()
        {
            return await _cajaCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Caja> BuscarPorClaveAsync(string clave, string valor)
        {
            return await _cajaCollection.Find(Builders<Caja>.Filter.Eq(clave, valor)).FirstOrDefaultAsync();
        }

        public async Task<List<Caja>> ListarPorClaveAsync(string clave, string valor)
        {
            return await _cajaCollection.Find(Builders<Caja>.Filter.Eq(clave, valor)).ToListAsync();
        }

        public async Task<List<Caja>> ListarFacturasAsync(DateTime fecha1, DateTime fecha2)
        {
            var filter = Builders<Caja>.Filter.And(
                Builders<Caja>.Filter.Gte(m => m.FechaApertura, fecha1),
                Builders<Caja>.Filter.Lte(m => m.FechaApertura, fecha2)
            );
            return await _cajaCollection.Find(filter).ToListAsync();
        }

        public async Task<string> GenerarNuevoIdAsync()
        {
            string nuevoId;
            Random random = new Random();
            const string caracteres = "0123456789";

            do
            {
                char[] idAleatorio = new char[6];
                for (int i = 0; i < 6; i++)
                {
                    idAleatorio[i] = caracteres[random.Next(caracteres.Length)];
                }
                nuevoId = new string(idAleatorio);
            }
            while (await _cajaCollection.Find(m => m.Id == nuevoId).AnyAsync());

            return nuevoId;
        }

        // Síncrono por compatibilidad si lo necesitas:
        public string GenerarNuevoId()
        {
            string nuevoId;
            Random random = new Random();
            const string caracteres = "0123456789";
            do
            {
                char[] idAleatorio = new char[6];
                for (int i = 0; i < 6; i++)
                    idAleatorio[i] = caracteres[random.Next(caracteres.Length)];
                nuevoId = new string(idAleatorio);
            }
            while (_cajaCollection.Find(m => m.Id == nuevoId).Any());
            return nuevoId;
        }
    }
}
