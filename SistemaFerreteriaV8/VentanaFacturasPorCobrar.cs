using Microsoft.VisualBasic;
using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaFerreteriaV8
{
    public partial class VentanaFacturasPorCobrar : Form
    {
        Factura Factura = null;

        public VentanaFacturasPorCobrar()
        {
            InitializeComponent();
        }

        private async void VentanaFacturasPorCobrar_Load(object sender, EventArgs e)
        {
            try
            {
                // Obtener las facturas no pagadas (con paginación) - Async!
                List<Factura> facturas = await  Factura.ListarUltimasFacturasAsync();

                if (facturas == null || facturas.Count == 0)
                {
                    MessageBox.Show("No se encontraron facturas pendientes de cobro.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Agregar las facturas a la tabla
                foreach (Factura factura in facturas)
                {
                    ListaFacturas.Rows.Add(
                        factura.Id,
                        factura.NombreCliente,
                        factura.Fecha.ToString("dd/MM/yyyy"),
                        factura.Total.ToString("C2")
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar las facturas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListaFacturas_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var input = Interaction.InputBox("Favor digitar la factura que desea buscar para editar");
                if (string.IsNullOrWhiteSpace(input)) return;
                if (!int.TryParse(input, out int id)) return;

                Factura = await Factura.BuscarAsync(id);

                var frm = Application.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
                if (frm != null)
                {
                    await frm.CargarFacturaAsync(Factura);
                    frm.esCargada = true;
                }
                this.Dispose();
            }
            catch
            {
                MessageBox.Show("Favor revisar el código de la factura");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (ListaFacturas?.CurrentRow == null)
            {
                MessageBox.Show("No hay filas seleccionadas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var idValue = ListaFacturas[0, ListaFacturas.CurrentRow.Index].Value;
            if (idValue == null) return;
            if (!int.TryParse(idValue.ToString(), out int id)) return;

            Factura = await Factura.BuscarAsync(id);

            var frm = Application.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
            if (frm != null)
            {
                await frm.CargarFacturaAsync(Factura);
                frm.esCargada = true;
            }

            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private async void ListaFacturas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || ListaFacturas[0, e.RowIndex]?.Value == null ||
                    string.IsNullOrWhiteSpace(ListaFacturas[0, e.RowIndex].Value.ToString()))
                {
                    MessageBox.Show("Seleccione una factura válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = int.Parse(ListaFacturas[0, e.RowIndex].Value.ToString());
                Factura factura = await Factura.BuscarAsync(id);
                if (factura == null)
                {
                    MessageBox.Show("La factura no fue encontrada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var frm = Application.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
                if (frm != null)
                {
                    await frm.CargarFacturaAsync(factura);
                    frm.esCargada = true;
                }
                else
                {
                    MessageBox.Show("La ventana VentanaVentas no está abierta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (ListaFacturas.CurrentRow?.Cells[0] != null)
            {
                string idStr = ListaFacturas.CurrentRow.Cells[0].Value.ToString();
                if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
                {
                    Factura factura = await Factura.BuscarAsync(id);
                    if (factura != null)
                    {
                        switch (factura.TipoFactura)
                        {
                            case "Comprobante Gubernamental":
                                factura.GenerarFacturaGubernamental();
                                break;
                            case "Comprobante Fiscal":
                                factura.GenerarFacturaComprobante();
                                break;
                            case "Consumo":
                                factura.GenerarFacturaAsync();
                                break;
                            default:
                                factura.GenerarFactura1();
                                break;
                        }
                    }
                }
            }
        }
    }
}
