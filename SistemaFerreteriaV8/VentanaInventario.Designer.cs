namespace SistemaFerreteriaV8
{
    partial class VentanaInventario
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
            this.BoxFiltrar = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.OrdenarPor = new System.Windows.Forms.ComboBox();
            this.TextoABuscar = new System.Windows.Forms.TextBox();
            this.Clave = new System.Windows.Forms.ComboBox();
            this.ListaDeProductos = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Resumen = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.GananciaEsperada = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.GananciaActual = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.InversionTotal = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.TotalProductos = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TextPagina = new System.Windows.Forms.Label();
            this.BoxFiltrar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListaDeProductos)).BeginInit();
            this.Resumen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // BoxFiltrar
            // 
            this.BoxFiltrar.Controls.Add(this.button2);
            this.BoxFiltrar.Controls.Add(this.button1);
            this.BoxFiltrar.Controls.Add(this.label1);
            this.BoxFiltrar.Controls.Add(this.OrdenarPor);
            this.BoxFiltrar.Controls.Add(this.TextoABuscar);
            this.BoxFiltrar.Controls.Add(this.Clave);
            this.BoxFiltrar.Location = new System.Drawing.Point(12, 4);
            this.BoxFiltrar.Name = "BoxFiltrar";
            this.BoxFiltrar.Size = new System.Drawing.Size(1028, 47);
            this.BoxFiltrar.TabIndex = 0;
            this.BoxFiltrar.TabStop = false;
            this.BoxFiltrar.Text = "Filtrar";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(886, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(124, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Exportar todo";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Red;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(710, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Exportar Lista actual";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(395, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ordenar Por";
            // 
            // OrdenarPor
            // 
            this.OrdenarPor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OrdenarPor.FormattingEnabled = true;
            this.OrdenarPor.Items.AddRange(new object[] {
            "Id",
            "Nombre",
            "Marca",
            "Categoria",
            "Costos",
            "Cantidad",
            "Precios",
            "Vendidos"});
            this.OrdenarPor.Location = new System.Drawing.Point(507, 17);
            this.OrdenarPor.Name = "OrdenarPor";
            this.OrdenarPor.Size = new System.Drawing.Size(121, 21);
            this.OrdenarPor.TabIndex = 2;
            this.OrdenarPor.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // TextoABuscar
            // 
            this.TextoABuscar.Location = new System.Drawing.Point(133, 17);
            this.TextoABuscar.Name = "TextoABuscar";
            this.TextoABuscar.Size = new System.Drawing.Size(160, 20);
            this.TextoABuscar.TabIndex = 1;
            this.TextoABuscar.TextChanged += new System.EventHandler(this.TextoABuscar_TextChanged);
            // 
            // Clave
            // 
            this.Clave.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Clave.FormattingEnabled = true;
            this.Clave.Items.AddRange(new object[] {
            "Id",
            "Nombre",
            "Marca",
            "Precio",
            "Costo",
            "Vendido"});
            this.Clave.Location = new System.Drawing.Point(6, 17);
            this.Clave.Name = "Clave";
            this.Clave.Size = new System.Drawing.Size(121, 21);
            this.Clave.TabIndex = 0;
            this.Clave.SelectedIndexChanged += new System.EventHandler(this.Clave_SelectedIndexChanged);
            // 
            // ListaDeProductos
            // 
            this.ListaDeProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListaDeProductos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column4,
            this.Column3,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8});
            this.ListaDeProductos.Location = new System.Drawing.Point(12, 57);
            this.ListaDeProductos.Name = "ListaDeProductos";
            this.ListaDeProductos.Size = new System.Drawing.Size(1028, 381);
            this.ListaDeProductos.TabIndex = 1;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Id";
            this.Column1.Name = "Column1";
            this.Column1.Width = 125;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Nombre";
            this.Column2.Name = "Column2";
            this.Column2.Width = 225;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Marca";
            this.Column4.Name = "Column4";
            this.Column4.Width = 125;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Categoria";
            this.Column3.Name = "Column3";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Costo";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Cantidad";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Precios";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Vendidos";
            this.Column8.Name = "Column8";
            // 
            // Resumen
            // 
            this.Resumen.Controls.Add(this.label10);
            this.Resumen.Controls.Add(this.label11);
            this.Resumen.Controls.Add(this.pictureBox7);
            this.Resumen.Controls.Add(this.label12);
            this.Resumen.Controls.Add(this.label13);
            this.Resumen.Controls.Add(this.pictureBox8);
            this.Resumen.Controls.Add(this.label14);
            this.Resumen.Controls.Add(this.label15);
            this.Resumen.Controls.Add(this.pictureBox9);
            this.Resumen.Controls.Add(this.label16);
            this.Resumen.Controls.Add(this.label17);
            this.Resumen.Controls.Add(this.pictureBox10);
            this.Resumen.Controls.Add(this.GananciaEsperada);
            this.Resumen.Controls.Add(this.label9);
            this.Resumen.Controls.Add(this.pictureBox4);
            this.Resumen.Controls.Add(this.GananciaActual);
            this.Resumen.Controls.Add(this.label7);
            this.Resumen.Controls.Add(this.pictureBox3);
            this.Resumen.Controls.Add(this.InversionTotal);
            this.Resumen.Controls.Add(this.label5);
            this.Resumen.Controls.Add(this.pictureBox2);
            this.Resumen.Controls.Add(this.TotalProductos);
            this.Resumen.Controls.Add(this.label2);
            this.Resumen.Controls.Add(this.pictureBox1);
            this.Resumen.Location = new System.Drawing.Point(18, 500);
            this.Resumen.Name = "Resumen";
            this.Resumen.Size = new System.Drawing.Size(1022, 185);
            this.Resumen.TabIndex = 2;
            this.Resumen.TabStop = false;
            this.Resumen.Text = "Resumen";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(790, 147);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 18);
            this.label10.TabIndex = 23;
            this.label10.Text = "0.00";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(789, 115);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(168, 20);
            this.label11.TabIndex = 22;
            this.label11.Text = "Ganancia Esperada";
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = global::SistemaFerreteriaV8.Properties.Resources.inventory_2_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24;
            this.pictureBox7.Location = new System.Drawing.Point(727, 115);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(55, 50);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox7.TabIndex = 21;
            this.pictureBox7.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(555, 147);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 18);
            this.label12.TabIndex = 20;
            this.label12.Text = "0.00";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(554, 115);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(142, 20);
            this.label13.TabIndex = 19;
            this.label13.Text = "Ganancia Actual";
            // 
            // pictureBox8
            // 
            this.pictureBox8.Image = global::SistemaFerreteriaV8.Properties.Resources.ganancias;
            this.pictureBox8.Location = new System.Drawing.Point(492, 115);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(55, 50);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox8.TabIndex = 18;
            this.pictureBox8.TabStop = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(316, 147);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(36, 18);
            this.label14.TabIndex = 17;
            this.label14.Text = "0.00";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(315, 115);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(120, 20);
            this.label15.TabIndex = 16;
            this.label15.Text = "Total Vendido";
            // 
            // pictureBox9
            // 
            this.pictureBox9.Image = global::SistemaFerreteriaV8.Properties.Resources.vetas;
            this.pictureBox9.Location = new System.Drawing.Point(253, 115);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(55, 50);
            this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox9.TabIndex = 15;
            this.pictureBox9.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(84, 147);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(36, 18);
            this.label16.TabIndex = 14;
            this.label16.Text = "0.00";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(83, 115);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(147, 16);
            this.label17.TabIndex = 13;
            this.label17.Text = "Productos Vendidos";
            // 
            // pictureBox10
            // 
            this.pictureBox10.Image = global::SistemaFerreteriaV8.Properties.Resources.vendido;
            this.pictureBox10.Location = new System.Drawing.Point(21, 115);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(55, 50);
            this.pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox10.TabIndex = 12;
            this.pictureBox10.TabStop = false;
            // 
            // GananciaEsperada
            // 
            this.GananciaEsperada.AutoSize = true;
            this.GananciaEsperada.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GananciaEsperada.Location = new System.Drawing.Point(790, 65);
            this.GananciaEsperada.Name = "GananciaEsperada";
            this.GananciaEsperada.Size = new System.Drawing.Size(36, 18);
            this.GananciaEsperada.TabIndex = 11;
            this.GananciaEsperada.Text = "0.00";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(789, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(168, 20);
            this.label9.TabIndex = 10;
            this.label9.Text = "Ganancia Esperada";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::SistemaFerreteriaV8.Properties.Resources.inventory_2_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24;
            this.pictureBox4.Location = new System.Drawing.Point(727, 33);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(55, 50);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 9;
            this.pictureBox4.TabStop = false;
            // 
            // GananciaActual
            // 
            this.GananciaActual.AutoSize = true;
            this.GananciaActual.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GananciaActual.Location = new System.Drawing.Point(555, 65);
            this.GananciaActual.Name = "GananciaActual";
            this.GananciaActual.Size = new System.Drawing.Size(36, 18);
            this.GananciaActual.TabIndex = 8;
            this.GananciaActual.Text = "0.00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(554, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 20);
            this.label7.TabIndex = 7;
            this.label7.Text = "Ganancia Actual";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::SistemaFerreteriaV8.Properties.Resources.ganancias;
            this.pictureBox3.Location = new System.Drawing.Point(492, 33);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(55, 50);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 6;
            this.pictureBox3.TabStop = false;
            // 
            // InversionTotal
            // 
            this.InversionTotal.AutoSize = true;
            this.InversionTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InversionTotal.Location = new System.Drawing.Point(316, 65);
            this.InversionTotal.Name = "InversionTotal";
            this.InversionTotal.Size = new System.Drawing.Size(36, 18);
            this.InversionTotal.TabIndex = 5;
            this.InversionTotal.Text = "0.00";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(315, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Inversion Total";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::SistemaFerreteriaV8.Properties.Resources.Inversion;
            this.pictureBox2.Location = new System.Drawing.Point(253, 33);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(55, 50);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // TotalProductos
            // 
            this.TotalProductos.AutoSize = true;
            this.TotalProductos.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalProductos.Location = new System.Drawing.Point(84, 65);
            this.TotalProductos.Name = "TotalProductos";
            this.TotalProductos.Size = new System.Drawing.Size(36, 18);
            this.TotalProductos.TabIndex = 2;
            this.TotalProductos.Text = "0.00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(83, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Total Productos";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SistemaFerreteriaV8.Properties.Resources.inventory_2_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24;
            this.pictureBox1.Location = new System.Drawing.Point(21, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(55, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::SistemaFerreteriaV8.Properties.Resources.arrow_back_ios_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24;
            this.pictureBox6.Location = new System.Drawing.Point(181, 444);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(55, 50);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox6.TabIndex = 13;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Click += new System.EventHandler(this.pictureBox6_Click);
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::SistemaFerreteriaV8.Properties.Resources.arrow_forward_ios_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24;
            this.pictureBox5.Location = new System.Drawing.Point(811, 444);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(55, 50);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox5.TabIndex = 12;
            this.pictureBox5.TabStop = false;
            this.pictureBox5.Click += new System.EventHandler(this.pictureBox5_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(459, 444);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 20);
            this.label3.TabIndex = 24;
            this.label3.Text = "Paginacion";
            // 
            // TextPagina
            // 
            this.TextPagina.AutoSize = true;
            this.TextPagina.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextPagina.Location = new System.Drawing.Point(459, 467);
            this.TextPagina.Name = "TextPagina";
            this.TextPagina.Size = new System.Drawing.Size(97, 20);
            this.TextPagina.TabIndex = 25;
            this.TextPagina.Text = "Paginacion";
            // 
            // VentanaInventario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.TextPagina);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.Resumen);
            this.Controls.Add(this.ListaDeProductos);
            this.Controls.Add(this.BoxFiltrar);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VentanaInventario";
            this.Text = "VentanaInventario";
            this.Load += new System.EventHandler(this.VentanaInventario_Load);
            this.BoxFiltrar.ResumeLayout(false);
            this.BoxFiltrar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListaDeProductos)).EndInit();
            this.Resumen.ResumeLayout(false);
            this.Resumen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox BoxFiltrar;
        private System.Windows.Forms.ComboBox Clave;
        private System.Windows.Forms.TextBox TextoABuscar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox OrdenarPor;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView ListaDeProductos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.GroupBox Resumen;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label GananciaEsperada;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label GananciaActual;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label InversionTotal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label TotalProductos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.PictureBox pictureBox10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label TextPagina;
    }
}