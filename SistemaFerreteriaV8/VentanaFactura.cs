using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaFerreteriaV8
{
    public partial class VentanaFactura : Form
    {
        public Factura Factura { get; set; }

        public VentanaFactura()
        {
            InitializeComponent();
        }

        private void VentanaFactura_Load(object sender, EventArgs e)
        {
            if (Factura != null)
            {
                Configuraciones config = new Configuraciones().ObtenerPorId(1);

                Titulo.Text = config?.Nombre ?? "";
                DireccionNegocio.Text = config?.Direccion ?? "";
                tel.Text = config?.Telefono ?? "";

                string serie = "B02";
                if (Factura.TipoFactura == "Comprobante Fiscal")
                    serie = "B01";
                else if (Factura.TipoFactura == "Comprobante Gubernamental")
                    serie = "B15";

                IdFactura.Text = Factura.Id.ToString() ?? "";
                NFC.Text = serie + (Factura.NFC ?? "");
                Valido.Text = config?.FechaExpiracion.ToString() ?? "";
                RNC.Text = config?.RNC ?? "";

                TipoFactura.Text = Factura.TipoFactura ?? "";
                RNCCliente.Text = Factura.RNC ?? "";
                Cliente.Text = Factura.NombreCliente ?? "";
                Direccion.Text = Factura.Direccion ?? "";
                Fecha.Text = Factura.Fecha != null ? Factura.Fecha.ToString() : "";

                if (Factura.Productos != null)
                {
                    foreach (var item in Factura.Productos)
                    {
                        dataGridView1.Rows.Add(item.Cantidad, item.Producto?.Nombre, item.Precio, item.Cantidad * item.Precio);
                    }
                }
                total.Text = Factura.Total.ToString("c2");
            }
        }

        private async void Eliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro que desea eliminar esta factura?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
              await Factura.EliminarFacturaAsync();
            }
        }

        /// <summary>
        /// Actualiza una factura con los datos del grid, solo si ya existe.
        /// </summary>
        public async void RegistrarFactura()
        {
            if (string.IsNullOrWhiteSpace(IdFactura.Text))
                return;

            List<ListProduct> listaProducto = new List<ListProduct>();

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Cells[0]?.Value == null || item.Cells[1]?.Value == null || item.Cells[2]?.Value == null)
                    continue;

                string nombre = item.Cells[1].Value?.ToString() ?? "0";
                string cantidadStr = item.Cells[0].Value?.ToString() ?? "0";
                string precioStr = item.Cells[2].Value?.ToString() ?? "0";

                if (!double.TryParse(cantidadStr, out double cantidad) ||
                    !double.TryParse(precioStr, out double precio))
                    continue;

                Productos productoActual = new Productos().Buscar("nombre", nombre);

                var productos = new ListProduct()
                {
                    Producto = productoActual,
                    Cantidad = cantidad,
                    Precio = precio
                };

                if (productoActual != null)
                {
                    productoActual.Vendido += cantidad;
                    productoActual.ActualizarProductos();
                }
                listaProducto.Add(productos);
            }

            // Busca la factura por ID y la actualiza
            var factura = await Factura.BuscarAsync(int.Parse(IdFactura.Text));

            if (factura != null)
            {
                factura.NombreCliente = Cliente.Text;
                var caja = await Caja.BuscarPorClaveAsync("estado", "true");
                factura.NombreEmpresa = caja?.Id ?? "";
                // factura.IdCliente = ... // si necesitas usarlo
                // factura.Fecha = ... // si necesitas actualizar la fecha
                factura.Productos = listaProducto;
                if (double.TryParse(total.Text, out double totalValue))
                    factura.Total = totalValue;
                // factura.Descuentos = ... // si usas descuentos
                // factura.Description = ... // si tienes descripción
                // factura.Direccion = ... // si tienes dirección
                // factura.MetodoDePago = ... // si necesitas actualizar método
                // factura.Paga = ... // si es necesario
                // factura.Enviar = ... // si es necesario
                // factura.tipoFactura = ... // si es necesario

                await factura.ActualizarFacturaAsync();
                MessageBox.Show("Factura actualizada correctamente.", "Actualización", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se encontró la factura para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Actualizar_Click(object sender, EventArgs e)
        {
            RegistrarFactura();
        }

        private void IdFactura_TextChanged(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            // Genera un conduce (reporte de entrega) en PDF
            List<ListProduct> listaProducto = new List<ListProduct>();

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Cells[0]?.Value == null || item.Cells[1]?.Value == null || item.Cells[2]?.Value == null)
                    continue;

                string nombre = item.Cells[1].Value?.ToString() ?? "0";
                string cantidadStr = item.Cells[0].Value?.ToString() ?? "0";
                string precioStr = item.Cells[2].Value?.ToString() ?? "0";

                if (!double.TryParse(cantidadStr, out double cantidad) ||
                    !double.TryParse(precioStr, out double precio))
                    continue;

                var productos = new ListProduct()
                {
                    Producto = new Productos() { Nombre = nombre },
                    Cantidad = cantidad,
                    Precio = precio
                };

                listaProducto.Add(productos);
            }
            var n = new Reportes() { FacturaActiva = Factura, Productos = listaProducto };
            n.GenerarConducePDF();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (Factura.TipoFactura)
            {
                case "Consumo":
                    Factura.GenerarFacturaAsync();
                    break;
                case "Comprobante Fiscal":
                    Factura.GenerarFacturaComprobante();
                    break;
                case "Comprobante Gubernamental":
                    Factura.GenerarFacturaGubernamental();
                    break;
                default:
                    Factura.GenerarFactura1();
                    break;
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double totalActivo1 = 0;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item?.Cells[0]?.Value == null || item.Cells[2]?.Value == null)
                    continue;

                if (double.TryParse(item.Cells[0].Value.ToString(), out double cantidad) &&
                    double.TryParse(item.Cells[2].Value.ToString(), out double precio))
                {
                    item.Cells[3].Value = cantidad * precio;
                    totalActivo1 += cantidad * precio;
                }
            }
            total.Text = totalActivo1.ToString("c2");
        }
    }
}
