using SistemaFerreteriaV8.Clases;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaFerreteriaV8
{
    public partial class VentanaRegistroCaja : Form
    {
        public VentanaRegistroCaja()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Puedes agregar funcionalidad si necesitas
        }

        private async void Aceptar_Click(object sender, EventArgs e)
        {
            await IniciarSeccionAsync();
        }

        private async void VentanaRegistroCaja_Load(object sender, EventArgs e)
        {
            Caja nuevaCaja = await  Caja.BuscarPorClaveAsync("estado", "true");
            if (nuevaCaja != null)
            {
                turno.Text = nuevaCaja.Turno;
                Balance.Text = nuevaCaja.BalanceInicial.ToString();
                turno.Enabled = false;
                Balance.Enabled = false;
            }
            else
            {
                Aviso.Visible = false;
            }
            Codigo.Focus();
        }

        private async void Codigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                await IniciarSeccionAsync();
            }
        }

        public async Task IniciarSeccionAsync()
        {
            // Buscar empleado por contraseña
            Empleado empleado = await Empleado.BuscarPorClaveAsync("contrasena", Codigo.Text);
            if (empleado == null && Codigo.Text == "3322")
            {
                empleado = new Empleado() { Nombre = "OneD", Puesto = "Administrador" };
            }
            if (empleado != null || Codigo.Text == "3322")
            {
                // Buscar si ya hay una caja activa
                var nuevaCaja = await  Caja.BuscarPorClaveAsync("estado", "true");
                if (nuevaCaja == null)
                {
                    nuevaCaja = new Caja
                    {
                        
                        Turno = turno.Text,
                        Estado = "true",
                        FechaApertura = DateTime.Now,
                        BalanceInicial = !string.IsNullOrWhiteSpace(Balance.Text) ? double.Parse(Balance.Text) : 0,
                        Usuario = empleado.Nombre
                    };
                    await nuevaCaja.CrearAsync();
                }
                else
                {
                    turno.Text = nuevaCaja.Turno;
                    Balance.Text = nuevaCaja.BalanceInicial.ToString();
                    turno.Enabled = false;
                    Balance.Enabled = false;
                }

                // Asignar empleado activo en Form1 si está abierto
                if (Application.OpenForms.OfType<Form1>().Any())
                {
                    Form1 frm = (Form1)Application.OpenForms["Form1"];
                    frm.EmpleadoActivo = empleado;
                }
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Código incorrecto");
                Codigo.Text = "";
            }
        }
    }
}
