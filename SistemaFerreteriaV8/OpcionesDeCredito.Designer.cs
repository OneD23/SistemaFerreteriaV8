namespace SistemaFerreteriaV8
{
    public partial class OpcionesDeCredito
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpcionesDeCredito));
            label1 = new Label();
            ListaCreditos = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Paga = new DataGridViewTextBoxColumn();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            ID = new Label();
            Nombre = new Label();
            LimiteCredito = new Label();
            CreditoUtilizado = new Label();
            label6 = new Label();
            CreditoDisponible = new Label();
            label8 = new Label();
            label5 = new Label();
            Editar = new Button();
            PagarTotal = new Button();
            ImprimirTotal = new Button();
            Cancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)ListaCreditos).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(203, 42);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(215, 25);
            label1.TabIndex = 0;
            label1.Text = "Historial de Crédito";
            // 
            // ListaCreditos
            // 
            ListaCreditos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ListaCreditos.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Paga });
            ListaCreditos.Location = new Point(61, 309);
            ListaCreditos.Margin = new Padding(4, 3, 4, 3);
            ListaCreditos.Name = "ListaCreditos";
            ListaCreditos.RowTemplate.DefaultCellStyle.ForeColor = Color.Black;
            ListaCreditos.Size = new Size(575, 173);
            ListaCreditos.TabIndex = 1;
            ListaCreditos.CellContentClick += ListaCreditos_CellContentClick;
            ListaCreditos.CellContentDoubleClick += ListaCreditos_CellContentDoubleClick;
            // 
            // Column1
            // 
            Column1.HeaderText = "Id";
            Column1.Name = "Column1";
            // 
            // Column2
            // 
            Column2.HeaderText = "Fecha";
            Column2.Name = "Column2";
            Column2.Width = 150;
            // 
            // Column3
            // 
            Column3.HeaderText = "Valor";
            Column3.Name = "Column3";
            // 
            // Paga
            // 
            Paga.HeaderText = "Paga";
            Paga.Name = "Paga";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(29, 95);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(30, 20);
            label2.TabIndex = 2;
            label2.Text = "Id:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(29, 133);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(76, 20);
            label3.TabIndex = 3;
            label3.Text = "Nombre:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(29, 171);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(150, 20);
            label4.TabIndex = 4;
            label4.Text = "Limite de Credito:";
            // 
            // ID
            // 
            ID.AutoSize = true;
            ID.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ID.ForeColor = Color.White;
            ID.Location = new Point(71, 95);
            ID.Margin = new Padding(4, 0, 4, 0);
            ID.Name = "ID";
            ID.Size = new Size(39, 20);
            ID.TabIndex = 5;
            ID.Text = "007";
            // 
            // Nombre
            // 
            Nombre.AutoSize = true;
            Nombre.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Nombre.ForeColor = Color.White;
            Nombre.Location = new Point(125, 133);
            Nombre.Margin = new Padding(4, 0, 4, 0);
            Nombre.Name = "Nombre";
            Nombre.Size = new Size(155, 20);
            Nombre.TabIndex = 6;
            Nombre.Text = "Bond James Bond";
            // 
            // LimiteCredito
            // 
            LimiteCredito.AutoSize = true;
            LimiteCredito.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LimiteCredito.ForeColor = Color.White;
            LimiteCredito.Location = new Point(211, 171);
            LimiteCredito.Margin = new Padding(4, 0, 4, 0);
            LimiteCredito.Name = "LimiteCredito";
            LimiteCredito.Size = new Size(49, 20);
            LimiteCredito.TabIndex = 7;
            LimiteCredito.Text = "2233";
            // 
            // CreditoUtilizado
            // 
            CreditoUtilizado.AutoSize = true;
            CreditoUtilizado.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CreditoUtilizado.ForeColor = Color.White;
            CreditoUtilizado.Location = new Point(211, 207);
            CreditoUtilizado.Margin = new Padding(4, 0, 4, 0);
            CreditoUtilizado.Name = "CreditoUtilizado";
            CreditoUtilizado.Size = new Size(49, 20);
            CreditoUtilizado.TabIndex = 9;
            CreditoUtilizado.Text = "2233";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.White;
            label6.Location = new Point(29, 207);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(147, 20);
            label6.TabIndex = 8;
            label6.Text = "Credito Utilizado:";
            // 
            // CreditoDisponible
            // 
            CreditoDisponible.AutoSize = true;
            CreditoDisponible.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CreditoDisponible.ForeColor = Color.White;
            CreditoDisponible.Location = new Point(215, 241);
            CreditoDisponible.Margin = new Padding(4, 0, 4, 0);
            CreditoDisponible.Name = "CreditoDisponible";
            CreditoDisponible.Size = new Size(49, 20);
            CreditoDisponible.TabIndex = 11;
            CreditoDisponible.Text = "2233";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.White;
            label8.Location = new Point(29, 241);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(161, 20);
            label8.TabIndex = 10;
            label8.Text = "Credito Disponible:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(236, 283);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(158, 20);
            label5.TabIndex = 12;
            label5.Text = "Compras a Crédito";
            // 
            // Editar
            // 
            Editar.BackColor = Color.Red;
            Editar.FlatStyle = FlatStyle.Flat;
            Editar.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Editar.ForeColor = Color.White;
            Editar.Location = new Point(34, 546);
            Editar.Margin = new Padding(4, 3, 4, 3);
            Editar.Name = "Editar";
            Editar.Size = new Size(103, 45);
            Editar.TabIndex = 15;
            Editar.Text = "Abonar";
            Editar.UseVisualStyleBackColor = false;
            Editar.Click += Editar_Click;
            // 
            // PagarTotal
            // 
            PagarTotal.BackColor = Color.Red;
            PagarTotal.FlatStyle = FlatStyle.Flat;
            PagarTotal.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            PagarTotal.ForeColor = Color.White;
            PagarTotal.Location = new Point(164, 546);
            PagarTotal.Margin = new Padding(4, 3, 4, 3);
            PagarTotal.Name = "PagarTotal";
            PagarTotal.Size = new Size(141, 45);
            PagarTotal.TabIndex = 16;
            PagarTotal.Text = "Pagar Total";
            PagarTotal.UseVisualStyleBackColor = false;
            PagarTotal.Click += button1_Click;
            // 
            // ImprimirTotal
            // 
            ImprimirTotal.BackColor = Color.Red;
            ImprimirTotal.FlatStyle = FlatStyle.Flat;
            ImprimirTotal.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ImprimirTotal.ForeColor = Color.White;
            ImprimirTotal.Location = new Point(330, 546);
            ImprimirTotal.Margin = new Padding(4, 3, 4, 3);
            ImprimirTotal.Name = "ImprimirTotal";
            ImprimirTotal.Size = new Size(196, 45);
            ImprimirTotal.TabIndex = 17;
            ImprimirTotal.Text = "Imprimir Historial";
            ImprimirTotal.UseVisualStyleBackColor = false;
            ImprimirTotal.Click += ImprimirTotal_Click;
            // 
            // Cancelar
            // 
            Cancelar.BackColor = Color.Red;
            Cancelar.FlatStyle = FlatStyle.Flat;
            Cancelar.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Cancelar.ForeColor = Color.White;
            Cancelar.Location = new Point(551, 546);
            Cancelar.Margin = new Padding(4, 3, 4, 3);
            Cancelar.Name = "Cancelar";
            Cancelar.Size = new Size(114, 45);
            Cancelar.TabIndex = 18;
            Cancelar.Text = "Cancelar";
            Cancelar.UseVisualStyleBackColor = false;
            Cancelar.Click += Cancelar_Click;
            // 
            // OpcionesDeCredito
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(692, 647);
            Controls.Add(Cancelar);
            Controls.Add(ImprimirTotal);
            Controls.Add(PagarTotal);
            Controls.Add(Editar);
            Controls.Add(label5);
            Controls.Add(CreditoDisponible);
            Controls.Add(label8);
            Controls.Add(CreditoUtilizado);
            Controls.Add(label6);
            Controls.Add(LimiteCredito);
            Controls.Add(Nombre);
            Controls.Add(ID);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(ListaCreditos);
            Controls.Add(label1);
            ForeColor = Color.White;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "OpcionesDeCredito";
            Text = "OpcionesDeCredito";
            Load += OpcionesDeCredito_Load;
            ((System.ComponentModel.ISupportInitialize)ListaCreditos).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView ListaCreditos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label ID;
        private System.Windows.Forms.Label Nombre;
        private System.Windows.Forms.Label LimiteCredito;
        private System.Windows.Forms.Label CreditoUtilizado;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label CreditoDisponible;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button Editar;
        private System.Windows.Forms.Button PagarTotal;
        private System.Windows.Forms.Button ImprimirTotal;
        private System.Windows.Forms.Button Cancelar;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Paga;
    }
}