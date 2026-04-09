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
        private Button btnTendencia;
        public Contabilidad()
        {
            InitializeComponent();
            SistemaFerreteriaV8.Clases.ThemeManager.ApplyToForm(this);
            InicializarBotonTendencia();
        }
        private void InicializarBotonTendencia()
        {
            btnTendencia = new Button
            {
                Text = "Tendencia / Gastos",
                Size = new Size(180, 42),
                Location = new Point(620, 12),
                BackColor = Color.FromArgb(255, 137, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnTendencia.FlatAppearance.BorderSize = 0;
            btnTendencia.Click += (_, __) => AbrirFormulario(new VentanaTendenciaGastos());
            panel1.Controls.Add(btnTendencia);
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
            AbrirFormulario(new VentanaTendenciaGastos());
        }
        private void Centro_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
