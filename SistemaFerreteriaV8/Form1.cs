using DocumentFormat.OpenXml.Bibliography;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1.Cms;
using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace SistemaFerreteriaV8
{
    public partial class Form1 : Form
    {
      
        Form formActivado = null;
        Button botonActivo = null;

        public Empleado EmpleadoActivo { set; get; }

        public void ActivarBoton(Button boton)
        {
            if (botonActivo != null)
            {
                botonActivo.BackColor = Color.FromArgb(255, 128, 0);
            }
            boton.BackColor = Color.Black;
            botonActivo = boton;
            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
            this.BackColor = Color.Black;
            Centro.BackColor = Color.Black;
        }
        public Form1()
        {
           
            InitializeComponent();
        }

        public void AbrirFormulario(Form hijo)
        {
            Caja caja = new Caja().BuscarPorClaveAsync("","activa");
      

            if (formActivado != null)
            {
                formActivado.Close();
            }

            formActivado = hijo;

            hijo.TopLevel = false;
            hijo.Dock = DockStyle.Fill;
            hijo.BringToFront();
            hijo.Show();

            Centro.Controls.Add(hijo);
            Centro.Tag = hijo;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ActivarBoton(sender as Button);

            AbrirFormulario(new VentanaVentas() { empleado = EmpleadoActivo });
        }
        private void button2_Click(object sender, EventArgs e)
        {
            bool Editar = false;
            
                if (EmpleadoActivo.Puesto == "Administrador")
                {
                    Editar = true;
                }
                else
                {
                    string codigo = Interaction.InputBox("Nesecita la clave del Administrador para editar");
                    Empleado iban = new Empleado().BuscarPorClave("contrasena", codigo);
                    if (iban != null)
                    {
                        if (iban.Puesto == "Administrador")
                        {
                            Editar = true;
                        }
                    }
                }
                if (Editar)
                {
                    ActivarBoton(sender as Button);
                    AbrirFormulario(new VentanaProductos() { empleado = EmpleadoActivo });
                }
                else
                {
                    MessageBox.Show("No tienes Acceso a este modulo");
                }
            
        }
        private void button3_Click(object sender, EventArgs e)
        {
            bool Editar = false;
            
                if (EmpleadoActivo.Puesto == "Administrador")
                {
                    Editar = true;
                }
                else
                {
                    string codigo = Interaction.InputBox("Nesecita la clave del Administrador para editar");
                    Empleado iban = new Empleado().BuscarPorClave("contrasena", codigo);
                    if (iban != null)
                    {
                        if (iban.Puesto == "Administrador")
                        {
                            Editar = true;
                        }
                    }
                }
                if (Editar)
                {
                    ActivarBoton(sender as Button);
                    AbrirFormulario(new Usuarios());
                }
                else
                {
                    MessageBox.Show("No tienes Acceso a este modulo");
                }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool Editar = false;
            
                if (EmpleadoActivo.Puesto == "Administrador")
                {
                    Editar = true;
                }
                else
                {
                    string codigo = Interaction.InputBox("Nesecita la clave del Administrador para editar");
                    Empleado iban = new Empleado().BuscarPorClave("contrasena", codigo);
                    if (iban != null)
                    {
                        if (iban.Puesto == "Administrador")
                        {
                            Editar = true;
                        }
                    }
                }
                if (Editar) {

                    ActivarBoton(sender as Button);
                    AbrirFormulario(new Contabilidad() { empleado = EmpleadoActivo });
                }
            
        }
        private void GlobalKeyListener_KeyPressed(object sender, KeyEventArgs e)
        {
            // Aquí puedes manejar la tecla presionada
            if (e.KeyCode == Keys.F3)
            {
                button2_Click(button2, e);
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
            Configuraciones config = new Configuraciones().ObtenerPorId(1);

            GlobalKeyListener.KeyPressed += GlobalKeyListener_KeyPressed;

            VentanaRegistroCaja newVentana = new VentanaRegistroCaja();
            newVentana.ShowDialog();

            if (EmpleadoActivo == null )
            {
                this.Dispose();
            }  
            
           this.WindowState = FormWindowState.Maximized;  
          
        }
        private void CerrarCaja_Click(object sender, EventArgs e)
        {
            VentanaCierreCaja nueva = new VentanaCierreCaja() {empleadoActivo = EmpleadoActivo };
            nueva.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool Editar = false;
            
                if (EmpleadoActivo.Puesto == "Administrador")
                {
                    Editar = true;
                }
                else
                {
                    string codigo = Interaction.InputBox("Nesecita la clave del Administrador para editar");
                    Empleado iban = new Empleado().BuscarPorClave("contrasena", codigo);
                    if (iban != null)
                    {
                        if (iban.Puesto == "Administrador")
                        {
                            Editar = true;
                        }
                    }
                }
                if (Editar)
                {
                    ActivarBoton(sender as Button);
                    AbrirFormulario(new VentanaConfiguraciones());
                }
                else
                {
                    MessageBox.Show("No tienes Acceso a este modulo");
                }
         
        }    

        private void timer1_Tick(object sender, EventArgs e)
        {
           /* Factura factura = new Factura().Buscar("impresa","true");

            if (factura != null)
            {
                factura.GenerarFactura();
            }*/
        }

        private void button6_Click(object sender, EventArgs e)
        {
            VentanaVentas frm = (VentanaVentas)Application.OpenForms["VentanaVentas"];
            if (Application.OpenForms.OfType<VentanaVentas>().Any())
            {
                frm.Cotizar();
                frm.LimpiarTodo();
            }
        }

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            await Factura.AsignarObjectIdDesdeIntId();
            MessageBox.Show("Actualización completada.");
        }
    }
}
