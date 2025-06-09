using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SistemaFerreteriaV8.Clases
{
    public class Productos
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("nombre")]
        public string Nombre { get; set; }
        [BsonElement("descripcion")]
        public string Descripcion { get; set; }
        [BsonElement("categoria")]
        public string Categoria { get; set; }
        [BsonElement("Marca")]
        public string Marca { get; set; }
        [BsonElement("precio")]
        public List<double> Precio { get; set; }
        [BsonElement("Costo")]
        public double Costo { get; set; }
        [BsonElement("cantidad")]
        public double Cantidad { get; set; }
        [BsonElement("vendido")]
        public double Vendido { get; set; }
        [BsonElement("descuento")]
        public double Descuento { get; set; }
        [BsonElement("fechaDeEntrada")]
        public DateTime FechaDeEntrada { get; set; }

        private IMongoCollection<Productos> Collection = new MongoClient(new OneKeys().URI).GetDatabase("Ferreteria").GetCollection<Productos>("Productos");

        public Productos()
        {          
            this.Id = GenerarId();         
            CrearIndices();           
        }

        private void CrearIndices()
        {
            Collection.Indexes.CreateOne(new CreateIndexModel<Productos>(Builders<Productos>.IndexKeys.Ascending(p => p.Nombre)));
            Collection.Indexes.CreateOne(new CreateIndexModel<Productos>(Builders<Productos>.IndexKeys.Ascending(p => p.Categoria)));
            Collection.Indexes.CreateOne(new CreateIndexModel<Productos>(Builders<Productos>.IndexKeys.Ascending(p => p.Marca)));
        }
        public long ContarProductos()
        {
            return Collection.CountDocuments(new BsonDocument());
        }

        public void InsertarProductos(Productos nuevoProductos)
        {
            Collection.InsertOne(nuevoProductos);
        }

        public void ActualizarProductos()
        {
            var filter = Builders<Productos>.Filter.Eq(p => p.Id, this.Id);
            Collection.ReplaceOne(filter, this);
        }

        public Productos Buscar(string id)
        {
            var filter = Builders<Productos>.Filter.Eq("Id", id);
            return Collection.Find(filter).FirstOrDefault();
        }

        public Productos Buscar(string campo, string valor)
        {
            var filter = Builders<Productos>.Filter.Eq(campo, valor);
            return Collection.Find(filter).FirstOrDefault();
        }

        public void EliminarPorId(string id)
        {
            var filter = Builders<Productos>.Filter.Eq("Id", id);
            Collection.DeleteOne(filter);
        }

        public List<Productos> Listar()
        {
            return Collection.Find(new BsonDocument()).ToList();
        }

       public static (List<Productos>, long) ListarPorPagina(int numeroPagina, int tamañoPagina)
{
    // Validar parámetros
    if (numeroPagina < 1) numeroPagina = 1;
    if (tamañoPagina < 1) tamañoPagina = 10; // Tamaño predeterminado

    int salto = (numeroPagina - 1) * tamañoPagina;

    // Obtener la colección
    IMongoCollection<Productos> Collections =
        new MongoClient(new OneKeys().URI)
        .GetDatabase("Ferreteria")
        .GetCollection<Productos>("Productos");

    // Aplicar índice y ordenar para optimizar la paginación
    var productos = Collections.Find(new BsonDocument())
                               .Sort(Builders<Productos>.Sort.Ascending(p => p.Id))
                               .Skip(salto)
                               .Limit(tamañoPagina)
                               .ToList();

    // Usar EstimatedDocumentCount para mejorar el rendimiento
    long totalProductos = Collections.EstimatedDocumentCount();

    return (productos, totalProductos);
}
        public static (List<Productos>, long) ListarPorPagina(int numeroPagina, int tamañoPagina, string clave = null, string valor = null)
        {
            // Validar parámetros
            if (numeroPagina < 1) numeroPagina = 1;
            if (tamañoPagina < 1) tamañoPagina = 10; // Tamaño predeterminado

            int salto = (numeroPagina - 1) * tamañoPagina;

            // Obtener la colección
            var collections = new MongoClient(new OneKeys().URI)
                              .GetDatabase("Ferreteria")
                              .GetCollection<Productos>("Productos");

            // Construir filtro de búsqueda con coincidencia parcial
            FilterDefinition<Productos> filtro = Builders<Productos>.Filter.Empty;

            if (!string.IsNullOrEmpty(clave) && !string.IsNullOrEmpty(valor))
            {
                filtro = Builders<Productos>.Filter.Regex(clave, new BsonRegularExpression(valor, "i")); // Búsqueda insensible a mayúsculas/minúsculas
            }

            // Realizar consulta con paginación y ordenación
            var productos = collections.Find(filtro)
                                       .Sort(Builders<Productos>.Sort.Descending("_id")) // Ordenar por ID descendente para mantener consistencia en la paginación
                                       .Skip(salto)
                                       .Limit(tamañoPagina)
                                       .ToList();

            long totalProductos = collections.CountDocuments(filtro);

            return (productos, totalProductos);
        }

        public static double CalcularInversion()
        {
            IMongoCollection<Productos> Collections =
                new MongoClient(new OneKeys().URI)
                .GetDatabase("Ferreteria")
                .GetCollection<Productos>("Productos");
            // Pipeline de agregación para sumar (Costo * Cantidad) en MongoDB
            var pipeline = new[]
            {
                new BsonDocument("$project", new BsonDocument
                 {
                    { "Inversion", new BsonDocument("$multiply", new BsonArray { "$Costo", "$cantidad" }) }
                 }),
             new BsonDocument("$group", new BsonDocument
             {
                 { "_id", BsonNull.Value },
                 { "TotalInversion", new BsonDocument("$sum", "$Inversion") }
            })
              };

            // Ejecutar el pipeline de agregación
            var resultado = Collections.Aggregate<BsonDocument>(pipeline).FirstOrDefault();

            // Retornar la inversión total o 0 si no hay datos
            return resultado != null ? resultado["TotalInversion"].ToDouble() : 0.0;
        }

        public static double CalcularGananciasActuales()
        {
            IMongoCollection<Productos> Collections =
              new MongoClient(new OneKeys().URI)
              .GetDatabase("Ferreteria")
              .GetCollection<Productos>("Productos");

            // Pipeline de agregación para calcular las ganancias
            var pipeline = new[]
            {
            new BsonDocument("$project", new BsonDocument
            {
                { "Ganancia", new BsonDocument("$multiply", new BsonArray
                    {
                        new BsonDocument("$subtract", new BsonArray { new BsonDocument("$arrayElemAt", new BsonArray { "$precio", 0 }), "$Costo" }),
                        "$vendido"
                    })
                }
            }),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", BsonNull.Value },
                { "TotalGanancia", new BsonDocument("$sum", "$Ganancia") }
                })
            };

            // Ejecutar el pipeline de agregación
            var resultado = Collections.Aggregate<BsonDocument>(pipeline).FirstOrDefault();

            // Retornar la ganancia total o 0 si no hay datos
            return resultado != null ? resultado["TotalGanancia"].ToDouble() : 0.0;
        }
        public static double CalcularGananciasEsperadas()
        {
            IMongoCollection<Productos> Collections =
            new MongoClient(new OneKeys().URI)
            .GetDatabase("Ferreteria")
            .GetCollection<Productos>("Productos");

            // Pipeline de agregación para calcular las ganancias esperadas
            var pipeline = new[]
            {
                new BsonDocument("$project", new BsonDocument
                {
                    { "GananciaEsperada", new BsonDocument("$multiply", new BsonArray
                        {
                            new BsonDocument("$subtract", new BsonArray { new BsonDocument("$arrayElemAt", new BsonArray { "$precio", 0 }), "$Costo" }),
                            "$cantidad"
                        })
                    }
                }),
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", BsonNull.Value },
                    { "TotalGananciaEsperada", new BsonDocument("$sum", "$GananciaEsperada") }
                })
            };

            // Ejecutar el pipeline de agregación
            var resultado = Collections.Aggregate<BsonDocument>(pipeline).FirstOrDefault();

            // Retornar la ganancia esperada total o 0 si no hay datos
            return resultado != null ? resultado["TotalGananciaEsperada"].ToDouble() : 0.0;
        }

        public static double CalcularTotalProductosVendidos()
        {
            IMongoCollection<Productos> Collections =
            new MongoClient(new OneKeys().URI)
            .GetDatabase("Ferreteria")
            .GetCollection<Productos>("Productos");

            // Pipeline de agregación para calcular el total de productos vendidos
            var pipeline = new[]
            {
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", BsonNull.Value }, // No agrupar por ningún campo, queremos un total global
                    { "TotalVendidos", new BsonDocument("$sum", "$vendido") }
                })
            };

            // Ejecutar el pipeline de agregación
            var resultado = Collections.Aggregate<BsonDocument>(pipeline).FirstOrDefault();

            // Retornar el total de productos vendidos o 0 si no hay datos
            return resultado != null ? resultado["TotalVendidos"].ToDouble() : 0.0;
        }


        public List<Productos> ListarPorNombre(string nombre)
        {
            var filter = Builders<Productos>.Filter.Regex("Nombre", new BsonRegularExpression(nombre, "i"));
            return Collection.Find(filter).Limit(20).ToList();
        }

        public List<Productos> ListarParecidos(string campo, string valor)
        {
            var filter = Builders<Productos>.Filter.Regex(campo, new BsonRegularExpression(valor, "i"));
            return Collection.Find(filter).ToList();
        }

        public Dictionary<string, double> VendidosPorProductos()
        {
            var result = Collection.Find(new BsonDocument()).Sort(Builders<Productos>.Sort.Descending("Vendido")).Limit(5).ToList();
            var dictionary = new Dictionary<string, double>();
            foreach (var item in result)
            {
                dictionary[item.Nombre] = item.Vendido;
            }
            return dictionary;
        }

        public Dictionary<string, double> BajaCantidadPorProductos()
        {
            var result = Collection.Find(new BsonDocument()).Sort(Builders<Productos>.Sort.Ascending("Cantidad")).Limit(5).ToList();
            var dictionary = new Dictionary<string, double>();
            foreach (var item in result)
            {
                dictionary[item.Nombre] = item.Cantidad;
            }
            return dictionary;
        }

        public Dictionary<string, double> VendidosPorCategoria()
        {
            var result = Collection.Find(new BsonDocument()).Sort(Builders<Productos>.Sort.Descending("Vendido")).Limit(5).ToList();
            var dictionary = new Dictionary<string, double>();
            foreach (var item in result)
            {
                dictionary[item.Categoria] = item.Vendido;
            }
            return dictionary;
        }

        public List<Productos> LeerProductosDesdeExcel(string path)
        {
            var productos = new List<Productos>();
            using (var workbook = new XLWorkbook(path))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    var producto = new Productos
                    {
                        Id = row.Cell(1).Value.ToString(),
                        Nombre = row.Cell(2).Value.ToString(),
                        Descripcion = row.Cell(3).Value.ToString(),
                        Categoria = row.Cell(4).Value.ToString(),
                        Marca = row.Cell(5).Value.ToString(),
                        Precio = new List<double>
                        {
                            double.Parse(row.Cell(6).Value.ToString()),
                            double.Parse(row.Cell(7).Value.ToString())
                        },
                        Costo = double.Parse(row.Cell(8).Value.ToString()),
                        Cantidad = double.Parse(row.Cell(9).Value.ToString()),
                        Vendido = double.Parse(row.Cell(10).Value.ToString()),
                        Descuento = 0,
                        FechaDeEntrada = DateTime.Now
                    };
                    productos.Add(producto);
                }
            }
            return productos;
        }

        public void CargarProductosEnMongoDB(string pathArchivoExcel)
        {
            var productos = LeerProductosDesdeExcel(pathArchivoExcel);
            foreach (var producto in productos)
            {
                InsertarProductos(producto);
            }
        }

        private string GenerarId()
        {
            var lastId = Collection.AsQueryable().OrderByDescending(x => x.Id).Take(1).Select(x => x.Id).FirstOrDefault();
            string newId = "";
            if (lastId == null)
            {
                newId = "1";
            }
            else
            {
                int lastIdInt;
                if (int.TryParse(lastId, out lastIdInt))
                {
                    newId = (lastIdInt + 1).ToString();
                }
                else
                {
                    newId = "000001";
                }
            }

            while (Collection.Find(x => x.Id == newId).Any())
            {
                int newIdInt;
                if (int.TryParse(newId, out newIdInt))
                {
                    newId = (newIdInt + 1).ToString();
                }
            }
            if (!string.IsNullOrWhiteSpace(newId))
            {
                return newId;
            }
            else
            {
                return "1";
            }
        }

        internal object ObtenerEstadisticas(FilterDefinition<Productos> filtro)
        {
            throw new NotImplementedException();
        }

        public static void ExportarProductosAExcel()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Archivos de Excel (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Guardar archivo de productos";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string rutaArchivo = saveFileDialog.FileName;
                    MessageBox.Show("Se estan exportando todos los productos, puede seguir vendiendo con normalidad cuanto el proceso termine se le notificara", "Aviso");

                    // Ejecutar la operación de exportación en un subproceso en segundo plano
                    Task.Run(() =>
                    {
                        IMongoCollection<Productos> Collections =
                       new MongoClient(new OneKeys().URI)
                       .GetDatabase("Ferreteria")
                       .GetCollection<Productos>("Productos");

                        var productos = Collections.Find(new BsonDocument()).ToList();

                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Productos");

                            string[] encabezados = { "ID", "Nombre", "Descripción", "Categoría", "Marca", "Precio1", "Precio2", "Precio3", "Precio4", "Costo", "Cantidad", "Vendido", "Descuento", "Fecha de Entrada" };
                            worksheet.Row(1).Style.Font.Bold = true;

                            for (int i = 0; i < encabezados.Length; i++)
                            {
                                worksheet.Cell(1, i + 1).SetValue(encabezados[i]);
                            }

                            var datos = productos.Select(p => new object[]
                            {
                            p.Id,
                            p.Nombre,
                            p.Descripcion,
                            p.Categoria,
                            p.Marca,
                            p.Precio.Count > 0 ? p.Precio[0] : 0,
                            p.Precio.Count > 1 ? p.Precio[1] : 0,
                            p.Precio.Count > 2 ? p.Precio[2] : 0,
                            p.Precio.Count > 3 ? p.Precio[3] : 0,
                            p.Costo,
                            p.Cantidad,
                            p.Vendido,
                            p.Descuento,
                            p.FechaDeEntrada.ToString("yyyy-MM-dd")
                            }).ToList();

                            worksheet.Cell(2, 1).InsertData(datos);
                            worksheet.Columns().AdjustToContents();
                            workbook.SaveAs(rutaArchivo);
                            MessageBox.Show("Productos exportados! puede Encontrarlo en la siguiente ubicacion: " + rutaArchivo, "Aviso");


                        }
                    });
                }
            }
        }
    }
}
