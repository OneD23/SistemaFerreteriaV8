using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;

namespace SistemaFerreteriaV8.Clases
{
    class PrinterClass
    {
        private StringBuilder _content;
        private Image _logo;
        private int _lineWidth;
        public string PrinterName { get; set; } // Nombre de la impresora

        public PrinterClass(int lineWidth = 48)
        {
            _content = new StringBuilder();
            _lineWidth = lineWidth; // Ancho del papel configurable
        }

        // Configurar el logotipo
        public void SetLogo(Image logo)
        {
            _logo = logo;
        }

        // Agregar una línea al contenido
        public void AddLine(string text)
        {
            _content.AppendLine(FormatLine(text));
        }

        // Agregar una línea centrada
        public void AddCenteredLine(string text)
        {
            int spaces = (_lineWidth - text.Length) / 2;
            if (spaces > 0)
            {
                _content.AppendLine(new string(' ', spaces) + text);
            }
            else
            {
                _content.AppendLine(text);
            }
        }

        // Agregar una línea alineada a la izquierda
        public void AddLeftAlignedLine(string text)
        {
            _content.AppendLine(text.PadRight(_lineWidth));
        }

        // Agregar una línea alineada a la derecha
        public void AddRightAlignedLine(string text)
        {
            _content.AppendLine(text.PadLeft(_lineWidth));
        }

        // Agregar una línea separadora
        public void AddSeparator()
        {
            _content.AppendLine(new string('-', _lineWidth));
        }

        // Método para agregar un artículo formateado
        public void AgregaArticulo(double precio, string articulo, double cantidad, double subtotal)
        {
            int cantidadWidth = 7;
            int maxArticuloLength = _lineWidth - (cantidadWidth + 20);
            int precioWidth = 10;
            int subtotalWidth = 10;

            StringBuilder elementos = new StringBuilder();
            string cantidadFormatted = cantidad.ToString().PadRight(cantidadWidth);
            string[] articuloPartes = DivideTexto(articulo, maxArticuloLength);
            elementos.Append(cantidadFormatted);
            elementos.Append(articuloPartes[0].PadRight(maxArticuloLength));
            elementos.Append(precio.ToString("C2").PadLeft(precioWidth));
            elementos.Append(subtotal.ToString("C2").PadLeft(subtotalWidth));

            if (!string.IsNullOrWhiteSpace(articuloPartes[1]))
            {
                elementos.AppendLine();
                elementos.Append("".PadRight(cantidadWidth));
                elementos.Append(articuloPartes[1]);
            }

            _content.AppendLine(elementos.ToString());
        }

        private string[] DivideTexto(string texto, int maxLength)
        {
            string[] partes = new string[2];
            if (texto.Length > maxLength)
            {
                partes[0] = texto.Substring(0, maxLength);
                partes[1] = texto.Substring(maxLength);
            }
            else
            {
                partes[0] = texto;
                partes[1] = "";
            }
            return partes;
        }

        // Imprimir el contenido actual
        public void Print(string text)
        {
            PrintDocument printDocument = new PrintDocument();

            // Configurar la impresora deseada
            if (!string.IsNullOrWhiteSpace(PrinterName))
            {
                printDocument.PrinterSettings.PrinterName = PrinterName;
            }

            printDocument.PrintPage += (sender, e) =>
            {
                float yPos = 0;
                float lineHeight = e.Graphics.MeasureString("A", new Font("Courier New", 11)).Height; // Altura de cada línea

                
                // Imprimir cada línea del contenido
               
                   e.Graphics.DrawString("", new Font("Courier New", 11), Brushes.Black, new PointF(0, yPos));
               
            };

            try
            {
                printDocument.Print();
            }
            catch (Exception ex)
            {
                
            }
        }


        public void Clear()
        {
            _content.Clear();
        }

        private string FormatLine(string text)
        {
            return text.Length > _lineWidth ? text.Substring(0, _lineWidth) : text;
        }
    }
}
