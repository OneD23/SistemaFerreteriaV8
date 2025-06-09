namespace SistemaFerreteriaV8
{
    partial class VentanaEstadisticas
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SevenDay = new System.Windows.Forms.Button();
            this.Hoy = new System.Windows.Forms.Button();
            this.Personalizado = new System.Windows.Forms.Button();
            this.Fecha1 = new System.Windows.Forms.DateTimePicker();
            this.Fecha2 = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TFacturas = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TVentas = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.TGanancias = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.VentaPorFecha = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ProductosBajos = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.Buscar = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.ProductsMostSales = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VentaPorFecha)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductosBajos)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductsMostSales)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(850, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 25);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ultimos 30 Dias";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(950, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 25);
            this.button2.TabIndex = 1;
            this.button2.Text = "Este mes";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // SevenDay
            // 
            this.SevenDay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SevenDay.ForeColor = System.Drawing.Color.White;
            this.SevenDay.Location = new System.Drawing.Point(750, 23);
            this.SevenDay.Name = "SevenDay";
            this.SevenDay.Size = new System.Drawing.Size(100, 25);
            this.SevenDay.TabIndex = 3;
            this.SevenDay.Text = "Ultimos 7 Dias";
            this.SevenDay.UseVisualStyleBackColor = true;
            this.SevenDay.Click += new System.EventHandler(this.SevenDay_Click);
            // 
            // Hoy
            // 
            this.Hoy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Hoy.ForeColor = System.Drawing.Color.White;
            this.Hoy.Location = new System.Drawing.Point(650, 23);
            this.Hoy.Name = "Hoy";
            this.Hoy.Size = new System.Drawing.Size(100, 25);
            this.Hoy.TabIndex = 2;
            this.Hoy.Text = "Hoy";
            this.Hoy.UseVisualStyleBackColor = true;
            this.Hoy.Click += new System.EventHandler(this.Hoy_Click);
            // 
            // Personalizado
            // 
            this.Personalizado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Personalizado.ForeColor = System.Drawing.Color.White;
            this.Personalizado.Location = new System.Drawing.Point(550, 23);
            this.Personalizado.Name = "Personalizado";
            this.Personalizado.Size = new System.Drawing.Size(100, 25);
            this.Personalizado.TabIndex = 5;
            this.Personalizado.Text = "Personalizado";
            this.Personalizado.UseVisualStyleBackColor = true;
            // 
            // Fecha1
            // 
            this.Fecha1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Fecha1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.Fecha1.Location = new System.Drawing.Point(164, 24);
            this.Fecha1.Name = "Fecha1";
            this.Fecha1.Size = new System.Drawing.Size(118, 26);
            this.Fecha1.TabIndex = 6;
            this.Fecha1.Value = new System.DateTime(2024, 2, 9, 0, 0, 0, 0);
            // 
            // Fecha2
            // 
            this.Fecha2.CalendarMonthBackground = System.Drawing.SystemColors.WindowFrame;
            this.Fecha2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Fecha2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.Fecha2.Location = new System.Drawing.Point(288, 24);
            this.Fecha2.Name = "Fecha2";
            this.Fecha2.Size = new System.Drawing.Size(118, 26);
            this.Fecha2.TabIndex = 7;
            this.Fecha2.Value = new System.DateTime(2024, 2, 9, 23, 59, 0, 0);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Salmon;
            this.panel1.Controls.Add(this.TFacturas);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(61, 75);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(345, 64);
            this.panel1.TabIndex = 8;
            // 
            // TFacturas
            // 
            this.TFacturas.AutoSize = true;
            this.TFacturas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TFacturas.ForeColor = System.Drawing.Color.White;
            this.TFacturas.Location = new System.Drawing.Point(81, 35);
            this.TFacturas.Name = "TFacturas";
            this.TFacturas.Size = new System.Drawing.Size(53, 20);
            this.TFacturas.TabIndex = 2;
            this.TFacturas.Text = "$ 0.00";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(80, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Total de facturas";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SistemaFerreteriaV8.Properties.Resources.cuenta;
            this.pictureBox1.Location = new System.Drawing.Point(12, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(61, 44);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Salmon;
            this.panel2.Controls.Add(this.TVentas);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Location = new System.Drawing.Point(412, 75);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(332, 64);
            this.panel2.TabIndex = 9;
            // 
            // TVentas
            // 
            this.TVentas.AutoSize = true;
            this.TVentas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TVentas.ForeColor = System.Drawing.Color.White;
            this.TVentas.Location = new System.Drawing.Point(88, 35);
            this.TVentas.Name = "TVentas";
            this.TVentas.Size = new System.Drawing.Size(53, 20);
            this.TVentas.TabIndex = 3;
            this.TVentas.Text = "$ 0.00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(85, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Total de ventas";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::SistemaFerreteriaV8.Properties.Resources.zakat;
            this.pictureBox2.Location = new System.Drawing.Point(13, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(66, 51);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Salmon;
            this.panel3.Controls.Add(this.TGanancias);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.pictureBox3);
            this.panel3.Location = new System.Drawing.Point(750, 75);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(300, 64);
            this.panel3.TabIndex = 9;
            // 
            // TGanancias
            // 
            this.TGanancias.AutoSize = true;
            this.TGanancias.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TGanancias.ForeColor = System.Drawing.Color.White;
            this.TGanancias.Location = new System.Drawing.Point(93, 35);
            this.TGanancias.Name = "TGanancias";
            this.TGanancias.Size = new System.Drawing.Size(53, 20);
            this.TGanancias.TabIndex = 4;
            this.TGanancias.Text = "$ 0.00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(91, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Total de ganancias";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::SistemaFerreteriaV8.Properties.Resources.beneficios;
            this.pictureBox3.Location = new System.Drawing.Point(15, 6);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(66, 52);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            // 
            // VentaPorFecha
            // 
            this.VentaPorFecha.BackColor = System.Drawing.Color.Salmon;
            chartArea1.Name = "ChartArea1";
            this.VentaPorFecha.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.VentaPorFecha.Legends.Add(legend1);
            this.VentaPorFecha.Location = new System.Drawing.Point(61, 145);
            this.VentaPorFecha.Name = "VentaPorFecha";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.VentaPorFecha.Series.Add(series1);
            this.VentaPorFecha.Size = new System.Drawing.Size(989, 229);
            this.VentaPorFecha.TabIndex = 10;
            this.VentaPorFecha.Text = "chart1";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Salmon;
            this.panel5.Controls.Add(this.ProductosBajos);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Location = new System.Drawing.Point(61, 380);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(485, 243);
            this.panel5.TabIndex = 13;
            // 
            // ProductosBajos
            // 
            this.ProductosBajos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProductosBajos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column4});
            this.ProductosBajos.Location = new System.Drawing.Point(7, 38);
            this.ProductosBajos.Name = "ProductosBajos";
            this.ProductosBajos.Size = new System.Drawing.Size(471, 191);
            this.ProductosBajos.TabIndex = 4;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Nombre";
            this.Column3.Name = "Column3";
            this.Column3.Width = 325;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Cantidad";
            this.Column4.Name = "Column4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(127, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(231, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Productos en baja Cantidad";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(61, 23);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 27);
            this.button3.TabIndex = 14;
            this.button3.Text = "Reporte";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Buscar
            // 
            this.Buscar.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Buscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Buscar.Location = new System.Drawing.Point(425, 23);
            this.Buscar.Name = "Buscar";
            this.Buscar.Size = new System.Drawing.Size(85, 27);
            this.Buscar.TabIndex = 15;
            this.Buscar.Text = "Buscar";
            this.Buscar.UseVisualStyleBackColor = false;
            this.Buscar.Click += new System.EventHandler(this.Buscar_Click);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Salmon;
            this.panel6.Controls.Add(this.ProductsMostSales);
            this.panel6.Controls.Add(this.label6);
            this.panel6.Location = new System.Drawing.Point(552, 380);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(498, 243);
            this.panel6.TabIndex = 10;
            // 
            // ProductsMostSales
            // 
            this.ProductsMostSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProductsMostSales.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.ProductsMostSales.Location = new System.Drawing.Point(15, 38);
            this.ProductsMostSales.Name = "ProductsMostSales";
            this.ProductsMostSales.Size = new System.Drawing.Size(468, 191);
            this.ProductsMostSales.TabIndex = 5;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Articulo";
            this.Column1.Name = "Column1";
            this.Column1.Width = 325;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Venta";
            this.Column2.Name = "Column2";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(121, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(256, 20);
            this.label6.TabIndex = 3;
            this.label6.Text = "Los 5 productos Mas Vendidos";
            // 
            // VentanaEstadisticas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.Buscar);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.VentaPorFecha);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Fecha2);
            this.Controls.Add(this.Fecha1);
            this.Controls.Add(this.Personalizado);
            this.Controls.Add(this.SevenDay);
            this.Controls.Add(this.Hoy);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VentanaEstadisticas";
            this.Text = "VentanaEstadisticas";
            this.Load += new System.EventHandler(this.VentanaEstadisticas_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VentaPorFecha)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductosBajos)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductsMostSales)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button SevenDay;
        private System.Windows.Forms.Button Hoy;
        private System.Windows.Forms.Button Personalizado;
        private System.Windows.Forms.DateTimePicker Fecha1;
        private System.Windows.Forms.DateTimePicker Fecha2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.DataVisualization.Charting.Chart VentaPorFecha;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label TFacturas;
        private System.Windows.Forms.Label TVentas;
        private System.Windows.Forms.Label TGanancias;
        private System.Windows.Forms.DataGridView ProductosBajos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button Buscar;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView ProductsMostSales;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}