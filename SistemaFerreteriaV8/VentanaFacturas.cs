using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaFerreteriaV8
{
    public partial class VentanaFacturas : Form
    {

        private void ConfigurarVistaProfesional()
        {
            ListaDeFacturas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ListaDeFacturas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ListaDeFacturas.MultiSelect = false;
            ListaDeFacturas.AllowUserToAddRows = false;
            ListaDeFacturas.AllowUserToDeleteRows = false;
            ListaDeFacturas.RowHeadersVisible = false;
            ListaDeFacturas.BorderStyle = BorderStyle.None;
            ListaDeFacturas.BackgroundColor = System.Drawing.Color.FromArgb(20, 20, 20);
            ListaDeFacturas.EnableHeadersVisualStyles = false;

            ListaDeFacturas.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(35, 35, 35);
            ListaDeFacturas.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            ListaDeFacturas.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);

            ListaDeFacturas.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            ListaDeFacturas.DefaultCellStyle.ForeColor = System.Drawing.Color.WhiteSmoke;
            ListaDeFacturas.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            ListaDeFacturas.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            ListaDeFacturas.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);

            ListaDeFacturas.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(28, 28, 28);

            Column2.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            Column6.DefaultCellStyle.Format = "C2";
            Paginacion.Text = "Página 1 de 1";
        }

        private const int pageSize = 50;
        private int currentPage = 1;
        private int totalPages = 1;
        private int searchVersion = 0;
        private bool isInitializing = true;

        private ProgressBar progressBarLoading;

        public VentanaFacturas()
        {
            InitializeComponent();
            SistemaFerreteriaV8.Clases.ThemeManager.ApplyToForm(this);

            progressBarLoading = new ProgressBar
            {
                Name = "progressBarLoading",
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30,
                Visible = false,
                Width = 100,
                Height = 20,
                Left = 508,
                Top = 85
            };

            this.Controls.Add(progressBarLoading);
            this.Controls.SetChildIndex(progressBarLoading, 0);
            ConfigurarVistaProfesional();

            button1.Click += async (_, __) => await CambiarPaginaAsync(-1);
            button2.Click += async (_, __) => await CambiarPaginaAsync(1);
            Fecha1.ValueChanged += async (_, __) => await ReiniciarYRecargarAsync();
            Fecha2.ValueChanged += async (_, __) => await ReiniciarYRecargarAsync();
        }

        private async void VentanaFacturas_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            Fecha1.Value = DateTime.Today.AddMonths(-1);
            Fecha2.Value = DateTime.Now;

            isInitializing = false;
            await CargarPaginaAsync();
        }

        private async Task ReiniciarYRecargarAsync()
        {
            if (isInitializing)
                return;

            currentPage = 1;
            await CargarPaginaAsync();
        }

        private async Task CambiarPaginaAsync(int delta)
        {
            var nuevaPagina = currentPage + delta;
            if (nuevaPagina < 1 || nuevaPagina > totalPages)
                return;

            currentPage = nuevaPagina;
            await CargarPaginaAsync();
        }

        private async Task CargarPaginaAsync()
        {
            progressBarLoading.Visible = true;

            try
            {
                string tipoFiltro = comboBox1.Text;
                string termino = Id.Text?.Trim();

                var resultado = await Factura.ListarFacturasPaginadasAsync(
                    Fecha1.Value,
                    Fecha2.Value,
                    currentPage,
                    pageSize,
                    tipoFiltro,
                    termino);

                var lista = resultado.Facturas;
                var total = resultado.Total;

                totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
                if (currentPage > totalPages)
                {
                    currentPage = totalPages;
                }

                var empleadosIds = lista
                    .Select(f => f.IdEmpleado)
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Distinct()
                    .ToList();

                var empleadosMap = new Dictionary<string, string>();
                foreach (var empleadoId in empleadosIds)
                {
                    var empleado = await Empleado.BuscarAsync(empleadoId);
                    empleadosMap[empleadoId] = empleado?.Nombre ?? string.Empty;
                }

                ListaDeFacturas.Rows.Clear();
                foreach (var item in lista)
                {
                    var empleadoNombre = string.IsNullOrWhiteSpace(item.IdEmpleado)
                        ? string.Empty
                        : (empleadosMap.TryGetValue(item.IdEmpleado, out var nombre) ? nombre : string.Empty);

                    ListaDeFacturas.Rows.Add(
                        item.Id,
                        item.Fecha,
                        item.NombreCliente,
                        item.TipoFactura,
                        (item.Description ?? string.Empty) + (item.Informacion ?? string.Empty),
                        empleadoNombre,
                        item.Total,
                        item.Enviar,
                        item.Paga);
                }

                CantidadFactura.Text = total.ToString();
                Paginacion.Text = $"Página {currentPage} de {totalPages}";
            }
            finally
            {
                progressBarLoading.Visible = false;
            }
        }

        private async void Id_TextChanged(object sender, EventArgs e)
        {
            if (isInitializing)
                return;

            searchVersion++;
            int versionActual = searchVersion;

            await Task.Delay(250);
            if (versionActual != searchVersion)
                return;

            currentPage = 1;
            await CargarPaginaAsync();
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing)
                return;

            Id.Text = string.Empty;
            currentPage = 1;
            await CargarPaginaAsync();
        }

        private async void ListaDeFacturas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || ListaDeFacturas[0, e.RowIndex]?.Value == null) return;
            if (!int.TryParse(ListaDeFacturas[0, e.RowIndex].Value.ToString(), out int id)) return;

            var facturaActiva = await Factura.BuscarAsync(id);

            if (facturaActiva != null)
            {
                var factura = new VentanaFactura() { Factura = facturaActiva };
                factura.Show();
            }
            else
            {
                MessageBox.Show("La factura no fue encontrada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
