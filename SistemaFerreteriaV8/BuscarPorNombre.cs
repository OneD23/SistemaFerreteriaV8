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
            SistemaFerreteriaV8.Clases.ThemeManager.ApplyToForm(this);
            ModernizarUI();
            Resize += (_, __) => ReorganizarLayout();
        }

        private void ModernizarUI()
        {
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            MinimumSize = new Size(760, 300);

            ListaProductos.BorderStyle = BorderStyle.None;
            ListaProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ListaProductos.RowHeadersVisible = false;
            ListaProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ListaProductos.MultiSelect = false;
            ListaProductos.EnableHeadersVisualStyles = false;
            ListaProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 36, 60);
            ListaProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            ListaProductos.DefaultCellStyle.BackColor = Color.FromArgb(64, 85, 122);
            ListaProductos.DefaultCellStyle.ForeColor = Color.White;
            ListaProductos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(46, 74, 125);
            ListaProductos.DefaultCellStyle.SelectionForeColor = Color.White;

            label1.Text = "Buscar producto:";
            label1.AutoSize = true;
            label2.Visible = false;

            textBox1.Font = new Font("Segoe UI", 10F);
            textBox1.BorderStyle = BorderStyle.FixedSingle;

            ReorganizarLayout();
        }

        private void ReorganizarLayout()
        {
            const int margen = 12;
            label1.Location = new Point(margen, margen);
            textBox1.Location = new Point(label1.Right + 10, margen - 2);
            textBox1.Size = new Size(Math.Min(320, ClientSize.Width - textBox1.Left - margen), 28);

            ListaProductos.Location = new Point(margen, textBox1.Bottom + 12);
            ListaProductos.Size = new Size(ClientSize.Width - (margen * 2), ClientSize.Height - ListaProductos.Top - margen);
        }

        private void CargarProductos(string textoBusqueda)
        {
            ListaProductos.Rows.Clear();
            var collection = new MongoClient(new OneKeys().URI)
                .GetDatabase(new OneKeys().DatabaseName)
                .GetCollection<Productos>("Productos");

            var filtro = Builders<Productos>.Filter.And(
                Builders<Productos>.Filter.Regex("Nombre", new BsonRegularExpression(textoBusqueda, "i")),
                Builders<Productos>.Filter.Ne("Nombre", "Generico")
            );

            var productos = collection.Find(filtro).ToList();
            foreach (var producto in productos)
            {
                var precioVisual = (producto.Precio != null && producto.Precio.Count > 0)
                    ? producto.Precio[Math.Min(precio, producto.Precio.Count - 1)]
                    : 0;

                ListaProductos.Rows.Add(producto.Id, producto.Nombre, producto.Marca, precioVisual.ToString("C2"));
            }
        }

        private void BuscarPorNombre_Load(object sender, EventArgs e)
        {
            CargarProductos(textBox1.Text?.Trim() ?? string.Empty);
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
            CargarProductos(textBox1.Text?.Trim() ?? string.Empty);
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
