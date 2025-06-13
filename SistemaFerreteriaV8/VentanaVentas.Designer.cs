namespace SistemaFerreteriaV8
{
    public partial class VentanaVentas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            button1 = new Button();
            Opciones = new GroupBox();
            button6 = new Button();
            Cancelar = new Button();
            button3 = new Button();
            menos = new Button();
            Cobrar = new Button();
            Guardar = new Button();
            Eliminar = new Button();
            mas = new Button();
            button2 = new Button();
            groupBox1 = new GroupBox();
            PanelDeCarga = new Panel();
            Carga = new Label();
            label14 = new Label();
            BarraDeCarga = new ProgressBar();
            ListaDeCompras = new DataGridView();
            BuscarPorNombreBox = new GroupBox();
            ListaProductos = new DataGridView();
            Column5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            NombreABuscar = new TextBox();
            label10 = new Label();
            groupBox2 = new GroupBox();
            button5 = new Button();
            VentaRapida = new Button();
            button4 = new Button();
            Id = new TextBox();
            N = new CheckBox();
            Hora = new Label();
            Fecha = new Label();
            label6 = new Label();
            label5 = new Label();
            NoFactura = new Label();
            label4 = new Label();
            tipoFactura = new ComboBox();
            label2 = new Label();
            NombreCliente = new TextBox();
            label3 = new Label();
            IdCliente = new TextBox();
            label1 = new Label();
            groupBox3 = new GroupBox();
            Aviso = new Label();
            descripcion = new TextBox();
            label12 = new Label();
            direccion = new TextBox();
            label11 = new Label();
            panel1 = new Panel();
            Descuento = new Label();
            label13 = new Label();
            ADescontar = new TextBox();
            FiltroDescuento = new ComboBox();
            Total = new Label();
            SubTotal = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            groupBox4 = new GroupBox();
            timer1 = new System.Windows.Forms.Timer(components);
            Nombre = new DataGridViewTextBoxColumn();
            Descripcion1 = new DataGridViewTextBoxColumn();
            Marca = new DataGridViewTextBoxColumn();
            Precio = new DataGridViewTextBoxColumn();
            Cantidad = new DataGridViewTextBoxColumn();
            SubTotal1 = new DataGridViewTextBoxColumn();
            Opciones.SuspendLayout();
            groupBox1.SuspendLayout();
            PanelDeCarga.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ListaDeCompras).BeginInit();
            BuscarPorNombreBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ListaProductos).BeginInit();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            panel1.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.Red;
            button1.FlatAppearance.BorderColor = Color.White;
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(7, 22);
            button1.Margin = new Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new Size(136, 57);
            button1.TabIndex = 0;
            button1.Text = "&Buscar\r\nPor nombre";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // Opciones
            // 
            Opciones.Controls.Add(button6);
            Opciones.Controls.Add(Cancelar);
            Opciones.Controls.Add(button3);
            Opciones.Controls.Add(menos);
            Opciones.Controls.Add(Cobrar);
            Opciones.Controls.Add(Guardar);
            Opciones.Controls.Add(Eliminar);
            Opciones.Controls.Add(mas);
            Opciones.Controls.Add(button2);
            Opciones.Controls.Add(button1);
            Opciones.FlatStyle = FlatStyle.Popup;
            Opciones.ForeColor = Color.White;
            Opciones.Location = new Point(772, 15);
            Opciones.Margin = new Padding(4, 3, 4, 3);
            Opciones.Name = "Opciones";
            Opciones.Padding = new Padding(4, 3, 4, 3);
            Opciones.Size = new Size(441, 215);
            Opciones.TabIndex = 1;
            Opciones.TabStop = false;
            Opciones.Text = "Opciones";
            // 
            // button6
            // 
            button6.BackColor = Color.Red;
            button6.FlatAppearance.BorderColor = Color.White;
            button6.FlatStyle = FlatStyle.Popup;
            button6.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button6.ForeColor = Color.White;
            button6.Location = new Point(7, 149);
            button6.Margin = new Padding(4, 3, 4, 3);
            button6.Name = "button6";
            button6.Size = new Size(136, 57);
            button6.TabIndex = 10;
            button6.Text = "Obtener Factura";
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click_1;
            // 
            // Cancelar
            // 
            Cancelar.BackColor = Color.Red;
            Cancelar.FlatAppearance.BorderColor = Color.White;
            Cancelar.FlatStyle = FlatStyle.Popup;
            Cancelar.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Cancelar.ForeColor = Color.White;
            Cancelar.Location = new Point(293, 85);
            Cancelar.Margin = new Padding(4, 3, 4, 3);
            Cancelar.Name = "Cancelar";
            Cancelar.Size = new Size(136, 57);
            Cancelar.TabIndex = 6;
            Cancelar.Text = "Cancelar";
            Cancelar.UseVisualStyleBackColor = false;
            Cancelar.Click += button10_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.Red;
            button3.FlatAppearance.BorderColor = Color.White;
            button3.FlatStyle = FlatStyle.Popup;
            button3.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.White;
            button3.Location = new Point(293, 22);
            button3.Margin = new Padding(4, 3, 4, 3);
            button3.Name = "button3";
            button3.Size = new Size(136, 57);
            button3.TabIndex = 9;
            button3.Text = "Lista de envios";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // menos
            // 
            menos.BackColor = Color.Red;
            menos.FlatAppearance.BorderColor = Color.White;
            menos.FlatStyle = FlatStyle.Popup;
            menos.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            menos.ForeColor = Color.White;
            menos.Location = new Point(293, 149);
            menos.Margin = new Padding(4, 3, 4, 3);
            menos.Name = "menos";
            menos.Size = new Size(136, 32);
            menos.TabIndex = 5;
            menos.Text = "-1";
            menos.UseVisualStyleBackColor = false;
            menos.Click += button13_Click;
            // 
            // Cobrar
            // 
            Cobrar.BackColor = Color.Red;
            Cobrar.FlatAppearance.BorderColor = Color.White;
            Cobrar.FlatStyle = FlatStyle.Popup;
            Cobrar.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Cobrar.ForeColor = Color.White;
            Cobrar.Location = new Point(150, 85);
            Cobrar.Margin = new Padding(4, 3, 4, 3);
            Cobrar.Name = "Cobrar";
            Cobrar.Size = new Size(136, 57);
            Cobrar.TabIndex = 1;
            Cobrar.Text = "Cobrar";
            Cobrar.UseVisualStyleBackColor = false;
            Cobrar.Click += Cobrar_Click;
            // 
            // Guardar
            // 
            Guardar.BackColor = Color.Red;
            Guardar.FlatAppearance.BorderColor = Color.White;
            Guardar.FlatStyle = FlatStyle.Popup;
            Guardar.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Guardar.ForeColor = Color.White;
            Guardar.Location = new Point(7, 85);
            Guardar.Margin = new Padding(4, 3, 4, 3);
            Guardar.Name = "Guardar";
            Guardar.Size = new Size(136, 57);
            Guardar.TabIndex = 0;
            Guardar.Text = "Guardar";
            Guardar.UseVisualStyleBackColor = false;
            Guardar.Click += button18_Click;
            // 
            // Eliminar
            // 
            Eliminar.BackColor = Color.Red;
            Eliminar.FlatAppearance.BorderColor = Color.White;
            Eliminar.FlatStyle = FlatStyle.Popup;
            Eliminar.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Eliminar.ForeColor = Color.White;
            Eliminar.Location = new Point(150, 149);
            Eliminar.Margin = new Padding(4, 3, 4, 3);
            Eliminar.Name = "Eliminar";
            Eliminar.Size = new Size(136, 57);
            Eliminar.TabIndex = 4;
            Eliminar.Text = "Eliminar";
            Eliminar.UseVisualStyleBackColor = false;
            Eliminar.Click += Eliminar_Click;
            // 
            // mas
            // 
            mas.BackColor = Color.Red;
            mas.FlatAppearance.BorderColor = Color.White;
            mas.FlatStyle = FlatStyle.Popup;
            mas.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            mas.ForeColor = Color.White;
            mas.Location = new Point(293, 177);
            mas.Margin = new Padding(4, 3, 4, 3);
            mas.Name = "mas";
            mas.Size = new Size(136, 31);
            mas.TabIndex = 3;
            mas.Text = "+1";
            mas.UseVisualStyleBackColor = false;
            mas.Click += button15_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Red;
            button2.FlatAppearance.BorderColor = Color.White;
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.White;
            button2.Location = new Point(150, 22);
            button2.Margin = new Padding(4, 3, 4, 3);
            button2.Name = "button2";
            button2.Size = new Size(136, 57);
            button2.TabIndex = 1;
            button2.Text = "Facturas Por cobrar";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Black;
            groupBox1.Controls.Add(PanelDeCarga);
            groupBox1.Controls.Add(ListaDeCompras);
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(15, 237);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(1196, 325);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Lista de Compra";
            // 
            // PanelDeCarga
            // 
            PanelDeCarga.Controls.Add(Carga);
            PanelDeCarga.Controls.Add(label14);
            PanelDeCarga.Controls.Add(BarraDeCarga);
            PanelDeCarga.Location = new Point(318, 22);
            PanelDeCarga.Margin = new Padding(4, 3, 4, 3);
            PanelDeCarga.Name = "PanelDeCarga";
            PanelDeCarga.Size = new Size(502, 287);
            PanelDeCarga.TabIndex = 1;
            PanelDeCarga.Visible = false;
            // 
            // Carga
            // 
            Carga.AutoSize = true;
            Carga.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Carga.Location = new Point(202, 174);
            Carga.Margin = new Padding(4, 0, 4, 0);
            Carga.Name = "Carga";
            Carga.Size = new Size(72, 24);
            Carga.TabIndex = 2;
            Carga.Text = "1 / 100";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label14.Location = new Point(145, 93);
            label14.Margin = new Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new Size(170, 24);
            label14.TabIndex = 1;
            label14.Text = "Por favor espere!";
            // 
            // BarraDeCarga
            // 
            BarraDeCarga.Location = new Point(58, 144);
            BarraDeCarga.Margin = new Padding(4, 3, 4, 3);
            BarraDeCarga.Name = "BarraDeCarga";
            BarraDeCarga.Size = new Size(371, 27);
            BarraDeCarga.TabIndex = 0;
            // 
            // ListaDeCompras
            // 
            ListaDeCompras.AllowUserToResizeColumns = false;
            ListaDeCompras.AllowUserToResizeRows = false;
            ListaDeCompras.BackgroundColor = Color.White;
            ListaDeCompras.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ListaDeCompras.Columns.AddRange(new DataGridViewColumn[] { Nombre, Descripcion1, Marca, Precio, Cantidad, SubTotal1 });
            ListaDeCompras.Location = new Point(7, 17);
            ListaDeCompras.Margin = new Padding(4, 3, 4, 3);
            ListaDeCompras.Name = "ListaDeCompras";
            ListaDeCompras.RowTemplate.DefaultCellStyle.ForeColor = Color.Black;
            ListaDeCompras.Size = new Size(1180, 301);
            ListaDeCompras.TabIndex = 0;
            ListaDeCompras.CellDoubleClick += ListaDeCompras_CellDoubleClick;
            ListaDeCompras.CellEndEdit += ListaDeCompras_CellEndEdit;
            // 
            // BuscarPorNombreBox
            // 
            BuscarPorNombreBox.BackColor = Color.Black;
            BuscarPorNombreBox.Controls.Add(ListaProductos);
            BuscarPorNombreBox.Controls.Add(NombreABuscar);
            BuscarPorNombreBox.Controls.Add(label10);
            BuscarPorNombreBox.ForeColor = Color.White;
            BuscarPorNombreBox.Location = new Point(0, 1);
            BuscarPorNombreBox.Margin = new Padding(4, 3, 4, 3);
            BuscarPorNombreBox.Name = "BuscarPorNombreBox";
            BuscarPorNombreBox.Padding = new Padding(4, 3, 4, 3);
            BuscarPorNombreBox.Size = new Size(751, 216);
            BuscarPorNombreBox.TabIndex = 21;
            BuscarPorNombreBox.TabStop = false;
            BuscarPorNombreBox.Text = "BuscarPorNombre";
            // 
            // ListaProductos
            // 
            ListaProductos.AllowUserToResizeRows = false;
            ListaProductos.BackgroundColor = Color.Black;
            ListaProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ListaProductos.Columns.AddRange(new DataGridViewColumn[] { Column5, dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4 });
            ListaProductos.Location = new Point(14, 48);
            ListaProductos.Margin = new Padding(4, 3, 4, 3);
            ListaProductos.Name = "ListaProductos";
            ListaProductos.ReadOnly = true;
            ListaProductos.RowHeadersVisible = false;
            ListaProductos.RowTemplate.DefaultCellStyle.ForeColor = Color.Black;
            ListaProductos.Size = new Size(723, 151);
            ListaProductos.TabIndex = 7;
            ListaProductos.CellClick += ListaProductos_CellContentClick;
            ListaProductos.CellContentClick += ListaProductos_CellContentClick;
            // 
            // Column5
            // 
            Column5.HeaderText = "Id";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Nombre";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Width = 300;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Marca";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            dataGridViewTextBoxColumn3.Width = 110;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Precio";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // NombreABuscar
            // 
            NombreABuscar.Location = new Point(102, 22);
            NombreABuscar.Margin = new Padding(4, 3, 4, 3);
            NombreABuscar.Name = "NombreABuscar";
            NombreABuscar.Size = new Size(137, 23);
            NombreABuscar.TabIndex = 6;
            NombreABuscar.TextChanged += textBox1_TextChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.Black;
            label10.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.ForeColor = Color.White;
            label10.Location = new Point(7, 22);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(76, 20);
            label10.TabIndex = 5;
            label10.Text = "Nombre:";
            label10.Click += label10_Click;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.Black;
            groupBox2.Controls.Add(BuscarPorNombreBox);
            groupBox2.Controls.Add(button5);
            groupBox2.Controls.Add(VentaRapida);
            groupBox2.Controls.Add(button4);
            groupBox2.Controls.Add(Id);
            groupBox2.Controls.Add(N);
            groupBox2.Controls.Add(Hora);
            groupBox2.Controls.Add(Fecha);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(NoFactura);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(tipoFactura);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(NombreCliente);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(IdCliente);
            groupBox2.Controls.Add(label1);
            groupBox2.ForeColor = Color.White;
            groupBox2.Location = new Point(14, 14);
            groupBox2.Margin = new Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 3, 4, 3);
            groupBox2.Size = new Size(751, 216);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Informe de Factutacion";
            // 
            // button5
            // 
            button5.BackColor = Color.Red;
            button5.FlatAppearance.BorderColor = Color.White;
            button5.FlatStyle = FlatStyle.Popup;
            button5.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button5.ForeColor = Color.White;
            button5.Location = new Point(411, 172);
            button5.Margin = new Padding(4, 3, 4, 3);
            button5.Name = "button5";
            button5.Size = new Size(169, 31);
            button5.TabIndex = 19;
            button5.Text = "Factura Matriciar";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click_1;
            // 
            // VentaRapida
            // 
            VentaRapida.BackColor = Color.Red;
            VentaRapida.FlatAppearance.BorderColor = Color.White;
            VentaRapida.FlatStyle = FlatStyle.Popup;
            VentaRapida.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            VentaRapida.ForeColor = Color.White;
            VentaRapida.Location = new Point(587, 172);
            VentaRapida.Margin = new Padding(4, 3, 4, 3);
            VentaRapida.Name = "VentaRapida";
            VentaRapida.Size = new Size(152, 31);
            VentaRapida.TabIndex = 18;
            VentaRapida.Text = "Venta rapida";
            VentaRapida.UseVisualStyleBackColor = false;
            VentaRapida.Click += button5_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.Red;
            button4.FlatAppearance.BorderColor = Color.White;
            button4.FlatStyle = FlatStyle.Popup;
            button4.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button4.ForeColor = Color.White;
            button4.Location = new Point(15, 172);
            button4.Margin = new Padding(4, 3, 4, 3);
            button4.Name = "button4";
            button4.Size = new Size(106, 31);
            button4.TabIndex = 10;
            button4.Text = "Scaner";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // Id
            // 
            Id.Location = new Point(128, 180);
            Id.Margin = new Padding(4, 3, 4, 3);
            Id.Name = "Id";
            Id.Size = new Size(162, 23);
            Id.TabIndex = 17;
            Id.KeyPress += Id_KeyPress;
            // 
            // N
            // 
            N.AutoSize = true;
            N.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            N.Location = new Point(211, 136);
            N.Margin = new Padding(4, 3, 4, 3);
            N.Name = "N";
            N.Size = new Size(123, 28);
            N.TabIndex = 16;
            N.Text = "Para enviar";
            N.UseVisualStyleBackColor = true;
            // 
            // Hora
            // 
            Hora.AutoSize = true;
            Hora.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Hora.Location = new Point(623, 54);
            Hora.Margin = new Padding(4, 0, 4, 0);
            Hora.Name = "Hora";
            Hora.Size = new Size(54, 20);
            Hora.TabIndex = 13;
            Hora.Text = "10:30";
            // 
            // Fecha
            // 
            Fecha.AutoSize = true;
            Fecha.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Fecha.Location = new Point(623, 23);
            Fecha.Margin = new Padding(4, 0, 4, 0);
            Fecha.Name = "Fecha";
            Fecha.Size = new Size(99, 20);
            Fecha.TabIndex = 12;
            Fecha.Text = "23/12/2026";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(562, 54);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(53, 20);
            label6.TabIndex = 10;
            label6.Text = "Hora:";
            label6.Click += label6_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(550, 23);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(64, 20);
            label5.TabIndex = 9;
            label5.Text = "Fecha:";
            // 
            // NoFactura
            // 
            NoFactura.AutoSize = true;
            NoFactura.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NoFactura.Location = new Point(623, 87);
            NoFactura.Margin = new Padding(4, 0, 4, 0);
            NoFactura.Name = "NoFactura";
            NoFactura.Size = new Size(59, 20);
            NoFactura.TabIndex = 11;
            NoFactura.Text = "00223";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(498, 87);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(108, 20);
            label4.TabIndex = 7;
            label4.Text = "No. Factura:";
            // 
            // tipoFactura
            // 
            tipoFactura.DropDownStyle = ComboBoxStyle.DropDownList;
            tipoFactura.FormattingEnabled = true;
            tipoFactura.Items.AddRange(new object[] { "Consumo", "Estandar", "Comprobante Fiscal", "Comprobante Gubernamental" });
            tipoFactura.Location = new Point(211, 105);
            tipoFactura.Margin = new Padding(4, 3, 4, 3);
            tipoFactura.Name = "tipoFactura";
            tipoFactura.Size = new Size(157, 23);
            tipoFactura.TabIndex = 6;
            tipoFactura.SelectedIndexChanged += tipoFactura_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(41, 106);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(140, 20);
            label2.TabIndex = 5;
            label2.Text = "Tipo de Factura:";
            // 
            // NombreCliente
            // 
            NombreCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            NombreCliente.AutoCompleteSource = AutoCompleteSource.CustomSource;
            NombreCliente.Location = new Point(211, 61);
            NombreCliente.Margin = new Padding(4, 3, 4, 3);
            NombreCliente.Name = "NombreCliente";
            NombreCliente.Size = new Size(185, 23);
            NombreCliente.TabIndex = 4;
            NombreCliente.TextChanged += NombreCliente_TextChanged;
            NombreCliente.Enter += NombreCliente_Enter;
            NombreCliente.KeyPress += NombreCliente_KeyPress;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(10, 61);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(166, 20);
            label3.TabIndex = 3;
            label3.Text = "Nombre del Cliente:";
            // 
            // IdCliente
            // 
            IdCliente.Location = new Point(211, 23);
            IdCliente.Margin = new Padding(4, 3, 4, 3);
            IdCliente.Name = "IdCliente";
            IdCliente.Size = new Size(143, 23);
            IdCliente.TabIndex = 1;
            IdCliente.KeyPress += IdCliente_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(98, 23);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(91, 20);
            label1.TabIndex = 0;
            label1.Text = "Id Cliente:";
            // 
            // groupBox3
            // 
            groupBox3.BackColor = Color.Black;
            groupBox3.Controls.Add(Aviso);
            groupBox3.Controls.Add(descripcion);
            groupBox3.Controls.Add(label12);
            groupBox3.Controls.Add(direccion);
            groupBox3.Controls.Add(label11);
            groupBox3.FlatStyle = FlatStyle.Popup;
            groupBox3.ForeColor = Color.White;
            groupBox3.Location = new Point(14, 561);
            groupBox3.Margin = new Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(4, 3, 4, 3);
            groupBox3.Size = new Size(523, 179);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Opciones";
            // 
            // Aviso
            // 
            Aviso.AutoSize = true;
            Aviso.Enabled = false;
            Aviso.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Aviso.ForeColor = Color.Red;
            Aviso.Location = new Point(92, 21);
            Aviso.Margin = new Padding(4, 0, 4, 0);
            Aviso.Name = "Aviso";
            Aviso.Size = new Size(367, 16);
            Aviso.TabIndex = 6;
            Aviso.Text = "La factura se marco para enviar debe agregar una direccion.";
            Aviso.Visible = false;
            // 
            // descripcion
            // 
            descripcion.Location = new Point(120, 76);
            descripcion.Margin = new Padding(4, 3, 4, 3);
            descripcion.Multiline = true;
            descripcion.Name = "descripcion";
            descripcion.Size = new Size(380, 91);
            descripcion.TabIndex = 5;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label12.Location = new Point(41, 76);
            label12.Margin = new Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new Size(52, 20);
            label12.TabIndex = 4;
            label12.Text = "Nota:";
            // 
            // direccion
            // 
            direccion.Location = new Point(120, 46);
            direccion.Margin = new Padding(4, 3, 4, 3);
            direccion.Name = "direccion";
            direccion.Size = new Size(380, 23);
            direccion.TabIndex = 3;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label11.Location = new Point(7, 46);
            label11.Margin = new Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new Size(89, 20);
            label11.TabIndex = 2;
            label11.Text = "Direccion:";
            label11.Click += label11_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(Descuento);
            panel1.Controls.Add(label13);
            panel1.Controls.Add(ADescontar);
            panel1.Controls.Add(FiltroDescuento);
            panel1.Controls.Add(Total);
            panel1.Controls.Add(SubTotal);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label7);
            panel1.ForeColor = Color.White;
            panel1.Location = new Point(7, 15);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(382, 174);
            panel1.TabIndex = 5;
            // 
            // Descuento
            // 
            Descuento.AutoSize = true;
            Descuento.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Descuento.ForeColor = Color.Black;
            Descuento.Location = new Point(217, 93);
            Descuento.Margin = new Padding(4, 0, 4, 0);
            Descuento.Name = "Descuento";
            Descuento.Size = new Size(60, 24);
            Descuento.TabIndex = 17;
            Descuento.Text = "$0.00";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label13.ForeColor = Color.Black;
            label13.Location = new Point(4, 93);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(179, 24);
            label13.TabIndex = 16;
            label13.Text = "Total Descontado:";
            // 
            // ADescontar
            // 
            ADescontar.Location = new Point(267, 55);
            ADescontar.Margin = new Padding(4, 3, 4, 3);
            ADescontar.Name = "ADescontar";
            ADescontar.Size = new Size(110, 23);
            ADescontar.TabIndex = 15;
            ADescontar.KeyPress += ADescontar_KeyPress;
            // 
            // FiltroDescuento
            // 
            FiltroDescuento.DropDownStyle = ComboBoxStyle.DropDownList;
            FiltroDescuento.FormattingEnabled = true;
            FiltroDescuento.Items.AddRange(new object[] { "%", "$" });
            FiltroDescuento.Location = new Point(217, 54);
            FiltroDescuento.Margin = new Padding(4, 3, 4, 3);
            FiltroDescuento.Name = "FiltroDescuento";
            FiltroDescuento.Size = new Size(46, 23);
            FiltroDescuento.TabIndex = 14;
            // 
            // Total
            // 
            Total.AutoSize = true;
            Total.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Total.ForeColor = Color.Black;
            Total.Location = new Point(217, 132);
            Total.Margin = new Padding(4, 0, 4, 0);
            Total.Name = "Total";
            Total.Size = new Size(60, 24);
            Total.TabIndex = 13;
            Total.Text = "$0.00";
            // 
            // SubTotal
            // 
            SubTotal.AutoSize = true;
            SubTotal.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            SubTotal.ForeColor = Color.Black;
            SubTotal.Location = new Point(217, 15);
            SubTotal.Margin = new Padding(4, 0, 4, 0);
            SubTotal.Name = "SubTotal";
            SubTotal.Size = new Size(60, 24);
            SubTotal.TabIndex = 11;
            SubTotal.Text = "$0.00";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.Black;
            label9.Location = new Point(140, 132);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(62, 24);
            label9.TabIndex = 10;
            label9.Text = "Total:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.Black;
            label8.Location = new Point(65, 54);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(126, 24);
            label8.TabIndex = 9;
            label8.Text = "Descuentos:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.Black;
            label7.Location = new Point(90, 15);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(105, 24);
            label7.TabIndex = 8;
            label7.Text = "Sub Total:";
            // 
            // groupBox4
            // 
            groupBox4.BackColor = Color.Black;
            groupBox4.Controls.Add(panel1);
            groupBox4.Location = new Point(813, 552);
            groupBox4.Margin = new Padding(4, 3, 4, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(4, 3, 4, 3);
            groupBox4.Size = new Size(398, 196);
            groupBox4.TabIndex = 6;
            groupBox4.TabStop = false;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 3000;
            timer1.Tick += timer1_Tick;
            // 
            // Nombre
            // 
            Nombre.HeaderText = "Nombre";
            Nombre.Name = "Nombre";
            Nombre.ReadOnly = true;
            Nombre.Width = 250;
            // 
            // Descripcion1
            // 
            Descripcion1.HeaderText = "Descripcion";
            Descripcion1.Name = "Descripcion1";
            Descripcion1.ReadOnly = true;
            Descripcion1.Width = 200;
            // 
            // Marca
            // 
            Marca.HeaderText = "Marca";
            Marca.Name = "Marca";
            Marca.ReadOnly = true;
            Marca.Width = 200;
            // 
            // Precio
            // 
            Precio.HeaderText = "Precio";
            Precio.Name = "Precio";
            Precio.ReadOnly = true;
            Precio.Resizable = DataGridViewTriState.True;
            // 
            // Cantidad
            // 
            Cantidad.HeaderText = "Cantidad";
            Cantidad.Name = "Cantidad";
            // 
            // SubTotal1
            // 
            SubTotal1.HeaderText = "Total";
            SubTotal1.Name = "SubTotal1";
            SubTotal1.ReadOnly = true;
            // 
            // VentanaVentas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1227, 804);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(Opciones);
            Controls.Add(groupBox1);
            Controls.Add(groupBox4);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "VentanaVentas";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += VentanaVentas_Load;
            Opciones.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            PanelDeCarga.ResumeLayout(false);
            PanelDeCarga.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ListaDeCompras).EndInit();
            BuscarPorNombreBox.ResumeLayout(false);
            BuscarPorNombreBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ListaProductos).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox4.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox Opciones;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox NombreCliente;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox IdCliente;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox tipoFactura;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Hora;
        private System.Windows.Forms.Label Fecha;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label NoFactura;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView ListaDeCompras;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button Cancelar;
        private System.Windows.Forms.Button menos;
        private System.Windows.Forms.Button Eliminar;
        private System.Windows.Forms.Button mas;
        private System.Windows.Forms.Button Cobrar;
        private System.Windows.Forms.Button Guardar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label Total;
        private System.Windows.Forms.Label SubTotal;
        private System.Windows.Forms.CheckBox N;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox descripcion;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox direccion;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox Id;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button VentaRapida;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label Aviso;
        private System.Windows.Forms.GroupBox BuscarPorNombreBox;
        private System.Windows.Forms.DataGridView ListaProductos;
        private System.Windows.Forms.TextBox NombreABuscar;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox ADescontar;
        private System.Windows.Forms.ComboBox FiltroDescuento;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label Descuento;
        private System.Windows.Forms.Panel PanelDeCarga;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ProgressBar BarraDeCarga;
        private System.Windows.Forms.Label Carga;
        private System.Windows.Forms.Button button6;
        private DataGridViewTextBoxColumn Nombre;
        private DataGridViewTextBoxColumn Descripcion1;
        private DataGridViewTextBoxColumn Marca;
        private DataGridViewTextBoxColumn Precio;
        private DataGridViewTextBoxColumn Cantidad;
        private DataGridViewTextBoxColumn SubTotal1;
    }
}

