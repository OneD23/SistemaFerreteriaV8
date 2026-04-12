using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaFerreteriaV8
{
    public partial class ListaDeEnvios : Form
    {
        private readonly Label _lblContexto = new() { AutoSize = true };

        public ListaDeEnvios()
        {
            InitializeComponent();
            SistemaFerreteriaV8.Clases.ThemeManager.ApplyToForm(this);
            AutoScroll = true;
            MinimumSize = new Size(760, 430);
            ModernizarUI();
            Resize += (_, __) => ReorganizarLayout();
        }

        private void ModernizarUI()
        {
            UiConsistencia.AplicarFormularioBase(this);
            Text = "Lista de Envíos";

            UiConsistencia.AplicarGrid(ListaEnvios);
            ListaEnvios.BorderStyle = BorderStyle.FixedSingle;
            ListaEnvios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ListaEnvios.RowHeadersVisible = false;
            ListaEnvios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ListaEnvios.MultiSelect = false;
            ListaEnvios.ColumnHeadersHeight = 34;
            Column1.FillWeight = 10;
            Column2.FillWeight = 30;
            Column3.FillWeight = 16;
            Column4.FillWeight = 28;
            Column5.FillWeight = 16;

            button4.Text = "Registrar entrega";
            button5.Text = "Cerrar";
            UiConsistencia.AplicarBotonPrimario(button4);
            UiConsistencia.AplicarBotonAccion(button5);
            button4.Width = button5.Width = 200;
            button4.Height = button5.Height = 44;

            label1.Font = new Font("Segoe UI", 17f, FontStyle.Bold);
            label1.Text = "Entregas pendientes";
            label1.ForeColor = Color.White;

            _lblContexto.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            _lblContexto.ForeColor = Color.FromArgb(191, 219, 254);
            _lblContexto.Text = "Seleccione una factura para registrar su entrega.";
            if (!Controls.Contains(_lblContexto))
            {
                Controls.Add(_lblContexto);
                _lblContexto.BringToFront();
            }

            AcceptButton = button4;
            CancelButton = button5;

            ReorganizarLayout();
        }

        private void ReorganizarLayout()
        {
            const int margen = 16;
            const int headerHeight = 84;
            const int footerHeight = 74;
            label1.Left = margen;
            label1.Top = margen;
            _lblContexto.Left = margen;
            _lblContexto.Top = label1.Bottom + 4;

            ListaEnvios.Location = new Point(margen, headerHeight + margen);
            ListaEnvios.Size = new Size(ClientSize.Width - (margen * 2), ClientSize.Height - headerHeight - footerHeight - (margen * 2));

            int y = ListaEnvios.Bottom + 14;
            int anchoBtn = 200;
            button5.SetBounds(ClientSize.Width - margen - anchoBtn, y, anchoBtn, 44);
            button4.SetBounds(button5.Left - 12 - anchoBtn, y, anchoBtn, 44);
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

                _lblContexto.Text = $"Pendientes para enviar: {ListaEnvios.Rows.Count}.";
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
                        var frm = WinFormsApp.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
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
                var frm = WinFormsApp.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
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
