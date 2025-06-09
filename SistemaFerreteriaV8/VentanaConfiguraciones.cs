using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaFerreteriaV8.Clases;
using System.IO;
using System.Drawing.Printing;

namespace SistemaFerreteriaV8
{
    public partial class VentanaConfiguraciones : Form
    {
        string ruta {  get; set; } string ruta2 {  get; set; }
        public VentanaConfiguraciones()
        {
            InitializeComponent();
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            Configuraciones config = new Configuraciones();
            Configuraciones config1 = new Configuraciones().ObtenerPorId(1);

            config.Nombre = NombreEmpresa.Text;
            config.Telefono = Telefono.Text;
            config.Correo = Correo.Text;
            config.Direccion = Direccion.Text;
            config.RNC = RNC.Text;
            config.NFCInicial = NFCInical.Text;
            config.UltimoNFC = (double.Parse(NFCInical.Text) - 1).ToString();
            config.NFCFinal = NFCFinal.Text;
            config.FechaExpiracion = FechaMaxima.Value;
            
            config.Precio = comboBox1.SelectedIndex;
            config.Icono = ruta;
            config.Imagen = ruta2;
            config.Seleccion = "Consumo";
            config.SGI = SGI.Text;
            config.SGF = SGF.Text;

            if (config.Impresora != null)
            {
                config.Impresora = config1.Impresora;
            }
            else
            {
                config.Impresora = comboBoxImpresoras.Text;
            }

            if (config1 == null || string.IsNullOrWhiteSpace(config1.SGA))
            {
                config.SGA = SGI.Text;
            }
            else
            {
                config.SGA = config1.SGA;
            }
            

            config.SCCI = SCI.Text;
            config.SCCF = SCF.Text;

            if (config1 == null || string.IsNullOrWhiteSpace(config1.SCCA))
            {
                config.SCCA = SCI.Text;
            }
            else
            {
                config.SCCA = config1.SCCA;
            }


            if (config1 == null || string.IsNullOrWhiteSpace(config1.NFCActual))
            {
                config.NFCActual = NFCInical.Text;
            }
            else
            {
                config.NFCActual = config1.NFCActual;
            }

            config.FontSize = FontSize.Text;

            config.Guardar();

            MessageBox.Show("Configuracion guardada!");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            NombreEmpresa.Text = "";
            Telefono.Text = "";
            Correo.Text = "";
            Direccion.Text = "";
            RNC.Text = "";
            NFCInical.Text = "";
            NFCFinal.Text = "";
            FechaMaxima.Text = DateTime.Now.ToShortDateString();
            comboBoxImpresoras.Text = "";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Server.Text = "";
        }
        private void VentanaConfiguraciones_Load(object sender, EventArgs e)
        {

            Configuraciones config = new Configuraciones().ObtenerPorId(1);
            if (config != null) { 
                SGI.Text = config.SGI;
                SGF.Text = config.SGF;

                SCI.Text = config.SCCI;
                SCF.Text = config.SCCF;

                NombreEmpresa.Text=  config.Nombre;
                Telefono.Text=  config.Telefono;
                Correo.Text = config.Correo;
                Direccion.Text = config.Direccion;
                RNC.Text = config.RNC;
                NFCInical.Text = config.NFCInicial;
                NFCFinal.Text = config.NFCFinal;
                comboBox1.SelectedIndex = config.Precio;
                ruta = config.Icono;
                ruta2 = config.Imagen;
                FontSize.Value = int.Parse(config.FontSize);

                if (config.Icono != null)
                {
                    try
                    {
                        Icon icono = new Icon(config.Icono);
                        pictureBoxIcono.Image = icono.ToBitmap();
                    }
                    catch (Exception)
                    {

                    }                
                }
                if (config.Imagen != null)
                {
                    try
                    {
                        Image image = Image.FromFile(config.Imagen);
                    pictureBox1.Image = image;
                    }
                    catch (Exception)
                    {

                    }
                }
                try
                {
                    FechaMaxima.Value = config.FechaExpiracion;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    // Manejar la excepción
                }

                // Limpia el ComboBox antes de llenarlo
                comboBoxImpresoras.Items.Clear();

                // Obtiene la lista de impresoras instaladas
                foreach (string printer in PrinterSettings.InstalledPrinters)
                {
                    comboBoxImpresoras.Items.Add(printer);
                }

                // Selecciona la primera impresora por defecto, si existe
                if (comboBoxImpresoras.Items.Count > 0)
                {
                    comboBoxImpresoras.SelectedIndex = 0;
                } 

                if (config.Impresora != null)
                {

                    comboBoxImpresoras.Text = config.Impresora;
                }         

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Crea un nuevo OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Establece las propiedades del OpenFileDialog
            openFileDialog.Filter = "Archivos de icono (*.ico)|*.ico|Todos los archivos (*.*)|*.*";
            openFileDialog.Title = "Seleccionar ícono";

            // Muestra el diálogo para seleccionar archivos
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Obtiene la ruta del archivo seleccionado
                    string rutaIcono = openFileDialog.FileName;

                    // Obtiene la carpeta donde se ejecuta el programa
                    string carpetaEjecucion = AppDomain.CurrentDomain.BaseDirectory;

                    // Construye la ruta de destino para copiar el archivo de icono
                    string rutaDestino = Path.Combine(carpetaEjecucion, Path.GetFileName(rutaIcono));

                    // Copia el archivo de icono a la carpeta de ejecución del programa
                    File.Copy(rutaIcono, rutaDestino, true);

                    // Guarda la ruta del archivo en la base de datos
                    // Aquí deberías tener tu lógica para guardar la ruta en la base de datos

                    // Opcionalmente, puedes cargar y mostrar el ícono en el formulario
                    Icon icono = new Icon(rutaDestino);
                    ruta = rutaDestino;
                    pictureBoxIcono.Image = icono.ToBitmap(); // Muestra el ícono en un control PictureBox
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al seleccionar el ícono: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Crea un nuevo OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Establece las propiedades del OpenFileDialog
            openFileDialog.Filter = "Archivos de icono (*.png)|*.png|Todos los archivos (*.*)|*.*";
            openFileDialog.Title = "Seleccionar ícono";

            // Muestra el diálogo para seleccionar archivos
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Obtiene la ruta del archivo seleccionado
                    string rutaIcono = openFileDialog.FileName;

                    // Obtiene la carpeta donde se ejecuta el programa
                    string carpetaEjecucion = AppDomain.CurrentDomain.BaseDirectory;

                    // Construye la ruta de destino para copiar el archivo de icono
                    string rutaDestino = Path.Combine(carpetaEjecucion, Path.GetFileName(rutaIcono));

                    // Copia el archivo de icono a la carpeta de ejecución del programa
                    File.Copy(rutaIcono, rutaDestino, true);

                    // Guarda la ruta del archivo en la base de datos
                    // Aquí deberías tener tu lógica para guardar la ruta en la base de datos

                    // Opcionalmente, puedes cargar y mostrar el ícono en el formulario
                    Image icono = Image.FromFile(rutaDestino);
                    ruta2 = rutaDestino;
                    pictureBox1.Image = icono; // Muestra el ícono en un control PictureBox
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al seleccionar el ícono: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}
