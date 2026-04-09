using SistemaFerreteriaV8.Clases;
using SistemaFerreteriaV8.Domain.Security;
using SistemaFerreteriaV8.Infrastructure.Security;
using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SistemaFerreteriaV8
{
    public partial class Form1 : Form
    {
        private Form formActivado = null;
        private Button botonActivo = null;

        public Empleado EmpleadoActivo { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        // Marca visual del botón activo en el menú
        public void ActivarBoton(Button boton)
        {
            if (botonActivo != null)
                botonActivo.BackColor = Color.FromArgb(255, 128, 0);

            boton.BackColor = Color.Black;
            botonActivo = boton;
            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
            this.BackColor = Color.Black;
            Centro.BackColor = Color.Black;
        }

        // Abrir un formulario hijo en el panel central
        public void AbrirFormulario(Form hijo)
        {
            // Si tienes lógica para validar caja activa, ponla async afuera.
            // Caja caja = await new Caja().BuscarPorClaveAsync("", "activa"); // <- Si es async

            if (formActivado != null)
                formActivado.Close();

            formActivado = hijo;
            hijo.TopLevel = false;
            hijo.Dock = DockStyle.Fill;
            hijo.BringToFront();
            Centro.Controls.Add(hijo);
            Centro.Tag = hijo;
            hijo.Show();
        }

        // Ventas
        private void button1_Click(object sender, EventArgs e)
        {
            ActivarBoton(sender as Button);
            AbrirFormulario(new VentanaVentas() { empleado = EmpleadoActivo });
        }

        // Productos (solo admin)
        private async void button2_Click(object sender, EventArgs e)
        {
            if (await PermissionAccess.EnsurePermissionAsync(
                    EmpleadoActivo,
                    AppPermissions.InventarioEditar,
                    this,
                    "acceder a Productos"))
            {
                ActivarBoton(sender as Button);
                AbrirFormulario(new VentanaProductos() { empleado = EmpleadoActivo });
            }
            else
            {
                MessageBox.Show("No tienes Acceso a este módulo", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Usuarios (solo admin)
        private async void button3_Click(object sender, EventArgs e)
        {
            if (await PermissionAccess.EnsurePermissionAsync(
                    EmpleadoActivo,
                    AppPermissions.EmpleadosGestionar,
                    this,
                    "gestionar usuarios y empleados"))
            {
                ActivarBoton(sender as Button);
                AbrirFormulario(new Usuarios());
            }
            else
            {
                MessageBox.Show("No tienes Acceso a este módulo", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Contabilidad (solo admin)
        private async void button4_Click(object sender, EventArgs e)
        {
            if (await PermissionAccess.EnsurePermissionAsync(
                    EmpleadoActivo,
                    AppPermissions.ReportesVer,
                    this,
                    "acceder a Contabilidad"))
            {
                ActivarBoton(sender as Button);
                AbrirFormulario(new Contabilidad() { empleado = EmpleadoActivo });
            }
            else
            {
                MessageBox.Show("No tienes Acceso a este módulo", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Configuraciones (solo admin)
        private async void button5_Click(object sender, EventArgs e)
        {
            if (await PermissionAccess.EnsurePermissionAsync(
                    EmpleadoActivo,
                    AppPermissions.ConfiguracionEditar,
                    this,
                    "editar configuraciones"))
            {
                ActivarBoton(sender as Button);
                AbrirFormulario(new VentanaConfiguraciones());
            }
            else
            {
                MessageBox.Show("No tienes Acceso a este módulo", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Cotizar
        private void button6_Click(object sender, EventArgs e)
        {
            var frm = WinFormsApp.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
            if (frm != null)
            {
                frm.CotizarAsync();
                frm.LimpiarTodo();
            }
        }

        // Cerrar caja
        private async void CerrarCaja_Click(object sender, EventArgs e)
        {
            var permitido = await PermissionAccess.EnsurePermissionAsync(
                EmpleadoActivo,
                AppPermissions.CajaCerrar,
                this,
                "cerrar caja");
            if (!permitido)
                return;

            var nueva = new VentanaCierreCaja() { empleadoActivo = EmpleadoActivo };
            nueva.ShowDialog();
        }

        // Evento load del formulario principal
        private void Form1_Load(object sender, EventArgs e)
        {
            var config = new Configuraciones().ObtenerPorId(1);

            GlobalKeyListener.KeyPressed += GlobalKeyListener_KeyPressed;

            var newVentana = new VentanaRegistroCaja();
            newVentana.ShowDialog();

            if (EmpleadoActivo == null)
            {
                this.Dispose();
                return;
            }

            this.WindowState = FormWindowState.Maximized;
        }

        // Acceso rápido (shortcut F3)
        private void GlobalKeyListener_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                button2_Click(button2, e);
        }

        // Async para alguna tarea especial (ejemplo: migración de ObjectId)
        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            await Factura.AsignarObjectIdDesdeIntId();
            MessageBox.Show("Actualización completada.");
        }

        // Permiso de admin para acciones sensibles
        private async Task<bool> TienePermisoAdmin()
        {
            if (EmpleadoActivo != null && EmpleadoActivo.Puesto == "Administrador")
                return true;

            return await PermissionAccess.EnsurePermissionAsync(
                EmpleadoActivo,
                AppPermissions.EmpleadosGestionar,
                this,
                "confirmar acceso administrativo");
        }

        // Si tienes algún timer descomenta esto y úsalo como necesitas
        /*
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Lógica de impresión automática o monitoreo aquí
        }
        */
    }
}
