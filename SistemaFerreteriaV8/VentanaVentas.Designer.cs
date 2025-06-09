namespace SistemaFerreteriaV8
{
    partial class VentanaVentas
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.Opciones = new System.Windows.Forms.GroupBox();
            this.Cancelar = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.menos = new System.Windows.Forms.Button();
            this.Cobrar = new System.Windows.Forms.Button();
            this.Guardar = new System.Windows.Forms.Button();
            this.Eliminar = new System.Windows.Forms.Button();
            this.mas = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PanelDeCarga = new System.Windows.Forms.Panel();
            this.Carga = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.BarraDeCarga = new System.Windows.Forms.ProgressBar();
            this.ListaDeCompras = new System.Windows.Forms.DataGridView();
            this.Nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Marca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BuscarPorNombreBox = new System.Windows.Forms.GroupBox();
            this.ListaProductos = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NombreABuscar = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.VentaRapida = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.Id = new System.Windows.Forms.TextBox();
            this.N = new System.Windows.Forms.CheckBox();
            this.Hora = new System.Windows.Forms.Label();
            this.Fecha = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.NoFactura = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tipoFactura = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.NombreCliente = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.IdCliente = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Aviso = new System.Windows.Forms.Label();
            this.descripcion = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.direccion = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Descuento = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.ADescontar = new System.Windows.Forms.TextBox();
            this.FiltroDescuento = new System.Windows.Forms.ComboBox();
            this.Total = new System.Windows.Forms.Label();
            this.SubTotal = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button6 = new System.Windows.Forms.Button();
            this.Opciones.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.PanelDeCarga.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListaDeCompras)).BeginInit();
            this.BuscarPorNombreBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListaProductos)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Red;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(6, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 49);
            this.button1.TabIndex = 0;
            this.button1.Text = "&Buscar\r\nPor nombre";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Opciones
            // 
            this.Opciones.Controls.Add(this.button6);
            this.Opciones.Controls.Add(this.Cancelar);
            this.Opciones.Controls.Add(this.button3);
            this.Opciones.Controls.Add(this.menos);
            this.Opciones.Controls.Add(this.Cobrar);
            this.Opciones.Controls.Add(this.Guardar);
            this.Opciones.Controls.Add(this.Eliminar);
            this.Opciones.Controls.Add(this.mas);
            this.Opciones.Controls.Add(this.button2);
            this.Opciones.Controls.Add(this.button1);
            this.Opciones.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Opciones.ForeColor = System.Drawing.Color.White;
            this.Opciones.Location = new System.Drawing.Point(662, 13);
            this.Opciones.Name = "Opciones";
            this.Opciones.Size = new System.Drawing.Size(378, 186);
            this.Opciones.TabIndex = 1;
            this.Opciones.TabStop = false;
            this.Opciones.Text = "Opciones";
            // 
            // Cancelar
            // 
            this.Cancelar.BackColor = System.Drawing.Color.Red;
            this.Cancelar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Cancelar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Cancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancelar.ForeColor = System.Drawing.Color.White;
            this.Cancelar.Location = new System.Drawing.Point(251, 74);
            this.Cancelar.Name = "Cancelar";
            this.Cancelar.Size = new System.Drawing.Size(117, 49);
            this.Cancelar.TabIndex = 6;
            this.Cancelar.Text = "Cancelar";
            this.Cancelar.UseVisualStyleBackColor = false;
            this.Cancelar.Click += new System.EventHandler(this.button10_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Red;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(251, 19);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(117, 49);
            this.button3.TabIndex = 9;
            this.button3.Text = "Lista de envios";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // menos
            // 
            this.menos.BackColor = System.Drawing.Color.Red;
            this.menos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.menos.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.menos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menos.ForeColor = System.Drawing.Color.White;
            this.menos.Location = new System.Drawing.Point(251, 129);
            this.menos.Name = "menos";
            this.menos.Size = new System.Drawing.Size(117, 28);
            this.menos.TabIndex = 5;
            this.menos.Text = "-1";
            this.menos.UseVisualStyleBackColor = false;
            this.menos.Click += new System.EventHandler(this.button13_Click);
            // 
            // Cobrar
            // 
            this.Cobrar.BackColor = System.Drawing.Color.Red;
            this.Cobrar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Cobrar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Cobrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cobrar.ForeColor = System.Drawing.Color.White;
            this.Cobrar.Location = new System.Drawing.Point(129, 74);
            this.Cobrar.Name = "Cobrar";
            this.Cobrar.Size = new System.Drawing.Size(117, 49);
            this.Cobrar.TabIndex = 1;
            this.Cobrar.Text = "Cobrar";
            this.Cobrar.UseVisualStyleBackColor = false;
            this.Cobrar.Click += new System.EventHandler(this.Cobrar_Click);
            // 
            // Guardar
            // 
            this.Guardar.BackColor = System.Drawing.Color.Red;
            this.Guardar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Guardar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Guardar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Guardar.ForeColor = System.Drawing.Color.White;
            this.Guardar.Location = new System.Drawing.Point(6, 74);
            this.Guardar.Name = "Guardar";
            this.Guardar.Size = new System.Drawing.Size(117, 49);
            this.Guardar.TabIndex = 0;
            this.Guardar.Text = "Guardar";
            this.Guardar.UseVisualStyleBackColor = false;
            this.Guardar.Click += new System.EventHandler(this.button18_Click);
            // 
            // Eliminar
            // 
            this.Eliminar.BackColor = System.Drawing.Color.Red;
            this.Eliminar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Eliminar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Eliminar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Eliminar.ForeColor = System.Drawing.Color.White;
            this.Eliminar.Location = new System.Drawing.Point(129, 129);
            this.Eliminar.Name = "Eliminar";
            this.Eliminar.Size = new System.Drawing.Size(117, 49);
            this.Eliminar.TabIndex = 4;
            this.Eliminar.Text = "Eliminar";
            this.Eliminar.UseVisualStyleBackColor = false;
            this.Eliminar.Click += new System.EventHandler(this.Eliminar_Click);
            // 
            // mas
            // 
            this.mas.BackColor = System.Drawing.Color.Red;
            this.mas.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.mas.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.mas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mas.ForeColor = System.Drawing.Color.White;
            this.mas.Location = new System.Drawing.Point(251, 153);
            this.mas.Name = "mas";
            this.mas.Size = new System.Drawing.Size(117, 27);
            this.mas.TabIndex = 3;
            this.mas.Text = "+1";
            this.mas.UseVisualStyleBackColor = false;
            this.mas.Click += new System.EventHandler(this.button15_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(129, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 49);
            this.button2.TabIndex = 1;
            this.button2.Text = "Facturas Por cobrar";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Black;
            this.groupBox1.Controls.Add(this.PanelDeCarga);
            this.groupBox1.Controls.Add(this.ListaDeCompras);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(13, 205);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1025, 282);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lista de Compra";
            // 
            // PanelDeCarga
            // 
            this.PanelDeCarga.Controls.Add(this.Carga);
            this.PanelDeCarga.Controls.Add(this.label14);
            this.PanelDeCarga.Controls.Add(this.BarraDeCarga);
            this.PanelDeCarga.Location = new System.Drawing.Point(273, 19);
            this.PanelDeCarga.Name = "PanelDeCarga";
            this.PanelDeCarga.Size = new System.Drawing.Size(430, 249);
            this.PanelDeCarga.TabIndex = 1;
            this.PanelDeCarga.Visible = false;
            // 
            // Carga
            // 
            this.Carga.AutoSize = true;
            this.Carga.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Carga.Location = new System.Drawing.Point(173, 151);
            this.Carga.Name = "Carga";
            this.Carga.Size = new System.Drawing.Size(72, 24);
            this.Carga.TabIndex = 2;
            this.Carga.Text = "1 / 100";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(124, 81);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(170, 24);
            this.label14.TabIndex = 1;
            this.label14.Text = "Por favor espere!";
            // 
            // BarraDeCarga
            // 
            this.BarraDeCarga.Location = new System.Drawing.Point(50, 125);
            this.BarraDeCarga.Name = "BarraDeCarga";
            this.BarraDeCarga.Size = new System.Drawing.Size(318, 23);
            this.BarraDeCarga.TabIndex = 0;
            // 
            // ListaDeCompras
            // 
            this.ListaDeCompras.AllowUserToResizeColumns = false;
            this.ListaDeCompras.AllowUserToResizeRows = false;
            this.ListaDeCompras.BackgroundColor = System.Drawing.Color.White;
            this.ListaDeCompras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListaDeCompras.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nombre,
            this.Column1,
            this.Marca,
            this.Column2,
            this.Column3,
            this.Column4});
            this.ListaDeCompras.Location = new System.Drawing.Point(6, 15);
            this.ListaDeCompras.Name = "ListaDeCompras";
            this.ListaDeCompras.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.ListaDeCompras.Size = new System.Drawing.Size(1011, 261);
            this.ListaDeCompras.TabIndex = 0;
            this.ListaDeCompras.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ListaDeCompras_CellDoubleClick);
            this.ListaDeCompras.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ListaDeCompras_CellEndEdit);
            // 
            // Nombre
            // 
            this.Nombre.HeaderText = "Nombre";
            this.Nombre.Name = "Nombre";
            this.Nombre.ReadOnly = true;
            this.Nombre.Width = 250;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Descripcion";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 200;
            // 
            // Marca
            // 
            this.Marca.HeaderText = "Marca";
            this.Marca.Name = "Marca";
            this.Marca.ReadOnly = true;
            this.Marca.Width = 200;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Precio";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Cantidad";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Total";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // BuscarPorNombreBox
            // 
            this.BuscarPorNombreBox.BackColor = System.Drawing.Color.Black;
            this.BuscarPorNombreBox.Controls.Add(this.ListaProductos);
            this.BuscarPorNombreBox.Controls.Add(this.NombreABuscar);
            this.BuscarPorNombreBox.Controls.Add(this.label10);
            this.BuscarPorNombreBox.ForeColor = System.Drawing.Color.White;
            this.BuscarPorNombreBox.Location = new System.Drawing.Point(0, 1);
            this.BuscarPorNombreBox.Name = "BuscarPorNombreBox";
            this.BuscarPorNombreBox.Size = new System.Drawing.Size(644, 187);
            this.BuscarPorNombreBox.TabIndex = 21;
            this.BuscarPorNombreBox.TabStop = false;
            this.BuscarPorNombreBox.Text = "BuscarPorNombre";
            // 
            // ListaProductos
            // 
            this.ListaProductos.AllowUserToResizeRows = false;
            this.ListaProductos.BackgroundColor = System.Drawing.Color.Black;
            this.ListaProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListaProductos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.ListaProductos.Location = new System.Drawing.Point(12, 42);
            this.ListaProductos.Name = "ListaProductos";
            this.ListaProductos.ReadOnly = true;
            this.ListaProductos.RowHeadersVisible = false;
            this.ListaProductos.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.ListaProductos.Size = new System.Drawing.Size(620, 131);
            this.ListaProductos.TabIndex = 7;
            this.ListaProductos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ListaProductos_CellContentClick);
            this.ListaProductos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ListaProductos_CellContentClick);
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Id";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Nombre";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 300;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Marca";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 110;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Precio";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // NombreABuscar
            // 
            this.NombreABuscar.Location = new System.Drawing.Point(87, 19);
            this.NombreABuscar.Name = "NombreABuscar";
            this.NombreABuscar.Size = new System.Drawing.Size(118, 20);
            this.NombreABuscar.TabIndex = 6;
            this.NombreABuscar.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Black;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(6, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 20);
            this.label10.TabIndex = 5;
            this.label10.Text = "Nombre:";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Black;
            this.groupBox2.Controls.Add(this.BuscarPorNombreBox);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.VentaRapida);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.Id);
            this.groupBox2.Controls.Add(this.N);
            this.groupBox2.Controls.Add(this.Hora);
            this.groupBox2.Controls.Add(this.Fecha);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.NoFactura);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tipoFactura);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.NombreCliente);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.IdCliente);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(644, 187);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Informe de Factutacion";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Red;
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(352, 149);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(145, 27);
            this.button5.TabIndex = 19;
            this.button5.Text = "Factura Matriciar";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // VentaRapida
            // 
            this.VentaRapida.BackColor = System.Drawing.Color.Red;
            this.VentaRapida.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.VentaRapida.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.VentaRapida.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentaRapida.ForeColor = System.Drawing.Color.White;
            this.VentaRapida.Location = new System.Drawing.Point(503, 149);
            this.VentaRapida.Name = "VentaRapida";
            this.VentaRapida.Size = new System.Drawing.Size(130, 27);
            this.VentaRapida.TabIndex = 18;
            this.VentaRapida.Text = "Venta rapida";
            this.VentaRapida.UseVisualStyleBackColor = false;
            this.VentaRapida.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Red;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(13, 149);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(91, 27);
            this.button4.TabIndex = 10;
            this.button4.Text = "Scaner";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Id
            // 
            this.Id.Location = new System.Drawing.Point(110, 156);
            this.Id.Name = "Id";
            this.Id.Size = new System.Drawing.Size(139, 20);
            this.Id.TabIndex = 17;
            this.Id.TextChanged += new System.EventHandler(this.Id_TextChanged);
            this.Id.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Id_KeyPress);
            // 
            // N
            // 
            this.N.AutoSize = true;
            this.N.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.N.Location = new System.Drawing.Point(181, 118);
            this.N.Name = "N";
            this.N.Size = new System.Drawing.Size(123, 28);
            this.N.TabIndex = 16;
            this.N.Text = "Para enviar";
            this.N.UseVisualStyleBackColor = true;
            // 
            // Hora
            // 
            this.Hora.AutoSize = true;
            this.Hora.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Hora.Location = new System.Drawing.Point(534, 47);
            this.Hora.Name = "Hora";
            this.Hora.Size = new System.Drawing.Size(54, 20);
            this.Hora.TabIndex = 13;
            this.Hora.Text = "10:30";
            // 
            // Fecha
            // 
            this.Fecha.AutoSize = true;
            this.Fecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Fecha.Location = new System.Drawing.Point(534, 20);
            this.Fecha.Name = "Fecha";
            this.Fecha.Size = new System.Drawing.Size(99, 20);
            this.Fecha.TabIndex = 12;
            this.Fecha.Text = "23/12/2026";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(482, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Hora:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(471, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Fecha:";
            // 
            // NoFactura
            // 
            this.NoFactura.AutoSize = true;
            this.NoFactura.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoFactura.Location = new System.Drawing.Point(534, 75);
            this.NoFactura.Name = "NoFactura";
            this.NoFactura.Size = new System.Drawing.Size(59, 20);
            this.NoFactura.TabIndex = 11;
            this.NoFactura.Text = "00223";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(427, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "No. Factura:";
            // 
            // tipoFactura
            // 
            this.tipoFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tipoFactura.FormattingEnabled = true;
            this.tipoFactura.Items.AddRange(new object[] {
            "Consumo",
            "Estandar",
            "Comprobante Fiscal",
            "Comprobante Gubernamental"});
            this.tipoFactura.Location = new System.Drawing.Point(181, 91);
            this.tipoFactura.Name = "tipoFactura";
            this.tipoFactura.Size = new System.Drawing.Size(135, 21);
            this.tipoFactura.TabIndex = 6;
            this.tipoFactura.SelectedIndexChanged += new System.EventHandler(this.tipoFactura_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(35, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Tipo de Factura:";
            // 
            // NombreCliente
            // 
            this.NombreCliente.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.NombreCliente.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.NombreCliente.Location = new System.Drawing.Point(181, 53);
            this.NombreCliente.Name = "NombreCliente";
            this.NombreCliente.Size = new System.Drawing.Size(159, 20);
            this.NombreCliente.TabIndex = 4;
            this.NombreCliente.TextChanged += new System.EventHandler(this.NombreCliente_TextChanged);
            this.NombreCliente.Enter += new System.EventHandler(this.NombreCliente_Enter);
            this.NombreCliente.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NombreCliente_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Nombre del Cliente:";
            // 
            // IdCliente
            // 
            this.IdCliente.Location = new System.Drawing.Point(181, 20);
            this.IdCliente.Name = "IdCliente";
            this.IdCliente.Size = new System.Drawing.Size(123, 20);
            this.IdCliente.TabIndex = 1;
            this.IdCliente.TextChanged += new System.EventHandler(this.IdCliente_TextChanged);
            this.IdCliente.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IdCliente_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(84, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Id Cliente:";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Black;
            this.groupBox3.Controls.Add(this.Aviso);
            this.groupBox3.Controls.Add(this.descripcion);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.direccion);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(12, 486);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(448, 155);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Opciones";
            // 
            // Aviso
            // 
            this.Aviso.AutoSize = true;
            this.Aviso.Enabled = false;
            this.Aviso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Aviso.ForeColor = System.Drawing.Color.Red;
            this.Aviso.Location = new System.Drawing.Point(79, 18);
            this.Aviso.Name = "Aviso";
            this.Aviso.Size = new System.Drawing.Size(367, 16);
            this.Aviso.TabIndex = 6;
            this.Aviso.Text = "La factura se marco para enviar debe agregar una direccion.";
            this.Aviso.Visible = false;
            // 
            // descripcion
            // 
            this.descripcion.Location = new System.Drawing.Point(103, 66);
            this.descripcion.Multiline = true;
            this.descripcion.Name = "descripcion";
            this.descripcion.Size = new System.Drawing.Size(326, 79);
            this.descripcion.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(35, 66);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 20);
            this.label12.TabIndex = 4;
            this.label12.Text = "Nota:";
            // 
            // direccion
            // 
            this.direccion.Location = new System.Drawing.Point(103, 40);
            this.direccion.Name = "direccion";
            this.direccion.Size = new System.Drawing.Size(326, 20);
            this.direccion.TabIndex = 3;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(6, 40);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 20);
            this.label11.TabIndex = 2;
            this.label11.Text = "Direccion:";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.Descuento);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.ADescontar);
            this.panel1.Controls.Add(this.FiltroDescuento);
            this.panel1.Controls.Add(this.Total);
            this.panel1.Controls.Add(this.SubTotal);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(6, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(327, 151);
            this.panel1.TabIndex = 5;
            // 
            // Descuento
            // 
            this.Descuento.AutoSize = true;
            this.Descuento.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Descuento.ForeColor = System.Drawing.Color.Black;
            this.Descuento.Location = new System.Drawing.Point(186, 81);
            this.Descuento.Name = "Descuento";
            this.Descuento.Size = new System.Drawing.Size(60, 24);
            this.Descuento.TabIndex = 17;
            this.Descuento.Text = "$0.00";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(3, 81);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(179, 24);
            this.label13.TabIndex = 16;
            this.label13.Text = "Total Descontado:";
            // 
            // ADescontar
            // 
            this.ADescontar.Location = new System.Drawing.Point(229, 48);
            this.ADescontar.Name = "ADescontar";
            this.ADescontar.Size = new System.Drawing.Size(95, 20);
            this.ADescontar.TabIndex = 15;
            this.ADescontar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ADescontar_KeyPress);
            // 
            // FiltroDescuento
            // 
            this.FiltroDescuento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FiltroDescuento.FormattingEnabled = true;
            this.FiltroDescuento.Items.AddRange(new object[] {
            "%",
            "$"});
            this.FiltroDescuento.Location = new System.Drawing.Point(186, 47);
            this.FiltroDescuento.Name = "FiltroDescuento";
            this.FiltroDescuento.Size = new System.Drawing.Size(40, 21);
            this.FiltroDescuento.TabIndex = 14;
            // 
            // Total
            // 
            this.Total.AutoSize = true;
            this.Total.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Total.ForeColor = System.Drawing.Color.Black;
            this.Total.Location = new System.Drawing.Point(186, 114);
            this.Total.Name = "Total";
            this.Total.Size = new System.Drawing.Size(60, 24);
            this.Total.TabIndex = 13;
            this.Total.Text = "$0.00";
            // 
            // SubTotal
            // 
            this.SubTotal.AutoSize = true;
            this.SubTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubTotal.ForeColor = System.Drawing.Color.Black;
            this.SubTotal.Location = new System.Drawing.Point(186, 13);
            this.SubTotal.Name = "SubTotal";
            this.SubTotal.Size = new System.Drawing.Size(60, 24);
            this.SubTotal.TabIndex = 11;
            this.SubTotal.Text = "$0.00";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(120, 114);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 24);
            this.label9.TabIndex = 10;
            this.label9.Text = "Total:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(56, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(126, 24);
            this.label8.TabIndex = 9;
            this.label8.Text = "Descuentos:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(77, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 24);
            this.label7.TabIndex = 8;
            this.label7.Text = "Sub Total:";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Black;
            this.groupBox4.Controls.Add(this.panel1);
            this.groupBox4.Location = new System.Drawing.Point(697, 478);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(341, 170);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Red;
            this.button6.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Location = new System.Drawing.Point(6, 129);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(117, 49);
            this.button6.TabIndex = 10;
            this.button6.Text = "Obtener Factura";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click_1);
            // 
            // VentanaVentas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1052, 697);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Opciones);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VentanaVentas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.VentanaVentas_Load);
            this.Opciones.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.PanelDeCarga.ResumeLayout(false);
            this.PanelDeCarga.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListaDeCompras)).EndInit();
            this.BuscarPorNombreBox.ResumeLayout(false);
            this.BuscarPorNombreBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListaProductos)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.DataGridViewTextBoxColumn Nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Marca;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
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
    }
}

