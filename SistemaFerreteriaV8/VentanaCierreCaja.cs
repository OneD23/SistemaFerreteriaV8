using Microsoft.VisualBasic;
using SistemaFerreteriaV8.Clases;
using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Infrastructure.Services;
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
    public partial class VentanaCierreCaja : Form
    {
        private double Vendido1, Registrado1;
        private Caja nueva;
        public Empleado empleadoActivo { get; set; }

        public VentanaCierreCaja()
        {
            InitializeComponent();
            SistemaFerreteriaV8.Clases.ThemeManager.ApplyToForm(this);
        }

        private async void VentanaCierreCaja_Load(object sender, EventArgs e)
        {
            try
            {
                // Solicitar el balance al cierre (mueve el prompt fuera del try/catch si prefieres)
                double balanceAlCierre = 0;
                bool valido = false;

                while (!valido)
                {
                    string input = Interaction.InputBox("Digita el total en caja:", "Cierre de Caja", "");
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        MessageBox.Show("Operación cancelada por el usuario.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                        return;
                    }
                    valido = double.TryParse(input, out balanceAlCierre);
                    if (!valido)
                        MessageBox.Show("Por favor, ingrese un valor numérico válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                var usuarioCaja = string.IsNullOrWhiteSpace(empleadoActivo?.Nombre) ? "Genérico" : empleadoActivo.Nombre;
                var preview = await AppServices.CashRegister.BuildClosePreviewAsync(
                    new CashRegisterClosePreviewRequest(usuarioCaja, balanceAlCierre, DateTime.Now));

                nueva = preview.CajaActiva;

                if (!preview.Success || nueva == null)
                {
                    MessageBox.Show(preview.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                    return;
                }

                fecha.Text = nueva.FechaApertura.ToShortDateString();

                ListaCompras.Rows.Clear();
                foreach (var item in preview.RelatedInvoices ?? Array.Empty<Factura.FacturaResumen>())
                {
                    ListaCompras.Rows.Add(
                        item.Id,
                        item.NombreCliente,
                        item.Fecha.ToLocalTime().ToShortTimeString(),
                        item.Total.ToString("C2"),
                        item.Editada ? "Sí" : "No",
                        item.MetodoDePago
                    );
                }

                var vendidoTotal = preview.ExpectedBalance - nueva.BalanceInicial;
                MontoApertura.Text = nueva.BalanceInicial.ToString("C2");
                Vendido.Text = vendidoTotal.ToString("C2");
                Sum.Text = preview.ExpectedBalance.ToString("C2");
                Registrado.Text = preview.ReportedBalance.ToString("C2");
                Usuario.Text = usuarioCaja;

                Resultado.Text = Math.Abs(preview.Difference) < 0.01
                    ? "Cuadre Exitoso"
                    : $"Existe un descuadre de {preview.Difference.ToString("C2")}";

                Vendido1 = vendidoTotal;
                Registrado1 = preview.ReportedBalance;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar la ventana: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var closeResult = await AppServices.CashRegister.CloseAsync(
                    new CashRegisterCloseRequest(
                        Usuario.Text,
                        Registrado1,
                        DateTime.Now));
                if (!closeResult.Success)
                {
                    MessageBox.Show(closeResult.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                nueva = closeResult.CajaActiva;
                var frm = WinFormsApp.OpenForms["Form1"] as Form1;
                frm?.Dispose();
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar la caja: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void GenerarFacturaComprobante()
        {
            Configuraciones config = new Configuraciones().ObtenerPorId(1);
            CreaTicket factura = new CreaTicket();

            factura.TextoCentro(config.Nombre ?? "FERRETERIA");
            factura.TextoCentro("RNC: " + config.RNC);
            factura.TextoCentro(config.Direccion ?? "Calle Duarte #1, esquina Sanchez");
            factura.TextoCentro(!string.IsNullOrWhiteSpace(config.Telefono) ? "Tel: " + config.Telefono : "Tel: 809-584-0696 / 809-330-5927");

            factura.TextoCentro("");
            factura.TextoIzquierda("Ventas del día " + DateTime.Now.ToShortDateString());
            factura.TextoIzquierda("Responsable: " + (empleadoActivo?.Nombre ?? "Genérico"));

            factura.LineasGuion();
            factura.TextoIzquierda("ID    Hora          Valor     Pago");
            factura.LineasGuion();

            double valorTotal = 0;

            foreach (DataGridViewRow item in ListaCompras.Rows)
            {
                if (item != null && item.Cells[0].Value != null)
                {
                    string id = item.Cells[0].Value.ToString().PadRight(6, ' ');
                    string fecha = item.Cells[2].Value.ToString().PadRight(14, ' ');
                    string valor = item.Cells[3].Value.ToString().PadRight(8, ' ');
                    string pago = item.Cells[5].Value.ToString().PadRight(8, ' ');

                    // Asegurarse de no fallar si hay error de conversión
                    if (double.TryParse(valor.Replace("$", "").Replace(",", ""), out double parsedValor))
                        valorTotal += parsedValor;

                    factura.TextoIzquierda(id + fecha + valor + pago);
                }
            }
            factura.LineasGuion();
            factura.TextoIzquierda("");
            factura.AgregaTotales("Balance Inicial: ", nueva.BalanceInicial);
            factura.AgregaTotales("Total facturado: ", Vendido1);
            factura.AgregaTotales("Suma total: ", (nueva.BalanceInicial + Vendido1));
            factura.TextoIzquierda("");
            factura.AgregaTotales("Registrado por el usuario: ", Registrado1);
            factura.TextoIzquierda("Nota: " + Resultado.Text);

            factura.ImprimirTiket(config.Impresora);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerarFacturaComprobante();
        }
    }
}
