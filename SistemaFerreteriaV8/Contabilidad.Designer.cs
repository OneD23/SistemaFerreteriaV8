namespace SistemaFerreteriaV8
{
    partial class Contabilidad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Contabilidad));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.Inventario = new System.Windows.Forms.Button();
            this.Estadistica = new System.Windows.Forms.Button();
            this.Centro = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.Inventario);
            this.panel1.Controls.Add(this.Estadistica);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1084, 69);
            this.panel1.TabIndex = 0;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Red;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(848, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(142, 42);
            this.button4.TabIndex = 3;
            this.button4.Text = "Facturas";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Inventario
            // 
            this.Inventario.BackColor = System.Drawing.Color.Red;
            this.Inventario.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Inventario.ForeColor = System.Drawing.Color.White;
            this.Inventario.Location = new System.Drawing.Point(435, 12);
            this.Inventario.Name = "Inventario";
            this.Inventario.Size = new System.Drawing.Size(142, 42);
            this.Inventario.TabIndex = 1;
            this.Inventario.Text = "Inventario";
            this.Inventario.UseVisualStyleBackColor = false;
            this.Inventario.Click += new System.EventHandler(this.Inventario_Click);
            // 
            // Estadistica
            // 
            this.Estadistica.BackColor = System.Drawing.Color.Red;
            this.Estadistica.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Estadistica.ForeColor = System.Drawing.Color.White;
            this.Estadistica.Location = new System.Drawing.Point(23, 12);
            this.Estadistica.Name = "Estadistica";
            this.Estadistica.Size = new System.Drawing.Size(142, 42);
            this.Estadistica.TabIndex = 0;
            this.Estadistica.Text = "Estadistica";
            this.Estadistica.UseVisualStyleBackColor = false;
            this.Estadistica.Click += new System.EventHandler(this.Estadistica_Click);
            // 
            // Centro
            // 
            this.Centro.BackColor = System.Drawing.Color.Black;
            this.Centro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Centro.Location = new System.Drawing.Point(0, 69);
            this.Centro.Name = "Centro";
            this.Centro.Size = new System.Drawing.Size(1084, 592);
            this.Centro.TabIndex = 1;
            this.Centro.Paint += new System.Windows.Forms.PaintEventHandler(this.Centro_Paint);
            // 
            // Contabilidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(44)))));
            this.ClientSize = new System.Drawing.Size(1084, 661);
            this.Controls.Add(this.Centro);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Contabilidad";
            this.Text = "ALFA FERRETERIA";
            this.Load += new System.EventHandler(this.Contabilidad_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel Centro;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button Inventario;
        private System.Windows.Forms.Button Estadistica;
    }
}