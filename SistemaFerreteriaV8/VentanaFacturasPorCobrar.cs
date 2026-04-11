using Microsoft.VisualBasic;
using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaFerreteriaV8
{
    public partial class VentanaFacturasPorCobrar : Form
    {
        Factura Factura = null;
        private readonly Label _lblContexto = new() { AutoSize = true };

        public VentanaFacturasPorCobrar()
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
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimizeBox = false;
            Text = "Facturas por Cobrar";

            UiConsistencia.AplicarGrid(ListaFacturas);
            ListaFacturas.BorderStyle = BorderStyle.FixedSingle;
            ListaFacturas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ListaFacturas.RowHeadersVisible = false;
            ListaFacturas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ListaFacturas.MultiSelect = false;
            ListaFacturas.ColumnHeadersHeight = 34;
            ListaFacturas.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            Column1.FillWeight = 12;
            Column2.FillWeight = 46;
            Column3.FillWeight = 20;
            Column4.FillWeight = 22;

            UiConsistencia.AplicarBotonPrimario(button1); // Abrir factura
            UiConsistencia.AplicarBotonAccion(button3);   // Buscar por ID
            UiConsistencia.AplicarBotonAccion(button4);   // Reimprimir
            UiConsistencia.AplicarBotonPeligro(button2);  // Cancelar

            button1.Text = "Abrir factura";
            button3.Text = "Buscar por ID";
            button4.Text = "Reimprimir";
            button2.Text = "Cancelar";
            button1.Width = button2.Width = button3.Width = button4.Width = 170;
            button1.Height = button2.Height = button3.Height = button4.Height = 44;

            label1.Font = new Font("Segoe UI", 17f, FontStyle.Bold);
            label1.Text = "Facturas pendientes de cobro";
            label1.ForeColor = Color.White;

            _lblContexto.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            _lblContexto.ForeColor = Color.FromArgb(191, 219, 254);
            _lblContexto.Text = "Seleccione una factura y ábrala para continuar en caja.";
            if (!Controls.Contains(_lblContexto))
            {
                Controls.Add(_lblContexto);
                _lblContexto.BringToFront();
            }

            BackColor = UiConsistencia.FondoPrincipal;
            AcceptButton = button1;
            CancelButton = button2;
            ReorganizarLayout();
        }

        private void ReorganizarLayout()
        {
            const int margen = 18;
            const int headerHeight = 82;
            const int footerHeight = 74;

            label1.Left = margen;
            label1.Top = margen;
            _lblContexto.Left = margen;
            _lblContexto.Top = label1.Bottom + 4;

            ListaFacturas.Location = new Point(margen, headerHeight + margen);
            ListaFacturas.Size = new Size(ClientSize.Width - (margen * 2), ClientSize.Height - headerHeight - footerHeight - (margen * 2));

            var yBotones = ListaFacturas.Bottom + 14;
            var espacio = 12;
            var ancho = (ListaFacturas.Width - (espacio * 3)) / 4;
            var x = ListaFacturas.Left;
            foreach (var btn in new[] { button1, button3, button4, button2 })
            {
                btn.SetBounds(x, yBotones, ancho, 44);
                x += ancho + espacio;
            }
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
                    _lblContexto.Text = "No hay facturas pendientes por cobrar en este momento.";
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

                _lblContexto.Text = $"Facturas pendientes cargadas: {facturas.Count}. Doble clic o use 'Abrir factura'.";
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

                var frm = WinFormsApp.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
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

            var frm = WinFormsApp.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
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

                var frm = WinFormsApp.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
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
