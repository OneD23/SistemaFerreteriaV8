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
        private ObjectId? lastId = null; // Para la paginación eficiente
        private const int pageSize = 20;

        public VentanaFacturas()
        {
            InitializeComponent();
           
        }

        private void VentanaFacturas_Load(object sender, EventArgs e)
        {
            Fecha1.Value = DateTime.Today.AddDays(-1);
            Fecha2.Value = DateTime.Now;
            List<Factura> lista =Factura.ListarFacturasPorFecha(DateTime.Today.AddMonths(-12),DateTime.Now);
            foreach (Factura item in lista)
            {
                ListaDeFacturas.Rows.Add(item.Id, item.Fecha
                    );
            }
        }

        
        private async void Id_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Id.Text)) return;

            ListaDeFacturas.Rows.Clear();
            List<Factura> Lista = new List<Factura>();

            if (comboBox1.Text == "Id" && int.TryParse(Id.Text, out int id))
            {
                Lista = Factura.ListarFacturasPorId(id, 1, pageSize);
            }
            else if (comboBox1.Text == "Cliente")
            {
                Lista = Factura.ListarFacturasPorNombre(Id.Text, 1, pageSize);
            }

            foreach (var item in Lista)
            {
                string empleadoNombre = new Empleado().Buscar(item.IdEmpleado)?.Nombre ?? "";
                ListaDeFacturas.Rows.Add(item.Id, item.Fecha, item.NombreCliente, item.tipoFactura,
                                         item.Description + item.Informacion, empleadoNombre, item.Total, item.Enviar, item.Paga);
            }

            CantidadFactura.Text = Lista.Count.ToString();
        }

       // private async void button1_Click(object sender, EventArgs e) => await RellenarListaAsync(Fecha1.Value, Fecha2.Value, null);
       // private async void button2_Click(object sender, EventArgs e) => await RellenarListaAsync(Fecha1.Value, Fecha2.Value, lastId);

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) => Id.Text = String.Empty;

        private async void ListaDeFacturas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || ListaDeFacturas[0, e.RowIndex]?.Value == null) return;

            if (!int.TryParse(ListaDeFacturas[0, e.RowIndex].Value.ToString(), out int id)) return;

            var facturaActiva = await Task.Run(() => Factura.Buscar(id));

            if (facturaActiva != null)
            {
                VentanaFactura factura = new VentanaFactura() { Factura = facturaActiva };
                factura.Show();
            }
            else
            {
                MessageBox.Show("La factura no fue encontrada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
