using SistemaFerreteriaV8.Clases;
using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Domain.Security;
using SistemaFerreteriaV8.Infrastructure.Security;
using SistemaFerreteriaV8.Infrastructure.Services;
using System;
using System.Drawing;
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
            SistemaFerreteriaV8.Clases.ThemeManager.ApplyToForm(this);
            Codigo.UseSystemPasswordChar = true;
            AutoScroll = true;
            MinimumSize = new Size(560, 470);
            ModernizarUI();
            Resize += (_, __) => ReorganizarLayout();
        }

        private void ModernizarUI()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            foreach (var btn in new[] { Aceptar, Cancelar })
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Height = 42;
            }

            ReorganizarLayout();
        }

        private void ReorganizarLayout()
        {
            int panelWidth = Math.Min(430, ClientSize.Width - 36);
            int xLabel = (ClientSize.Width - panelWidth) / 2;
            int xInput = xLabel + 160;
            int wInput = panelWidth - 168;
            int y = 78;
            int h = 30;
            int gap = 46;

            label1.Left = (ClientSize.Width - label1.Width) / 2;
            label1.Top = 30;

            Aviso.Left = (ClientSize.Width - Aviso.Width) / 2;
            Aviso.Top = y;
            y += 34;

            ConfigCampo(label3, Codigo, xLabel, xInput, wInput, y, h);
            y += gap;
            ConfigCampo(label2, Balance, xLabel, xInput, wInput, y, h);
            y += gap;
            ConfigCampo(label4, turno, xLabel, xInput, wInput, y, h);

            int yBtns = ClientSize.Height - 70;
            int btnW = 130;
            int space = 20;
            int startX = (ClientSize.Width - ((btnW * 2) + space)) / 2;
            Cancelar.SetBounds(startX, yBtns, btnW, 42);
            Aceptar.SetBounds(startX + btnW + space, yBtns, btnW, 42);
        }

        private static void ConfigCampo(Label label, Control input, int xLabel, int xInput, int wInput, int y, int h)
        {
            label.AutoSize = false;
            label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            label.Location = new System.Drawing.Point(xLabel, y);
            label.Size = new System.Drawing.Size(150, h);
            input.Location = new System.Drawing.Point(xInput, y);
            input.Size = new System.Drawing.Size(wInput, h);
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
            var cashState = await AppServices.CashRegister.ValidateOpenStateAsync();
            if (cashState.Success && cashState.CajaActiva != null)
            {
                turno.Text = cashState.CajaActiva.Turno;
                Balance.Text = cashState.CajaActiva.BalanceInicial.ToString();
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
            var auth = await SecurityServices.AuthenticationService.AuthenticateAsync(Codigo.Text);
            if (!auth.IsAuthenticated)
            {
                MessageBox.Show("Código incorrecto");
                Codigo.Text = "";
                return;
            }

            if (!SecurityServices.AuthorizationService.HasPermission(auth, AppPermissions.CajaAbrir))
            {
                MessageBox.Show("Tu usuario no tiene permiso para abrir caja.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Codigo.Text = "";
                return;
            }

            var empleado = new Empleado
            {
                Id = MongoDB.Bson.ObjectId.TryParse(auth.EmployeeId, out var objectId)
                    ? objectId
                    : MongoDB.Bson.ObjectId.GenerateNewId(),
                Nombre = auth.EmployeeName,
                Puesto = auth.Role.ToString()
            };

            var cashState = await AppServices.CashRegister.GetActiveAsync(empleado.Nombre);
            if (cashState.Success && cashState.CajaActiva != null)
            {
                turno.Text = cashState.CajaActiva.Turno;
                Balance.Text = cashState.CajaActiva.BalanceInicial.ToString();
                turno.Enabled = false;
                Balance.Enabled = false;
            }
            else if (cashState.ErrorType == CashRegisterErrorType.NotFound)
            {
                if (!double.TryParse(Balance.Text, out var balanceInicial))
                {
                    MessageBox.Show("El balance inicial no es válido.");
                    return;
                }

                var openResult = await AppServices.CashRegister.OpenAsync(
                    new CashRegisterOpenRequest(turno.Text, balanceInicial, empleado.Nombre));
                if (!openResult.Success)
                {
                    MessageBox.Show(openResult.Message, "Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                turno.Text = openResult.CajaActiva?.Turno ?? turno.Text;
                Balance.Text = openResult.CajaActiva?.BalanceInicial.ToString() ?? Balance.Text;
                turno.Enabled = false;
                Balance.Enabled = false;
            }
            else
            {
                MessageBox.Show(cashState.Message, "Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Asignar empleado activo en Form1 si está abierto
            if (WinFormsApp.OpenForms.OfType<Form1>().Any())
            {
                Form1 frm = (Form1)WinFormsApp.OpenForms["Form1"];
                frm.EmpleadoActivo = empleado;
            }
            this.Dispose();
        }
    }
}
