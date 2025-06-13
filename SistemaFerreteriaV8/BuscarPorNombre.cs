using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using MongoDB.Bson;
using MongoDB.Driver;
using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaFerreteriaV8
{
    public partial class BuscarPorNombre : Form
    {
        int precio = new Configuraciones().ObtenerPorId(1).Precio;
        public List<Productos> listaProductos {  get; set; }
        private VentanaVentas frm;

        public VentanaVentas ReferenciaVentanaVentas
        {
            get
            {
                if (frm == null || frm.IsDisposed)
                {
                    frm = (VentanaVentas)Application.OpenForms["VentanaVentas"];
                }
                return frm;
            }
        }
        public BuscarPorNombre()
        {
            InitializeComponent();
        }

        private void BuscarPorNombre_Load(object sender, EventArgs e)
        {
            // Limpiar la tabla antes de llenarla
            ListaProductos.Rows.Clear();

            // Convertir el texto de búsqueda a minúsculas
            string textoBusqueda = "Cem";

                // Configurar el cliente de MongoDB

                var collection = new MongoClient(new OneKeys().URI).GetDatabase("Ferreteria").GetCollection<Productos>("Productos");

                // Crear un filtro para buscar nombres que contengan el texto ingresado, ignorando "Generico"
                var filtro = Builders<Productos>.Filter.And(
                    Builders<Productos>.Filter.Regex("Nombre", new BsonRegularExpression(textoBusqueda, "i")), // "i" -> Insensible a mayúsculas/minúsculas
                    Builders<Productos>.Filter.Ne("Nombre", "Generico") // Excluir "Generico"
                );

                // Definir una proyección para recuperar solo los campos necesarios
                var proyeccion = Builders<Productos>.Projection
                    .Include("nombre")
                    .Include("descripcion")
                    .Include("marca")
                    .Include("precio")
                    .Include("_id"); // Excluir el campo "_id" si no es necesario

                // Consultar MongoDB con filtro y proyección
                var productos = collection.Find(filtro).Project(proyeccion).ToList();

                // Rellenar la tabla con los resultados
                foreach (var producto in productos)
                {
                    ListaProductos.Rows.Add(
                        producto.GetValue("_id", ""),
                        producto.GetValue("nombre", ""),
                        producto.GetValue("descripcion", ""),
                        producto.GetValue("marca", ""),
                        producto.GetValue("precio", 0)[0]
                    );
                }
            
        }



        private void ListaProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ReferenciaVentanaVentas != null)
            {
                if (ListaProductos.CurrentRow?.Cells[0]?.Value != null)
                {
                    var codigo = ListaProductos.CurrentRow.Cells[0].Value.ToString();

                    if (!string.IsNullOrEmpty(codigo))
                    {
                        Task.Run(() =>
                        {
                            // Operaciones en segundo plano
                            ReferenciaVentanaVentas.codigoProducto = codigo;
                            ReferenciaVentanaVentas.DetectaProductoAsync();
                            ReferenciaVentanaVentas.AsignarTotales();
                        }).ContinueWith(task =>
                        {
                            // Operaciones en el hilo principal
                            if (!task.IsFaulted) // Asegurarse de que no haya errores en el Task
                            {
                                this.Hide();
                            }
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Limpiar la tabla antes de llenarla
            ListaProductos.Rows.Clear();

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                // Convertir el texto de búsqueda a minúsculas
                string textoBusqueda = textBox1.Text.ToLower();

                // Configurar el cliente de MongoDB
               
                var collection = new MongoClient(new OneKeys().URI).GetDatabase("Ferreteria").GetCollection<Productos>("Productos");

                // Crear un filtro para buscar nombres que contengan el texto ingresado, ignorando "Generico"
                var filtro = Builders<Productos>.Filter.And(
                    Builders<Productos>.Filter.Regex("Nombre", new BsonRegularExpression(textoBusqueda, "i")), // "i" -> Insensible a mayúsculas/minúsculas
                    Builders<Productos>.Filter.Ne("Nombre", "Generico") // Excluir "Generico"
                );

                // Definir una proyección para recuperar solo los campos necesarios
                var proyeccion = Builders<Productos>.Projection
                    .Include("nombre")
                    .Include("descripcion")
                    .Include("marca")
                    .Include("precio")
                    .Include("_id"); // Excluir el campo "_id" si no es necesario

                // Consultar MongoDB con filtro y proyección
                var productos = collection.Find(filtro).Project(proyeccion).ToList();

                // Rellenar la tabla con los resultados
                foreach (var producto in productos)
                {
                    ListaProductos.Rows.Add(
                        producto.GetValue("_id", ""),
                        producto.GetValue("nombre", ""),
                        producto.GetValue("descripcion", ""),
                        producto.GetValue("marca", ""),
                        producto.GetValue("precio", 0)
                    );
                }
            }
        }

        private void ListaProductos_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Up)
            {
                // Verifica si hay una fila seleccionada y no es la primera
                if (ListaProductos.SelectedRows.Count > 0 && ListaProductos.SelectedRows[0].Index > 0)
                {
                    // Reduce el índice de la fila seleccionada
                    int newIndex = ListaProductos.SelectedRows[0].Index - 1;
                    ListaProductos.Rows[newIndex].Selected = true;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                // Verifica si hay una fila seleccionada y no es la última
                if (ListaProductos.SelectedRows.Count > 0 && ListaProductos.SelectedRows[0].Index < ListaProductos.Rows.Count - 1)
                {
                    // Incrementa el índice de la fila seleccionada
                    int newIndex = ListaProductos.SelectedRows[0].Index + 1;
                    ListaProductos.Rows[newIndex].Selected = true;
                }
            }

        }

        private void BuscarPorNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                MoverDataGridView(-1); // Mover hacia arriba
            }
            else if (e.KeyCode == Keys.Down)
            {
                MoverDataGridView(1); // Mover hacia abajo
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            }
        private void MoverDataGridView(int offset)
        {
            ListaProductos.Focus(); // Asegurarse de que el DataGridView tenga el foco

            int selectedIndex = -1; // Inicializar el índice de la fila seleccionada
            if (ListaProductos.SelectedRows.Count > 0)
            {
                selectedIndex = ListaProductos.SelectedRows[0].Index; // Obtener el índice de la fila seleccionada si hay alguna
            }

            int newIndex = selectedIndex + offset;

            // Verificar si el nuevo índice está dentro del rango válido
            if (newIndex >= 0 && newIndex < ListaProductos.Rows.Count)
            {
                ListaProductos.ClearSelection(); // Desseleccionar todas las filas
                ListaProductos.Rows[newIndex].Selected = true; // Seleccionar la nueva fila
                ListaProductos.FirstDisplayedScrollingRowIndex = newIndex; // Hacer que la fila sea visible
            }
        }


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
