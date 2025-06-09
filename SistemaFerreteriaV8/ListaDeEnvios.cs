using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaFerreteriaV8
{
    public partial class ListaDeEnvios : Form
    {
        public ListaDeEnvios()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ListaDeEnvios_Load(object sender, EventArgs e)
        {
            try
            {
                // Obtener facturas que deben ser enviadas
                List<Factura> listaEnvios =  Factura.ListarFacturas("enviar", "true");

                foreach (var item in listaEnvios)
                {
                    // Verificar que el estado no sea "Entregada"
                    if (item.Estado != "Entregada")
                    {
                        ListaEnvios.Rows.Add(
                            item.Id,
                            item.NombreCliente,
                            item.Total,
                            item.Direccion,
                            item.Fecha.ToString("dd/MM/yyyy") // Formato de fecha
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar errores y notificar al usuario
                MessageBox.Show(
                    $"Ocurrió un error al cargar la lista de envíos: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (ListaEnvios.CurrentRow != null && ListaEnvios.CurrentRow.Cells[0].Value != null)
            {
                try
                {
                    // Obtener el ID seleccionado en la fila actual
                    var idValue = ListaEnvios.CurrentRow.Cells[0].Value.ToString();
                    int id = int.TryParse(idValue, out var parsedId) ? parsedId : 0;

                    // Verificar si el ID es válido
                    if (id == 0)
                    {
                        MessageBox.Show("ID de factura no válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Buscar la factura de manera asincrónica
                    Factura factura =  Factura.Buscar(id);

                    // Confirmar entrega
                    var dialogResult = MessageBox.Show(
                        "Estás registrando la entrega de esta factura. ¿Es correcto?",
                        "Aviso",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (dialogResult == DialogResult.Yes)
                    {
                        // Actualizar el estado de la factura
                        factura.Estado = "Entregada";
                         factura.ActualizarFactura();

                        MessageBox.Show(
                            "La factura fue registrada como entregada a su destino.",
                            "Realizado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Asterisk
                        );
                    }
                    else
                    {
                        MessageBox.Show(
                            "Se canceló la entrega.",
                            "Aviso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation
                        );
                    }

                    // Actualizar VentanaVentas si está abierta
                    if (Application.OpenForms.OfType<VentanaVentas>().Any())
                    {
                        var frm = (VentanaVentas)Application.OpenForms["VentanaVentas"];
                        frm.CargarFactura(factura);
                    }

                    // Cerrar la ventana actual
                    this.Dispose();
                }
                catch (Exception ex)
                {
                    // Manejar cualquier error y mostrar un mensaje
                    MessageBox.Show(
                        $"Ocurrió un error al procesar la factura: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            else
            {
                MessageBox.Show("No se seleccionó ninguna factura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void ListaEnvios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           /* var id = ListaEnvios[0, e.RowIndex].Value;

            id = id != null ? int.Parse(id.ToString()) : 0;
            Factura factura= new Factura().Buscar(int.Parse(id.ToString()));
            VentanaVentas frm = (VentanaVentas)Application.OpenForms["VentanaVentas"];
            if (Application.OpenForms.OfType<VentanaVentas>().Any())
            {
                frm.CargarFactura(factura);
            }
            this.Dispose();*/
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
