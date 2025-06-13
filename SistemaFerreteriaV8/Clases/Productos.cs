using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private static readonly IMongoCollection<Productos> _collection =
            new MongoClient(new OneKeys().URI)
                .GetDatabase("Ferreteria")
                .GetCollection<Productos>("Productos");

        public Productos()
        {
            Id = GenerarId();
            CrearIndices();
        }

        #region Índices
        private void CrearIndices()
        {
            _collection.Indexes.CreateOne(new CreateIndexModel<Productos>(
                Builders<Productos>.IndexKeys.Ascending(p => p.Nombre)));
            _collection.Indexes.CreateOne(new CreateIndexModel<Productos>(
                Builders<Productos>.IndexKeys.Ascending(p => p.Categoria)));
            _collection.Indexes.CreateOne(new CreateIndexModel<Productos>(
                Builders<Productos>.IndexKeys.Ascending(p => p.Marca)));
        }
        #endregion

        #region CRUD Síncrono
        public long ContarProductos() => _collection.CountDocuments(Builders<Productos>.Filter.Empty);
        public void InsertarProductos(Productos prod) => _collection.InsertOne(prod);
        public void ActualizarProductos() => _collection.ReplaceOne(p => p.Id == this.Id, this);
        public Productos Buscar(string id) => _collection.Find(p => p.Id == id).FirstOrDefault();
        public Productos Buscar(string campo, string valor) =>
            _collection.Find(Builders<Productos>.Filter.Eq(campo, valor)).FirstOrDefault();
        public void EliminarPorId(string id) => _collection.DeleteOne(p => p.Id == id);
        public List<Productos> Listar() => _collection.Find(Builders<Productos>.Filter.Empty).ToList();
        #endregion

        #region CRUD Async
        public static async Task<long> ContarProductosAsync() =>
            await _collection.CountDocumentsAsync(Builders<Productos>.Filter.Empty);

        public static async Task InsertarProductosAsync(Productos prod) =>
            await _collection.InsertOneAsync(prod);

        public async Task ActualizarProductosAsync() =>
            await _collection.ReplaceOneAsync(p => p.Id == this.Id, this);

        public static async Task<Productos> BuscarAsync(string id) =>
            await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();

        public static async Task<Productos> BuscarPorClaveAsync(string campo, string valor) =>
            await _collection.Find(Builders<Productos>.Filter.Eq(campo, valor)).FirstOrDefaultAsync();

        public static async Task<List<Productos>> ListarAsync() =>
            await _collection.Find(Builders<Productos>.Filter.Empty).ToListAsync();

        public static async void TaskEliminarAsync(string id) =>
            await _collection.DeleteOneAsync(p => p.Id == id);

        #endregion

        #region Paginación
        public static async Task<(List<Productos> Productos, long Total)> ListarPorPaginaAsync(
            int numeroPagina, int tamañoPagina, string clave = null, string valor = null)
        {
            if (numeroPagina < 1) numeroPagina = 1;
            if (tamañoPagina < 1) tamañoPagina = 10;
            int skip = (numeroPagina - 1) * tamañoPagina;

            var filter = Builders<Productos>.Filter.Empty;
            if (!string.IsNullOrWhiteSpace(clave) && !string.IsNullOrWhiteSpace(valor))
            {
                filter = Builders<Productos>.Filter.Regex(clave, new BsonRegularExpression(valor, "i"));
            }

            var productos = await _collection.Find(filter)
                .SortByDescending(p => p.Id)
                .Skip(skip)
                .Limit(tamañoPagina)
                .ToListAsync();

            var total = await _collection.CountDocumentsAsync(filter);
            return (productos, total);
        }
        #endregion

        #region Agregaciones

        public static async Task<double> CalcularInversionAsync()
        {
            var pipeline = new[]
            {
                new BsonDocument("$project",
                  new BsonDocument("Inversion",
                    new BsonDocument("$multiply", new BsonArray{"$Costo","$cantidad"}))),
                new BsonDocument("$group",
                  new BsonDocument
                    { { "_id", BsonNull.Value }, { "TotalInversion", new BsonDocument("$sum","$Inversion") } })
            };
            var result = await _collection.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
            return result?["TotalInversion"].ToDouble() ?? 0;
        }

        public static async Task<double> CalcularGananciasActualesAsync()
        {
            var pipeline = new[]
            {
        // 1) Proyectamos la ganancia: (avg(precio) – Costo) * vendido
        new BsonDocument("$project",
            new BsonDocument("Ganancia",
                new BsonDocument("$multiply", new BsonArray
                {
                    // aquí calculamos el promedio de todos los elementos de precio
                    new BsonDocument("$subtract", new BsonArray
                    {
                        new BsonDocument("$avg", "$precio"),
                        // si Costo no es numérico en la BD, puedes convertirlo:
                        // new BsonDocument("$toDouble", "$Costo")
                        "$Costo"
                    }),
                    // igual para vendido, convierte si fuese necesario:
                    // new BsonDocument("$toDouble", "$vendido")
                    "$vendido"
                })
            )
        ),
        // 2) Agrupamos todos los documentos y sumamos la ganancia parcial
        new BsonDocument("$group",
            new BsonDocument
            {
                { "_id", BsonNull.Value },
                { "TotalGanancia", new BsonDocument("$sum", "$Ganancia") }
            }
        )
    };

            var result = await _collection
                .Aggregate<BsonDocument>(pipeline)
                .FirstOrDefaultAsync();

            return result?["TotalGanancia"].ToDouble() ?? 0;
        }


        public static async Task<double> CalcularGananciasEsperadasAsync()
        {
            var pipeline = new[]
            {
        // 1) Proyectamos la ganancia esperada: (avg(precio) – Costo) * cantidad
        new BsonDocument("$project",
            new BsonDocument("GananciaEsperada",
                new BsonDocument("$multiply", new BsonArray
                {
                    new BsonDocument("$subtract", new BsonArray
                    {
                        // Promedio de todos los precios del array
                        new BsonDocument("$avg", "$precio"),
                        // Si Costo no es numérico en BD, usa: new BsonDocument("$toDouble", "$Costo")
                        "$Costo"
                    }),
                    // Multiplicamos por la cantidad
                    "$cantidad"
                })
            )
        ),
        // 2) Agrupamos y sumamos todas las ganancias esperadas
        new BsonDocument("$group",
            new BsonDocument
            {
                { "_id", BsonNull.Value },
                { "TotalGananciaEsperada", new BsonDocument("$sum", "$GananciaEsperada") }
            }
        )
    };

            var result = await _collection
                .Aggregate<BsonDocument>(pipeline)
                .FirstOrDefaultAsync();

            return result?["TotalGananciaEsperada"].ToDouble() ?? 0;
        }


        public static async Task<double> CalcularTotalProductosVendidosAsync()
        {
            var pipeline = new[]
            {
                new BsonDocument("$group",
                  new BsonDocument
                    { { "_id", BsonNull.Value }, { "TotalVendidos", new BsonDocument("$sum","$vendido") } })
            };
            var result = await _collection.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
            return result?["TotalVendidos"].ToDouble() ?? 0;
        }

        #endregion

        #region Estadísticas simples

        public Dictionary<string, double> VendidosPorProductos(int limite = 5)
        {
            return _collection.Find(Builders<Productos>.Filter.Empty)
                .SortByDescending(p => p.Vendido)
                .Limit(limite)
                .ToList()
                .ToDictionary(p => p.Nombre, p => p.Vendido);
        }

        public Dictionary<string, double> BajaCantidadPorProductos(int limite = 5)
        {
            return _collection.Find(Builders<Productos>.Filter.Empty)
                .SortBy(p => p.Cantidad)
                .Limit(limite)
                .ToList()
                .ToDictionary(p => p.Nombre, p => p.Cantidad);
        }

        public Dictionary<string, double> VendidosPorCategoria(int limite = 5)
        {
            return _collection.Find(Builders<Productos>.Filter.Empty)
                .SortByDescending(p => p.Vendido)
                .Limit(limite)
                .ToList()
                .GroupBy(p => p.Categoria)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.Vendido));
        }

        #endregion

        #region Importación / Exportación Excel

        public List<Productos> LeerProductosDesdeExcel(string path)
        {
            var productos = new List<Productos>();
            using var wb = new XLWorkbook(path);
            var ws = wb.Worksheet(1);
            foreach (var row in ws.RangeUsed().RowsUsed().Skip(1))
            {
                var prod = new Productos
                {
                    Id = row.Cell(1).GetString(),
                    Nombre = row.Cell(2).GetString(),
                    Descripcion = row.Cell(3).GetString(),
                    Categoria = row.Cell(4).GetString(),
                    Marca = row.Cell(5).GetString(),
                    Precio = new List<double>
                    {
                        row.Cell(6).GetDouble(), row.Cell(7).GetDouble(),
                        row.Cell(8).GetDouble(), row.Cell(9).GetDouble()
                    },
                    Costo = row.Cell(10).GetDouble(),
                    Cantidad = row.Cell(11).GetDouble(),
                    Vendido = row.Cell(12).GetDouble(),
                    Descuento = row.Cell(13).GetDouble(),
                    FechaDeEntrada = DateTime.Now
                };
                productos.Add(prod);
            }
            return productos;
        }

        public async Task CargarProductosEnMongoDBAsync(string excelPath)
        {
            var list = LeerProductosDesdeExcel(excelPath);
            if (list == null || !list.Any()) return;
            await _collection.InsertManyAsync(list);
        }

        public static async Task ExportarProductosAExcelAsync(string rutaArchivo)
        {
            var client = new MongoClient(new OneKeys().URI);
            var col = client.GetDatabase("Ferreteria").GetCollection<Productos>("Productos");
            var all = await col.Find(Builders<Productos>.Filter.Empty).ToListAsync();

            using var wb = new XLWorkbook();
            var ws = wb.AddWorksheet("Productos");
            var headers = new[] { "ID", "Nombre", "Descripción", "Categoría", "Marca", "Precio1", "Precio2", "Precio3", "Precio4", "Costo", "Cantidad", "Vendido", "Descuento", "FechaEntrada" };
            for (int i = 0; i < headers.Length; i++) ws.Cell(1, i + 1).Value = headers[i];
            for (int r = 0; r < all.Count; r++)
            {
                var p = all[r];
                ws.Cell(r + 2, 1).Value = p.Id;
                ws.Cell(r + 2, 2).Value = p.Nombre;
                ws.Cell(r + 2, 3).Value = p.Descripcion;
                ws.Cell(r + 2, 4).Value = p.Categoria;
                ws.Cell(r + 2, 5).Value = p.Marca;
                for (int i = 0; i < 4; i++) ws.Cell(r + 2, 6 + i).Value = p.Precio.ElementAtOrDefault(i);
                ws.Cell(r + 2, 10).Value = p.Costo;
                ws.Cell(r + 2, 11).Value = p.Cantidad;
                ws.Cell(r + 2, 12).Value = p.Vendido;
                ws.Cell(r + 2, 13).Value = p.Descuento;
                ws.Cell(r + 2, 14).Value = p.FechaDeEntrada.ToString("yyyy-MM-dd");
            }
            wb.SaveAs(rutaArchivo);
        }

        #endregion

        #region Generación de ID

        private string GenerarId()
        {
            var last = _collection.AsQueryable()
                .OrderByDescending(p => p.Id)
                .Select(p => p.Id)
                .FirstOrDefault();
            if (last == null || !int.TryParse(last, out int lastNum))
                return "1";
            var next = (lastNum + 1).ToString();
            while (_collection.Find(p => p.Id == next).Any())
                next = (int.Parse(next) + 1).ToString();
            return next;
        }

        #endregion

        #region Helper no implementado

        internal object ObtenerEstadisticas(FilterDefinition<Productos> filtro)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}