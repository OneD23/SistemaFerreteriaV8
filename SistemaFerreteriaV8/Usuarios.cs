using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaFerreteriaV8
{
    public partial class Usuarios : Form
    {
        public Usuarios()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 frm = (Form1)Application.OpenForms["Form1"];
            if (Application.OpenForms.OfType<Form1>().Any())
            {
                frm.AbrirFormulario(new VentanaCliente());
            }

            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 frm = (Form1)Application.OpenForms["Form1"];
            if (Application.OpenForms.OfType<Form1>().Any())
            {
                frm.AbrirFormulario(new VentanaEmpleado());
            }

            this.Dispose();
        }
    }
}
