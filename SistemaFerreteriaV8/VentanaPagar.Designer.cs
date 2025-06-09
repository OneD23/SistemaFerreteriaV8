namespace SistemaFerreteriaV8
{
    partial class VentanaPagar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VentanaPagar));
            this.label1 = new System.Windows.Forms.Label();
            this.subTotal = new System.Windows.Forms.TextBox();
            this.Descuento = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Total = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.MetodoPago = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Efectivo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Pagar = new System.Windows.Forms.Button();
            this.Limpiar = new System.Windows.Forms.Button();
            this.Cancelar = new System.Windows.Forms.Button();
            this.Devuelta = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Imprimir = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TipoFactura = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(92, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sub Total:";
            // 
            // subTotal
            // 
            this.subTotal.Enabled = false;
            this.subTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subTotal.Location = new System.Drawing.Point(190, 164);
            this.subTotal.Name = "subTotal";
            this.subTotal.Size = new System.Drawing.Size(218, 26);
            this.subTotal.TabIndex = 2;
            // 
            // Descuento
            // 
            this.Descuento.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Descuento.Location = new System.Drawing.Point(213, 199);
            this.Descuento.Name = "Descuento";
            this.Descuento.Size = new System.Drawing.Size(195, 26);
            this.Descuento.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(92, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Descuentos:";
            // 
            // Total
            // 
            this.Total.Enabled = false;
            this.Total.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Total.Location = new System.Drawing.Point(154, 235);
            this.Total.Name = "Total";
            this.Total.Size = new System.Drawing.Size(254, 26);
            this.Total.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(92, 237);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "Total:";
            // 
            // MetodoPago
            // 
            this.MetodoPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MetodoPago.FormattingEnabled = true;
            this.MetodoPago.Items.AddRange(new object[] {
            "Efectivo",
            "Pago Contra Entrega",
            "Credito",
            "Tarjeta Debito/Credito",
            "Cheque",
            "Transferencia"});
            this.MetodoPago.Location = new System.Drawing.Point(247, 272);
            this.MetodoPago.Name = "MetodoPago";
            this.MetodoPago.Size = new System.Drawing.Size(161, 28);
            this.MetodoPago.TabIndex = 7;
            this.MetodoPago.SelectedIndexChanged += new System.EventHandler(this.MetodoPago_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(92, 274);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 24);
            this.label4.TabIndex = 8;
            this.label4.Text = "Metodo de pago:";
            // 
            // Efectivo
            // 
            this.Efectivo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Efectivo.Location = new System.Drawing.Point(170, 313);
            this.Efectivo.Name = "Efectivo";
            this.Efectivo.Size = new System.Drawing.Size(238, 26);
            this.Efectivo.TabIndex = 10;
            this.Efectivo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Efectivo_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(92, 315);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 24);
            this.label5.TabIndex = 9;
            this.label5.Text = "Efectivo:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(130, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(262, 37);
            this.label6.TabIndex = 11;
            this.label6.Text = "Ventana de pago";
            // 
            // Pagar
            // 
            this.Pagar.BackColor = System.Drawing.Color.Red;
            this.Pagar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Pagar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pagar.ForeColor = System.Drawing.Color.White;
            this.Pagar.Location = new System.Drawing.Point(33, 447);
            this.Pagar.Name = "Pagar";
            this.Pagar.Size = new System.Drawing.Size(107, 40);
            this.Pagar.TabIndex = 12;
            this.Pagar.Text = "Pagar";
            this.Pagar.UseVisualStyleBackColor = false;
            this.Pagar.Click += new System.EventHandler(this.Pagar_Click);
            // 
            // Limpiar
            // 
            this.Limpiar.BackColor = System.Drawing.Color.Red;
            this.Limpiar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Limpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Limpiar.ForeColor = System.Drawing.Color.White;
            this.Limpiar.Location = new System.Drawing.Point(217, 447);
            this.Limpiar.Name = "Limpiar";
            this.Limpiar.Size = new System.Drawing.Size(107, 40);
            this.Limpiar.TabIndex = 13;
            this.Limpiar.Text = "Limpiar";
            this.Limpiar.UseVisualStyleBackColor = false;
            this.Limpiar.Click += new System.EventHandler(this.Limpiar_Click);
            // 
            // Cancelar
            // 
            this.Cancelar.BackColor = System.Drawing.Color.Red;
            this.Cancelar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Cancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancelar.ForeColor = System.Drawing.Color.White;
            this.Cancelar.Location = new System.Drawing.Point(400, 447);
            this.Cancelar.Name = "Cancelar";
            this.Cancelar.Size = new System.Drawing.Size(107, 40);
            this.Cancelar.TabIndex = 14;
            this.Cancelar.Text = "Cancelar";
            this.Cancelar.UseVisualStyleBackColor = false;
            this.Cancelar.Click += new System.EventHandler(this.Cancelar_Click);
            // 
            // Devuelta
            // 
            this.Devuelta.Enabled = false;
            this.Devuelta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Devuelta.Location = new System.Drawing.Point(190, 351);
            this.Devuelta.Name = "Devuelta";
            this.Devuelta.Size = new System.Drawing.Size(218, 26);
            this.Devuelta.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(92, 353);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 24);
            this.label7.TabIndex = 15;
            this.label7.Text = "Devuelta:";
            // 
            // Imprimir
            // 
            this.Imprimir.AutoSize = true;
            this.Imprimir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Imprimir.ForeColor = System.Drawing.Color.White;
            this.Imprimir.Location = new System.Drawing.Point(306, 132);
            this.Imprimir.Name = "Imprimir";
            this.Imprimir.Size = new System.Drawing.Size(42, 24);
            this.Imprimir.TabIndex = 21;
            this.Imprimir.Text = "Si";
            this.Imprimir.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(92, 132);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(209, 24);
            this.label9.TabIndex = 20;
            this.label9.Text = "Desea Imprimir Factura:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(92, 103);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 24);
            this.label8.TabIndex = 23;
            this.label8.Text = "Tipo factura:";
            // 
            // TipoFactura
            // 
            this.TipoFactura.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TipoFactura.FormattingEnabled = true;
            this.TipoFactura.Items.AddRange(new object[] {
            "Consumo",
            "Estandar",
            "Comprobante Fiscal",
            "Comprobante Gubernamental"});
            this.TipoFactura.Location = new System.Drawing.Point(211, 101);
            this.TipoFactura.Name = "TipoFactura";
            this.TipoFactura.Size = new System.Drawing.Size(197, 28);
            this.TipoFactura.TabIndex = 22;
            // 
            // VentanaPagar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(536, 509);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.TipoFactura);
            this.Controls.Add(this.Imprimir);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Devuelta);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Cancelar);
            this.Controls.Add(this.Limpiar);
            this.Controls.Add(this.Pagar);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Efectivo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.MetodoPago);
            this.Controls.Add(this.Total);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Descuento);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.subTotal);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VentanaPagar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ventana de pago";
            this.Load += new System.EventHandler(this.VentanaPagar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox subTotal;
        private System.Windows.Forms.TextBox Descuento;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Total;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox MetodoPago;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Efectivo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button Pagar;
        private System.Windows.Forms.Button Limpiar;
        private System.Windows.Forms.Button Cancelar;
        private System.Windows.Forms.TextBox Devuelta;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox Imprimir;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox TipoFactura;
    }
}