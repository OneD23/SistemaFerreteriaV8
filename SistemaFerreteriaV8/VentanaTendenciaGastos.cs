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
        private readonly Label lblEstado = new Label();
        private readonly Button btnGuardar = new Button();
        private readonly Button btnActualizar = new Button();
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
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 52 };
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

        private async Task CargarTendenciaAsync()
        {
            if (cargando) return;
            cargando = true;
            btnActualizar.Enabled = false;
            btnGuardar.Enabled = false;
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
                var puntoGasto = serieGasto.Points.AddXY(dia, gastoMensual);

                if (cruceDia == -1 && gastoMensual > 0 && acumulado >= gastoMensual)
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
            cargando = false;
        }
    }
}
