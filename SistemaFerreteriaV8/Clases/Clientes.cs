using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaFerreteriaV8.Clases
{
    public class Cliente
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("cedula")]
        public string Cedula { get; set; }

        [BsonElement("direccion")]
        public string Direccion { get; set; }

        [BsonElement("telefono")]
        public string Telefono { get; set; }

        [BsonElement("correo")]
        public string Correo { get; set; }

        [BsonElement("cuenta")]
        public string Cuenta { get; set; }

        [BsonElement("limiteCredito")]
        public double LimiteCredito { get; set; }

        [BsonElement("creditoActivo")]
        public List<Factura> CreditosActivo { get; set; } = new List<Factura>();

        [BsonElement("fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        private readonly IMongoCollection<Cliente> _clientesCollection;

        public Cliente()
        {
            _clientesCollection = new MongoClient(new OneKeys().URI)
                .GetDatabase("Ferreteria")
                .GetCollection<Cliente>("cliente");
        }

        public async Task CrearAsync()
        {
            await _clientesCollection.InsertOneAsync(this);
        }

        public async Task EditarAsync()
        {
            await _clientesCollection.ReplaceOneAsync(m => m.Id == this.Id, this);
        }

        public async Task EliminarAsync()
        {
            await _clientesCollection.DeleteOneAsync(m => m.Id == this.Id);
        }

        public  async Task<Cliente> BuscarAsync(string id) 
        {
            return await _clientesCollection.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Cliente>> ListarAsync()
        {
            return await _clientesCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Cliente> BuscarPorClaveAsync(string clave, string valor)
        {
            var filtro = Builders<Cliente>.Filter.Eq(clave, valor);
            return await _clientesCollection.Find(filtro).FirstOrDefaultAsync();
        }

        public async Task<List<Cliente>> ListarPorClaveAsync(string clave, string valor)
        {
            var filtro = Builders<Cliente>.Filter.Eq(clave, valor);
            return await _clientesCollection.Find(filtro).ToListAsync();
        }

        /// <summary>
        /// Genera un nuevo Id único de 6 dígitos para un cliente.
        /// </summary>
        public async Task<string> GenerarNuevoIdAsync()
        {
            string nuevoId;
            var random = new Random();
            const string caracteres = "0123456789";
            bool existe;

            do
            {
                char[] idAleatorio = new char[6];
                for (int i = 0; i < 6; i++)
                    idAleatorio[i] = caracteres[random.Next(caracteres.Length)];
                nuevoId = new string(idAleatorio);

                existe = await _clientesCollection.Find(m => m.Id == nuevoId).AnyAsync();
            } while (existe);

            return nuevoId;
        }

        
    }
}
