using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Drawing;

namespace SistemaFerreteriaV8.Clases
{
    class CreaTicket2
    {
        public static StringBuilder line = new StringBuilder();
        string ticket = "";
        string parte1, parte2;
        string rutaLogo = @"C:/Users/wmr22/Downloads/logo.png"; // Ruta del archivo de imagen del logo


        public static int max = 45;
        int cort;
        public string LineasGuion()
        {
            string LineaGuion = "----------------------------------------";   // agrega lineas separadoras -

            return line.AppendLine(LineaGuion).ToString();
        }

        // Método para obtener un carácter ASCII basado en un valor de escala de grises
        private char GetAsciiChar(int grayValue)
        {
            // Lista de caracteres ASCII en orden de oscurecimiento
            char[] asciiChars = { ' ', '.', ':', '-', '=', '+', '*', '#', '%', '@' };

            // Calcular el índice del carácter ASCII basado en el valor de escala de grises
            int index = (grayValue * (asciiChars.Length - 1)) / 255;

            // Devolver el carácter ASCII correspondiente
            return asciiChars[index];
        }

        // Método principal para imprimir el ticket
        public void ImprimirTiket(string stringimpresora)
        {
          

            RawPrinterHelper.SendStringToPrinter(stringimpresora, line.ToString());

            line = new StringBuilder();
        }

        //fin prueba
        public void EncabezadoVenta()
        {
            string LineEncavesado = "Cant   Producto         Precio   Total";   // agrega lineas de  encabezados
            line.AppendLine(LineEncavesado);
        }
        public void TextoIzquierda(string par1)                          // agrega texto a la izquierda
        {
            max = par1.Length;
            if (max > 40)                                 // **********
            {
                cort = max - 40;
                parte1 = par1.Remove(40, cort);        // si es mayor que 40 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            line.AppendLine(ticket = parte1);

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
        public void TextoDerecha(string par1)
        {
            ticket = "";
            max = par1.Length;
            if (max > 40)                                 // **********
            {
                cort = max - 40;
                parte1 = par1.Remove(40, cort);           // si es mayor que 40 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            max = 40 - par1.Length;                     // obtiene la cantidad de espacios para llegar a 40
            for (int i = 0; i < max; i++)
            {
                ticket += " ";                          // agrega espacios para alinear a la derecha
            }
            line.AppendLine(ticket += parte1 + "\n");                //Agrega el texto

        }
        public void TextoCentro(string par1)
        {
            ticket = "";
            max = par1.Length;
            if (max > 40)                                 // **********
            {
                cort = max - 40;
                parte1 = par1.Remove(40, cort);          // si es mayor que 40 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            max = (int)(40 - parte1.Length) / 2;         // saca la cantidad de espacios libres y divide entre dos
            for (int i = 0; i < max; i++)                // **********
            {
                ticket += " ";                           // Agrega espacios antes del texto a centrar
            }                                            // **********
            line.AppendLine(ticket += parte1);

        }
        public void TextoExtremos(string par1, string par2)
        {
            max = par1.Length;
            if (max > 18)                                 // **********
            {
                cort = max - 18;
                parte1 = par1.Remove(18, cort);          // si par1 es mayor que 18 lo corta
            }
            else { parte1 = par1; }                      // **********
            ticket = parte1;                             // agrega el primer parametro
            max = par2.Length;
            if (max > 18)                                 // **********
            {
                cort = max - 18;
                parte2 = par2.Remove(18, cort);          // si par2 es mayor que 18 lo corta
            }
            else { parte2 = par2; }
            max = 40 - (parte1.Length + parte2.Length);
            for (int i = 0; i < max; i++)                 // **********
            {
                ticket += " ";                            // Agrega espacios para poner par2 al final
            }                                             // **********
            line.AppendLine(ticket += parte2 + "\n");                   // agrega el segundo parametro al final

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


            parte2 = total.ToString("c2");
            max = 40 - (parte1.Length + parte2.Length);
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

            //cantidad de espacio para poner despues de la cantidad
            while (7 - cant.ToString().Length >= i)
            {
                espacios += " ";
                i++;
            }
            //cantidad de espacios luego del articulo
            if (Articulo.Length >= 15)
            {
                int o = 0;
                foreach (char item in Articulo)
                {
                    if (o < 15)
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
            //Cantidad de espacio luego del articulo
            while (15 - Articulo.Length >= i)
            {
                espacios += " ";
                i++;
            }
            if (Articulo.Length >= 15)
            {
                espacios = "  ";
            }
            //agregando el precio
            elementos += espacios + precio.ToString();

            //agregando espaciosmluego del precio
            i = 1;
            espacios = "";
            while (10 - subtotal.ToString().Length >= i)
            {
                espacios += " ";
                i++;
            }
            elementos += espacios + subtotal.ToString();
            if (Articulo.Length > 15)
            {
                elementos += "\n       " + art[1];
            }
            line.AppendLine(elementos);


        }

        /* public void ImprimirTiket(string stringimpresora )
         {

             RawPrinterHelper.SendStringToPrinter(stringimpresora, line.ToString());

             line = new StringBuilder();

         }*/
        public string obtener()
        {

            return line.ToString();

        }


    }

    #region Clase para enviar a impresora texto plano
    public class RawPrinterHelper2
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Asume que falla a menos que específicamente tenga éxito.

            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";

            // Abre la impresora.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Inicia un documento.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Inicia una página.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Escribe tus bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // Si no tuviste éxito, GetLastError puede dar más información
            // sobre por qué no.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // ¿Cuántos caracteres hay en la cadena?
            dwCount = szString.Length;
            // Asume que la impresora espera texto ANSI, y luego convierte
            // la cadena a texto ANSI.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Envía la cadena ANSI convertida a la impresora.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
    }
    #endregion
}
