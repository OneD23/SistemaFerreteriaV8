using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFerreteriaV8.Clases
{
   public class Empleado
    {
        [BsonId]
        public string Id { get; set; }

        //Nombre
        [BsonElement("nombre")]
        public string Nombre { get; set; }

        //Puesto
        [BsonElement("puesto")]
        public string Puesto { get; set; }

        //Cedula
        [BsonElement("cedula")]
        public string Cedula { get; set; }

        //contrasena
        [BsonElement("contrasena")]
        public string Contrasena { get; set; }

        //Direccion
        [BsonElement("direccion")]
        public string Direccion { get; set; }

        //Telefono
        [BsonElement("telefono")]
        public string Telefono { get; set; }

        //Correo
        [BsonElement("correo")]
        public string Correo { get; set; }

        //Cuenta
        [BsonElement("cuenta")]
        public string Cuenta { get; set; }

        //Fecha
        [BsonElement("fecha")]
        DateTime fecha { get; set; }

        private readonly IMongoCollection<Empleado> _EmpleadoCollection;

        public Empleado()
        {

            _EmpleadoCollection = new MongoClient(new OneKeys().URI).GetDatabase("Ferreteria").GetCollection<Empleado>("Empleado");
        }

        public void Crear()
        {
            _EmpleadoCollection.InsertOne(this);
        }

        public void Editar()
        {
            _EmpleadoCollection.ReplaceOne(m => m.Id == this.Id, this);
        }

        public void Eliminar()
        {
            _EmpleadoCollection.DeleteOne(m => m.Id == this.Id);
        }

        public Empleado Buscar(string id)
        {
            return _EmpleadoCollection.Find(m => m.Id == id).FirstOrDefault();
        }

        public List<Empleado> Listar()
        {
            return _EmpleadoCollection.Find(_ => true).ToList();
        }

        public Empleado BuscarPorClave(string clave, string valor)
        {
            return _EmpleadoCollection.Find(Builders<Empleado>.Filter.Eq(clave, valor)).FirstOrDefault();
        }

        public List<Empleado> ListarPorClave(string clave, string valor)
        {
            return _EmpleadoCollection.Find(Builders<Empleado>.Filter.Eq(clave, valor)).ToList();
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

            } while (_EmpleadoCollection.Find(m => m.Id == nuevoId).Any());

            return nuevoId;
        }
    }
}
