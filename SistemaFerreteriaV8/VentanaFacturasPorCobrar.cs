using Microsoft.VisualBasic;
using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        private  void VentanaFacturasPorCobrar_Load(object sender, EventArgs e)
        {
            try
            {
                // Obtener las facturas no pagadas (con paginación)
                List<Factura> facturas =  Factura.ListarUltimasFacturas();

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
                        factura.Fecha.ToString("dd/MM/yyyy"), // Formato de fecha
                        factura.Total.ToString("C2") // Formato de moneda
                    );
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show($"Ocurrió un error al cargar las facturas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private  void ListaFacturas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }


        private  void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int id = 0;

                id = id != null ? int.Parse(Interaction.InputBox("Favor digitar la factura que desea buscar para editar")) : 0;
                Factura = Factura.Buscar(int.Parse(id.ToString()));

                VentanaVentas frm = (VentanaVentas)Application.OpenForms["VentanaVentas"];
                if (Application.OpenForms.OfType<VentanaVentas>().Any())
                {
                    frm.CargarFactura(this.Factura);
                    frm.esCargada = true;
                }
                this.Dispose();

            }
            catch (Exception)
            {

                MessageBox.Show("Favor revisar el codigo de la factura");
            }
            
        }

        private  void button1_Click(object sender, EventArgs e)
        {
            if (ListaFacturas?.CurrentRow == null)
            {
                MessageBox.Show("No hay filas seleccionadas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var idValue = ListaFacturas[0, ListaFacturas.CurrentRow.Index].Value;
            int id = idValue != null ? int.Parse(idValue.ToString()) : 0;

            Factura = Factura.Buscar(id);

            var frm = Application.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
            if (frm != null)
            {
                frm.CargarFactura(Factura);
                frm.esCargada = true;
            }

            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ListaFacturas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Validar índice y valor de la celda
                if (e.RowIndex < 0 || ListaFacturas[0, e.RowIndex]?.Value == null ||
                    string.IsNullOrWhiteSpace(ListaFacturas[0, e.RowIndex].Value.ToString()))
                {
                    MessageBox.Show("Seleccione una factura válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el ID de la factura
                int id = int.Parse(ListaFacturas[0, e.RowIndex].Value.ToString());

                // Buscar la factura de manera asincrónica
                Factura factura = Factura.Buscar(id);
                if (factura == null)
                {
                    MessageBox.Show("La factura no fue encontrada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Verificar si la ventana VentanaVentas está abierta
                var frm = Application.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
                if (frm != null)
                {
                    frm.CargarFactura(factura);
                    frm.esCargada = true;
                }
                else
                {
                    MessageBox.Show("La ventana VentanaVentas no está abierta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Cerrar la ventana actual
                this.Dispose();
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ListaFacturas.CurrentRow.Cells[0] != null)
            {
                string id = ListaFacturas.CurrentRow.Cells[0].Value.ToString();
                if (!string.IsNullOrEmpty(id))
                {
                    Factura factura = Factura.Buscar(int.Parse(id));
                    if (factura != null)
                    {
                        if (factura.tipoFactura == "Comprobante Gubernamental")
                        {
                            factura.GenerarFacturaGubernamental();
                        }
                        else if (factura.tipoFactura == "Comprobante Fiscal")
                        {
                            factura.GenerarFacturaComprobante();
                        }
                        else if (factura.tipoFactura == "Consumo")
                        {
                            factura.GenerarFactura();
                        }
                        else
                        {
                            factura.GenerarFactura1();
                        }
                    }
                }
            }  
        }
    }
}
