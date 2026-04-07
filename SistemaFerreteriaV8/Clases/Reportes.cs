using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic;

namespace SistemaFerreteriaV8.Clases
{
    /// <summary>
    /// Clase para la generación de reportes PDF y otras utilidades de impresión.
    /// </summary>
    public class Reportes
    {
        public Factura FacturaActiva { get; set; }
        public List<ListProduct> Productos { get; set; }

        /// <summary>
        /// Genera un reporte PDF de la venta actual de forma asíncrona y lo abre.
        /// </summary>
        public async Task GenerarReporteVentasPDFAsync()
        {
            try
            {
                byte[] contenidoPDF = await GenerarFacturaMatriciarAsync();
                string filePath = "factura.pdf";
                await File.WriteAllBytesAsync(filePath, contenidoPDF);

                if (File.Exists(filePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("No se pudo encontrar el archivo generado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar abrir el archivo PDF: " + ex.Message);
                MessageBox.Show("Error al abrir el archivo PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Genera y abre un PDF de tipo Conduce.
        /// </summary>
        public async void GenerarConducePDF()
        {
            try
            {
                byte[] contenidoPDF = await GenerarConduce();
                string filePath = "Conduce.pdf";
                File.WriteAllBytes(filePath, contenidoPDF);

                if (File.Exists(filePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("No se pudo encontrar el archivo generado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar abrir el archivo PDF: " + ex.Message);
            }
        }

        /// <summary>
        /// Genera un reporte de ventas por fechas y lo abre.
        /// </summary>
        // En tu clase de Reportes o similar:
        public async Task GenerarReportesAsync(DateTime fecha1, DateTime fecha2)
        {
            // 1. Obtener la lista de facturas asincrónicamente
            var lista = await Factura.ListarFacturasPorFechaAsync(fecha1, fecha2);

            // 2. Generar el PDF (esto puede ser un Task si el proceso es pesado)
            await Task.Run(() =>
            {
                GenerarReportes(lista, fecha1, fecha2);
            });
        }

        /// <summary>
        /// Exporta el resumen de ventas a CSV (útil para BI/Excel).
        /// </summary>
        public async Task ExportarReporteVentasCsvAsync(DateTime fecha1, DateTime fecha2, string rutaArchivo)
        {
            var facturas = await Factura.ListarFacturasPorFechaAsync(fecha1, fecha2);
            var facturasValidas = facturas.Where(f => f != null && !f.Cotizacion && !f.Eliminada).ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Id,Fecha,Cliente,RNC,TipoFactura,TipoPago,Total,Efectivo");

            foreach (var f in facturasValidas)
            {
                string cliente = (f.NombreCliente ?? string.Empty).Replace(',', ' ');
                string rnc = (f.RNC ?? string.Empty).Replace(',', ' ');
                string tipoFactura = (f.TipoFactura ?? string.Empty).Replace(',', ' ');
                string tipoPago = (f.TipoDePago ?? string.Empty).Replace(',', ' ');

                sb.AppendLine($"{f.Id},{f.Fecha:yyyy-MM-dd HH:mm:ss},{cliente},{rnc},{tipoFactura},{tipoPago},{f.Total:F2},{f.Efectivo:F2}");
            }

            await File.WriteAllTextAsync(rutaArchivo, sb.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// Genera un reporte de ventas con una lista de facturas.
        /// </summary>
        public void GenerarReportes(List<Factura> lista, DateTime fecha1, DateTime fecha2)
        {
            try
            {
                byte[] contenidoPDF = GenerarReporte(lista, fecha1, fecha2);
                string filePath = "Conduce.pdf";
                File.WriteAllBytes(filePath, contenidoPDF);

                if (File.Exists(filePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("No se pudo encontrar el archivo generado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar abrir el archivo PDF: " + ex.Message);
            }
        }

        // Helper para texto alineado a extremos
        private void TextoExtremo(Document doc, string texto1, string texto2, int fontsize = 10, string font = FontFactory.HELVETICA, string font2 = FontFactory.HELVETICA)
        {
            PdfPTable table2 = new PdfPTable(2) { WidthPercentage = 100 };
            PdfPCell cellNombre = new PdfPCell(new Phrase(texto1, FontFactory.GetFont(font, fontsize))) { Border = PdfPCell.NO_BORDER };
            PdfPCell cellFecha = new PdfPCell(new Phrase(texto2, FontFactory.GetFont(font2, fontsize))) { Border = PdfPCell.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT };
            table2.AddCell(cellNombre);
            table2.AddCell(cellFecha);
            doc.Add(table2);
        }

        // Helper para texto centrado
        private void TextoCentro(Document doc, string texto1, int fontsize = 14, string font = FontFactory.HELVETICA)
        {
            doc.Add(new Paragraph(texto1, FontFactory.GetFont(font, fontsize)) { Alignment = Element.ALIGN_CENTER });
        }

        // Helper para texto a la izquierda
        private void TextoIzquierda(Document doc, string texto1, int fontsize = 14, string font = FontFactory.HELVETICA)
        {
            doc.Add(new Paragraph(texto1, FontFactory.GetFont(font, fontsize)) { Alignment = Element.ALIGN_LEFT });
        }

        /// <summary>
        /// Genera un PDF de la factura activa de manera asíncrona.
        /// </summary>
        /// <summary>
        /// Genera la factura actual en formato PDF, lista para impresión en impresora matricial.
        /// </summary>
        /// <returns>Un arreglo de bytes con el contenido PDF.</returns>
        public async Task<byte[]> GenerarFacturaMatriciarAsync()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var config = new Configuraciones().ObtenerPorId(1) ?? new Configuraciones();
                string imagePath = config.Imagen ?? string.Empty;

                var productosTMP = new List<ListProduct>();
                double totalSinProcesar = 0;
                double totalProcesado = 0;

                foreach (var item in FacturaActiva.Productos)
                {
                    productosTMP.Add(item);

                    // Cuando agrupamos de a 5 productos o llegamos al último
                    if (productosTMP.Count == 5 || item == FacturaActiva.Productos.Last())
                    {
                        // Logo
                        if (File.Exists(imagePath))
                        {
                            var logo = iTextSharp.text.Image.GetInstance(imagePath);
                            logo.ScaleToFit(70f, 70f);
                            logo.Alignment = Element.ALIGN_LEFT;
                            doc.Add(logo);
                        }

                        TextoIzquierda(doc, config.Nombre?.ToUpper() ?? "FERRETERÍA");
                        TextoExtremo(doc, config.Direccion, FacturaActiva.TipoFactura, font2: FontFactory.HELVETICA_BOLD);

                        // Detalle de comprobante fiscal y asignación NFC si aplica
                        await AsignarNFCYDatosClienteAsync(doc, config);

                        // Info cliente y encabezado tabla
                        TextoExtremo(doc, "Cliente: " + FacturaActiva.NombreCliente, "Fecha: " + FacturaActiva.Fecha.ToString("dd/MM/yyyy"));
                        if (!string.IsNullOrWhiteSpace(FacturaActiva.RNC))
                            TextoExtremo(doc, "RNC: " + FacturaActiva.RNC, "No. Factura: " + FacturaActiva.Id);
                        TextoExtremo(doc, "Dirección: " + FacturaActiva.Direccion, "");
                        if (FacturaActiva.IdCliente != "0")
                        {
                            Cliente cl = await new Cliente().BuscarAsync(FacturaActiva.IdCliente);
                            TextoExtremo(doc, "Tel: " + (cl?.Telefono ?? ""), "");
                        }
                        doc.Add(new Paragraph("\n") { Alignment = Element.ALIGN_CENTER });

                        // Tabla productos
                        PdfPTable table = new PdfPTable(6) { WidthPercentage = 100 };
                        float[] columnWidths = { 10f, 30f, 15f, 15f, 20f, 20f };
                        table.SetWidths(columnWidths);

                        table.AddCell(new PdfPCell(new Phrase("Cantidad", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase("Artículo", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase("Precio", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase("ITBIS", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase("Precio Neto", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase("Subtotal", FontFactory.GetFont(FontFactory.HELVETICA, 10))));

                        foreach (var factura in productosTMP)
                        {
                            if (factura.Producto != null && factura.Producto.Categoria?.Trim() == "Sin Procesar")
                            {
                                double itebis = (factura.Precio / 1.18);
                                double precio = factura.Precio - itebis;
                                table.AddCell(new PdfPCell(new Phrase(factura.Cantidad.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                                table.AddCell(new PdfPCell(new Phrase(factura.Producto.Nombre, FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                                table.AddCell(new PdfPCell(new Phrase(factura.Precio.ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                                table.AddCell(new PdfPCell(new Phrase("0", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                                table.AddCell(new PdfPCell(new Phrase(factura.Precio.ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                                table.AddCell(new PdfPCell(new Phrase((factura.Precio * factura.Cantidad).ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                                totalSinProcesar += factura.Precio * factura.Cantidad;
                            }
                            else
                            {
                                double precio = (factura.Precio / 1.18);
                                double itebis = factura.Precio - precio;
                                table.AddCell(new PdfPCell(new Phrase(factura.Cantidad.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                                table.AddCell(new PdfPCell(new Phrase(factura.Producto.Nombre, FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                                table.AddCell(new PdfPCell(new Phrase(precio.ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                                table.AddCell(new PdfPCell(new Phrase(itebis.ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                                table.AddCell(new PdfPCell(new Phrase(factura.Precio.ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                                table.AddCell(new PdfPCell(new Phrase((factura.Precio * factura.Cantidad).ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                                totalProcesado += factura.Precio * factura.Cantidad;
                            }
                        }

                        doc.Add(table);
                        doc.Add(new Paragraph("\n") { Alignment = Element.ALIGN_CENTER });
                        productosTMP.Clear();
                    }
                }

                // Pie de página: totales, notas, firmas
                TextoExtremo(doc, "Nota:  " + FacturaActiva.Description,
                    "Sub total: " + ((totalSinProcesar) + (totalProcesado / 1.18)).ToString("c2") + "\n"
                    + "ITBIS: " + (totalProcesado - (totalProcesado / 1.18)).ToString("c2") + "\n"
                    + "Total de venta: " + (totalSinProcesar + totalProcesado).ToString("c2"));

                doc.Add(new Paragraph("\n") { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph("__________________________                               __________________________", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8)) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph("      Despachado por                                                     Recibido por       ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8)) { Alignment = Element.ALIGN_CENTER });

                doc.Close();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Lógica auxiliar para asignación de comprobantes y datos de cliente en PDF (refactoriza según necesidad).
        /// </summary>
        private async Task AsignarNFCYDatosClienteAsync(Document doc, Configuraciones config)
        {
            string acom = "";
            if (FacturaActiva.TipoFactura == "Consumo")
            {
                acom = "B02";
                if (string.IsNullOrWhiteSpace(FacturaActiva.NFC))
                {
                    double ultimoNFC = double.Parse(config.SCCA);
                    if (ultimoNFC <= double.Parse(config.SCCF))
                    {
                        string numeroFormateado = (ultimoNFC + 1).ToString().PadLeft(8, '0');
                        config.SCCA = numeroFormateado;
                        FacturaActiva.NFC = numeroFormateado;
                        config.Guardar();
                        await FacturaActiva.ActualizarFacturaAsync();
                    }
                    else
                    {
                        MessageBox.Show("Ya alcanzó su secuencia de comprobante fiscal máxima");
                    }
                }
            }
            else if (FacturaActiva.TipoFactura == "Comprobante Fiscal" && string.IsNullOrEmpty(FacturaActiva.RNC))
            {
                if (FacturaActiva.RNC == null || string.IsNullOrEmpty(FacturaActiva.RNC))
                {
                    string[] datos = BuscarPorRNC(Interaction.InputBox("Favor digitar el RNC:", "Busqueda de RNC"));
                    if (datos != null && datos[0] != null)
                    {
                        FacturaActiva.RNC = datos[0];
                        FacturaActiva.NombreCliente = datos[1];
                        await FacturaActiva.ActualizarFacturaAsync();
                    }
                    else
                    {
                        MessageBox.Show("Este código o RNC no pertenece a ningún cliente!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                acom = "B01";
                if (string.IsNullOrWhiteSpace(FacturaActiva.NFC))
                {
                    double ultimoNFC = double.Parse(config.NFCActual);
                    if (ultimoNFC <= double.Parse(config.NFCFinal))
                    {
                        string numeroFormateado = (ultimoNFC + 1).ToString().PadLeft(8, '0');
                        config.SCCA = numeroFormateado;
                        FacturaActiva.NFC = numeroFormateado;
                        config.Guardar();
                        await FacturaActiva.ActualizarFacturaAsync();
                    }
                    else
                    {
                        MessageBox.Show("Ya alcanzó su secuencia de comprobante fiscal máxima");
                    }
                }
            }
            else if (FacturaActiva.TipoFactura == "Comprobante Gubernamental")
            {
                if (FacturaActiva.RNC == null || string.IsNullOrEmpty(FacturaActiva.RNC))
                {
                    string[] datos = BuscarPorRNC(Interaction.InputBox("Favor digitar el RNC:", "Busqueda de RNC"));
                    if (datos != null && datos[0] != null)
                    {
                        FacturaActiva.RNC = datos[0];
                        FacturaActiva.NombreCliente = datos[1];
                        await FacturaActiva.ActualizarFacturaAsync();
                    }
                    else
                    {
                        MessageBox.Show("Este código o RNC no pertenece a ningún cliente!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                acom = "B15";
                if (string.IsNullOrWhiteSpace(FacturaActiva.NFC))
                {
                    double ultimoNFC = double.Parse(config.SGA);
                    if (ultimoNFC <= double.Parse(config.SGF))
                    {
                        string numeroFormateado = (ultimoNFC + 1).ToString().PadLeft(8, '0');
                        config.SCCA = numeroFormateado;
                        FacturaActiva.NFC = numeroFormateado;
                        config.Guardar();
                        await FacturaActiva.ActualizarFacturaAsync();
                    }
                    else
                    {
                        MessageBox.Show("Ya alcanzó su secuencia de comprobante fiscal máxima");
                    }
                }
            }

            // Encabezado comprobante fiscal
            TextoExtremo(doc, "RNC: " + config.RNC, "NFC: " + acom + (FacturaActiva.NFC ?? ""));
            TextoExtremo(doc, "Tel: " + config.Telefono, "Válido Hasta: " + config.FechaExpiracion.ToShortDateString());
            doc.Add(new Paragraph("\n") { Alignment = Element.ALIGN_CENTER });
        }

        /// <summary>
        /// Genera el PDF de conduce.
        /// </summary>
        private async Task<byte[]> GenerarConduce()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                string imagePath = "logo.png"; // Ruta por defecto del logo
                Configuraciones config = new Configuraciones().ObtenerPorId(1);
                if (config != null) {  imagePath = config.Imagen; }

                List<ListProduct> productosTMP = new List<ListProduct>();
                double totalTemporal = 0;
                double totalTemporal1 = 0;

                // Agrega logo si existe
                if (File.Exists(imagePath))
                {
                    var logo = iTextSharp.text.Image.GetInstance(imagePath);
                    logo.ScaleToFit(70f, 70f);
                    logo.Alignment = Element.ALIGN_LEFT;
                    doc.Add(logo);
                }

                // Encabezado
                TextoIzquierda(doc, config.Nombre?.ToUpper() ?? "FERRETERÍA");
                TextoExtremo(doc, config.Direccion, "Conduce", font2: FontFactory.HELVETICA_BOLD);

                // Numeración NCF si aplica
                double ultimoNFC = double.TryParse(config.UltimoNFC, out double valNFC) ? valNFC : 0;
                if (ultimoNFC <= double.Parse(config.NFCFinal ?? "0"))
                {
                    if (FacturaActiva.NFC == null)
                    {
                        string numeroFormateado = (ultimoNFC + 1).ToString().PadLeft(8, '0');
                        if (FacturaActiva.TipoFactura == "Consumo")
                        {
                            FacturaActiva.NFC = "NCF: B02" + numeroFormateado;
                        }
                        else if (FacturaActiva.TipoFactura == "Comprobante")
                        {
                            FacturaActiva.NFC = "NCF: B01" + numeroFormateado;
                        }
                        FacturaActiva.ActualizarFacturaAsync();
                        config.UltimoNFC = numeroFormateado;
                        config.Guardar();
                    }
                }

                TextoExtremo(doc, "RNC: " + config.RNC, FacturaActiva.NFC ?? "");
                TextoExtremo(doc, "Tel: " + config.Telefono, "Válido Hasta: " + config.FechaExpiracion.ToShortDateString());
                doc.Add(new Paragraph("\n") { Alignment = Element.ALIGN_CENTER });

                TextoExtremo(doc, "Cliente: " + FacturaActiva.NombreCliente, "Fecha: " + FacturaActiva.Fecha.ToString("dd/MM/yyyy"));
                if (!string.IsNullOrWhiteSpace(FacturaActiva.RNC))
                {
                    TextoExtremo(doc, "RNC: " + FacturaActiva.RNC, "No. Conduce: " + FacturaActiva.Id);
                }
                TextoExtremo(doc, "Dirección: " + FacturaActiva.Direccion, "No. Conduce: " + FacturaActiva.Id);
                if (FacturaActiva.IdCliente != "0")
                {
                    Cliente cl =await new Cliente().BuscarAsync(FacturaActiva.IdCliente);
                    TextoExtremo(doc, "Tel: " + (cl?.Telefono ?? ""), "");
                }
                doc.Add(new Paragraph("\n") { Alignment = Element.ALIGN_CENTER });

                // Tabla de productos
                PdfPTable table = new PdfPTable(6)
                {
                    WidthPercentage = 100
                };
                float[] columnWidths = { 10f, 30f, 15f, 15f, 20f, 20f };
                table.SetWidths(columnWidths);

                table.AddCell(new PdfPCell(new Phrase("Cantidad", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                table.AddCell(new PdfPCell(new Phrase("Artículo", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                table.AddCell(new PdfPCell(new Phrase("Precio", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                table.AddCell(new PdfPCell(new Phrase("ITBIS", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                table.AddCell(new PdfPCell(new Phrase("Precio Neto", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                table.AddCell(new PdfPCell(new Phrase("Subtotal", FontFactory.GetFont(FontFactory.HELVETICA, 10))));

                foreach (var item in FacturaActiva.Productos)
                {
                    if (item.Producto != null && item.Producto.Categoria?.Trim() == "Sin Procesar")
                    {
                        double itbis = (item.Precio / 1.18);
                        double precio = item.Precio - itbis;
                        table.AddCell(new PdfPCell(new Phrase(item.Cantidad.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase(item.Producto.Nombre, FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase(item.Precio.ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase("0", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase(item.Precio.ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase((item.Precio * item.Cantidad).ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        totalTemporal1 += item.Precio * item.Cantidad;
                    }
                    else
                    {
                        double precio = (item.Precio / 1.18);
                        double itbis = item.Precio - precio;
                        table.AddCell(new PdfPCell(new Phrase(item.Cantidad.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase(item.Producto.Nombre, FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase(precio.ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase(itbis.ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase(item.Precio.ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        table.AddCell(new PdfPCell(new Phrase((item.Precio * item.Cantidad).ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                        totalTemporal += item.Precio * item.Cantidad;
                    }
                }

                doc.Add(table);
                doc.Add(new Paragraph("\n") { Alignment = Element.ALIGN_CENTER });

                // Totales y pie de página
                TextoExtremo(doc, "Nota:  " + FacturaActiva.Description,
                    "Sub total: " + ((totalTemporal1 + totalTemporal) - (totalTemporal / 1.18)).ToString("c2") + "\n"
                    + "ITBIS: " + (totalTemporal - (totalTemporal / 1.18)).ToString("c2") + "\n"
                    + "Total de venta: " + (totalTemporal1 + totalTemporal).ToString("c2"));

                doc.Add(new Paragraph("\n") { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph("__________________________                               __________________________", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8)) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph("      Despachado por                                                     Recibido por       ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8)) { Alignment = Element.ALIGN_CENTER });

                doc.Close();
                return ms.ToArray();
            }
        }


        /// <summary>
        /// Genera el reporte de ventas PDF con todas las facturas del rango dado.
        /// </summary>
        private byte[] GenerarReporte(List<Factura> facturas, DateTime fecha1, DateTime fecha2)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Configuraciones config = new Configuraciones().ObtenerPorId(1);
                Document doc = new Document(PageSize.A4.Rotate());
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                double totalVentas = 0;

                string imagePath = config.Imagen;
                if (File.Exists(imagePath))
                {
                    using (var bitmap = new Bitmap(imagePath))
                    {
                        var logo = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Png);
                        logo.ScaleToFit(200f, 200f);
                        logo.Alignment = Element.ALIGN_CENTER;
                        doc.Add(logo);
                    }
                }

                TextoCentro(doc, config.Nombre, 18, FontFactory.HELVETICA_BOLD);
                TextoCentro(doc, "Reporte de Ventas", 14, FontFactory.HELVETICA_BOLD);
                TextoCentro(doc, $"Desde: {fecha1:dd/MM/yyyy}  Hasta: {fecha2:dd/MM/yyyy}", 12, FontFactory.HELVETICA);
                doc.Add(new Paragraph("\n"));

                var facturasValidas = facturas.Where(f => f != null && !f.Cotizacion && !f.Eliminada).ToList();
                var totalFacturas = facturasValidas.Count;
                var totalGeneral = facturasValidas.Sum(f => f.Total);
                var ticketPromedio = totalFacturas > 0 ? totalGeneral / totalFacturas : 0;
                var totalEfectivo = facturasValidas
                    .Where(f => string.Equals(f.TipoDePago, "Efectivo", StringComparison.OrdinalIgnoreCase))
                    .Sum(f => f.Total);
                var totalCredito = facturasValidas
                    .Where(f => string.Equals(f.TipoDePago, "Credito", StringComparison.OrdinalIgnoreCase) || string.Equals(f.TipoDePago, "Crédito", StringComparison.OrdinalIgnoreCase))
                    .Sum(f => f.Total);
                var porcentajeEfectivo = totalGeneral > 0 ? (totalEfectivo / totalGeneral) * 100 : 0;

                PdfPTable tablaResumen = new PdfPTable(4) { WidthPercentage = 100 };
                tablaResumen.SetWidths(new float[] { 25f, 25f, 25f, 25f });
                tablaResumen.AddCell(new PdfPCell(new Phrase("Facturas", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                tablaResumen.AddCell(new PdfPCell(new Phrase("Ventas Totales", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                tablaResumen.AddCell(new PdfPCell(new Phrase("Ticket Promedio", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                tablaResumen.AddCell(new PdfPCell(new Phrase("% Efectivo", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))) { BackgroundColor = BaseColor.LIGHT_GRAY });

                tablaResumen.AddCell(new PdfPCell(new Phrase(totalFacturas.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                tablaResumen.AddCell(new PdfPCell(new Phrase(totalGeneral.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                tablaResumen.AddCell(new PdfPCell(new Phrase(ticketPromedio.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                tablaResumen.AddCell(new PdfPCell(new Phrase(porcentajeEfectivo.ToString("N2") + "%", FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                doc.Add(tablaResumen);
                doc.Add(new Paragraph("\n"));

                PdfPTable table = new PdfPTable(8);
                float[] columnWidths = { 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f };
                table.SetWidths(columnWidths);

                table.AddCell(new PdfPCell(new Phrase("RNC / CÉDULA", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
                table.AddCell(new PdfPCell(new Phrase("NCF", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
                table.AddCell(new PdfPCell(new Phrase("TIPO VENTA", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
                table.AddCell(new PdfPCell(new Phrase("FECHA", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
                table.AddCell(new PdfPCell(new Phrase("MONTO", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
                table.AddCell(new PdfPCell(new Phrase("ITBIS", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
                table.AddCell(new PdfPCell(new Phrase("PAGO", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
                table.AddCell(new PdfPCell(new Phrase("EFECTIVO", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));

                foreach (var factura in facturasValidas)
                {
                    table.AddCell(new PdfPCell(new Phrase(factura.RNC ?? string.Empty, FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase(
                        factura.NFC != null && factura.NFC.Split(':').Length >= 2 ? factura.NFC.Split(':')[1] : "",
                        FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase(factura.TipoFactura ?? string.Empty, FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase(factura.Fecha.ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase(factura.Total.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase((factura.Total * 0.18).ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase(factura.TipoDePago ?? string.Empty, FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    table.AddCell(new PdfPCell(new Phrase(factura.Efectivo.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));

                    totalVentas += factura.Total;
                }

                doc.Add(table);
                doc.Add(new Paragraph("\nTop 5 clientes por monto vendido", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));

                var topClientes = facturasValidas
                    .GroupBy(f => string.IsNullOrWhiteSpace(f.NombreCliente) ? "Cliente sin nombre" : f.NombreCliente)
                    .Select(g => new { Cliente = g.Key, Total = g.Sum(x => x.Total) })
                    .OrderByDescending(x => x.Total)
                    .Take(5)
                    .ToList();

                PdfPTable tablaTopClientes = new PdfPTable(2) { WidthPercentage = 60 };
                tablaTopClientes.SetWidths(new float[] { 70f, 30f });
                tablaTopClientes.AddCell(new PdfPCell(new Phrase("Cliente", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                tablaTopClientes.AddCell(new PdfPCell(new Phrase("Total", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))) { BackgroundColor = BaseColor.LIGHT_GRAY });

                foreach (var cliente in topClientes)
                {
                    tablaTopClientes.AddCell(new PdfPCell(new Phrase(cliente.Cliente, FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                    tablaTopClientes.AddCell(new PdfPCell(new Phrase(cliente.Total.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10))));
                }

                doc.Add(tablaTopClientes);
                doc.Add(new Paragraph("Total de ventas: " + totalVentas.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph("Ventas a crédito: " + totalCredito.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10)) { Alignment = Element.ALIGN_CENTER });
                doc.Close();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Busca información de un cliente por RNC en un archivo local.
        /// </summary>
        private string[] BuscarPorRNC(string rnc)
        {
            string rutaArchivo = @"rnc.txt";
            string[] datos = new string[2];

            if (File.Exists(rutaArchivo))
            {
                foreach (string linea in File.ReadAllLines(rutaArchivo))
                {
                    if (linea.Contains(rnc))
                    {
                        // La línea debe estar separada por "|"
                        MessageBox.Show("RNC: " + linea.Split('|')[0] +
                            "\nNombre: " + linea.Split('|')[1] +
                            "\nDescripcion: " + linea.Split('|')[3] +
                            $"\nFecha: {linea.Split('|')[8]}  \nEstado: {linea.Split('|')[9]}"
                            , "RNC encontrado!");

                        datos[0] = rnc;
                        datos[1] = linea.Split('|')[1];
                        return datos;
                    }
                }
            }
            else
            {
                Console.WriteLine("El archivo no existe.");
            }
            return datos;
        }
    }
}
