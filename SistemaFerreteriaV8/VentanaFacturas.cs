using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SistemaFerreteriaV8
{
    public partial class VentanaFacturas : Form
    {
        private ObjectId? lastId = null; // Para paginación si lo usas
        private const int pageSize = 20;

        private ProgressBar progressBarLoading;

        public VentanaFacturas()
        {
            InitializeComponent();

            // Crear y configurar el ProgressBar manualmente
            progressBarLoading = new ProgressBar
            {
                Name = "progressBarLoading",
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30,
                Visible = false,
                Width = 100,   // o pon Location/Size a tu gusto
                Height = 20,
                Left = 508,
                Top = 85
            };

            // Añadirlo al formulario
            this.Controls.Add(progressBarLoading);
            // Opcional: para que quede delante de otros controles
            this.Controls.SetChildIndex(progressBarLoading, 0);
        }

        private async void VentanaFacturas_Load(object sender, EventArgs e)
        {
            // 1) Configuro y muestro la barra de progreso
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.MarqueeAnimationSpeed = 30;
            progressBarLoading.Visible = true;

            try
            {
                Fecha1.Value = DateTime.Today.AddDays(-1);
                Fecha2.Value = DateTime.Now;

                // 2) Cargo las facturas de forma asíncrona
                var lista = await Factura.ListarFacturasPorFechaAsync(
                    DateTime.Today.AddMonths(-12),
                    DateTime.Now);

                // 3) Relleno el DataGridView
                ListaDeFacturas.Rows.Clear();
                foreach (Factura item in lista)
                {
                    ListaDeFacturas.Rows.Add(item.Id, item.Fecha);
                }
            }
            finally
            {
                // 4) Oculto la barra de progreso
                progressBarLoading.Visible = false;
            }
        }

        private async void Id_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Id.Text)) return;

            ListaDeFacturas.Rows.Clear();
            List<Factura> lista = new List<Factura>();

            if (comboBox1.Text == "Id" && int.TryParse(Id.Text, out int id))
            {
                lista = await Factura.ListarFacturasPorIdAsync(id.ToString(), 1, pageSize);
            }
            else if (comboBox1.Text == "Cliente")
            {
                lista = await Factura.ListarFacturasPorNombreAsync(Id.Text, 1, pageSize);
            }

            foreach (var item in lista)
            {
                var empleadoNombre = (await  Empleado.BuscarAsync(item.IdEmpleado))?.Nombre ?? "";
                ListaDeFacturas.Rows.Add(item.Id, item.Fecha, item.NombreCliente, item.TipoFactura,
                                         item.Description + item.Informacion, empleadoNombre, item.Total, item.Enviar, item.Paga);
            }

            CantidadFactura.Text = lista.Count.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) => Id.Text = string.Empty;

        private async void ListaDeFacturas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || ListaDeFacturas[0, e.RowIndex]?.Value == null) return;

            if (!int.TryParse(ListaDeFacturas[0, e.RowIndex].Value.ToString(), out int id)) return;

            // Buscar asíncrono
            var facturaActiva = await Factura.BuscarAsync(id);

            if (facturaActiva != null)
            {
                var factura = new VentanaFactura() { Factura = facturaActiva };
                factura.Show();
            }
            else
            {
                MessageBox.Show("La factura no fue encontrada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
