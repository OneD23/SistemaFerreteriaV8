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
        private TextBox txtColorPrimario;
        private TextBox txtColorPanel;
        private TextBox txtColorFondo;
        private DataGridView gridSecuencias;
        public VentanaConfiguraciones()
        {
            InitializeComponent();
            InicializarSeccionTema();
            InicializarGridSecuencias();
            AplicarTemaVisualUniforme();
            OrganizarLayoutConfiguraciones();
        }
        private void InicializarGridSecuencias()
        {
            gridSecuencias = new DataGridView
            {
                Name = "gridSecuencias",
                Location = new Point(20, 35),
                Size = new Size(470, 310),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.FromArgb(230, 234, 240),
                BorderStyle = BorderStyle.None
            };

            gridSecuencias.Columns.Add("Tipo", "Tipo de Secuencia");
            gridSecuencias.Columns.Add("Desde", "Desde");
            gridSecuencias.Columns.Add("Hasta", "Hasta");
            gridSecuencias.Columns[0].ReadOnly = true;
            gridSecuencias.Columns[0].FillWeight = 45;
            gridSecuencias.Columns[1].FillWeight = 27;
            gridSecuencias.Columns[2].FillWeight = 28;

            gridSecuencias.Rows.Add("Comprobante de Consumo", "", "");
            gridSecuencias.Rows.Add("Comprobante Fiscal", "", "");
            gridSecuencias.Rows.Add("Comprobante Gubernamental", "", "");
            gridSecuencias.Rows.Add("Notas de Crédito", "", "");

            groupBox3.Controls.Add(gridSecuencias);
            OcultarControlesSecuenciasAntiguos();
        }
        private void OcultarControlesSecuenciasAntiguos()
        {
            foreach (Control control in groupBox3.Controls)
            {
                if (control != gridSecuencias && control != FechaMaxima && control != label10)
                {
                    control.Visible = false;
                }
            }
            label10.Visible = true;
            FechaMaxima.Visible = true;
        }
        private void InicializarSeccionTema()
        {
            var tituloTema = new Label
            {
                Text = "Personalización de colores:",
                ForeColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold),
                Location = new Point(18, 206),
                AutoSize = true
            };

            txtColorPrimario = CrearTextColor(new Point(160, 236));
            txtColorPanel = CrearTextColor(new Point(160, 268));
            txtColorFondo = CrearTextColor(new Point(160, 300));

            var lblPrimario = CrearLabelColor("Primario:", new Point(18, 239));
            var lblPanel = CrearLabelColor("Paneles:", new Point(18, 271));
            var lblFondo = CrearLabelColor("Fondo:", new Point(18, 303));

            Button btnPrimario = CrearBotonColor("Elegir", new Point(290, 234), (s, e) => SeleccionarColor(txtColorPrimario));
            Button btnPanel = CrearBotonColor("Elegir", new Point(290, 266), (s, e) => SeleccionarColor(txtColorPanel));
            Button btnFondo = CrearBotonColor("Elegir", new Point(290, 298), (s, e) => SeleccionarColor(txtColorFondo));

            groupBox4.Controls.Add(tituloTema);
            groupBox4.Controls.Add(lblPrimario);
            groupBox4.Controls.Add(lblPanel);
            groupBox4.Controls.Add(lblFondo);
            groupBox4.Controls.Add(txtColorPrimario);
            groupBox4.Controls.Add(txtColorPanel);
            groupBox4.Controls.Add(txtColorFondo);
            groupBox4.Controls.Add(btnPrimario);
            groupBox4.Controls.Add(btnPanel);
            groupBox4.Controls.Add(btnFondo);
        }
        private void AplicarTemaVisualUniforme()
        {
            Configuraciones config = new Configuraciones().ObtenerPorId(1);
            Color fondo = ParseColor(config?.ColorFondo, Color.FromArgb(21, 34, 56));
            Color panel = ParseColor(config?.ColorPanel, Color.FromArgb(36, 52, 77));
            Color primario = ParseColor(config?.ColorPrimario, Color.FromArgb(255, 137, 0));
            Color texto = Color.FromArgb(236, 240, 245);

            BackColor = fondo;
            groupBox1.BackColor = fondo;
            groupBox1.ForeColor = texto;
            groupBox2.BackColor = panel;
            groupBox3.BackColor = panel;
            groupBox4.BackColor = panel;
            groupBox2.ForeColor = texto;
            groupBox3.ForeColor = texto;
            groupBox4.ForeColor = texto;

            AplicarTemaRecursivo(groupBox1, texto);

            foreach (var button in new[] { Guardar, button1, button2, button3, button4, button5 })
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
                button.ForeColor = Color.White;
            }

            Guardar.BackColor = primario;
            button1.BackColor = Color.FromArgb(51, 65, 85);
            button2.BackColor = Color.FromArgb(51, 65, 85);
            button3.BackColor = primario;
            button4.BackColor = primario;
            button5.BackColor = primario;
        }
        private void AplicarTemaRecursivo(Control parent, Color textColor)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Label label)
                {
                    label.ForeColor = textColor;
                    label.BackColor = Color.Transparent;
                    label.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
                }
                else if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = Color.FromArgb(243, 244, 246);
                    textBox.ForeColor = Color.FromArgb(15, 23, 42);
                    textBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.FlatStyle = FlatStyle.Flat;
                    comboBox.BackColor = Color.FromArgb(243, 244, 246);
                    comboBox.ForeColor = Color.FromArgb(15, 23, 42);
                    comboBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.CalendarMonthBackground = Color.FromArgb(243, 244, 246);
                    dateTimePicker.CalendarForeColor = Color.FromArgb(15, 23, 42);
                    dateTimePicker.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                }
                else if (control is NumericUpDown numeric)
                {
                    numeric.BackColor = Color.FromArgb(243, 244, 246);
                    numeric.ForeColor = Color.FromArgb(15, 23, 42);
                    numeric.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                }
                else if (control is DataGridView grid)
                {
                    grid.EnableHeadersVisualStyles = false;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(226, 232, 240);
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
                    grid.DefaultCellStyle.BackColor = Color.White;
                    grid.DefaultCellStyle.ForeColor = Color.FromArgb(15, 23, 42);
                    grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(191, 219, 254);
                    grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
                    grid.GridColor = Color.FromArgb(148, 163, 184);
                    grid.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
                }

                if (control.HasChildren)
                    AplicarTemaRecursivo(control, textColor);
            }
        }
        private void OrganizarLayoutConfiguraciones()
        {
            groupBox1.Location = new Point(20, 12);
            groupBox1.Size = new Size(1120, 640);
            groupBox2.Location = new Point(16, 30);
            groupBox2.Size = new Size(560, 255);
            groupBox4.Location = new Point(16, 290);
            groupBox4.Size = new Size(560, 336);
            groupBox3.Location = new Point(590, 30);
            groupBox3.Size = new Size(510, 470);

            label10.Location = new Point(120, 372);
            FechaMaxima.Location = new Point(235, 372);
            Guardar.Location = new Point(590, 540);
            button1.Location = new Point(760, 540);
            button2.Location = new Point(930, 540);
            Guardar.Size = button1.Size = button2.Size = new Size(120, 42);

            Server.Location = new Point(145, 35);
            Server.Size = new Size(210, 25);
            button3.Location = new Point(370, 30);
            button3.Size = new Size(120, 34);
            comboBoxImpresoras.Location = new Point(213, 75);
            comboBoxImpresoras.Size = new Size(277, 25);
            comboBox1.Location = new Point(160, 112);
            comboBox1.Size = new Size(140, 25);
            FontSize.Location = new Point(186, 156);
            FontSize.Size = new Size(105, 25);
            button4.Location = new Point(415, 235);
            button5.Location = new Point(415, 270);
            button4.Size = button5.Size = new Size(125, 30);
            pictureBoxIcono.Location = new Point(447, 168);
            pictureBox1.Location = new Point(447, 101);
            pictureBoxIcono.Size = pictureBox1.Size = new Size(72, 50);
        }
        private Color ParseColor(string colorHex, Color fallback)
        {
            if (string.IsNullOrWhiteSpace(colorHex))
                return fallback;
            try
            {
                return ColorTranslator.FromHtml(colorHex);
            }
            catch
            {
                return fallback;
            }
        }
        private Label CrearLabelColor(string texto, Point point)
        {
            return new Label
            {
                Text = texto,
                ForeColor = Color.WhiteSmoke,
                Location = point,
                Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Bold),
                AutoSize = true
            };
        }
        private TextBox CrearTextColor(Point point)
        {
            return new TextBox
            {
                Location = point,
                Size = new Size(120, 22),
                Text = "#FF8900"
            };
        }
        private Button CrearBotonColor(string texto, Point point, EventHandler onClick)
        {
            var button = new Button
            {
                Text = texto,
                Location = point,
                Size = new Size(70, 24),
                BackColor = Color.FromArgb(255, 137, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Popup
            };
            button.Click += onClick;
            return button;
        }
        private void SeleccionarColor(TextBox destino)
        {
            using (var colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    destino.Text = ColorTranslator.ToHtml(colorDialog.Color);
                    destino.BackColor = colorDialog.Color;
                }
            }
        }
        private void AplicarPreviewColor(TextBox destino)
        {
            try
            {
                destino.BackColor = ColorTranslator.FromHtml(destino.Text);
                destino.ForeColor = Color.Black;
            }
            catch
            {
                destino.BackColor = Color.White;
                destino.ForeColor = Color.Black;
            }
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            Configuraciones config1 = new Configuraciones().ObtenerPorId(1);
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(NombreEmpresa.Text))
                errores.Add("El nombre de la empresa es obligatorio.");

            string nfcInicialTxt = ObtenerDesdeGrid(1, true);
            string nfcFinalTxt = ObtenerDesdeGrid(1, false);
            string sgiTxt = ObtenerDesdeGrid(2, true);
            string sgfTxt = ObtenerDesdeGrid(2, false);
            string sciTxt = ObtenerDesdeGrid(0, true);
            string scfTxt = ObtenerDesdeGrid(0, false);

            if (!double.TryParse(nfcInicialTxt, out double nfcInicial))
                errores.Add("NFC Inicial debe ser numérico.");

            if (!double.TryParse(nfcFinalTxt, out double nfcFinal))
                errores.Add("NFC Final debe ser numérico.");
            else if (double.TryParse(nfcInicialTxt, out double nfcIniTmp) && nfcFinal < nfcIniTmp)
                errores.Add("NFC Final no puede ser menor que NFC Inicial.");

            if (!double.TryParse(sgiTxt, out _))
                errores.Add("SGI debe ser numérico.");

            if (!double.TryParse(sgfTxt, out _))
                errores.Add("SGF debe ser numérico.");

            if (!double.TryParse(sciTxt, out _))
                errores.Add("SCI debe ser numérico.");

            if (!double.TryParse(scfTxt, out _))
                errores.Add("SCF debe ser numérico.");

            if (comboBox1.SelectedIndex < 0)
                errores.Add("Debes seleccionar un tipo de precio.");

            if (errores.Any())
            {
                MessageBox.Show(
                    "Corrige estos campos antes de guardar:\n\n- " + string.Join("\n- ", errores),
                    "Validación de configuración",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var config = new Configuraciones
            {
                Id = 1,
                Nombre = NombreEmpresa.Text.Trim(),
                Telefono = Telefono.Text?.Trim(),
                Correo = Correo.Text?.Trim(),
                Direccion = Direccion.Text?.Trim(),
                RNC = RNC.Text?.Trim(),
                NFCInicial = nfcInicial.ToString("0"),
                UltimoNFC = (nfcInicial - 1).ToString("0"),
                NFCFinal = nfcFinal.ToString("0"),
                FechaExpiracion = FechaMaxima.Value,
                Precio = comboBox1.SelectedIndex,
                Icono = !string.IsNullOrWhiteSpace(ruta) ? ruta : config1?.Icono,
                Imagen = !string.IsNullOrWhiteSpace(ruta2) ? ruta2 : config1?.Imagen,
                Seleccion = "Consumo",
                SGI = ObtenerDesdeGrid(2, true),
                SGF = ObtenerDesdeGrid(2, false),
                SCCI = ObtenerDesdeGrid(0, true),
                SCCF = ObtenerDesdeGrid(0, false),
                FontSize = FontSize.Value.ToString(),
                Impresora = !string.IsNullOrWhiteSpace(comboBoxImpresoras.Text) ? comboBoxImpresoras.Text : config1?.Impresora,
                ColorPrimario = !string.IsNullOrWhiteSpace(txtColorPrimario?.Text) ? txtColorPrimario.Text.Trim() : config1?.ColorPrimario,
                ColorPanel = !string.IsNullOrWhiteSpace(txtColorPanel?.Text) ? txtColorPanel.Text.Trim() : config1?.ColorPanel,
                ColorFondo = !string.IsNullOrWhiteSpace(txtColorFondo?.Text) ? txtColorFondo.Text.Trim() : config1?.ColorFondo
            };

            config.SGA = string.IsNullOrWhiteSpace(config1?.SGA) ? config.SGI : config1.SGA;
            config.SCCA = string.IsNullOrWhiteSpace(config1?.SCCA) ? config.SCCI : config1.SCCA;
            config.NFCActual = string.IsNullOrWhiteSpace(config1?.NFCActual) ? config.NFCInicial : config1.NFCActual;
            config.NFCInicial = ObtenerDesdeGrid(1, true);
            config.NFCFinal = ObtenerDesdeGrid(1, false);
            config.SNCI = ObtenerDesdeGrid(3, true);
            config.SNCF = ObtenerDesdeGrid(3, false);

            var fiscalService = new FiscalService();
            if (!fiscalService.ValidarRangos(config, out var erroresFiscales))
            {
                MessageBox.Show(
                    "La configuración fiscal tiene inconsistencias:\n\n- " + string.Join("\n- ", erroresFiscales),
                    "Validación fiscal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                config.Guardar();
                MessageBox.Show("Configuración guardada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar la configuración:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                CargarGridSecuenciasDesdeConfiguracion(config);
                comboBox1.SelectedIndex = config.Precio;
                ruta = config.Icono;
                ruta2 = config.Imagen;
                if (decimal.TryParse(config.FontSize, out decimal fontSizeVal))
                {
                    var valor = Math.Max(FontSize.Minimum, Math.Min(FontSize.Maximum, fontSizeVal));
                    FontSize.Value = valor;
                }
                txtColorPrimario.Text = string.IsNullOrWhiteSpace(config.ColorPrimario) ? "#FF8900" : config.ColorPrimario;
                txtColorPanel.Text = string.IsNullOrWhiteSpace(config.ColorPanel) ? "#24344D" : config.ColorPanel;
                txtColorFondo.Text = string.IsNullOrWhiteSpace(config.ColorFondo) ? "#152238" : config.ColorFondo;
                AplicarPreviewColor(txtColorPrimario);
                AplicarPreviewColor(txtColorPanel);
                AplicarPreviewColor(txtColorFondo);

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
        private void CargarGridSecuenciasDesdeConfiguracion(Configuraciones config)
        {
            if (gridSecuencias == null || gridSecuencias.Rows.Count < 4)
                return;
            gridSecuencias.Rows[0].Cells[1].Value = config?.SCCI ?? "";
            gridSecuencias.Rows[0].Cells[2].Value = config?.SCCF ?? "";
            gridSecuencias.Rows[1].Cells[1].Value = config?.NFCInicial ?? "";
            gridSecuencias.Rows[1].Cells[2].Value = config?.NFCFinal ?? "";
            gridSecuencias.Rows[2].Cells[1].Value = config?.SGI ?? "";
            gridSecuencias.Rows[2].Cells[2].Value = config?.SGF ?? "";
            gridSecuencias.Rows[3].Cells[1].Value = config?.SNCI ?? "";
            gridSecuencias.Rows[3].Cells[2].Value = config?.SNCF ?? "";
        }
        private string ObtenerDesdeGrid(int rowIndex, bool desde)
        {
            if (gridSecuencias == null || gridSecuencias.Rows.Count <= rowIndex)
                return "";
            int col = desde ? 1 : 2;
            return gridSecuencias.Rows[rowIndex].Cells[col].Value?.ToString()?.Trim() ?? "";
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
