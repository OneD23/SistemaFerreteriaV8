using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaFerreteriaV8.Clases;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Forms.DataVisualization.Charting;

namespace SistemaFerreteriaV8
{
    public partial class VentanaEstadisticas : Form
    {
        public VentanaEstadisticas()
        {
            InitializeComponent();
        }

        private async void VentanaEstadisticas_Load(object sender, EventArgs e)
        {
            AplicarTemaProfesional();
            await CargarAsync(DateTime.Today.AddDays(-10), DateTime.Now);
        }

        private void AplicarTemaProfesional()
        {
            var bg = Color.FromArgb(15, 23, 42);
            var card = Color.FromArgb(30, 41, 59);
            var border = Color.FromArgb(71, 85, 105);
            var accent = Color.FromArgb(59, 130, 246);
            var success = Color.FromArgb(16, 185, 129);
            var textPrimary = Color.White;
            var textMuted = Color.FromArgb(148, 163, 184);

            BackColor = bg;

            // Cards KPI
            foreach (var panel in new[] { panel1, panel2, panel3 })
            {
                panel.BackColor = card;
                panel.BorderStyle = BorderStyle.FixedSingle;
            }

            label1.ForeColor = textMuted;
            label2.ForeColor = textMuted;
            label3.ForeColor = textMuted;
            TFacturas.ForeColor = textPrimary;
            TVentas.ForeColor = success;
            TGanancias.ForeColor = success;

            label1.Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold);
            label2.Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold);
            label3.Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold);
            TFacturas.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            TVentas.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            TGanancias.Font = new Font("Segoe UI", 14, FontStyle.Bold);

            // Botones filtro
            var botonesRango = new[] { Personalizado, Hoy, SevenDay, button1, button2, Buscar, button3 };
            foreach (var btn in botonesRango)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = border;
                btn.BackColor = Color.FromArgb(15, 23, 42);
                btn.ForeColor = textPrimary;
                btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
            Buscar.BackColor = accent;
            button3.BackColor = Color.FromArgb(2, 132, 199);

            // Date pickers
            foreach (var dt in new[] { Fecha1, Fecha2 })
            {
                dt.CalendarMonthBackground = card;
                dt.CalendarForeColor = textPrimary;
                dt.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            }

            // Chart style
            VentaPorFecha.BackColor = card;
            VentaPorFecha.BorderlineDashStyle = ChartDashStyle.Solid;
            VentaPorFecha.BorderlineColor = border;
            if (VentaPorFecha.ChartAreas.Count > 0)
            {
                var area = VentaPorFecha.ChartAreas[0];
                area.BackColor = card;
                area.AxisX.LabelStyle.ForeColor = textMuted;
                area.AxisY.LabelStyle.ForeColor = textMuted;
                area.AxisX.LineColor = border;
                area.AxisY.LineColor = border;
                area.AxisX.MajorGrid.LineColor = Color.FromArgb(51, 65, 85);
                area.AxisY.MajorGrid.LineColor = Color.FromArgb(51, 65, 85);
            }
            if (VentaPorFecha.Legends.Count > 0)
            {
                VentaPorFecha.Legends[0].BackColor = card;
                VentaPorFecha.Legends[0].ForeColor = textPrimary;
            }

            panel5.BackColor = card;
            panel6.BackColor = card;
            panel5.BorderStyle = BorderStyle.FixedSingle;
            panel6.BorderStyle = BorderStyle.FixedSingle;
            label4.ForeColor = textPrimary;
            label6.ForeColor = textPrimary;
            label4.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            label6.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            AplicarEstiloGrid(ProductosBajos, card, border, textPrimary, textMuted);
            AplicarEstiloGrid(ProductsMostSales, card, border, textPrimary, textMuted);
        }

        private void AplicarEstiloGrid(DataGridView grid, Color card, Color border, Color textPrimary, Color textMuted)
        {
            grid.BackgroundColor = card;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.RowHeadersVisible = false;
            grid.GridColor = border;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 65, 85);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = textPrimary;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9, FontStyle.Bold);
            grid.DefaultCellStyle.BackColor = card;
            grid.DefaultCellStyle.ForeColor = textPrimary;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(30, 58, 138);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(22, 32, 50);
            grid.AlternatingRowsDefaultCellStyle.ForeColor = textPrimary;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            grid.DefaultCellStyle.Padding = new Padding(3);
        }

        private async void Hoy_Click(object sender, EventArgs e)
        {
            await CargarAsync(DateTime.Today.AddDays(-1), DateTime.Today);
        }

        private async void SevenDay_Click(object sender, EventArgs e)
        {
            await CargarAsync(DateTime.Today.AddDays(-7), DateTime.Today);
            Fecha1.Value = DateTime.Today.AddDays(-7);
            Fecha2.Value = DateTime.Today;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await CargarAsync(DateTime.Today.AddDays(-30), DateTime.Today);
            Fecha1.Value = DateTime.Today.AddDays(-30);
            Fecha2.Value = DateTime.Today;
        }

        private async void Buscar_Click(object sender, EventArgs e)
        {
            await CargarAsync(Fecha1.Value, Fecha2.Value);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var reportes = new Reportes();
            await reportes.GenerarReportesAsync(Fecha1.Value, Fecha2.Value);

            string carpetaReportes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            Directory.CreateDirectory(carpetaReportes);
            string rutaCsv = Path.Combine(carpetaReportes, $"ReporteVentas_{DateTime.Now:yyyyMMdd_HHmmss}.csv");

            await reportes.ExportarReporteVentasCsvAsync(Fecha1.Value, Fecha2.Value, rutaCsv);
            MessageBox.Show($"Reporte PDF generado y CSV exportado en:\n{rutaCsv}", "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Carga las estadísticas del sistema para el rango de fechas especificado.
        /// </summary>
        private async Task CargarAsync(DateTime fecha1, DateTime fecha2)
        {
            List<Factura> facturas = await Factura.ListarFacturasPorFechaAsync(fecha1, fecha2);
            Dictionary<string, double> Products = new Dictionary<string, double>();
            ProductosBajos.Rows.Clear();
            ProductsMostSales.Rows.Clear();
            VentaPorFecha.Series.Clear();

            TFacturas.Text = facturas.Count.ToString();
            double ganancias = 0;

            var serie1 = new Series("Ventas");
            serie1.BorderWidth = 2;
            serie1.Color = Color.FromArgb(56, 189, 248);
            double total = 0;

            foreach (var factura in facturas)
            {
                serie1.Points.AddXY("No. Fact. " + factura.Id, factura.Total);
                total += factura.Total;

                if (factura.Productos != null)
                {
                    foreach (var item in factura.Productos)
                    {
                        ganancias += (item.Precio * item.Cantidad) - (item.Producto.Costo * item.Cantidad);

                        if (Products.ContainsKey(item.Producto.Nombre))
                            Products[item.Producto.Nombre] += item.Cantidad;
                        else
                            Products[item.Producto.Nombre] = item.Cantidad;
                    }
                }
            }

            TVentas.Text = total.ToString("c2");
            TGanancias.Text = ganancias.ToString("C2");
            VentaPorFecha.Series.Add(serie1);

            // Productos más vendidos
            var masVendidos = new Productos().VendidosPorProductos();
            foreach (var item in masVendidos)
                ProductsMostSales.Rows.Add(item.Key, item.Value);

            // Productos en baja existencia
            var masBajo = new Productos().BajaCantidadPorProductos();
            foreach (var item in masBajo)
                ProductosBajos.Rows.Add(item.Key, item.Value);
        }
    }


    // Puedes mover esta clase a un archivo aparte si quieres.
    public class ReporteVentas
    {
        public async Task GenerarReporteVentasPDFAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var facturas = await Factura.ListarFacturasPorFechaAsync(fechaInicio, fechaFin);
            double totalVentas = facturas.Sum(f => f.Total);

            byte[] contenidoPDF = await Task.Run(() =>
                GenerarContenidoReportePDF(facturas, totalVentas, fechaInicio, fechaFin));

            GuardarYOfrecerAbrirPDF(contenidoPDF, "ReporteVentas.pdf");
        }

        private static byte[] GenerarContenidoReportePDF(List<Factura> facturas, double totalVentas, DateTime fecha1, DateTime fecha2)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Configuraciones config = new Configuraciones().ObtenerPorId(1);
                Document doc = new Document(PageSize.A4.Rotate());
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Logo
                string imagePath = config.Imagen;
                if (File.Exists(imagePath))
                {
                    var logo = iTextSharp.text.Image.GetInstance(imagePath);
                    logo.ScaleToFit(200f, 200f);
                    logo.Alignment = Element.ALIGN_CENTER;
                    doc.Add(logo);
                }

                doc.Add(new Paragraph(config.Nombre, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18)) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph("Reporte de Ventas", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14)) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph($"Desde: {fecha1:dd/MM/yyyy} Hasta: {fecha2:dd/MM/yyyy}", FontFactory.GetFont(FontFactory.HELVETICA, 12)) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph("\n"));

                PdfPTable table = new PdfPTable(8) { WidthPercentage = 100 };
                string[] headers = {
                    "RNC / CEDULA O PASAPORTE", "COMPROBANTE FISCAL", "TIPO DE VENTA", "FECHA",
                    "MONTO", "ITBIS", "FORMA DE PAGO", "EFECTIVO"
                };
                foreach (var h in headers)
                    table.AddCell(new PdfPCell(new Phrase(h, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));

                foreach (var factura in facturas)
                {
                    table.AddCell(new PdfPCell(new Phrase(factura.RNC ?? "", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase(factura.NFC ?? "", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase(factura.TipoFactura ?? "", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase(factura.Fecha.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase(factura.Total.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase((factura.Total * 0.18).ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase(factura.TipoDePago ?? "", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase(factura.Efectivo.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                }

                doc.Add(table);
                doc.Add(new Paragraph("Total de ventas: " + totalVentas.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)) { Alignment = Element.ALIGN_CENTER });
                doc.Close();
                return ms.ToArray();
            }
        }

        private static void GuardarYOfrecerAbrirPDF(byte[] contenidoPDF, string fileName)
        {
            File.WriteAllBytes(fileName, contenidoPDF);
            if (MessageBox.Show("¿Desea abrir el reporte generado?", "Reporte generado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = fileName,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo abrir el archivo: " + ex.Message);
                }
            }
        }
    }
}
