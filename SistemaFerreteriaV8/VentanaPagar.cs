using Microsoft.VisualBasic;
using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace SistemaFerreteriaV8
{
    public partial class VentanaPagar : Form
    {
        public Factura facturaActiva { get; set; }
        public Empleado EmpleadoActivo { get; set; }
        public Cliente ClienteActivo { get; set; }
        public string metodoAntes { get; set; }

        public VentanaPagar()
        {
            InitializeComponent();
            SistemaFerreteriaV8.Clases.ThemeManager.ApplyToForm(this);
            ModernizarUI();
            Resize += (_, __) => ReorganizarLayout();
        }

        private void ModernizarUI()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            foreach (var btn in new[] { Pagar, Limpiar, Cancelar })
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Height = 44;
            }

            foreach (var ctl in new Control[] { TipoFactura, MetodoPago, subTotal, Descuento, Total, Efectivo, Devuelta })
                ctl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            ReorganizarLayout();
        }

        private void ReorganizarLayout()
        {
            int panelWidth = Math.Min(520, ClientSize.Width - 40);
            int xLabel = (ClientSize.Width - panelWidth) / 2;
            int xInput = xLabel + 150;
            int wInput = panelWidth - 160;
            int y = 56;
            int h = 30;
            int gap = 40;

            label1.Left = (ClientSize.Width - label1.Width) / 2;
            label1.Top = 20;

            ConfigCampo(label8, TipoFactura, xLabel, xInput, wInput, y, h);
            y += gap;

            label9.Location = new Point(xLabel, y + 2);
            Imprimir.Location = new Point(xInput, y + 2);
            y += gap;

            ConfigCampo(label2, subTotal, xLabel, xInput, wInput, y, h);
            y += gap;
            ConfigCampo(label3, Descuento, xLabel, xInput, wInput, y, h);
            y += gap;
            Total.Location = new Point(xInput, y);
            Total.Size = new Size(wInput, h);
            y += gap;
            ConfigCampo(label4, MetodoPago, xLabel, xInput, wInput, y, h);
            y += gap;
            ConfigCampo(label5, Efectivo, xLabel, xInput, wInput, y, h);
            y += gap;
            ConfigCampo(label6, Devuelta, xLabel, xInput, wInput, y, h);

            int yBtns = ClientSize.Height - 72;
            int btnW = 130;
            int space = 18;
            int startX = (ClientSize.Width - (btnW * 3 + space * 2)) / 2;
            Pagar.SetBounds(startX, yBtns, btnW, 44);
            Limpiar.SetBounds(startX + btnW + space, yBtns, btnW, 44);
            Cancelar.SetBounds(startX + (btnW + space) * 2, yBtns, btnW, 44);
        }

        private static void ConfigCampo(Label label, Control input, int xLabel, int xInput, int wInput, int y, int h)
        {
            label.AutoSize = false;
            label.TextAlign = ContentAlignment.MiddleRight;
            label.Location = new Point(xLabel, y);
            label.Size = new Size(140, h);
            input.Location = new Point(xInput, y);
            input.Size = new Size(wInput, h);
        }

        private void VentanaPagar_Load(object sender, EventArgs e)
        {
            if (facturaActiva != null)
            {
                metodoAntes = facturaActiva.MetodoDePago;
                Configuraciones config = new Configuraciones().ObtenerPorId(1);
                Imprimir.Checked = true;
                TipoFactura.Text = facturaActiva.TipoFactura;
                subTotal.Text = facturaActiva.Total.ToString("c2");
                Descuento.Text = facturaActiva.Descuentos.ToString("c2");
                Total.Text = (facturaActiva.Total - facturaActiva.Descuentos).ToString("c2");
                MetodoPago.SelectedIndex = 0;
            }
        }

        private void MetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            Efectivo.Enabled = MetodoPago.SelectedIndex == 0;
        }

        private void Efectivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (double.TryParse(Efectivo.Text, out double efectivo))
                {
                    double total = facturaActiva.Total - facturaActiva.Descuentos;
                    Devuelta.Text = (efectivo - total).ToString("c2");
                }
                else
                {
                    MessageBox.Show("Monto de efectivo no válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private async void Pagar_Click(object sender, EventArgs e)
        {
            Configuraciones confi = new Configuraciones().ObtenerPorId(1);
            if (confi != null)
            {
                await RealizarPagoAsync();
            }
            else MessageBox.Show("Todavia este Sistema no se ha configurado para empezar a trabajar! Dirijase a configuraciones para configurar correctamente", "ATENCION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private async Task RealizarPagoAsync()
        {
            bool pagoProcesado = false;
            if (facturaActiva == null || facturaActiva.Eliminada)
                return;

            facturaActiva.TipoFactura = TipoFactura.Text;
            facturaActiva.MetodoDePago = MetodoPago.Text;

            switch (MetodoPago.Text)
            {
                case "Efectivo":
                    facturaActiva.Paga = true;
                    facturaActiva.MetodoDePago = "Efectivo";
                    facturaActiva.Fecha = DateTime.Now;

                    if (!string.IsNullOrEmpty(Efectivo.Text) && double.TryParse(Efectivo.Text, out double montoEfectivo))
                    {
                        facturaActiva.Efectivo = montoEfectivo;
                    }

                    Cliente clienteCredito = null;
                    if (!string.IsNullOrWhiteSpace(facturaActiva.IdCliente) && facturaActiva.IdCliente != "0")
                        clienteCredito = await new Cliente().BuscarAsync(facturaActiva.IdCliente);

                    if (clienteCredito == null)
                        clienteCredito = ClienteActivo;

                    if (clienteCredito != null && clienteCredito.CreditosActivo != null && clienteCredito.CreditosActivo.Exists(m => m.Id == facturaActiva.Id))
                    {
                        clienteCredito.CreditosActivo.RemoveAll(m => m.Id == facturaActiva.Id);
                        await clienteCredito.EditarAsync();
                    }
                    await facturaActiva.ActualizarFacturaAsync();
                    pagoProcesado = true;
                    break;

                case "Credito":
                    facturaActiva.Paga = false;
                    Cliente cliente = null;

                    if (!string.IsNullOrEmpty(facturaActiva.IdCliente) && facturaActiva.IdCliente != "0")
                    {
                        cliente = await new Cliente().BuscarAsync(facturaActiva.IdCliente);
                    }
                    else
                    {
                        string input = Interaction.InputBox("Digite el Nombre o Id del cliente al cual le cargará el crédito:");
                        if (string.IsNullOrWhiteSpace(input))
                        {
                            MessageBox.Show("Entrada vacía. Proceso cancelado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        cliente = int.TryParse(input, out _) ?
                            await new Cliente().BuscarAsync(input) :
                            await new Cliente().BuscarPorClaveAsync("nombre", input);
                    }

                    if (cliente != null)
                    {
                        if (MessageBox.Show($"¿Está seguro que quiere cargar el monto de {facturaActiva.Total:C2} a la cuenta de:\n\n" +
                            $"Id: {cliente.Id}\n" +
                            $"Nombre: {cliente.Nombre}\n" +
                            $"Dirección: {cliente.Direccion}\n",
                            "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            double creditoActivo = cliente.CreditosActivo?.Sum(f => f.Total) ?? 0;

                            if (creditoActivo + facturaActiva.Total > cliente.LimiteCredito)
                            {
                                MessageBox.Show(
                                    $"El cliente tiene un crédito activo de {creditoActivo:C2}. " +
                                    $"Si le suma los {facturaActiva.Total:C2} de la factura, superará el crédito permitido: {cliente.LimiteCredito:C2}",
                                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                if (cliente.CreditosActivo == null)
                                    cliente.CreditosActivo = new List<Factura>();

                                if (!cliente.CreditosActivo.Any(f => f.Id == facturaActiva.Id))
                                    cliente.CreditosActivo.Add(facturaActiva);
                                await cliente.EditarAsync();

                                facturaActiva.Paga = false;
                                await facturaActiva.ActualizarFacturaAsync();
                                pagoProcesado = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Se canceló el proceso de pago", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se pudo encontrar el cliente. Revise los datos ingresados e intente de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                default:
                    MessageBox.Show("Seleccione un método de pago válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }

            if (pagoProcesado)
            {
                // Impresión y cierre de ventana
                switch (facturaActiva.TipoFactura)
                {
                    case "Consumo":
                        facturaActiva.GenerarFacturaAsync();
                        break;
                    case "Comprobante Fiscal":
                        facturaActiva.GenerarFacturaComprobante();
                        break;
                    case "Comprobante Gubernamental":
                        facturaActiva.GenerarFacturaGubernamental();
                        break;
                    default:
                        facturaActiva.GenerarFactura1();
                        break;
                }
                await facturaActiva.RegistrarProductosAsync(1);

                var frm = Application.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
                frm?.LimpiarTodo();

                this.Dispose();
            }
        }

        private void MostrarMensaje(string mensaje, string titulo, MessageBoxIcon icono)
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, icono);
        }

        private void Limpiar_Click(object sender, EventArgs e)
        {
            Efectivo.Text = Descuento.Text = "";
        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private string[] BuscarPorRNC(string rnc)
        {
            if (rnc.Length > 8)
            {
                string rutaArchivo = @"rnc.txt";
                string[] datos = new string[2];

                if (File.Exists(rutaArchivo))
                {
                    string[] lineas = File.ReadAllLines(rutaArchivo);
                    foreach (string linea in lineas)
                    {
                        if (linea.Contains(rnc))
                        {
                            MessageBox.Show(linea.Replace("|", " "), "RNC encontrado!");
                            datos[0] = rnc;
                            datos[1] = linea.Split('|')[1];
                        }
                    }
                }
                else
                {
                    Console.WriteLine("El archivo no existe.");
                }
                return datos;
            }
            else
            {
                return null;
            }
        }
    }
}
