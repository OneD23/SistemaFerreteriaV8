using Microsoft.VisualBasic;
using SistemaFerreteriaV8.Clases;
using System;
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
                ListaPrecio.Rows.Add(
                    ProductoSeleccionado.Precio[0],
                    ProductoSeleccionado.Precio[1],
                    ProductoSeleccionado.Precio[2],
                    ProductoSeleccionado.Precio[3]
                );
            }
        }

        // Mejor práctica: Siempre async para autenticación
        private async void ListaPrecio_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Si se selecciona la columna especial (administrador)
            if (e.ColumnIndex == 3)
            {
                var clave = Interaction.InputBox("Necesita la clave de un administrador para continuar");
                var empleado = await Empleado.BuscarPorClaveAsync("contrasena", clave);

                if (empleado != null && empleado.Puesto == "Administrador")
                {
                    CambiarPrecioSeleccionado(e.RowIndex, e.ColumnIndex);
                }
                else
                {
                    MessageBox.Show("Usuario inválido");
                }
            }
            else
            {
                CambiarPrecioSeleccionado(e.RowIndex, e.ColumnIndex);
            }
        }

        // Método para cambiar el precio de manera segura
        private void CambiarPrecioSeleccionado(int rowIndex, int columnIndex)
        {
            var frm = Application.OpenForms.OfType<VentanaVentas>().FirstOrDefault();
            if (frm != null)
            {
                string valorCelda = frm.obtenerValorDeCelda(ListaPrecio.Rows[rowIndex].Cells[columnIndex]);
                frm.CambiarPrecio(valorCelda);
            }
            this.Dispose();
        }

        // Botón aceptar hace lo mismo que el click directo en la celda (respetando seguridad)
        private async void Aceptar_Click(object sender, EventArgs e)
        {
            int col = ListaPrecio.CurrentCell.ColumnIndex;
            int row = ListaPrecio.CurrentCell.RowIndex;
            if (col == 3)
            {
                var clave = Interaction.InputBox("Necesita la clave de un administrador para continuar");
                var empleado = await Empleado.BuscarPorClaveAsync("contrasena", clave);

                if (empleado != null && empleado.Puesto == "Administrador")
                {
                    CambiarPrecioSeleccionado(row, col);
                }
                else
                {
                    MessageBox.Show("Usuario inválido");
                }
            }
            else
            {
                CambiarPrecioSeleccionado(row, col);
            }
        }
    }
}
