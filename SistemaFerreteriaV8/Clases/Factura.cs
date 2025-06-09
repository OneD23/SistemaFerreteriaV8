using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Octetus.ConsultasDgii.Services;
using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaFerreteriaV8.Clases
{
    [BsonIgnoreExtraElements]
    public class Factura
    {
        // Identificador nativo de MongoDB
        [BsonId]
        public ObjectId ObjectId { get; set; }

        // Id secuencial para mostrar (NO usar como clave primaria real)
        [BsonElement("Id")]
        public int Id { get; set; }

        [BsonElement("RNC")]
        public string RNC { get; set; }
        [BsonElement("tipoFactura")]
        public string TipoFactura { get; set; }
        [BsonElement("NFC")]
        public string NFC { get; set; }
        [BsonElement("nombreEmpresa")]
        public string NombreEmpresa { get; set; }
        [BsonElement("idEmpleado")]
        public string IdEmpleado { get; set; }
        [BsonElement("idCliente")]
        public string IdCliente { get; set; }
        [BsonElement("nombreCliente")]
        public string NombreCliente { get; set; }
        [BsonElement("descripcion")]
        public string Description { get; set; }
        [BsonElement("direccion")]
        public string Direccion { get; set; }
        [BsonElement("metodoDePago")]
        public string MetodoDePago { get; set; }
        [BsonElement("tipoDePago")]
        public string TipoDePago { get; set; }
        [BsonElement("moneda")]
        public string Moneda { get; set; }
        [BsonElement("ventaDirecta")]
        public bool VentaDirecta { get; set; }
        [BsonElement("fecha")]
        public DateTime Fecha { get; set; }
        [BsonElement("productos")]
        public List<ListProduct> Productos { get; set; }
        [BsonElement("descuentos")]
        public double Descuentos { get; set; }
        [BsonElement("total")]
        public double Total { get; set; }
        [BsonElement("paga")]
        public bool Paga { get; set; }
        [BsonElement("efectivo")]
        public double Efectivo { get; set; }
        [BsonElement("enviar")]
        public bool Enviar { get; set; }
        [BsonElement("impresa")]
        public bool Impresa { get; set; }
        [BsonElement("estado")]
        public string Estado { get; set; }
        [BsonElement("informacion")]
        public string Informacion { get; set; }
        [BsonElement("editada")]
        public bool Editada { get; set; }
        [BsonElement("cotizacion")]
        public bool Cotizacion { get; set; }
        [BsonElement("eliminada")]
        public bool Eliminada { get; set; }

        // Mongo Collection (singleton)
        private static readonly IMongoCollection<Factura> collection = new MongoClient(new OneKeys().URI)
            .GetDatabase("Ferreteria")
            .GetCollection<Factura>("facturas");

        // Secuenciador atómico para Ids correlativos
        private static int GetNextSequenceValue(string name)
        {
            var counterCollection = new MongoClient(new OneKeys().URI)
                .GetDatabase("Ferreteria")
                .GetCollection<BsonDocument>("counters");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", name);
            var update = Builders<BsonDocument>.Update.Inc("seq", 1);
            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };
            var result = counterCollection.FindOneAndUpdate(filter, update, options);
            return result["seq"].AsInt32;
        }

        public Factura()
        {
            this.Id = GetNextSequenceValue("facturaid");
            this.Fecha = DateTime.Now;
            this.ObjectId = ObjectId.GenerateNewId();
            // Inicializa lista por defecto
            this.Productos = new List<ListProduct>();
        }

        // DTO para proyecciones rápidas
        public class FacturaResumen
        {
            public int Id { get; set; }
            public string NombreCliente { get; set; }
            public DateTime Fecha { get; set; }
            public double Total { get; set; }
            public bool Eliminada { get; set; }
            public string MetodoDePago { get; set; }
            public bool Cotizacion { get; set; }
            public bool Editada { get; set; }
        }

        #region Métodos CRUD Asíncronos

        public async Task InsertarFacturaAsync()
        {
            await collection.InsertOneAsync(this);
        }

        public async Task ActualizarFacturaAsync()
        {
            await collection.ReplaceOneAsync(f => f.Id == this.Id, this);
        }

        public async Task EliminarFacturaAsync()
        {
            this.Eliminada = true;
            await collection.ReplaceOneAsync(f => f.Id == this.Id, this);
        }

        public static async Task<Factura> BuscarAsync(int id)
        {
            var filter = Builders<Factura>.Filter.Eq("Id", id);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public static async Task<Factura> BuscarAsync(string clave, string valor)
        {
            var filter = Builders<Factura>.Filter.Eq(clave, valor);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        #endregion

        #region Listados y Paginación

      

public static async Task<List<Factura>> ListarFacturasAsync(string clave, string valor)
    {
        // Validación básica de parámetros
        if (string.IsNullOrWhiteSpace(clave))
            throw new ArgumentException("La clave de búsqueda no puede ser nula o vacía.", nameof(clave));

        // Valor por defecto para evitar errores de regex vacíos
        valor ??= "";

        // Filtro usando expresión regular (insensible a mayúsculas/minúsculas)
        var filter = Builders<Factura>.Filter.Regex(clave, new BsonRegularExpression(valor, "i"));

        // Consulta asincrónica a MongoDB
        var facturas = await collection.Find(filter).ToListAsync();

        // Retorna una lista vacía si es null (seguridad extra)
        return facturas ?? new List<Factura>();
    }


    public static async Task<(List<FacturaResumen>, double)> ListarFacturasCierre(string clave, string valor)
        {
            var filter = Builders<Factura>.Filter.Regex(clave, new BsonRegularExpression(valor, "i"));

            var projection = Builders<Factura>.Projection
                .Include(f => f.Id)
                .Include(f => f.NombreCliente)
                .Include(f => f.Fecha)
                .Include(f => f.Total)
                .Include(f => f.Eliminada)
                .Include(f => f.MetodoDePago)
                .Include(f => f.Cotizacion)
                .Include(f => f.Editada);

            var facturas = await collection.Find(filter)
                .Project<FacturaResumen>(projection)
                .ToListAsync();

            var facturasPagadas = facturas.Where(f =>
                f.Total > 0 && !f.Cotizacion && !f.Eliminada &&
                (f.MetodoDePago == "Efectivo" || f.MetodoDePago == "Pago Contra Entrega"))
                .ToList();

            double sumaTotal = facturasPagadas.Sum(f => f.Total);

            return (facturasPagadas, sumaTotal);
        }


        public static async Task<List<FacturaResumen>> ListarUltimasFacturasAsync()
        {
            var projection = Builders<Factura>.Projection
                .Include(f => f.Id)
                .Include(f => f.NombreCliente)
                .Include(f => f.Fecha)
                .Include(f => f.Total);

            return await collection.Find(FilterDefinition<Factura>.Empty)
                .Sort(Builders<Factura>.Sort.Descending(f => f.Id))
                .Limit(15)
                .Project<FacturaResumen>(projection)
                .ToListAsync();
        }

        public static async Task<List<Factura>> ListarFacturasPorNombreAsync(string nombre, int pageNumber, int pageSize)
        {
            var filter = Builders<Factura>.Filter.Regex("nombreCliente", new BsonRegularExpression(nombre, "i"));
            return await collection.Find(filter)
                .Sort(Builders<Factura>.Sort.Descending("nombreCliente"))
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public static async Task<List<Factura>> ListarFacturasPorFechaAsync(DateTime fecha1, DateTime fecha2)
        {
            var filter = Builders<Factura>.Filter.And(
                Builders<Factura>.Filter.Gte(m => m.Fecha, fecha1),
                Builders<Factura>.Filter.Lte(m => m.Fecha, fecha2)
            );

            var projection = Builders<Factura>.Projection
                .Include(f => f.Id)
                .Include(f => f.Fecha)
                .Include(f => f.Total);

            return await collection.Find(filter)
                .Project<Factura>(projection)
                .ToListAsync();
        }

        #endregion

        #region Operaciones sobre productos en factura

        public async Task RegistrarProductosAsync(int signo)
        {
            foreach (var item in this.Productos)
            {
                item.Producto.Vendido += signo * item.Cantidad;
                item.Producto.Cantidad -= signo * item.Cantidad;
                item.Producto.ActualizarProductos();
            }
        }

        #endregion

        #region Generación de Tickets (sin cambios profundos)
        // Puedes adaptar aquí los métodos de ticket que usas. No hago refactor profundo porque pueden depender de tu lógica actual, pero recuerda:
        // - Solo busca el cliente UNA VEZ.
        // - Si puedes, hazlo asíncrono.
        // - Evita consultas repetidas a Configuración y Cliente.
        #endregion

        #region Métodos de utilidades, ejemplo de índice
        // Recuerda crear los índices desde Mongo Shell:
        // db.facturas.createIndex({ "Id": 1 })
        // db.facturas.createIndex({ "NombreCliente": 1 })
        // db.facturas.createIndex({ "Fecha": -1 })
        #endregion
    




public async void GenerarFactura1()
        {
            Configuraciones confi = new Configuraciones().ObtenerPorId(1);
            CreaTicket2 factura = new CreaTicket2();

            //Encabezdo
            factura.TextoCentro(confi.Nombre != null ? confi.Nombre : "FERRETERIA");
            factura.TextoCentro(confi.Direccion != null ? confi.Direccion : "");
            factura.TextoCentro(confi.Telefono != null ? "Tel: " + confi.Telefono : "Tel: 809-487-1244");
            factura.TextoCentro(confi.Telefono != null ? "RNC:" + confi.RNC : "");



            factura.TextoCentro("");
            if (this.Cotizacion)
            {
                factura.TextoLargo("Esta factura es una cotizacion y no es valida");
            }
            else
            {
                factura.TextoIzquierda("Factura de Consumo");
            }

            factura.TextoDerecha("No. Factura: " + this.Id);
            factura.TextoExtremos("Fecha: " +this.Fecha.ToShortDateString(), "Hora: " + this.Fecha.ToShortTimeString());
            factura.TextoIzquierda("Nombre/Razon social: " + this.NombreCliente);
            factura.TextoIzquierda("Direccion: " + this.Direccion);
            var clienteBuscado = await new Cliente().BuscarAsync(this.IdCliente);
            string telefono = clienteBuscado?.Telefono ?? "";
            factura.TextoIzquierda("Tel: " + telefono);
            factura.TextoIzquierda("Metodo de pago: " + this.MetodoDePago);

            int i = 0;
            factura.LineasGuion();
            factura.EncabezadoVenta();
            factura.LineasGuion();

            double totalFact = 0;
            double totalItebisFact = 0;
            foreach (var item in this.Productos)
            {
                if (item == null) continue;
                factura.AgregaArticulo(item.Precio, item.Producto.Nombre, item.Cantidad, (item.Cantidad * item.Precio));

                if(item.Producto.Categoria == "Procesado")
                {
                    totalItebisFact += item.Precio - (item.Precio / 1.18);
                }
                totalFact += (item.Cantidad * item.Precio);
            }
            factura.LineasGuion();
            factura.TextoIzquierda("");
            factura.AgregaTotales("SubTotal: ", this.Total - totalItebisFact);
            factura.AgregaTotales("Itbis: ", totalItebisFact);

            if (this.Descuentos != null && this.Descuentos != 0)
            {
                factura.AgregaTotales("Descuento: ", this.Descuentos);
                factura.AgregaTotales("Total: ", this.Total - this.Descuentos);
            }
            else
            {
                factura.AgregaTotales("Total: ", this.Total);
            }

           

            if (MetodoDePago == "Efectivo")
            {
                factura.AgregaTotales("Efectivo: ", this.Efectivo);
                factura.AgregaTotales("Devuelta: ", Efectivo - this.Total);
            }

            if (!string.IsNullOrWhiteSpace(this.Description))
            {
                factura.TextoIzquierda("");
                factura.TextoIzquierda("");
                factura.LineasGuion();
                factura.TextoCentro("Notas:");
                factura.TextoLargo(this.Description);
            }

            factura.TextoIzquierda("");
            factura.LineasGuion();

            factura.TextoIzquierda("");
            if (MetodoDePago == "Credito")
            {
                factura.TextoCentro("______________________________");
                factura.TextoCentro(this.NombreCliente);
            }
            factura.TextoIzquierda("");
            factura.TextoIzquierda("Se requiere para devoluciones o cambios.");
            factura.TextoCentro("Gracias por su compra!");
            factura.TextoIzquierda("");
           

            factura.ImprimirTiket(confi.Impresora);

            /*PrinterClass printerClass = new PrinterClass();
            printerClass.PrinterName = confi.Impresora;
            printerClass.Print("");
            */

        }
        public async Task GenerarFacturaAsync()
        {
            // Carga configuración una sola vez
            var confi = new Configuraciones().ObtenerPorId(1);
            var ticket = new CreaTicket2();

            // Encabezado centralizado
            ticket.TextoCentro(!string.IsNullOrWhiteSpace(confi?.Nombre) ? confi.Nombre : "FERRETERIA");
            ticket.TextoCentro(confi?.Direccion ?? "");
            ticket.TextoCentro(!string.IsNullOrWhiteSpace(confi?.Telefono) ? "Tel: " + confi.Telefono : "Tel: 809-487-1244");
            ticket.TextoCentro(!string.IsNullOrWhiteSpace(confi?.RNC) ? "RNC:" + confi.RNC : "");

            ticket.TextoCentro("");
            ticket.TextoIzquierda("Factura de Consumo");

            // Manejo del NCF (comprobante fiscal)
            if (string.IsNullOrWhiteSpace(this.NFC))
            {
                double ultimoNFC = double.TryParse(confi?.SCCA, out var nfcTmp) ? nfcTmp : 0;
                double limiteNFC = double.TryParse(confi?.SCCF, out var nfcMax) ? nfcMax : 0;

                if (ultimoNFC <= limiteNFC)
                {
                    string numeroFormateado = (ultimoNFC + 1).ToString().PadLeft(8, '0');
                    ticket.TextoIzquierda("NCF: B02" + numeroFormateado);

                    // Actualiza NFC en la configuración y en la factura
                    confi.SCCA = numeroFormateado;
                    this.NFC = numeroFormateado;
                    confi.Guardar();
                    await this.ActualizarFacturaAsync(); // async para no bloquear la UI
                }
                else
                {
                    MessageBox.Show("Ya alcanzó su secuencia de comprobante fiscal máxima");
                    return;
                }
            }
            else
            {
                ticket.TextoIzquierda("NCF: B02" + this.NFC);
            }

            ticket.TextoDerecha("No. Factura: " + this.Id);
            ticket.TextoExtremos("Fecha: " + this.Fecha.ToShortDateString(), "Hora: " + this.Fecha.ToShortTimeString());

            ticket.TextoIzquierda("Nombre/Razón social: " + this.NombreCliente);
            ticket.TextoIzquierda("Dirección: " + this.Direccion);

            // Solo busca el cliente una vez
            string telCliente = "";
            var cliente = new Cliente().BuscarAsync(this.IdCliente);

            var clienteBuscado = await new Cliente().BuscarAsync(this.IdCliente);
            string telefono = clienteBuscado?.Telefono ?? "";
            ticket.TextoIzquierda("Tel: " + telefono); 

            ticket.TextoIzquierda("Método de pago: " + this.MetodoDePago);

            ticket.LineasGuion();
            ticket.EncabezadoVenta();
            ticket.LineasGuion();

            // Detalle de productos y cálculos
            double totalItebisFact = 0;
            foreach (var item in this.Productos ?? new List<ListProduct>())
            {
                if (item == null) continue;
                ticket.AgregaArticulo(item.Precio, item.Producto.Nombre, item.Cantidad, item.Cantidad * item.Precio);

                if (item.Producto.Categoria == "Procesado")
                    totalItebisFact += item.Precio - (item.Precio / 1.18);
            }

            ticket.LineasGuion();
            ticket.TextoIzquierda("");
            ticket.AgregaTotales("SubTotal: ", this.Total - totalItebisFact);
            ticket.AgregaTotales("Itbis: ", totalItebisFact);
            ticket.AgregaTotales("Total: ", this.Total);

            if (MetodoDePago == "Efectivo")
            {
                ticket.AgregaTotales("Efectivo: ", this.Efectivo);
                ticket.AgregaTotales("Devuelta: ", this.Efectivo - this.Total);
            }

            if (!string.IsNullOrWhiteSpace(this.Description))
            {
                ticket.TextoIzquierda("");
                ticket.LineasGuion();
                ticket.TextoCentro("Notas:");
                ticket.TextoLargo(this.Description);
            }

            ticket.TextoIzquierda("");
            ticket.LineasGuion();
            ticket.TextoIzquierda("");
            if (MetodoDePago == "Credito")
            {
                ticket.TextoCentro("______________________________");
                ticket.TextoCentro(this.NombreCliente);
            }
            ticket.TextoIzquierda("");
            ticket.TextoIzquierda("Se requiere para devoluciones o cambios.");
            ticket.TextoCentro("¡Gracias por su compra!");

            // Impresión del ticket
            ticket.ImprimirTiket(confi.Impresora);

            // Si usas una impresora personalizada, descomenta esto:
            // var printerClass = new PrinterClass();
            // printerClass.PrinterName = confi.Impresora;
            // printerClass.Print("");
        }


        public async void GenerarFacturaComprobante()
        {
            Configuraciones confi = new Configuraciones().ObtenerPorId(1);
            CreaTicket2 factura = new CreaTicket2();

            //Encabezdo

            //Encabezdo
            factura.TextoCentro(confi.Nombre != null ? confi.Nombre : "FERRETERIA");
            factura.TextoCentro(confi.Direccion != null ? confi.Direccion : "");
            factura.TextoCentro(confi.Telefono != null ? "Tel: " + confi.Telefono : "Tel: 809-487-1244");
            factura.TextoCentro(confi.Telefono != null ? "RNC:" + confi.RNC : "");

            factura.TextoCentro("");
            factura.TextoIzquierda("Comprobante fiscal");

            if (string.IsNullOrWhiteSpace(this.NFC))
            {
                double ultimoNFC = double.Parse(confi.NFCActual);
                if (ultimoNFC <= double.Parse(confi.NFCFinal))
                {
                    // Incrementar ultimoNFC y convertirlo a cadena.
                    string numeroFormateado = (ultimoNFC + 1).ToString();

                    // Rellenar con ceros a la izquierda para asegurar una longitud total de 8 caracteres.
                    numeroFormateado = numeroFormateado.PadLeft(8, '0');

                    // Usar el número formateado en la factura.
                    factura.TextoIzquierda("NCF: B01" + numeroFormateado);

                    // Actualizar la configuración con el nuevo último NFC.
                    confi.NFCActual = numeroFormateado;
                    this.NFC = "B01" + numeroFormateado;
                    confi.Guardar();
                    this.ActualizarFacturaAsync();
                }
                else
                {
                    MessageBox.Show("Ya alcanzo su secuencia de comprobante fiscal maxima, no se pueden generar mas comprobantes", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                factura.TextoIzquierda("NCF: B01" + this.NFC);
            }

            //si no hay rnc Registrado
            if(this.RNC == null || string.IsNullOrEmpty(this.RNC))
            {

                string NombreEncontrado = string.Empty;
                string RNCEncontrado = string.Empty;
                string rncTemporal = Interaction.InputBox("Favor digitar el rnc", "Busqueda de RNC");


                var dgii = new ServicioConsultasWebDgii();
                var response = dgii.ConsultarRncRegistrados(rncTemporal);

                if (response.Success)
                {
                    MessageBox.Show("RNC: " + response.RncOCedula + "\n" +
                                   "Nombre / Rason Social: " + response.Nombre + "\n" +
                                   "Tipo: " + response.Tipo + "\n" +
                                   "Actividad : " + response.Actividad + "\n" +
                                   "Estado: " + response.Estado + "\n", "RNC Registrado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    NombreEncontrado = response.Nombre;
                    RNCEncontrado = response.RncOCedula;

                }
                else
                {
                    var response2 = dgii.ConsultarRncContribuyentes(rncTemporal);
                    if (response2.Success)
                    {
                        MessageBox.Show("RNC: " + response2.CedulaORnc + "\n" +
                                    "Nombre / Rason Social: " + response2.NombreORazónSocial + "\n" +
                                    "Nombre Comercial: " + response2.NombreComercial + "\n" +
                                    "Categoria: " + response2.Categoría + "\n" +
                                    "Actividad : " + response2.ActividadEconomica + "\n" +
                                    "Estado: " + response2.Estado + "\n", "Contribuyente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        NombreEncontrado = response2.NombreORazónSocial;
                        RNCEncontrado = response2.CedulaORnc;
                    }
                    else
                    {
                        MessageBox.Show("Nombre encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                if (this != null)
                {
                    this.RNC = RNCEncontrado;
                    this.NombreCliente = NombreEncontrado;                
                }
                else
                {                    
                    this.RNC = RNCEncontrado;
                    this.NombreCliente = NombreEncontrado;
                }
                this.ActualizarFacturaAsync();   
            }

            factura.TextoIzquierda("Valido Hasta: " + confi.FechaExpiracion.ToShortDateString());

            factura.TextoDerecha("No. Factura: " + this.Id);
            factura.TextoExtremos("Fecha: " + this.Fecha.ToShortDateString(), "Hora: " + this.Fecha.ToShortTimeString());
            factura.TextoIzquierda("RNC: " + this.RNC);
            factura.TextoIzquierda("Nombre/Razon social: " + this.NombreCliente);
            factura.TextoIzquierda("Direccion: " + this.Direccion);

            var clienteBuscado = await new Cliente().BuscarAsync(this.IdCliente);
            string telefono = clienteBuscado?.Telefono ?? "";
            factura.TextoIzquierda("Tel: " + telefono);

            factura.TextoIzquierda("Metodo de pago: " + this.MetodoDePago);


            int i = 0;
            factura.LineasGuion();
            factura.EncabezadoVenta();
            factura.LineasGuion();


            double totalFact = 0;
            double totalItebisFact = 0;
            foreach (var item in this.Productos)
            {
                if (item == null) continue;
                factura.AgregaArticulo(item.Precio, item.Producto.Nombre, item.Cantidad, (item.Cantidad * item.Precio));

                if (item.Producto.Categoria == "Procesado")
                {
                    totalItebisFact += item.Precio - (item.Precio / 1.18);
                }
                totalFact += (item.Cantidad * item.Precio);
            }
            factura.LineasGuion();
            factura.TextoIzquierda("");
            factura.AgregaTotales("SubTotal: ", this.Total - totalItebisFact);
            factura.AgregaTotales("Itbis: ", totalItebisFact);
            factura.AgregaTotales("Total: ", this.Total); 

            if (MetodoDePago == "Efectivo")
            {
                factura.AgregaTotales("Efectivo: ", this.Efectivo);
                factura.AgregaTotales("Devuelta: ", Efectivo - this.Total);
            }

            if (!string.IsNullOrWhiteSpace(this.Description))
            {
                factura.TextoIzquierda("");
                factura.TextoIzquierda("");
                factura.LineasGuion();
                factura.TextoCentro("Notas:");
                factura.TextoLargo(this.Description);
            }

            factura.TextoIzquierda("");
            factura.LineasGuion();
            factura.TextoIzquierda("");
            if (MetodoDePago == "Credito")
            {
                factura.TextoCentro("______________________________");
                factura.TextoCentro(this.NombreCliente);
            }
            factura.TextoIzquierda("");
            factura.TextoIzquierda("Se requiere para devoluciones o cambios.");
            factura.TextoCentro("Gracias por su compra!");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("|");

            factura.ImprimirTiket(confi.Impresora);

           /* PrinterClass printerClass = new PrinterClass();
            printerClass.PrinterName = confi.Impresora;
            printerClass.Print("");*/

        }
        //Factura Gubernamental
        public async void GenerarFacturaGubernamental()
        {
            Configuraciones confi = new Configuraciones().ObtenerPorId(1);
            CreaTicket2 factura = new CreaTicket2();

            //Encabezdo
            factura.TextoCentro(confi.Nombre != null ? confi.Nombre : "FERRETERIA");
            factura.TextoCentro(confi.Direccion != null ? confi.Direccion : "");           
            factura.TextoCentro(confi.Telefono != null ? "Tel: " + confi.Telefono : "Tel: 809-487-1244");
            factura.TextoCentro(confi.Telefono != null ? "RNC:" + confi.RNC : "");

            factura.TextoCentro("");
            factura.TextoIzquierda("Factura Gubernamental");          

            if (string.IsNullOrWhiteSpace(this.NFC))
            {
                double ultimoNFC = double.Parse(confi.SGA);
                if (ultimoNFC < double.Parse(confi.SGF))
                {
                    // Incrementar ultimoNFC y convertirlo a cadena.
                    string numeroFormateado = (ultimoNFC + 1).ToString();

                    // Rellenar con ceros a la izquierda para asegurar una longitud total de 8 caracteres.
                    numeroFormateado = numeroFormateado.PadLeft(8, '0');

                    // Usar el número formateado en la factura.
                    factura.TextoIzquierda("NCF: B15" + numeroFormateado);

                    // Actualizar la configuración con el nuevo último NFC.
                    confi.SGA = numeroFormateado;
                    this.NFC = "B15" + numeroFormateado;

                    this.ActualizarFacturaAsync();
                    confi.Guardar();
                }
                else
                {
                    MessageBox.Show("Ya alcanzo su secuencia de comprobante fiscal maxima");
                }
            }
            else
                {
                    factura.TextoIzquierda("NCF: B15" + this.NFC);
                }
            //si no hay rnc Registrado
            if (this.RNC == null || string.IsNullOrEmpty(this.RNC))
            {

                string NombreEncontrado = string.Empty;
                string RNCEncontrado = string.Empty;
                string rncTemporal = Interaction.InputBox("Favor digitar el rnc", "Busqueda de RNC");


                var dgii = new ServicioConsultasWebDgii();
                var response = dgii.ConsultarRncRegistrados(rncTemporal);

                if (response.Success)
                {
                    MessageBox.Show("RNC: " + response.RncOCedula + "\n" +
                                   "Nombre / Rason Social: " + response.Nombre + "\n" +
                                   "Tipo: " + response.Tipo + "\n" +
                                   "Actividad : " + response.Actividad + "\n" +
                                   "Estado: " + response.Estado + "\n", "RNC Registrado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    NombreEncontrado = response.Nombre;
                    RNCEncontrado = response.RncOCedula;

                }
                else
                {
                    var response2 = dgii.ConsultarRncContribuyentes(rncTemporal);
                    if (response2.Success)
                    {
                        MessageBox.Show("RNC: " + response2.CedulaORnc + "\n" +
                                    "Nombre / Rason Social: " + response2.NombreORazónSocial + "\n" +
                                    "Nombre Comercial: " + response2.NombreComercial + "\n" +
                                    "Categoria: " + response2.Categoría + "\n" +
                                    "Actividad : " + response2.ActividadEconomica + "\n" +
                                    "Estado: " + response2.Estado + "\n", "Contribuyente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        NombreEncontrado = response2.NombreORazónSocial;
                        RNCEncontrado = response2.CedulaORnc;
                    }
                    else
                    {
                        MessageBox.Show("Nombre encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                if (this != null)
                {
                    this.RNC = RNCEncontrado;
                    this.NombreCliente = NombreEncontrado;
                }
                else
                {
                    this.RNC = RNCEncontrado;
                    this.NombreCliente = NombreEncontrado;
                }
                this.ActualizarFacturaAsync();
            }

            factura.TextoIzquierda("Valido Hasta: " + confi.FechaExpiracion.ToShortDateString());

            factura.TextoDerecha("No. Factura: " + this.Id);
            factura.TextoExtremos("Fecha: " + this.Fecha.ToShortDateString(), "Hora: " + this.Fecha.ToShortTimeString());
            factura.TextoIzquierda("RNC: " + this.RNC);
            factura.TextoIzquierda("Nombre/Razon social: " + this.NombreCliente);
            factura.TextoIzquierda("Direccion: " + this.Direccion);

            var clienteBuscado = await new Cliente().BuscarAsync(this.IdCliente);
            string telefono = clienteBuscado?.Telefono ?? "";
            factura.TextoIzquierda("Tel: " + telefono); factura.TextoIzquierda("Metodo de pago: " + this.MetodoDePago);

            int i = 0;
            factura.LineasGuion();
            factura.EncabezadoVenta();
            factura.LineasGuion();

            double totalFact = 0;
            double totalItebisFact = 0;
            foreach (var item in this.Productos)
            {
                if (item == null) continue;
                factura.AgregaArticulo(item.Precio, item.Producto.Nombre, item.Cantidad, (item.Cantidad * item.Precio));
              
                if (item.Producto.Categoria == "Procesado")
                {
                   
                    totalItebisFact += item.Precio - (item.Precio / 1.18);
                }
                totalFact += (item.Cantidad * item.Precio);
            }
            factura.LineasGuion();
            factura.TextoIzquierda("");
            factura.AgregaTotales("SubTotal: ", this.Total - totalItebisFact);
            factura.AgregaTotales("Itbis: ", totalItebisFact);
            factura.AgregaTotales("Total: ", this.Total);

            if (MetodoDePago == "Efectivo")
            {
                factura.AgregaTotales("Efectivo: ", this.Efectivo);
                factura.AgregaTotales("Devuelta: ", Efectivo - this.Total);
            }

            if (!string.IsNullOrWhiteSpace(this.Description))
            {
                factura.TextoIzquierda("");
                factura.TextoIzquierda("");
                factura.LineasGuion();
                factura.TextoCentro("Notas:");
                factura.TextoLargo(this.Description);
            }

            factura.TextoIzquierda("");
            factura.LineasGuion();
            factura.TextoIzquierda("");
            if (MetodoDePago == "Credito")
            {
                factura.TextoCentro("______________________________");
                factura.TextoCentro(this.NombreCliente);
            }
            factura.TextoIzquierda("");
            factura.TextoIzquierda("Se requiere para devoluciones o cambios.");
            factura.TextoCentro("Gracias por su compra!");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("");
            factura.TextoIzquierda("|");

            factura.ImprimirTiket(confi.Impresora);
/*
            PrinterClass printerClass = new PrinterClass();
            printerClass.PrinterName = confi.Impresora;
            printerClass.Print("");
*/
        }        
    }   
}
