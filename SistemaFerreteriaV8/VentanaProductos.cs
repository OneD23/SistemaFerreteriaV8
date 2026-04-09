using Microsoft.VisualBasic;
using MongoDB.Driver;
using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace SistemaFerreteriaV8
{
    public partial class VentanaProductos : Form
    {
        public Empleado empleado { get; set; }
        Productos ProductoActivo = null;
        Button botonAnterior = new Button();

        int paginaActual = 0; // Índice de la página actual
        const int productosPorPagina = 20; // Cantidad de productos por página
        double totalProductos = 0;

        public VentanaProductos()
        {
            InitializeComponent();
            AutoScroll = true;
            MinimumSize = new Size(1100, 700);
            AplicarTemaProfesional();
            OrganizarLayoutProfesional();
            Resize += (_, __) => OrganizarLayoutProfesional();
        }
        private void AplicarTemaProfesional()
        {
            Configuraciones config = new Configuraciones().ObtenerPorId(1);
            var fondoPrincipal = ParseColor(config?.ColorFondo, Color.FromArgb(21, 34, 56));
            var fondoPanel = ParseColor(config?.ColorPanel, Color.FromArgb(36, 52, 77));
            var textoPrincipal = Color.FromArgb(236, 240, 245);
            var azulPrimario = ParseColor(config?.ColorPrimario, Color.FromArgb(255, 137, 0));
            var azulSecundario = Color.FromArgb(41, 62, 95);
            var grisBorde = Color.FromArgb(77, 95, 125);

            BackColor = fondoPrincipal;
            ForeColor = textoPrincipal;

            groupBox1.BackColor = fondoPanel;
            groupBox1.ForeColor = textoPrincipal;
            groupBox1.Text = "";

            groupBox2.BackColor = fondoPanel;
            groupBox2.ForeColor = textoPrincipal;
            groupBox2.Text = "";

            label8.ForeColor = textoPrincipal;
            label9.ForeColor = textoPrincipal;
            label8.BackColor = Color.Transparent;
            label9.BackColor = Color.Transparent;

            var etiquetas = new[]
            {
                label1, label2, label3, label4, label5, label6, label7, label10, label11, label12, label13, Lugar
            };
            foreach (var label in etiquetas)
            {
                label.ForeColor = textoPrincipal;
                label.BackColor = Color.Transparent;
            }

            var entradas = new TextBox[] { Id, Nombre, Descripcion, Precio, Precio2, Precio3, Precio4, Costo, Cantidad, Marca, textBox1 };
            foreach (var entrada in entradas)
            {
                entrada.BorderStyle = BorderStyle.FixedSingle;
                entrada.BackColor = Color.FromArgb(242, 244, 247);
                entrada.ForeColor = Color.FromArgb(15, 23, 42);
                entrada.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            }

            Categoria.BackColor = Color.FromArgb(248, 250, 252);
            Categoria.ForeColor = Color.FromArgb(15, 23, 42);
            Categoria.FlatStyle = FlatStyle.Flat;
            comboBox1.BackColor = Color.FromArgb(248, 250, 252);
            comboBox1.ForeColor = Color.FromArgb(15, 23, 42);
            comboBox1.FlatStyle = FlatStyle.Flat;

            var accionesPrincipales = new[] { Nuevo, Editar, Guardar };
            foreach (var boton in accionesPrincipales)
            {
                EstilizarBoton(boton, azulPrimario);
            }

            var accionesSecundarias = new[] { Eliminar, Buscar, Cancelar, button1, button2, button3, button4, button5, button6 };
            foreach (var boton in accionesSecundarias)
            {
                EstilizarBoton(boton, azulSecundario);
            }

            button2.Text = "Exportar productos desde Excel";
            button1.BackColor = Color.FromArgb(5, 111, 242);
            button2.BackColor = Color.FromArgb(35, 156, 71);
            button3.BackColor = Color.FromArgb(53, 65, 88);

            button5.Image = RedimensionarIcono(Properties.Resources.arrow_back_ios_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24, 16, 16);
            button4.Image = RedimensionarIcono(Properties.Resources.arrow_forward_ios_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24, 16, 16);
            button5.ImageAlign = ContentAlignment.MiddleLeft;
            button4.ImageAlign = ContentAlignment.MiddleRight;
            button5.TextImageRelation = TextImageRelation.ImageBeforeText;
            button4.TextImageRelation = TextImageRelation.TextBeforeImage;
            button6.Image = RedimensionarIcono(Properties.Resources.inventory_2_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24, 16, 16);
            button6.ImageAlign = ContentAlignment.MiddleLeft;
            button6.TextImageRelation = TextImageRelation.ImageBeforeText;

            ListaProductos.BackgroundColor = Color.FromArgb(58, 76, 107);
            ListaProductos.BorderStyle = BorderStyle.None;
            ListaProductos.EnableHeadersVisualStyles = false;
            ListaProductos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 234, 240);
            ListaProductos.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(42, 55, 90);
            ListaProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            ListaProductos.DefaultCellStyle.BackColor = Color.White;
            ListaProductos.DefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
            ListaProductos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(30, 110, 226);
            ListaProductos.DefaultCellStyle.SelectionForeColor = Color.White;
            ListaProductos.GridColor = grisBorde;
            ListaProductos.RowTemplate.Height = 28;
            ListaProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private Color ParseColor(string colorHex, Color colorPredeterminado)
        {
            if (string.IsNullOrWhiteSpace(colorHex))
                return colorPredeterminado;
            try
            {
                return ColorTranslator.FromHtml(colorHex);
            }
            catch
            {
                return colorPredeterminado;
            }
        }
        private Image RedimensionarIcono(Image imagen, int ancho, int alto)
        {
            if (imagen == null)
                return null;
            return new Bitmap(imagen, new Size(ancho, alto));
        }

        private void EstilizarBoton(Button boton, Color fondo)
        {
            boton.BackColor = fondo;
            boton.ForeColor = Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            boton.Cursor = Cursors.Hand;
        }

        private void OrganizarLayoutProfesional()
        {
            FormBorderStyle = FormBorderStyle.None;
            int width = Math.Max(1100, ClientSize.Width);
            int height = Math.Max(700, ClientSize.Height);

            groupBox1.Location = new Point(20, 20);
            groupBox1.Size = new Size(370, height - 40);

            groupBox2.Location = new Point(405, 20);
            groupBox2.Size = new Size(Math.Max(780, width - 425), height - 40);

            label8.Location = new Point(102, 30);
            label8.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label8.Text = "Producto";

            int xLabel = 20;
            int xInput = 140;
            int yInicio = 85;
            int salto = 38;
            int anchoInput = 205;

            label1.Location = new Point(xLabel + 72, yInicio + (salto * 0));
            Id.Location = new Point(xInput, yInicio + (salto * 0) - 2);
            Id.Size = new Size(anchoInput, 25);

            label2.Location = new Point(xLabel + 17, yInicio + (salto * 1));
            Nombre.Location = new Point(xInput, yInicio + (salto * 1) - 2);
            Nombre.Size = new Size(anchoInput, 25);

            label3.Location = new Point(xLabel + 3, yInicio + (salto * 2));
            Categoria.Location = new Point(xInput, yInicio + (salto * 2) - 1);
            Categoria.Size = new Size(anchoInput, 25);

            label13.Location = new Point(xLabel + 31, yInicio + (salto * 3));
            Marca.Location = new Point(xInput, yInicio + (salto * 3) - 2);
            Marca.Size = new Size(anchoInput, 25);

            label4.Location = new Point(xLabel, yInicio + (salto * 4));
            Descripcion.Location = new Point(xInput, yInicio + (salto * 4) - 2);
            Descripcion.Size = new Size(anchoInput, 25);

            label5.Location = new Point(xLabel + 31, yInicio + (salto * 5));
            Precio.Location = new Point(xInput, yInicio + (salto * 5) - 2);
            Precio.Size = new Size(anchoInput, 25);

            label10.Location = new Point(xLabel + 21, yInicio + (salto * 6));
            Precio2.Location = new Point(xInput, yInicio + (salto * 6) - 2);
            Precio2.Size = new Size(anchoInput, 25);

            label12.Location = new Point(xLabel + 21, yInicio + (salto * 7));
            Precio3.Location = new Point(xInput, yInicio + (salto * 7) - 2);
            Precio3.Size = new Size(anchoInput, 25);

            label11.Location = new Point(xLabel + 21, yInicio + (salto * 8));
            Precio4.Location = new Point(xInput, yInicio + (salto * 8) - 2);
            Precio4.Size = new Size(anchoInput, 25);

            label6.Location = new Point(xLabel + 44, yInicio + (salto * 9));
            Costo.Location = new Point(xInput, yInicio + (salto * 9) - 2);
            Costo.Size = new Size(anchoInput, 25);

            label7.Location = new Point(xLabel + 14, yInicio + (salto * 10));
            Cantidad.Location = new Point(xInput, yInicio + (salto * 10) - 2);
            Cantidad.Size = new Size(anchoInput, 25);

            Nuevo.Location = new Point(20, 505);
            Editar.Location = new Point(135, 505);
            Guardar.Location = new Point(250, 505);
            Eliminar.Location = new Point(20, 550);
            Buscar.Location = new Point(135, 550);
            Cancelar.Location = new Point(250, 550);
            foreach (var boton in new[] { Nuevo, Editar, Guardar, Eliminar, Buscar, Cancelar })
            {
                boton.Size = new Size(100, 40);
            }

            button1.Location = new Point(20, 595);
            button1.Size = new Size(330, 38);
            button2.Location = new Point(20, 637);
            button2.Size = new Size(330, 38);
            button3.Location = new Point(20, 679);
            button3.Size = new Size(330, 38);

            label9.Location = new Point(300, 27);
            label9.Font = new Font("Segoe UI", 18F, FontStyle.Bold);

            comboBox1.Location = new Point(18, 74);
            comboBox1.Size = new Size(140, 28);
            textBox1.Location = new Point(165, 74);
            textBox1.Size = new Size(250, 28);
            button1.Location = new Point(groupBox2.Width - 295, 74);
            button1.Size = new Size(132, 30);
            button3.Location = new Point(groupBox2.Width - 156, 74);
            button3.Size = new Size(132, 30);

            ListaProductos.Location = new Point(18, 110);
            ListaProductos.Size = new Size(groupBox2.Width - 35, Math.Max(340, groupBox2.Height - 275));

            int yPaginacion = ListaProductos.Bottom + 8;
            Lugar.Location = new Point((groupBox2.Width / 2) - 45, yPaginacion);
            button5.Location = new Point(Lugar.Left - 185, yPaginacion + 24);
            button6.Location = new Point(Lugar.Left - 10, yPaginacion + 24);
            button4.Location = new Point(Lugar.Left + 165, yPaginacion + 24);

            foreach (var boton in new[] { button4, button5, button6 })
            {
                boton.Size = new Size(110, 35);
            }
        }
        void ActivarCampos(bool enanble)
        {
            Id.Enabled= enanble;
            Nombre.Enabled= enanble;
            Categoria.Enabled= enanble;
            Descripcion.Enabled= enanble;
            Precio.Enabled= enanble;
            Precio2.Enabled= enanble;
            Precio3.Enabled= enanble;
            Precio4.Enabled= enanble;
            Costo.Enabled= enanble;
            Cantidad.Enabled= enanble;
            Marca.Enabled= enanble;
        }
        void BuscarProducto(string Ids)
        {
            Ids = !string.IsNullOrWhiteSpace(Ids) ? Ids : "0";

            Productos producto= new Productos().Buscar(Ids);

            if (producto != null)
            {
                double cantTemp = producto.Cantidad/* - producto.Vendido*/;
                ProductoActivo =producto;
                Id.Text= producto.Id;
                Nombre.Text= producto.Nombre;
                Categoria.Text= !string.IsNullOrWhiteSpace(producto.Categoria) && producto.Categoria == "No Procesado"? "No Procesado" : producto.Categoria;
                Descripcion.Text= producto.Descripcion;
                Costo.Text = producto.Costo != null ? producto.Costo.ToString():"0";
                Precio.Text = producto.Precio != null ? producto.Precio[0].ToString() : "0";
                Precio2.Text = producto.Precio != null ? producto.Precio[1].ToString() : "0";
                Precio3.Text = producto.Precio != null ? producto.Precio[2].ToString() : "0";
                Precio4.Text = producto.Precio != null ? producto.Precio[3].ToString() : "0";
                Cantidad.Text = cantTemp != null ? cantTemp.ToString() : "0";
                Marca.Text = producto.Marca;
            }
            else
            {
                Id.Text = "";
                Nombre.Text = "";
                Categoria.Text = "";
                Descripcion.Text = "";
                Costo.Text = "";
                Precio.Text = "";
                Cantidad.Text = "";
            }
            ActivarCampos(false);
        }
        void CargarLista(string accion)
        {
            // Conexión a la base de datos MongoDB
            var cliente = new MongoClient(new OneKeys().URI);
            var database = cliente.GetDatabase(new OneKeys().DatabaseName);
            var coleccion = database.GetCollection<BsonDocument>("Productos");

            // Ajustar la página actual según la acción
            if (accion.ToLower() == "iniciar")
            {
                // Solo calcular el total al inicio una sola vez
                if (totalProductos == 0)
                {
                    totalProductos = (int)coleccion.CountDocuments(FilterDefinition<BsonDocument>.Empty);
                }
                paginaActual = 0; // Resetear a la primera página
            }
            else if (accion.ToLower() == "avanza" && paginaActual < 1-(int)Math.Ceiling((double)totalProductos / productosPorPagina))
            {
                paginaActual++;
            }
            else if (accion.ToLower() == "atras" && paginaActual > 0)
            {
                paginaActual--;
            }

            // Calcular el salto y el límite para la paginación
            int salto = paginaActual * productosPorPagina;

            // Configurar la proyección para optimizar la consulta
            var proyeccion = Builders<BsonDocument>.Projection
                .Include("_id") // ID del producto
                .Include("nombre")
                .Include("descripcion")
                .Include("Costo")
                .Include("precio")
                .Include("cantidad")
                .Include("vendido");

            // Consultar los productos en la base de datos con paginación y proyección
            var listaPaginada = coleccion
                .Find(FilterDefinition<BsonDocument>.Empty) // Sin filtro (puedes añadir uno si es necesario)
                .Project(proyeccion) // Aplicar proyección
                .Skip(salto)
                .Limit(productosPorPagina)
                .ToList();

            // Limpiar la lista actual antes de agregar los nuevos productos
            ListaProductos.Rows.Clear();

            // Agregar los productos de la página actual a la tabla
            foreach (var documento in listaPaginada)
            {
                ListaProductos.Rows.Add(
                    documento.GetValue("_id", ""),
                    documento.GetValue("nombre", ""),
                    documento.GetValue("descripcion", ""),
                    documento.GetValue("Costo", 0),
                    documento.GetValue("precio", 0)[0],
                    Convert.ToDouble(documento.GetValue("cantidad", 0)), // Conversión explícita
                                                                                                                               // Cantidad restante
                    documento.GetValue("vendido", 0)
                );
            }

            // Mostrar un mensaje si no hay más productos para mostrar
            if (!listaPaginada.Any())
            {
                Console.WriteLine("No hay más productos para mostrar.");
            }

            // Mostrar la información de la paginación
            int paginaTotal = (int)Math.Ceiling((double)totalProductos / productosPorPagina);
            Lugar.Text = $"Página {paginaActual + 1} de {paginaTotal}";
        }


        public void SelecionDeBoton(Button botonActivo)
        {
            if (botonAnterior != botonActivo)
            {
                botonActivo.BackColor = Color.White;
                botonActivo.ForeColor = Color.Red;

                if (botonAnterior != null)
                {
                    botonAnterior.BackColor = SystemColors.ActiveCaption;
                    botonAnterior.ForeColor = Color.White;
                }
                botonAnterior = botonActivo;
            }
        }
        private void button16_Click(object sender, EventArgs e)
        {
            Form1 frm = (Form1)Application.OpenForms["Form1"];
            if (Application.OpenForms.OfType<Form1>().Any())
            {
                frm.AbrirFormulario(new Form());
            }
            this.Dispose();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SelecionDeBoton((Button)sender);
            ProductoActivo = new Productos();
            
            Id.Text = ProductoActivo.Id.ToString();
            
            Nombre.Text = "";
            Categoria.Text = "";
            Descripcion.Text = "";
            Costo.Text = "";
            Precio.Text = "";            
            Precio2.Text = ""; 
            Precio3.Text = ""; 
            Precio4.Text = "";
            Cantidad.Text = "";
            ActivarCampos(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
            double precio1 = !string.IsNullOrWhiteSpace(Precio.Text) ? double.Parse(Precio.Text) : 0;
            double precio2 = !string.IsNullOrWhiteSpace(Precio2.Text) ? double.Parse(Precio2.Text) : 0;
            double precio3 = !string.IsNullOrWhiteSpace(Precio3.Text) ? double.Parse(Precio3.Text) : 0;
            double precio4 = !string.IsNullOrWhiteSpace(Precio4.Text) ? double.Parse(Precio4.Text) : 0;

            ProductoActivo.Id = Id.Text;
            ProductoActivo.Nombre = Nombre.Text;
            ProductoActivo.Categoria = Categoria.Text;
            ProductoActivo.Descripcion = Descripcion.Text;
            ProductoActivo.Marca = Marca.Text;
            ProductoActivo.Costo = !string.IsNullOrWhiteSpace(Costo.Text) ? double.Parse(Costo.Text) : 0;

            ProductoActivo.Precio = new List<double>{precio1, precio2, precio3, precio4};
             
            double Cantidades = double.Parse(!string.IsNullOrWhiteSpace(Cantidad.Text) ? Cantidad.Text :"0");
            

            Productos productos = new Productos().Buscar(Id.Text);

            if (productos == null)
            {
                ProductoActivo.Cantidad = Cantidades;
                ProductoActivo.InsertarProductos(ProductoActivo);

              
                MessageBox.Show("Producto Creado Correctamente");
            }
            else
            {
                if (productos.Nombre != ProductoActivo.Nombre)
                {
                    if(MessageBox.Show("ya existe un articulo con este codigo, " +
                        "si continua remplazaras " +
                        productos.Nombre +" por " + ProductoActivo.Nombre +"\n" +
                        "Deseas continuar? ", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        ProductoActivo.Cantidad = Cantidades;
                        ProductoActivo.ActualizarProductos();
                       
                        MessageBox.Show("Producto Actualizado Correctamente");
                    }
                }
                else
                {
                    
                    ProductoActivo.Cantidad = Cantidades;
                    ProductoActivo.ActualizarProductos();
                    MessageBox.Show("Producto Actualizado Correctamente");
                }                     
            }

            
            SelecionDeBoton((Button)sender);
            CargarLista("Iniciar");
            Id.Text = "";
            Nombre.Text = "";
            Categoria.Text = "";
            Descripcion.Text = "";
            Costo.Text = "";
            Precio.Text = "";
            Cantidad.Text = "";
            Marca.Text = "";
            Precio2.Text = "";            
            Precio3.Text = "";            
            Precio4.Text = "";
            ActivarCampos(false);

        }

        private void Id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Nombre.Focus();
            }
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            ActivarCampos(true);
            SelecionDeBoton((Button)sender);
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            SelecionDeBoton((Button)sender);
            string id = !string.IsNullOrWhiteSpace(Id.Text) != null ? Id.Text : "0";
            if (id != "0")
            {
                if (MessageBox.Show($"Esta seguro que quiere borrar el {Nombre.Text} de su inventario", "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
                {
                    new Productos().EliminarPorId(id);
                    CargarLista("Iniciar");
                }
            }
            Id.Text = string.Empty;
            Nombre.Text = string.Empty;
            Categoria.Text = string.Empty;
            Descripcion.Text = string.Empty;
            Costo.Text = string.Empty;
            Precio.Text = string.Empty;
            Cantidad.Text = string.Empty;
            Marca.Text = string.Empty;
            Precio2.Text = string.Empty;
            Precio3.Text = string.Empty;
            Precio4.Text = string.Empty;
        }
        private void Buscar_Click(object sender, EventArgs e)
        {
            string id = Interaction.InputBox("Ingrese el Id: ", "Buscar por Id");
            BuscarProducto(id);
            SelecionDeBoton((Button)sender);
        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            Id.Text = string.Empty;
            Nombre.Text = string.Empty;
            Categoria.Text = string.Empty;
            Descripcion.Text = string.Empty;
            Costo.Text = string.Empty;
            Precio.Text = string.Empty;
            Cantidad.Text = string.Empty;
            ActivarCampos(false);
            SelecionDeBoton((Button)sender);
        }

        private void ListaProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string id = ListaProductos.CurrentRow.Cells[0].Value != null ? ListaProductos.CurrentRow.Cells[0].Value.ToString() : "0" ;

            BuscarProducto(id);           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string id = Interaction.InputBox($"Ingrese la cantidad de { ProductoActivo.Nombre} que quiere ingresar:: ", "Ingresar");
            double cantidad = !string.IsNullOrWhiteSpace(id)? double.Parse(id):0;

            ProductoActivo.Cantidad += cantidad;
            Cantidad.Text = ProductoActivo.Cantidad.ToString();
            ProductoActivo.ActualizarProductos();

            SelecionDeBoton((Button)sender);
            CargarLista("Iniciar");
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos RNC (*.xlxs)|*.rnc|Todos los archivos (*.*)|*.*"; // Filtra por extensión .rnc
            openFileDialog.Title = "Seleccionar un archivo RNC";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtén el camino del archivo seleccionado
                string selectedFilePath = openFileDialog.FileName;
                //openFileDialog.FileName;

               await new Productos().CargarProductosEnMongoDBAsync(selectedFilePath);
            }            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            /* VentanaDeCarga ventana = new VentanaDeCarga();
             ventana.Show();*/
            MessageBox.Show("Se estan Actualizandos los productos por favor espere");


            CargarLista("Iniciar");
            MessageBox.Show("Productos actualizados!!");

           
            
        }

        private void Precio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char )Keys.Enter)
            {
                if (!string.IsNullOrEmpty(Precio.Text))
                {
                    double precioN = double.Parse(Precio.Text);

                    Precio2.Text = (precioN - (precioN * 0.05)).ToString();
                    Precio3.Text = (precioN - (precioN * 0.10)).ToString();
                    Precio4.Text = (precioN - (precioN * 0.20)).ToString();
                }
            }
        }


        private async void VentanaProductos_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;

            // Ejemplo de invocación en el hilo principal
            if (ListaProductos.InvokeRequired)
            {
                ListaProductos.Invoke(new Action(() => CargarLista("Iniciar")));
            }
            else
            {
                CargarLista("Iniciar");
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                ListaProductos.Rows.Clear();
                Lugar.Text = "Busqueda Activa";
                List<Productos> list = new List<Productos>();

                // Limpiar la tabla antes de llenarla
                ListaProductos.Rows.Clear();

                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    // Convertir el texto de búsqueda a minúsculas
                    string textoBusqueda = textBox1.Text.ToLower();

                    // Configurar el cliente de MongoDB

                    var collection = new MongoClient(new OneKeys().URI).GetDatabase(new OneKeys().DatabaseName).GetCollection<Productos>("Productos");

                    // Crear un filtro para buscar nombres que contengan el texto ingresado, ignorando "Generico"
                    var filtro = Builders<Productos>.Filter.And(
                        Builders<Productos>.Filter.Regex("Nombre", new BsonRegularExpression(textoBusqueda, "i")), // "i" -> Insensible a mayúsculas/minúsculas
                        Builders<Productos>.Filter.Ne("Nombre", "Generico") // Excluir "Generico"
                    );

                    // Definir una proyección para recuperar solo los campos necesarios
                    var proyeccion = Builders<Productos>.Projection
                        .Include("nombre")
                        .Include("descripcion")
                        .Include("Costo")
                        .Include("precio").Include("cantidad").Include("vendido")
                        .Include("_id"); // Excluir el campo "_id" si no es necesario

                    // Consultar MongoDB con filtro y proyección
                    var productos = collection.Find(filtro).Project(proyeccion).ToList();

                    // Rellenar la tabla con los resultados
                    foreach (var producto in productos)
                    {
                        ListaProductos.Rows.Add(
                            producto.GetValue("_id", ""),
                            producto.GetValue("nombre", ""),
                            producto.GetValue("descripcion", ""),
                            producto.GetValue("Costo", ""),
                            producto.GetValue("precio", 0)[0],
                            double.Parse(producto.GetValue("cantidad", 0).ToString()) - double.Parse(producto.GetValue("vendido", 0).ToString()),
                            producto.GetValue("vendido", 0)
                        );
                    }
                }

                foreach (Productos item in list)
                {
                    ListaProductos.Rows.Add(item.Id, item.Nombre, item.Descripcion, item.Costo, item.Precio, item.Cantidad - item.Vendido);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CargarLista("avanza");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CargarLista("atras");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CargarLista("iniciar");
        }
    }    
}
