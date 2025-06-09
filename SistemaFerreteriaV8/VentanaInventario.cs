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
    public partial class VentanaInventario : Form
    {
        int paginaActual = 1;
        static int ProductoXPagina = 20;
        public VentanaInventario()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void VentanaInventario_Load(object sender, EventArgs e)
        {
            BoxFiltrar.ForeColor = Color.White;
            ListaDeProductos.RowsDefaultCellStyle.ForeColor = Color.Black;
            Clave.SelectedIndex = 1;
            Listar("c");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }
        private void Listar(string suma = null)
        {
            var (lista, cantidad) = Productos.ListarPorPagina(paginaActual, ProductoXPagina);

            if (suma == "Mas")
            {
                paginaActual++;
            }
            else if (suma == "Menos")
            {
                paginaActual--;
            }

            long totalPaginas = (cantidad / ProductoXPagina) <= 1 ? 1 : (cantidad / ProductoXPagina);

            if (paginaActual >= 1 && paginaActual <= totalPaginas)
            {
                ListaDeProductos.Rows.Clear();
                foreach (var item in lista)
                {
                    ListaDeProductos.Rows.Add(item.Id, item.Nombre, item.Marca, item.Categoria, item.Costo, item.Cantidad, item.Precio[0], item.Vendido);
                }
                TotalProductos.Text = cantidad.ToString();
                TextPagina.Text = $"{paginaActual} de {totalPaginas}";
                label16.Text = Productos.CalcularTotalProductosVendidos().ToString("c2").Substring(1);
                InversionTotal.Text = Productos.CalcularInversion().ToString("c2");
                GananciaActual.Text = Productos.CalcularGananciasActuales().ToString("c2");
                GananciaEsperada.Text = Productos.CalcularGananciasEsperadas().ToString("c2");
            }
            else
            {
                if (suma == "Menos") paginaActual++;
                else if (suma == "Mas") paginaActual--;
                TextPagina.Text = $"{paginaActual} de 1";
            }
        }
        private void Listar2()
        {
            ListaDeProductos.Rows.Clear();
            var (lista, cantidad) = Productos.ListarPorPagina(paginaActual, ProductoXPagina, Clave.Text, TextoABuscar.Text);

            if (0 < paginaActual && paginaActual < (cantidad / ProductoXPagina))
            { 
                foreach (var item in lista)
                {
                    ListaDeProductos.Rows.Add(item.Id, item.Nombre);
                }
                TotalProductos.Text = cantidad.ToString();
                TextPagina.Text = paginaActual + " de " + (cantidad / ProductoXPagina).ToString();
                label16.Text = Productos.CalcularTotalProductosVendidos().ToString("c2").Substring(1);
                InversionTotal.Text = Productos.CalcularInversion().ToString("c2");
                GananciaActual.Text = Productos.CalcularGananciasActuales().ToString("c2");
                GananciaEsperada.Text = Productos.CalcularGananciasEsperadas().ToString("c2");
            }
         
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

            Listar("Mas");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
       
            Listar("Menos");
        }

        private void TextoABuscar_TextChanged(object sender, EventArgs e)
        {
            Listar2();
        }

        private void Clave_SelectedIndexChanged(object sender, EventArgs e)
        {
            Listar2();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            ListaDeProductos.Sort(ListaDeProductos.Columns[OrdenarPor.SelectedIndex], ListSortDirection.Ascending);
              

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Productos.ExportarProductosAExcel();
        }
    }
}
