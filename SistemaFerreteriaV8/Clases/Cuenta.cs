using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaFerreteriaV8.Clases
{
    public class Cuenta
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("idCliente")]
        public string IdCliente { get; set; }

        [BsonElement("idEmpleado")]
        public string IdEmpleado { get; set; }

        [BsonElement("idFactura")]
        public string IdFactura { get; set; }

        [BsonElement("valor")]
        public double Valor { get; set; } 
        [BsonElement("abono")]
        public double Abono { get; set; }

        [BsonElement("fechaInicio")]
        public DateTime FechaInicio { get; set; }

        [BsonElement("fechaLimite")]
        public DateTime FechaLimite { get; set; }

        [BsonElement("activo")]
        public bool Activo { get; set; }


        private readonly IMongoCollection<Cuenta> _ClienteCollection;

        public Cuenta()
        {
            _ClienteCollection = new MongoClient(new OneKeys().URI).GetDatabase("Ferreteria").GetCollection<Cuenta>("Cuenta");
        }


        public void Crear()
        {
            _ClienteCollection.InsertOne(this);
        }

        public void Editar()
        {
            _ClienteCollection.ReplaceOne(m => m.Id == this.Id, this);
        }

        public void Eliminar()
        {
            _ClienteCollection.DeleteOne(m => m.Id == this.Id);
        }

        public Cuenta Buscar(string id)
        {
            return _ClienteCollection.Find(m => m.Id == id).FirstOrDefault();
        }

        public List<Cuenta> Listar()
        {
            return _ClienteCollection.Find(_ => true).ToList();
        }

        public Cuenta BuscarPorClave(string clave, string valor)
        {
            return _ClienteCollection.Find(Builders<Cuenta>.Filter.Eq(clave, valor)).FirstOrDefault();
        }
        public Cuenta BuscarPorIdFactura(string valor)
        {         
            return _ClienteCollection.Find(C => C.IdFactura == valor).FirstOrDefault();
        }

        public List<Cuenta> ListarPorClave(string clave, string valor)
        {
            return _ClienteCollection.Find(Builders<Cuenta>.Filter.Eq(clave, valor)).ToList();
        }
        public List<Cuenta> ListarFacturas(DateTime fecha1, DateTime fecha2)
        {
            var filter = Builders<Cuenta>.Filter.And(Builders<Cuenta>.Filter.Gte(m => m.FechaInicio, fecha1), Builders<Cuenta>.Filter.Lte(m => m.FechaInicio, fecha2));
            return _ClienteCollection.Find(filter).ToList();
        }
        public string GenerarNuevoId()
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

            } while (_ClienteCollection.Find(m => m.Id == nuevoId).Any());

            return nuevoId;
        }
    }
}
