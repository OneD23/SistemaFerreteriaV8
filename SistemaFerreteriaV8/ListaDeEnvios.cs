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

        private async void ListaDeEnvios_Load(object sender, EventArgs e)
        {
            try
            {
                // Obtener facturas que deben ser enviadas (async)
                var listaEnvios = await Factura.ListarFacturasAsync("enviar", "true");

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
                            item.Fecha.ToString("dd/MM/yyyy")
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocurrió un error al cargar la lista de envíos: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void button2_Click(object sender, EventArgs e) { /* Implementar si se requiere */ }
        private void button3_Click(object sender, EventArgs e) { /* Implementar si se requiere */ }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (ListaEnvios.CurrentRow != null && ListaEnvios.CurrentRow.Cells[0].Value != null)
            {
                try
                {
                    var idValue = ListaEnvios.CurrentRow.Cells[0].Value.ToString();
                    // Si tu ID es string/cadena cambia esta línea, si es int, déjala igual.
                    var factura = await Factura.BuscarAsync(int.Parse(idValue));

                    if (factura == null)
                    {
                        MessageBox.Show("Factura no encontrada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var dialogResult = MessageBox.Show(
                        "Estás registrando la entrega de esta factura. ¿Es correcto?",
                        "Aviso",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (dialogResult == DialogResult.Yes)
                    {
                        factura.Estado = "Entregada";
                        await factura.ActualizarFacturaAsync();

                        MessageBox.Show(
                            "La factura fue registrada como entregada a su destino.",
                            "Realizado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Asterisk
                        );

                        // Actualizar VentanaVentas si está abierta
                        var frm = Application.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
                        if (frm != null)
                        {
                            await frm.CargarFacturaAsync(factura);
                        }

                        this.Dispose();
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
                }
                catch (Exception ex)
                {
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

        private async void ListaEnvios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ejemplo de abrir factura al hacer click en celda (descomentar y adaptar si deseas esta función)
            /*
            var id = ListaEnvios[0, e.RowIndex].Value?.ToString();
            if (!string.IsNullOrWhiteSpace(id))
            {
                var factura = await Factura.BuscarAsync(id);
                var frm = Application.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
                if (frm != null)
                {
                    await frm.CargarFacturaAsync(factura);
                }
                this.Dispose();
            }
            */
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
