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
    public partial class Contabilidad : Form
    {
        public Empleado empleado { get; set; }
        Form formActivado = null;
        public List<Productos> listaProductos {  get; set; }
        public Contabilidad()
        {
            InitializeComponent();
        }
        public void AbrirFormulario(Form hijo)
        {
            if (formActivado != null)
            {
                formActivado.Close();
            }

            formActivado = hijo;

            hijo.TopLevel = false;
            hijo.Dock = DockStyle.Fill;
            hijo.BringToFront();
            hijo.Show();

            Centro.Controls.Add(hijo);
            Centro.Tag = hijo;
        }
        private void Estadistica_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new VentanaEstadisticas() );
        }
        private void Inventario_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new VentanaInventario() );
        }
        private void button4_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new VentanaFacturas());
        }
        private void Contabilidad_Load(object sender, EventArgs e)
        {
           // AbrirFormulario(new VentanaEstadisticas() { listaProductos = listaProductos });
        }
        private void Centro_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
