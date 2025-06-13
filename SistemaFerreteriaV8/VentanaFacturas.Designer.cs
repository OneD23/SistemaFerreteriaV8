namespace SistemaFerreteriaV8
{
    partial class VentanaFacturas
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
            ListaDeFacturas = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewCheckBoxColumn();
            Column5 = new DataGridViewCheckBoxColumn();
            label1 = new Label();
            label2 = new Label();
            CantidadFactura = new Label();
            Fecha1 = new DateTimePicker();
            Fecha2 = new DateTimePicker();
            label4 = new Label();
            label5 = new Label();
            Id = new TextBox();
            comboBox1 = new ComboBox();
            groupBox1 = new GroupBox();
            button1 = new Button();
            button2 = new Button();
            Paginacion = new Label();
            ((System.ComponentModel.ISupportInitialize)ListaDeFacturas).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // ListaDeFacturas
            // 
            ListaDeFacturas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ListaDeFacturas.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column7, Column8, Column9, Column6, Column4, Column5 });
            ListaDeFacturas.Location = new Point(16, 175);
            ListaDeFacturas.Margin = new Padding(4, 3, 4, 3);
            ListaDeFacturas.Name = "ListaDeFacturas";
            ListaDeFacturas.ReadOnly = true;
            ListaDeFacturas.Size = new Size(1234, 360);
            ListaDeFacturas.TabIndex = 0;
            ListaDeFacturas.CellClick += ListaDeFacturas_CellContentClick;
            ListaDeFacturas.CellContentClick += ListaDeFacturas_CellContentClick;
            // 
            // Column1
            // 
            Column1.HeaderText = "Id";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Width = 80;
            // 
            // Column2
            // 
            Column2.HeaderText = "Fecha";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Width = 125;
            // 
            // Column3
            // 
            Column3.HeaderText = "Cliente";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Width = 125;
            // 
            // Column7
            // 
            Column7.HeaderText = "Tipo de Factura";
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            Column7.Width = 130;
            // 
            // Column8
            // 
            Column8.HeaderText = "Descripcion";
            Column8.Name = "Column8";
            Column8.ReadOnly = true;
            Column8.Width = 175;
            // 
            // Column9
            // 
            Column9.HeaderText = "Empleado";
            Column9.Name = "Column9";
            Column9.ReadOnly = true;
            // 
            // Column6
            // 
            Column6.HeaderText = "Valor";
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            // 
            // Column4
            // 
            Column4.HeaderText = "Para enviar";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            // 
            // Column5
            // 
            Column5.HeaderText = "Salda";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(498, 38);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(121, 31);
            label1.TabIndex = 1;
            label1.Text = "Facturas";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(946, 563);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(132, 20);
            label2.TabIndex = 2;
            label2.Text = "Total de facturas:";
            // 
            // CantidadFactura
            // 
            CantidadFactura.AutoSize = true;
            CantidadFactura.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CantidadFactura.ForeColor = Color.White;
            CantidadFactura.Location = new Point(1107, 563);
            CantidadFactura.Margin = new Padding(4, 0, 4, 0);
            CantidadFactura.Name = "CantidadFactura";
            CantidadFactura.Size = new Size(18, 20);
            CantidadFactura.TabIndex = 3;
            CantidadFactura.Text = "0";
            // 
            // Fecha1
            // 
            Fecha1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Fecha1.Format = DateTimePickerFormat.Custom;
            Fecha1.Location = new Point(114, 134);
            Fecha1.Margin = new Padding(4, 3, 4, 3);
            Fecha1.Name = "Fecha1";
            Fecha1.Size = new Size(142, 26);
            Fecha1.TabIndex = 4;
            Fecha1.Value = new DateTime(2024, 2, 9, 0, 0, 0, 0);
            // 
            // Fecha2
            // 
            Fecha2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Fecha2.Format = DateTimePickerFormat.Custom;
            Fecha2.Location = new Point(391, 134);
            Fecha2.Margin = new Padding(4, 3, 4, 3);
            Fecha2.Name = "Fecha2";
            Fecha2.Size = new Size(142, 26);
            Fecha2.TabIndex = 5;
            Fecha2.Value = new DateTime(2024, 2, 9, 23, 59, 0, 0);
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(14, 135);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(80, 25);
            label4.TabIndex = 6;
            label4.Text = "Desde:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(298, 134);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(74, 25);
            label5.TabIndex = 7;
            label5.Text = "Hasta:";
            // 
            // Id
            // 
            Id.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Id.Location = new Point(173, 37);
            Id.Margin = new Padding(4, 3, 4, 3);
            Id.Name = "Id";
            Id.Size = new Size(199, 26);
            Id.TabIndex = 9;
            Id.Tag = "Ejemplo: 12345";
            Id.TextChanged += Id_TextChanged;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Font = new Font("Microsoft Sans Serif", 12F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Id", "Cliente", "Tipo de Fatura", "Empleado" });
            comboBox1.Location = new Point(7, 37);
            comboBox1.Margin = new Padding(4, 3, 4, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(158, 28);
            comboBox1.TabIndex = 11;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(Id);
            groupBox1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(872, 78);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(379, 84);
            groupBox1.TabIndex = 12;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtrar";
            // 
            // button1
            // 
            button1.Location = new Point(187, 599);
            button1.Margin = new Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new Size(128, 45);
            button1.TabIndex = 13;
            button1.Text = "Anterior";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(792, 599);
            button2.Margin = new Padding(4, 3, 4, 3);
            button2.Name = "button2";
            button2.Size = new Size(128, 45);
            button2.TabIndex = 14;
            button2.Text = "Siguiente";
            button2.UseVisualStyleBackColor = true;
            // 
            // Paginacion
            // 
            Paginacion.AutoSize = true;
            Paginacion.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Paginacion.ForeColor = Color.White;
            Paginacion.Location = new Point(474, 608);
            Paginacion.Margin = new Padding(4, 0, 4, 0);
            Paginacion.Name = "Paginacion";
            Paginacion.Size = new Size(132, 20);
            Paginacion.TabIndex = 15;
            Paginacion.Text = "Total de facturas:";
            // 
            // VentanaFacturas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1265, 763);
            Controls.Add(Paginacion);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(groupBox1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(Fecha2);
            Controls.Add(Fecha1);
            Controls.Add(CantidadFactura);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(ListaDeFacturas);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "VentanaFacturas";
            Text = "VentanaFacturas";
            Load += VentanaFacturas_Load;
            ((System.ComponentModel.ISupportInitialize)ListaDeFacturas).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ListaDeFacturas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label CantidadFactura;
        private System.Windows.Forms.DateTimePicker Fecha1;
        private System.Windows.Forms.DateTimePicker Fecha2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label Paginacion;
    }
}