using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using SistemaFerreteriaV8.Clases;
using System.IO;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Drawing.Printing;

namespace SistemaFerreteriaV8
{
    public partial class VentanaPagar : Form
    {
        public Factura facturaActiva { set; get; }
        public Empleado EmpleadoActivo { set; get; }
        public Cliente ClienteActivo { set; get; }

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
                TipoFactura.Text = facturaActiva.tipoFactura;
                subTotal.Text = facturaActiva.Total.ToString("c2");
                Descuento.Text = facturaActiva.Descuentos.ToString("c2");

                Total.Text = (facturaActiva.Total - facturaActiva.Descuentos).ToString("c2");
                MetodoPago.SelectedIndex = 0;                
            }
        }
        private void MetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(MetodoPago.SelectedIndex != 0) {
            Efectivo.Enabled = false;
            }
            else
            {
                Efectivo.Enabled = true;
            }
        }

        private void Efectivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                double total = facturaActiva.Total - facturaActiva.Descuentos;

                double efectivo = double.Parse(Efectivo.Text);

                Devuelta.Text = (efectivo - total).ToString("c2");
            }
        }
        private void Pagar_Click(object sender, EventArgs e)
        {
            bool sentinela = true;
            if (facturaActiva != null && !facturaActiva.Eliminada)
            {                
                facturaActiva.tipoFactura = TipoFactura.Text;
                    
                //configuracion de pagos
                facturaActiva.MetodoDePago = MetodoPago.Text;

                switch (MetodoPago.Text)
                {
                    case "Efectivo":
                        facturaActiva.Paga = true;
                        facturaActiva.MetodoDePago = "Efectivo";
                        facturaActiva.Fecha = DateTime.UtcNow;
                        if (!string.IsNullOrEmpty(Efectivo.Text))
                        {
                            facturaActiva.Efectivo = double.Parse(Efectivo.Text);
                        }
                       
                        sentinela = true;

                        if (ClienteActivo != null &&ClienteActivo.CreditosActivo.Exists(m => m.Id == facturaActiva.Id))
                        {
                            ClienteActivo.CreditosActivo.RemoveAll(m => m.Id == facturaActiva.Id);                            
                            ClienteActivo.Editar();
                        }
                        facturaActiva.ActualizarFactura();

                        break;

                    case "Credito":

                        facturaActiva.Paga = false;

                        Cliente cliente = new Cliente();
                        if (facturaActiva.IdCliente != null && facturaActiva.IdCliente != "0")
                        {
                            cliente = new Cliente().Buscar(facturaActiva.IdCliente);
                        }
                        else
                        {
                            string input = Interaction.InputBox("Digite el Nombre o Id del cliente al cual le cargara el credito: ");

                            bool esNumero = int.TryParse(input, out _);

                            //buscar por nombre si es el caso
                            if (esNumero)
                            {
                                cliente = new Cliente().Buscar(input);
                            }
                            else
                            {
                                cliente = new Cliente().BuscarPorClave("nombre", input);
                            }         
                        }
                        if (cliente != null)
                        {                       

                            if (MessageBox.Show($"Esta seguro que quiere cargar el monto de {facturaActiva.Total} a la cuenta de: \n\n" +
                                  $"Id: {cliente.Id}. \n" +
                                  $"Nombre: {cliente.Nombre}. \n" +
                                  $"Direccion: {cliente.Direccion}. \n", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                //Iniciar el proceso de pago
                                double creditoActivo = 0;
                                if (cliente.CreditosActivo != null)
                                {
                                    foreach (var item in cliente.CreditosActivo)
                                    {
                                        creditoActivo += item.Total;
                                    }
                                }
                                else
                                {
                                    cliente.CreditosActivo = new List<Factura>();
                                }
                               
                                if(creditoActivo < cliente.LimiteCredito)
                                {
                                  if (creditoActivo + facturaActiva.Total > cliente.LimiteCredito)
                                    {
                                        MessageBox.Show($"El cliente tiene un credito activo de {creditoActivo} si le sumas los { facturaActiva.Total} de la factura superara el monto de credito permitido que es {cliente.LimiteCredito}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        //Estaras aqui si pasas todas las validaciones                                       
                                        cliente.CreditosActivo.Add( facturaActiva);
                                        cliente.Editar();

                                        facturaActiva.Paga = false;
                                        facturaActiva.ActualizarFactura();
                                        sentinela = true;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Este Cliente ya supero el credito permitido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Se cancelo el proceso de pago", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                           
                        }
                        else
                        {
                            MessageBox.Show("No se pudo encontrar el cliente, revise los datos ingresados e intente de nuevo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            sentinela = false;
                        }

                        break;

                    default:
                        break;
                }
                if (sentinela)
                {
                    //terminando el pago y cerrando la ventana
                    if (facturaActiva.tipoFactura == "Consumo")
                    {
                        facturaActiva.GenerarFactura();
                    }
                    else if (facturaActiva.tipoFactura == "Comprobante Fiscal")
                    {
                        facturaActiva.GenerarFacturaComprobante();
                    }
                    else if (facturaActiva.tipoFactura == "Comprobante Gubernamental")
                    {
                        facturaActiva.GenerarFacturaGubernamental();
                    }
                    else
                    {
                        facturaActiva.GenerarFactura1();

                    }
                    facturaActiva.RegistrarProductos(1);
                    VentanaVentas frm = (VentanaVentas)Application.OpenForms["VentanaVentas"];
                    if (Application.OpenForms.OfType<VentanaVentas>().Any())
                    {
                        frm.LimpiarTodo();
                    }
                    this.Dispose();
                }
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


                string rutaArchivo = @"rnc.txt"; // Cambia esto por la ruta de tu archivo
                string textoABuscar = "texto buscado"; // Cambia esto por el texto que quieres buscar
                string[] datos = new string[2];

                // Verificar si el archivo existe
                if (File.Exists(rutaArchivo))
                {
                    // Leer todas las líneas del archivo
                    string[] lineas = File.ReadAllLines(rutaArchivo);

                    // Recorrer cada línea en busca del texto
                    foreach (string linea in lineas)
                    {
                        if (linea.Contains(rnc))
                        {
                            MessageBox.Show(linea.Replace("|", " "), "RNC encontrado!");
                            datos[0] = rnc;
                            datos[1] = linea.Split('|')[1];
                            // Muestra la línea que contiene el texto
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
