using SistemaFerreteriaV8.Clases;
using SistemaFerreteriaV8.Domain.Security;
using SistemaFerreteriaV8.Infrastructure.Security;
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
    public partial class Usuarios : Form
    {
        private readonly Button _btnPermisosUsuario = new Button();
        private readonly Button _btnAuditoria = new Button();

        public Usuarios()
        {
            InitializeComponent();
            SistemaFerreteriaV8.Clases.ThemeManager.ApplyToForm(this);
            InicializarBotonPermisos();
        }

        private void InicializarBotonPermisos()
        {
            _btnPermisosUsuario.Text = "Permisos Usuario";
            _btnPermisosUsuario.Width = 150;
            _btnPermisosUsuario.Height = 34;
            _btnPermisosUsuario.Left = 12;
            _btnPermisosUsuario.Top = Math.Max(button1.Bottom, button2.Bottom) + 12;
            _btnPermisosUsuario.Click += async (_, _) => await AbrirPermisosUsuarioAsync();
            Controls.Add(_btnPermisosUsuario);

            _btnAuditoria.Text = "Auditoría";
            _btnAuditoria.Width = 150;
            _btnAuditoria.Height = 34;
            _btnAuditoria.Left = _btnPermisosUsuario.Right + 12;
            _btnAuditoria.Top = _btnPermisosUsuario.Top;
            _btnAuditoria.Click += async (_, _) => await AbrirConsultaAuditoriaAsync();
            Controls.Add(_btnAuditoria);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (!await PermissionAccess.EnsurePermissionAsync(
                    PermissionAccess.GetActiveEmployee(),
                    AppPermissions.ClientesEditar,
                    this,
                    "editar clientes"))
                return;

            Form1 frm = (Form1)WinFormsApp.OpenForms["Form1"];
            if (WinFormsApp.OpenForms.OfType<Form1>().Any())
            {
                frm.AbrirFormulario(new VentanaCliente());
            }

            this.Dispose();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!await PermissionAccess.EnsurePermissionAsync(
                    PermissionAccess.GetActiveEmployee(),
                    AppPermissions.EmpleadosGestionar,
                    this,
                    "gestionar empleados"))
                return;

            Form1 frm = (Form1)WinFormsApp.OpenForms["Form1"];
            if (WinFormsApp.OpenForms.OfType<Form1>().Any())
            {
                frm.AbrirFormulario(new VentanaEmpleado());
            }

            this.Dispose();
        }
        private async Task AbrirPermisosUsuarioAsync()
        {
            if (!await PermissionAccess.EnsurePermissionAsync(
                    PermissionAccess.GetActiveEmployee(),
                    AppPermissions.EmpleadosGestionar,
                    this,
                    "administrar permisos por usuario"))
                return;

            using var ventana = new VentanaPermisosUsuario();
            ventana.ShowDialog(this);
        }

        private async Task AbrirConsultaAuditoriaAsync()
        {
            if (!await PermissionAccess.EnsurePermissionAsync(
                    PermissionAccess.GetActiveEmployee(),
                    AppPermissions.ReportesVer,
                    this,
                    "consultar auditoría operativa"))
                return;

            using var ventana = new VentanaAuditoriaConsulta();
            ventana.ShowDialog(this);
        }

    }
}
