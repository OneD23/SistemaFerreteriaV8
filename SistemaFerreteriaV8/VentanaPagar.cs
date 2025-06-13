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
                    facturaActiva.Fecha = DateTime.UtcNow;

                    if (!string.IsNullOrEmpty(Efectivo.Text) && double.TryParse(Efectivo.Text, out double montoEfectivo))
                    {
                        facturaActiva.Efectivo = montoEfectivo;
                    }

                    if (ClienteActivo != null && ClienteActivo.CreditosActivo != null && ClienteActivo.CreditosActivo.Exists(m => m.Id == facturaActiva.Id))
                    {
                        ClienteActivo.CreditosActivo.RemoveAll(m => m.Id == facturaActiva.Id);
                        await ClienteActivo.EditarAsync();
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
                facturaActiva.RegistrarProductosAsync(1);

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
