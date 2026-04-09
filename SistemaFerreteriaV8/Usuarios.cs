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
        public Usuarios()
        {
            InitializeComponent();
            SistemaFerreteriaV8.Clases.ThemeManager.ApplyToForm(this);
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
    }
}
