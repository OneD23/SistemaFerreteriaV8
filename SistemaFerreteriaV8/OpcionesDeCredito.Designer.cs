namespace SistemaFerreteriaV8
{
    partial class OpcionesDeCredito
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
            this.label1 = new System.Windows.Forms.Label();
            this.ListaCreditos = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Paga = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ID = new System.Windows.Forms.Label();
            this.Nombre = new System.Windows.Forms.Label();
            this.LimiteCredito = new System.Windows.Forms.Label();
            this.CreditoUtilizado = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CreditoDisponible = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Editar = new System.Windows.Forms.Button();
            this.PagarTotal = new System.Windows.Forms.Button();
            this.ImprimirTotal = new System.Windows.Forms.Button();
            this.Cancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ListaCreditos)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(174, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Historial de Crédito";
            // 
            // ListaCreditos
            // 
            this.ListaCreditos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListaCreditos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Paga});
            this.ListaCreditos.Location = new System.Drawing.Point(52, 268);
            this.ListaCreditos.Name = "ListaCreditos";
            this.ListaCreditos.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.ListaCreditos.Size = new System.Drawing.Size(493, 150);
            this.ListaCreditos.TabIndex = 1;
            this.ListaCreditos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ListaCreditos_CellContentClick);
            this.ListaCreditos.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ListaCreditos_CellContentDoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Id";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Fecha";
            this.Column2.Name = "Column2";
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Valor";
            this.Column3.Name = "Column3";
            // 
            // Paga
            // 
            this.Paga.HeaderText = "Paga";
            this.Paga.Name = "Paga";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(25, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Id:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(25, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Nombre:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(25, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Limite de Credito:";
            // 
            // ID
            // 
            this.ID.AutoSize = true;
            this.ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ID.ForeColor = System.Drawing.Color.White;
            this.ID.Location = new System.Drawing.Point(61, 82);
            this.ID.Name = "ID";
            this.ID.Size = new System.Drawing.Size(39, 20);
            this.ID.TabIndex = 5;
            this.ID.Text = "007";
            // 
            // Nombre
            // 
            this.Nombre.AutoSize = true;
            this.Nombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Nombre.ForeColor = System.Drawing.Color.White;
            this.Nombre.Location = new System.Drawing.Point(107, 115);
            this.Nombre.Name = "Nombre";
            this.Nombre.Size = new System.Drawing.Size(155, 20);
            this.Nombre.TabIndex = 6;
            this.Nombre.Text = "Bond James Bond";
            // 
            // LimiteCredito
            // 
            this.LimiteCredito.AutoSize = true;
            this.LimiteCredito.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LimiteCredito.ForeColor = System.Drawing.Color.White;
            this.LimiteCredito.Location = new System.Drawing.Point(181, 148);
            this.LimiteCredito.Name = "LimiteCredito";
            this.LimiteCredito.Size = new System.Drawing.Size(49, 20);
            this.LimiteCredito.TabIndex = 7;
            this.LimiteCredito.Text = "2233";
            // 
            // CreditoUtilizado
            // 
            this.CreditoUtilizado.AutoSize = true;
            this.CreditoUtilizado.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreditoUtilizado.ForeColor = System.Drawing.Color.White;
            this.CreditoUtilizado.Location = new System.Drawing.Point(181, 179);
            this.CreditoUtilizado.Name = "CreditoUtilizado";
            this.CreditoUtilizado.Size = new System.Drawing.Size(49, 20);
            this.CreditoUtilizado.TabIndex = 9;
            this.CreditoUtilizado.Text = "2233";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(25, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 20);
            this.label6.TabIndex = 8;
            this.label6.Text = "Credito Utilizado:";
            // 
            // CreditoDisponible
            // 
            this.CreditoDisponible.AutoSize = true;
            this.CreditoDisponible.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreditoDisponible.ForeColor = System.Drawing.Color.White;
            this.CreditoDisponible.Location = new System.Drawing.Point(184, 209);
            this.CreditoDisponible.Name = "CreditoDisponible";
            this.CreditoDisponible.Size = new System.Drawing.Size(49, 20);
            this.CreditoDisponible.TabIndex = 11;
            this.CreditoDisponible.Text = "2233";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(25, 209);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(161, 20);
            this.label8.TabIndex = 10;
            this.label8.Text = "Credito Disponible:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(202, 245);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(158, 20);
            this.label5.TabIndex = 12;
            this.label5.Text = "Compras a Crédito";
            // 
            // Editar
            // 
            this.Editar.BackColor = System.Drawing.Color.Red;
            this.Editar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Editar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Editar.ForeColor = System.Drawing.Color.White;
            this.Editar.Location = new System.Drawing.Point(29, 473);
            this.Editar.Name = "Editar";
            this.Editar.Size = new System.Drawing.Size(88, 39);
            this.Editar.TabIndex = 15;
            this.Editar.Text = "Abonar";
            this.Editar.UseVisualStyleBackColor = false;
            this.Editar.Click += new System.EventHandler(this.Editar_Click);
            // 
            // PagarTotal
            // 
            this.PagarTotal.BackColor = System.Drawing.Color.Red;
            this.PagarTotal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PagarTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PagarTotal.ForeColor = System.Drawing.Color.White;
            this.PagarTotal.Location = new System.Drawing.Point(141, 473);
            this.PagarTotal.Name = "PagarTotal";
            this.PagarTotal.Size = new System.Drawing.Size(121, 39);
            this.PagarTotal.TabIndex = 16;
            this.PagarTotal.Text = "Pagar Total";
            this.PagarTotal.UseVisualStyleBackColor = false;
            this.PagarTotal.Click += new System.EventHandler(this.button1_Click);
            // 
            // ImprimirTotal
            // 
            this.ImprimirTotal.BackColor = System.Drawing.Color.Red;
            this.ImprimirTotal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImprimirTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImprimirTotal.ForeColor = System.Drawing.Color.White;
            this.ImprimirTotal.Location = new System.Drawing.Point(283, 473);
            this.ImprimirTotal.Name = "ImprimirTotal";
            this.ImprimirTotal.Size = new System.Drawing.Size(168, 39);
            this.ImprimirTotal.TabIndex = 17;
            this.ImprimirTotal.Text = "Imprimir Historial";
            this.ImprimirTotal.UseVisualStyleBackColor = false;
            this.ImprimirTotal.Click += new System.EventHandler(this.ImprimirTotal_Click);
            // 
            // Cancelar
            // 
            this.Cancelar.BackColor = System.Drawing.Color.Red;
            this.Cancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancelar.ForeColor = System.Drawing.Color.White;
            this.Cancelar.Location = new System.Drawing.Point(472, 473);
            this.Cancelar.Name = "Cancelar";
            this.Cancelar.Size = new System.Drawing.Size(98, 39);
            this.Cancelar.TabIndex = 18;
            this.Cancelar.Text = "Cancelar";
            this.Cancelar.UseVisualStyleBackColor = false;
            this.Cancelar.Click += new System.EventHandler(this.Cancelar_Click);
            // 
            // OpcionesDeCredito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(593, 561);
            this.Controls.Add(this.Cancelar);
            this.Controls.Add(this.ImprimirTotal);
            this.Controls.Add(this.PagarTotal);
            this.Controls.Add(this.Editar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.CreditoDisponible);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.CreditoUtilizado);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.LimiteCredito);
            this.Controls.Add(this.Nombre);
            this.Controls.Add(this.ID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ListaCreditos);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OpcionesDeCredito";
            this.Text = "OpcionesDeCredito";
            this.Load += new System.EventHandler(this.OpcionesDeCredito_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ListaCreditos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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