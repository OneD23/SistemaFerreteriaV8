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
            SistemaFerreteriaV8.Clases.ThemeManager.ApplyToForm(this);
            AutoScroll = true;
            MinimumSize = new Size(760, 430);
            ModernizarUI();
            Resize += (_, __) => ReorganizarLayout();
        }

        private void ModernizarUI()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            ListaFacturas.BorderStyle = BorderStyle.None;
            ListaFacturas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ListaFacturas.RowHeadersVisible = false;
            ListaFacturas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ListaFacturas.MultiSelect = false;
            ListaFacturas.EnableHeadersVisualStyles = false;
            ListaFacturas.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(24, 36, 60);
            ListaFacturas.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            ListaFacturas.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(64, 85, 122);
            ListaFacturas.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            ListaFacturas.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(46, 74, 125);
            ListaFacturas.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            foreach (var btn in new[] { button1, button2, button3, button4 })
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Height = 42;
            }

            button1.Text = "Abrir factura";
            button3.Text = "Buscar por ID";
            button4.Text = "Reimprimir";

            ReorganizarLayout();
        }

        private void ReorganizarLayout()
        {
            const int margen = 18;
            label1.Left = (ClientSize.Width - label1.Width) / 2;
            label1.Top = 22;

            ListaFacturas.Location = new System.Drawing.Point(margen, label1.Bottom + 16);
            ListaFacturas.Size = new System.Drawing.Size(ClientSize.Width - margen * 2, ClientSize.Height - 170);

            int yBotones = ListaFacturas.Bottom + 14;
            int espacio = 12;
            int ancho = (ListaFacturas.Width - (espacio * 3)) / 4;
            int x = ListaFacturas.Left;

            foreach (var btn in new[] { button1, button3, button4, button2 })
            {
                btn.SetBounds(x, yBotones, ancho, 42);
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
