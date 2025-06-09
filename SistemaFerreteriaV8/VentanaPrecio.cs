using Microsoft.VisualBasic;
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
    public partial class VentanaPrecio : Form
    {
        public DataGridView dataGridView { set; get; }
        public Productos ProductoSeleccionado { get; set; }
        public VentanaPrecio()
        {
            InitializeComponent();
        }

        private void VentanaPrecio_Load(object sender, EventArgs e)
        {
            if (ProductoSeleccionado != null)
            {
                NombreProducto.Text = ProductoSeleccionado.Nombre;
                ListaPrecio.Rows.Add(ProductoSeleccionado.Precio[0], ProductoSeleccionado.Precio[1], ProductoSeleccionado.Precio[2], ProductoSeleccionado.Precio[3]);
            }

        }

        private void ListaPrecio_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                Empleado empleado = new Empleado().BuscarPorClave("contrasena", Interaction.InputBox("Necesita la clave de un administrador para continuar"));
                if (empleado != null && empleado.Puesto == "Administrador")
                {
                    VentanaVentas frm = (VentanaVentas)Application.OpenForms["VentanaVentas"];
                    if (Application.OpenForms.OfType<Form1>().Any())
                    {
                        frm.CambiarPrecio(frm.obtenerValorDeCelda(ListaPrecio.Rows[e.RowIndex].Cells[e.ColumnIndex]));
                    }
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Usuario invalido");
                }
            }
            else
            {
                VentanaVentas frm = (VentanaVentas)Application.OpenForms["VentanaVentas"];
                if (Application.OpenForms.OfType<Form1>().Any())
                {
                    frm.CambiarPrecio(frm.obtenerValorDeCelda(ListaPrecio.Rows[e.RowIndex].Cells[e.ColumnIndex]));
                }
                this.Dispose();
            }
          
        }

        private void ListaPrecio_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void Aceptar_Click(object sender, EventArgs e)
        {
            if (ListaPrecio.CurrentCell.ColumnIndex == 3)
            {
                Empleado empleado = new Empleado().BuscarPorClave("contrasena",Interaction.InputBox("Necesita la clave de un administrador para continuar"));
                if (empleado != null && empleado.Puesto == "Administrador")
                {
                    VentanaVentas frm = (VentanaVentas)Application.OpenForms["VentanaVentas"];
                    if (Application.OpenForms.OfType<Form1>().Any())
                    {
                        frm.CambiarPrecio(frm.obtenerValorDeCelda(ListaPrecio.Rows[ListaPrecio.CurrentCell.RowIndex].Cells[ListaPrecio.CurrentCell.ColumnIndex]));
                    }
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Usuario invalido");
                }
            }
            else
            {
                VentanaVentas frm = (VentanaVentas)Application.OpenForms["VentanaVentas"];
                if (Application.OpenForms.OfType<Form1>().Any())
                {
                    frm.CambiarPrecio(frm.obtenerValorDeCelda(ListaPrecio.Rows[ListaPrecio.CurrentCell.RowIndex].Cells[ListaPrecio.CurrentCell.ColumnIndex]));
                }
                this.Dispose();
            }
        }

    }
}
