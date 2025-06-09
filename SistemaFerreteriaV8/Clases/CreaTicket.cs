using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using iTextSharp.text;


namespace SistemaFerreteriaV8.Clases
{
    class CreaTicket
    {
        public static StringBuilder line = new StringBuilder();
        string ticket = "";
        string parte1, parte2;

        public static int max = 45;
        int cort;
        public  string LineasGuion()
        {
            string LineaGuion = "---------------------------------------------";   // agrega lineas separadoras -

            return line.AppendLine(LineaGuion).ToString();
        }


        public  void EncabezadoVenta()
        {
            string LineEncavesado = "Cant  Producto          Precio  Itbis  Total";   // agrega lineas de  encabezados
            line.AppendLine(LineEncavesado);
        }
        public void TextoIzquierda(string par1)                          // agrega texto a la izquierda
        {
            max = par1.Length;
            if (max > 45)                                 // **********
            {
                cort = max - 45;
                parte1 = par1.Remove(45, cort);        // si es mayor que 45 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            line.AppendLine(ticket = parte1);

        }
        public void TextoDerecha(string par1)
        {
            ticket = "";
            max = par1.Length;
            if (max > 43)                                 // **********
            {
                cort = max - 43;
                parte1 = par1.Remove(43, cort);           // si es mayor que 45 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            max = 45 - par1.Length;                     // obtiene la cantidad de espacios para llegar a 45
            for (int i = 0; i < max; i++)
            {
                ticket += " ";                          // agrega espacios para alinear a la derecha
            }
            line.AppendLine(par1.PadLeft(43, ' ')+"\n");                //Agrega el texto

        }
        public void TextoCentro(string par1)
        {
            ticket = "";
            max = par1.Length;
            if (max > 45)                                 // **********
            {
                cort = max - 45;
                parte1 = par1.Remove(45, cort);          // si es mayor que 45 caracteres, lo corta
            }
            else { parte1 = par1; }                      // **********
            max = (int)(45 - parte1.Length) / 2;         // saca la cantidad de espacios libres y divide entre dos
            for (int i = 0; i < max; i++)                // **********
            {
                ticket += " ";                           // Agrega espacios antes del texto a centrar
            }                                            // **********
            line.AppendLine(ticket += parte1 + "\n");

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
            max = 45 - (parte1.Length + parte2.Length);
            for (int i = 0; i < max; i++)                 // **********
            {
                ticket += " ";                            // Agrega espacios para poner par2 al final
            }                                             // **********
            line.AppendLine(ticket += parte2 + "\n");                   // agrega el segundo parametro al final

        }
        public void AgregaTotales(string par1, double total)
        {
            max = par1.Length;
            if (max > 23)                                 // **********
            {
                cort = max - 23;
                parte1 = par1.Remove(23, cort);          // si es mayor que 25 lo corta
            }
            else { parte1 = par1; }                      // **********
            ticket = parte1;


            parte2 = total.ToString("c2");
            max = 44 - (parte1.Length + parte2.Length);
            for (int i = 0; i < max; i++)                // **********
            {
                ticket += " ";                           // Agrega espacios para poner el valor de moneda al final
            }                                            // **********
            line.AppendLine(ticket += parte2);

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
        // se le pasan los Aticulos  con sus detalles
        public void AgregaArticulo(double precio, string Articulo, double cant, double subtotal)
        {
            string elementos = "";
            string restante = "";
            string nombre1 = "";

            elementos += cant.ToString().PadRight(6, ' ');

            if (Articulo.Length > 14)
            {
                int i = 0;
                foreach (var caracter in Articulo)
                {
                    if (i <=14)
                    {
                        nombre1 += caracter;
                    }
                    else
                    {
                        restante += caracter;
                    }
                    i++;
                }
                elementos += nombre1.PadRight(18, ' ');
            }
            else
            {
                elementos += Articulo.PadRight(18, ' ');
            }
            
            elementos += (precio / 1.18).ToString("F2").PadRight(8, ' ');
            elementos += (precio - (precio / 1.18)).ToString("F2").PadRight(8, ' ');
            elementos += subtotal.ToString().PadRight(7, ' ');

            if(restante.Length > 1)
            {
                elementos += "\n       " + restante;
            }

            line.AppendLine(elementos);
        }

        public void ImprimirTiket(string stringimpresora)
        {

            RawPrinterHelper.SendStringToPrinter(stringimpresora, line.ToString());

            line = new StringBuilder();

        }
        public StringBuilder obtener()
        {
            return line;
        }
        public void Limpiar()
        {
            line = new StringBuilder("");
        }

    }



    #region Clase para enviar a imprsora texto plano
    public class RawPrinterHelper
    {
        // Structure and API declarions:
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

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";
            // di.pOutputFile = @"C:\Users\Roland\Documents\Visual Studio 2015\Projects\pjtVentas\Ventas";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
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
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
    }
}
#endregion}