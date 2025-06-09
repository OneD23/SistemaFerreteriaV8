using Microsoft.SqlServer.Server;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFerreteriaV8.Clases
{
    [BsonIgnoreExtraElements]
    public class Configuraciones
    {
        [BsonId]
        public int Id { get; set; } = 1;
        //Nombre
        [BsonElement("nombre")]
        public string Nombre {  get; set; }
        //Correo
        [BsonElement("correo")]
        public string Correo {  get; set; }
        //RNC
        [BsonElement("RNC")]
        public string RNC { get; set; }
        //NFCInicial
        [BsonElement("NFCInicial")]
        public string NFCInicial { get; set; }
        //NFCActual
        [BsonElement("NFCActual")]
        public string NFCActual { get; set; }
        //Ultimo NFC
        [BsonElement("UltimoNFC")]
        public string UltimoNFC { get; set; }
        //NFCFinal
        [BsonElement("NFCFinal")]
        public string NFCFinal { get; set; }
        //FechaExpiracion
        [BsonElement("fechaExpiracion")]
        public DateTime FechaExpiracion { get; set; }
        //direccion
        [BsonElement("direccion")]
        public string Direccion { get; set; }
        //Telefono
        [BsonElement("Telefono")]
        public string Telefono { get; set; }
        //Impresora
        [BsonElement("impresora")]
        public string Impresora { get; set; }
        //fontSize
        [BsonElement("fontSize")]
        public string FontSize { get; set; }

        //********************************************************Comprobates***********************************

        //Secuencia Gubernamental Inicial
        [BsonElement("sgi")]
        public string SGI { get; set; }

        //Secuencia Gubernamental Actual
        [BsonElement("sga")]
        public string SGA { get; set; }

        //Secuencia Gubernamental final
        [BsonElement("sgf")]
        public string SGF { get; set; }      

        //Secuencia de notas de credito inicial
        [BsonElement("snci")]
        public string SNCI { get; set; }

        //Secuencia de notas de credito Final
        [BsonElement("sncf")]
        public string SNCF { get; set; }

        //Secuencia de notas de credito Actual
        [BsonElement("snca")]
        public string SNCA { get; set; }


        //Secuencia de Comprobante de consumo

        [BsonElement("scci")]
        public string SCCI { get; set; }

        [BsonElement("scca")]
        public string SCCA { get; set; }

        [BsonElement("sccf")]
        public string SCCF { get; set; }

        [BsonElement("fscc")]
        public DateTime FSCC { get; set; }

        [BsonElement("Seleccion")]
        public string Seleccion { get; set; }


        //********************************************************Comprobates***********************************

        //Icono
        [BsonElement("icono")]
        public string Icono { get; set; }
        //imagen
        [BsonElement("imagen")]
        public string Imagen { get; set; }
        //Precio
        [BsonElement("precio")]
        public int Precio { get; set; }

        IMongoCollection<Configuraciones> Collection = new MongoClient(new OneKeys().URI).GetDatabase("Ferreteria").GetCollection<Configuraciones>("configuraciones");


       
        public void Guardar()
        {
            if (this.Id != 0 && Collection.Find(c => c.Id == this.Id).Any())
            {
                Collection.ReplaceOne(c => c.Id == this.Id, this);
            }
            else
            {
                Collection.InsertOne(this);
            }
        }

        public void Actualizar()
        {
            Collection.ReplaceOne(m => m.Id == this.Id, this);
        }

        public void Eliminar()
        {
            var filter = Builders<Configuraciones>.Filter.Eq(c => c.Id, this.Id);
            Collection.DeleteOne(filter);
        }

        public  Configuraciones ObtenerPorId(int id)
        {
            var filter = Builders<Configuraciones>.Filter.Eq(c => c.Id, id);
            try
            {
                return Collection.Find(filter).FirstOrDefault();
            }
            catch (MongoDB.Driver.MongoConnectionException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "No se pudo conectar a la base de datos MongoDB.\n\n" +
                    "Verifica que el servidor esté corriendo y la cadena de conexión sea correcta.\n\n" +
                    "Detalles técnicos:\n" + ex.Message,
                    "Error de conexión",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
            catch (TimeoutException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Tiempo de espera agotado al intentar conectar con la base de datos.\n\n" +
                    "Revisa la conexión o el estado de MongoDB.\n\n" +
                    "Detalles:\n" + ex.Message,
                    "Error de tiempo de espera",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Ocurrió un error inesperado al consultar la base de datos:\n\n" +
                    ex.Message,
                    "Error inesperado",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
        }
    }
}


