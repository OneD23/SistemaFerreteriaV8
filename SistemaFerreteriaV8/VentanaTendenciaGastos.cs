using MongoDB.Driver;
using SistemaFerreteriaV8.Clases;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SistemaFerreteriaV8
{
    public class VentanaTendenciaGastos : Form
    {
        private readonly Chart chartTendencia = new Chart();
        private readonly TextBox txtGastoMensual = new TextBox();
        private readonly TextBox txtGastoDiario = new TextBox();
        private readonly TextBox txtDescripcionGasto = new TextBox();
        private readonly Label lblEstado = new Label();
        private readonly Label lblResumen = new Label();
        private readonly Button btnGuardar = new Button();
        private readonly Button btnActualizar = new Button();
        private readonly Button btnRegistrarGasto = new Button();
        private bool cargando = false;

        public VentanaTendenciaGastos()
        {
            Text = "Tendencia de Ventas vs Gasto Mensual";
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.FromArgb(21, 34, 56);
            Padding = new Padding(12);

            ConstruirUI();
            Load += async (_, __) =>
            {
                CargarGastoMensual();
                await CargarTendenciaAsync();
            };
        }

        private void ConstruirUI()
        {
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 92 };
            Controls.Add(panelTop);

            var lblGasto = new Label
            {
                Text = "Gasto mensual:",
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Width = 130,
                Height = 28,
                Location = new Point(8, 12)
            };
            panelTop.Controls.Add(lblGasto);

            txtGastoMensual.Location = new Point(145, 14);
            txtGastoMensual.Size = new Size(150, 28);
            panelTop.Controls.Add(txtGastoMensual);

            btnGuardar.Text = "Guardar gasto";
            btnGuardar.Location = new Point(305, 12);
            btnGuardar.Size = new Size(120, 30);
            btnGuardar.Click += async (_, __) => await GuardarGastoMensualAsync();
            panelTop.Controls.Add(btnGuardar);

            btnActualizar.Text = "Actualizar gráfica";
            btnActualizar.Location = new Point(432, 12);
            btnActualizar.Size = new Size(130, 30);
            btnActualizar.Click += async (_, __) => await CargarTendenciaAsync();
            panelTop.Controls.Add(btnActualizar);

            lblEstado.ForeColor = Color.White;
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(580, 18);
            panelTop.Controls.Add(lblEstado);

            var lblGastoDiario = new Label
            {
                Text = "Gasto diario:",
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Width = 130,
                Height = 28,
                Location = new Point(8, 52)
            };
            panelTop.Controls.Add(lblGastoDiario);

            txtGastoDiario.Location = new Point(145, 54);
            txtGastoDiario.Size = new Size(150, 28);
            panelTop.Controls.Add(txtGastoDiario);

            txtDescripcionGasto.Location = new Point(305, 54);
            txtDescripcionGasto.Size = new Size(257, 28);
            panelTop.Controls.Add(txtDescripcionGasto);

            btnRegistrarGasto.Text = "Registrar gasto";
            btnRegistrarGasto.Location = new Point(570, 52);
            btnRegistrarGasto.Size = new Size(120, 30);
            btnRegistrarGasto.Click += async (_, __) => await RegistrarGastoDiarioAsync();
            panelTop.Controls.Add(btnRegistrarGasto);

            lblResumen.ForeColor = Color.LightGreen;
            lblResumen.AutoSize = true;
            lblResumen.Location = new Point(700, 58);
            panelTop.Controls.Add(lblResumen);

            chartTendencia.Dock = DockStyle.Fill;
            chartTendencia.BackColor = Color.FromArgb(36, 52, 77);
            var area = new ChartArea("area")
            {
                BackColor = Color.FromArgb(36, 52, 77)
            };
            area.AxisX.LabelStyle.ForeColor = Color.White;
            area.AxisY.LabelStyle.ForeColor = Color.White;
            area.AxisX.LineColor = Color.SlateGray;
            area.AxisY.LineColor = Color.SlateGray;
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(55, 70, 97);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(55, 70, 97);
            chartTendencia.ChartAreas.Add(area);
            Controls.Add(chartTendencia);
        }

        private Configuraciones ObtenerConfig()
        {
            return new Configuraciones().ObtenerPorId(1) ?? new Configuraciones { Id = 1 };
        }

        private void CargarGastoMensual()
        {
            var config = ObtenerConfig();
            txtGastoMensual.Text = config.GastoMensual.ToString("0.##");
        }

        private async Task GuardarGastoMensualAsync()
        {
            if (!double.TryParse(txtGastoMensual.Text, out var gasto) || gasto < 0)
            {
                MessageBox.Show("Ingrese un gasto mensual válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var config = ObtenerConfig();
            config.GastoMensual = gasto;
            config.Guardar();
            await CargarTendenciaAsync();
        }

        private async Task RegistrarGastoDiarioAsync()
        {
            if (!double.TryParse(txtGastoDiario.Text, out var monto) || monto <= 0)
            {
                MessageBox.Show("Ingrese un monto de gasto diario válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcionGasto.Text))
            {
                MessageBox.Show("Ingrese una descripción del gasto.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var gasto = new GastoDiario
            {
                Fecha = DateTime.Now,
                Monto = monto,
                Descripcion = txtDescripcionGasto.Text.Trim()
            };

            await gasto.CrearAsync();
            txtGastoDiario.Clear();
            txtDescripcionGasto.Clear();
            await CargarTendenciaAsync();
        }

        private async Task CargarTendenciaAsync()
        {
            if (cargando) return;
            cargando = true;
            btnActualizar.Enabled = false;
            btnGuardar.Enabled = false;
            btnRegistrarGasto.Enabled = false;
            lblEstado.Text = "Cargando tendencia...";
            lblEstado.ForeColor = Color.Gainsboro;

            chartTendencia.Series.Clear();
            chartTendencia.ChartAreas[0].AxisX.StripLines.Clear();

            var serieVentas = new Series("Ventas acumuladas")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = Color.LimeGreen
            };

            var serieGasto = new Series("Límite gasto mensual")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = Color.Red
            };

            double gastoMensual = double.TryParse(txtGastoMensual.Text, out var g) ? g : 0;
            int diaActual = DateTime.Now.Day;
            var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var finMes = inicioMes.AddMonths(1).AddTicks(-1);
            var empleados = await Empleado.ListarAsync();
            var gastosDiarios = await GastoDiario.ListarPorRangoAsync(inicioMes, finMes);
            var totalNomina = empleados.Sum(e => e.SueldoMensual);
            var totalGastosDiarios = gastosDiarios.Sum(gd => gd.Monto);
            var gastoTotalMes = gastoMensual + totalNomina + totalGastosDiarios;

            var facturasMes = (await Factura.ListarFacturasPorFechaAsync(inicioMes, finMes))
                .Where(f => !f.Eliminada)
                .OrderBy(f => f.Fecha)
                .ToList();

            double acumulado = 0;
            int cruceDia = -1;
            for (int dia = 1; dia <= diaActual; dia++)
            {
                acumulado += facturasMes
                    .Where(f => f.Fecha.Day == dia)
                    .Sum(f => f.Total);

                serieVentas.Points.AddXY(dia, acumulado);
                var puntoGasto = serieGasto.Points.AddXY(dia, gastoTotalMes);

                if (cruceDia == -1 && gastoTotalMes > 0 && acumulado >= gastoTotalMes)
                {
                    cruceDia = dia;
                    serieGasto.Points[puntoGasto].MarkerStyle = MarkerStyle.Cross;
                    serieGasto.Points[puntoGasto].MarkerSize = 12;
                }

                if (cruceDia != -1 && dia > cruceDia)
                {
                    serieGasto.Points[puntoGasto].IsEmpty = true; // romper línea roja al superar gasto
                }
            }

            chartTendencia.Series.Add(serieVentas);
            chartTendencia.Series.Add(serieGasto);
            var gananciaNeta = acumulado - gastoTotalMes;
            lblResumen.Text = $"Ventas: {acumulado:C2} | Gastos: {gastoTotalMes:C2} (Nómina: {totalNomina:C2}, Diario: {totalGastosDiarios:C2}) | Ganancia: {gananciaNeta:C2}";
            lblResumen.ForeColor = gananciaNeta >= 0 ? Color.LightGreen : Color.OrangeRed;

            if (cruceDia != -1)
            {
                chartTendencia.ChartAreas[0].AxisX.StripLines.Add(new StripLine
                {
                    IntervalOffset = cruceDia,
                    StripWidth = 0.15,
                    BackColor = Color.FromArgb(140, Color.Red)
                });
                lblEstado.Text = $"✅ Punto de equilibrio alcanzado el día {cruceDia}.";
                lblEstado.ForeColor = Color.LightGreen;
            }
            else
            {
                lblEstado.Text = "⚠ Aún no se supera el gasto mensual.";
                lblEstado.ForeColor = Color.Gold;
            }

            btnActualizar.Enabled = true;
            btnGuardar.Enabled = true;
            btnRegistrarGasto.Enabled = true;
            cargando = false;
        }
    }
}
