using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Drawing.Imaging;

using static Org.BouncyCastle.Math.EC.ECCurve;
using DocumentFormat.OpenXml.Office.Drawing;
using System.Windows.Forms;

namespace SistemaFerreteriaV8.Clases
{
    public class Imprimir
    {
            public  StringBuilder line = new StringBuilder();
            string ticket = "";
            string parte1, parte2;
            public int SizeFont { set; get; }

            public static int max = 600;
            int cort;
            public  string LineasGuion()
            {
                string LineaGuion = "----------------------------------------";   // agrega lineas separadoras -

                return line.AppendLine(LineaGuion).ToString();
            }
           

        public  void EncabezadoVenta()
            {
                string LineEncavesado = "Cant   Producto            Precio    Total";   // agrega lineas de  encabezados
                line.AppendLine(LineEncavesado);
            }
            public void TextoIzquierda(string par1)                          // agrega texto a la izquierda
            {
                max = par1.Length;
                if (max > 100)                                 // **********
                {
                    cort = max - 100;
                    parte1 = par1.Remove(100, cort);        // si es mayor que 100 caracteres, lo corta
                }
                else { parte1 = par1; }                      // **********
                line.AppendLine(ticket = parte1);
            }
            public void TextoDerecha(string par1)
            {
                ticket = "";
                max = par1.Length;
                if (max > 100)                                 // **********
                {
                    cort = max - 100;
                    parte1 = par1.Remove(100, cort);           // si es mayor que 100 caracteres, lo corta
                }
                else { parte1 = par1; }                      // **********
                max = 100 - par1.Length;                     // obtiene la cantidad de espacios para llegar a 100
                for (int i = 0; i < max; i++)
                {
                    ticket += " ";                          // agrega espacios para alinear a la derecha
                }
                line.AppendLine(ticket += parte1 + "\n");                //Agrega el texto

            }
            public void TextoCentro(Graphics g, string texto)
            {
            g.DrawString(texto, new Font("Arial", 16, FontStyle.Bold), Brushes.Black, new Point((max / 2) - texto.Length, 320));


             }
         public void TextoCentro2( string texto)
            {
            ticket = "";
            max = texto.Length;
            if (max > 45)                                 // **********
            {
                cort = max - 45;
                parte1 = texto.Remove(45, cort);          // si es mayor que 45 caracteres, lo corta
            }
            else { parte1 = texto; }                      // **********
            max = (int)(45 - parte1.Length) / 2;         // saca la cantidad de espacios libres y divide entre dos
            for (int i = 0; i < max; i++)                // **********
            {
                ticket += " ";                           // Agrega espacios antes del texto a centrar
            }                                            // **********
            line.AppendLine(ticket += parte1 + "\n");
        }
        public void TextoLargo(string par1)                          // agrega texto a la izquierda
        {
            max = par1.Length;
            string textoFinal = "";
            int i = 0;
            foreach (char c in par1)
            {
                textoFinal += c;
                if (i > 40)
                {
                    textoFinal += '\n';
                    i = 0;
                }
            }
            // **********
            line.AppendLine(ticket = textoFinal);

        }

        public void LineaTop(Graphics g /*int x1, int x2*/)
        { // Dibujar línea superior
            g.DrawString("╔═════════════╦════════════════════════════════════════════╦════════════════╦",
                new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(0, 320));

            // Dibujar línea media
            g.DrawString("║ Cantidad    ║ Descripción                                  ║ Precio         ║",
                new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(0, 335));

            // Dibujar línea inferior
            g.DrawString("╚═════════════╩════════════════════════════════════════════╩════════════════╩",
                new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(0, 350));

        }
        public void GenerarLineaTabla(Graphics g, string[] textoCeldas)
        {
            // Calcular el ancho de cada celda basado en el texto más largo
            int[] anchos = new int[textoCeldas.Length];
            for (int i = 0; i < textoCeldas.Length; i++)
            {
                anchos[i] = textoCeldas[i].Length + 2; // Añadir un espacio extra a cada lado de la celda
            }

            // Calcular la longitud total de la línea
            int longitudTotal = anchos.Sum() + 1 + textoCeldas.Length; // Sumar los anchos de las celdas y los separadores

            // Generar la línea superior
            string lineaSuperior = "╔";
            for (int i = 0; i < textoCeldas.Length; i++)
            {
                lineaSuperior += new string('═', anchos[i]) + "╦";
            }
            lineaSuperior = lineaSuperior.Substring(0, lineaSuperior.Length - 1) + "╗";

            // Generar la línea de contenido
            string lineaContenido = "║";
            for (int i = 0; i < textoCeldas.Length; i++)
            {
                lineaContenido += " " + textoCeldas[i].PadRight(anchos[i] - 1) + "║";
            }

            // Generar la línea inferior
            string lineaInferior = "╚";
            for (int i = 0; i < textoCeldas.Length; i++)
            {
                lineaInferior += new string('═', anchos[i]) + "╩";
            }
            lineaInferior = lineaInferior.Substring(0, lineaInferior.Length - 1) + "╝";

            // Dibujar las líneas
            g.DrawString(lineaSuperior, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(0, 320));
            g.DrawString(lineaContenido, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(0, 335));
            g.DrawString(lineaInferior, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(0, 350));
        }
        public string TextoExtremos(string par1, string par2)
            {
                max = par1.Length;
                if (max > 300)                                 // **********
                {
                    cort = max - 300;
                    parte1 = par1.Remove(300, cort);          // si par1 es mayor que 50 lo corta
                }
                else { parte1 = par1; }                      // **********
                ticket = parte1;                             // agrega el primer parametro
                max = par2.Length;
                if (max > 300)                                 // **********
                {
                    cort = max - 300;
                    parte2 = par2.Remove(300, cort);          // si par2 es mayor que 50 lo corta
                }
                else { parte2 = par2; }
                max = 600 - (parte1.Length + parte2.Length);
                for (int i = 0; i < max; i++)                 // **********
                {
                    ticket += " ";                            // Agrega espacios para poner par2 al final
                }                                             // **********
                return ticket += parte2 ;                   // agrega el segundo parametro al final

            }
            public void AgregaTotales(string par1, double total)
            {
                max = par1.Length;
                if (max > 25)                                 // **********
                {
                    cort = max - 25;
                    parte1 = par1.Remove(25, cort);          // si es mayor que 25 lo corta
                }
                else { parte1 = par1; }                      // **********
                ticket = parte1;

                parte2 = "$ " + total.ToString();
                max = 100 - (parte1.Length + parte2.Length);
                for (int i = 0; i < max; i++)                // **********
                {
                    ticket += " ";                           // Agrega espacios para poner el valor de moneda al final
                }                                            // **********
                line.AppendLine(ticket += parte2);

            }

            // se le pasan los Aticulos  con sus detalles
            public void AgregaArticulo(double precio, string Articulo, double cant, double subtotal)
            {
                string elementos = "";
                string espacios = "";
                string[] art = new string[2];
                elementos = cant.ToString();
                int i = 1;
                while (7 - cant.ToString().Length >= i)
                {
                    espacios += " ";
                    i++;
                }
                if (Articulo.Length >= 50)
                {
                    int o = 0;
                    foreach (char item in Articulo)
                    {
                        if (o < 50)
                        {
                            art[0] += item;
                        }
                        else
                        {
                            art[1] += item;
                        }
                        o++;
                    }
                    elementos += espacios + art[0];
                }
                else
                {
                    elementos += espacios + Articulo;
                }

                i = 1;
                espacios = "";

                while (50 - Articulo.Length >= i)
                {
                    espacios += " ";
                    i++;
                }
                if (Articulo.Length >= 50)
                {
                    espacios = "  ";
                }
                elementos += espacios + precio.ToString();

                i = 1;
                espacios = "";
                while (10 - cant.ToString().Length >= i)
                {
                    espacios += " ";
                    i++;
                }
                elementos += espacios + subtotal.ToString();
                if (Articulo.Length > 50)
                {
                    elementos += "\n       " + art[1];
                }
                line.AppendLine(elementos);


            }

            public void ImprimirFactura(string name)
            {
                var pd = new PrintDocument { PrinterSettings = { PrinterName = name } };
                pd.PrintPage += PrintPage;
                pd.Print();
            }


        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            if(new Configuraciones().ObtenerPorId(1).Nombre != "Ferreteria el mello"){
                Image logo;
                logo = Properties.Resources.logo_Alfa_Ferreteria;
                //var img = Properties.Resources.logo_Alfa_Ferreteria; // Acceder a la imagen desde los recursos


                Graphics graphics = ev.Graphics;

                // Definir el tamaño y la posición de la imagen
                Rectangle imagenRect = new Rectangle(98, 10, 100, 100);

                // Dibujar la imagen en el rectángulo especificado con suavizado
                using (var attributes = new ImageAttributes())
                {
                    attributes.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(logo, imagenRect, 0, 0, logo.Width, logo.Height, GraphicsUnit.Pixel, attributes);
                }

                Font fuente = new Font("Courier New", 7);
                Brush brush = Brushes.Black;
                PointF punto = new PointF(0, 120);
                graphics.DrawString(line.ToString().Split('|')[0], fuente, brush, punto);

                line = new StringBuilder();
            }
        }

        //imprimir totales

        public void ImprimirFacturaPrecio(string name)
        {
            try
            {
                var pd = new PrintDocument { PrinterSettings = { PrinterName = "Microsoft XPS Document Writer" } };
                pd.PrintPage += PrintPagePrecio;
                pd.Print();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al imprimir: {ex.Message}");
            }
        }


        private void PrintPagePrecio(object sender, PrintPageEventArgs ev)
        {
            if (new Configuraciones().ObtenerPorId(1).Nombre != "Ferreteria el mello")
            {
               

                // Definir el tamaño y la posición de la imagen
                
                Font fuente = new Font("Courier New", SizeFont,FontStyle.Bold);
                Brush brush = Brushes.Black;
                PointF punto = new PointF(0, 120);
                //graphics.DrawString(line.ToString(), fuente, brush, punto);
                ev.Graphics.DrawString(line.ToString(), fuente, brush,punto);

                line = new StringBuilder();
            }
        }

    }
}