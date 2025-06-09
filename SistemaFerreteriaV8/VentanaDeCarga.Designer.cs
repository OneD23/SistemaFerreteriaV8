namespace SistemaFerreteriaV8
{
    partial class VentanaDeCarga
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
            this.Barra = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Barra
            // 
            this.Barra.Location = new System.Drawing.Point(23, 153);
            this.Barra.Name = "Barra";
            this.Barra.Size = new System.Drawing.Size(323, 23);
            this.Barra.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.Barra.TabIndex = 0;
            this.Barra.UseWaitCursor = true;
            this.Barra.Value = 50;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(343, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "Actualizando Productos";
            // 
            // VentanaDeCarga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 256);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Barra);
            this.Name = "VentanaDeCarga";
            this.Text = "VentanaDeCarga";
            this.Load += new System.EventHandler(this.VentanaDeCarga_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar Barra;
        private System.Windows.Forms.Label label1;
    }
}