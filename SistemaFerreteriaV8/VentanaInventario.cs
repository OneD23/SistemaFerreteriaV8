using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaFerreteriaV8
{
    public partial class VentanaInventario : Form
    {
        private int paginaActual = 1;
        private const int ProductosPorPagina = 20;

        public VentanaInventario()
        {
            InitializeComponent();
        }

        private async void VentanaInventario_Load(object sender, EventArgs e)
        {
            // Estilos y configuración inicial
            BoxFiltrar.ForeColor = Color.White;
            ListaDeProductos.RowsDefaultCellStyle.ForeColor = Color.Black;
            Clave.SelectedIndex = 1;

            // Listar la primera página
            await ListarAsync();
        }

        /// <summary>
        /// Lista productos con paginación y actualiza indicadores.
        /// </summary>
        /// <param name="direccion">"Mas", "Menos", o null para recarga.</param>
        private async Task ListarAsync(string direccion = null)
        {
            if (direccion == "Mas") paginaActual++;
            else if (direccion == "Menos" && paginaActual > 1) paginaActual--;

            // Llamada async al listado paginado
            var (productos, total) = await Productos.ListarPorPaginaAsync(paginaActual, ProductosPorPagina);

            long totalPaginas = Math.Max(1, (total + ProductosPorPagina - 1) / ProductosPorPagina);
            if (paginaActual < 1) paginaActual = 1;
            if (paginaActual > totalPaginas) paginaActual = (int)totalPaginas;

            // Refrescar UI
            ListaDeProductos.Rows.Clear();
            foreach (var p in productos)
            {
                ListaDeProductos.Rows.Add(
                    p.Id,
                    p.Nombre,
                    p.Marca,
                    p.Categoria,
                    p.Costo.ToString("C2"),
                    p.Cantidad,
                    p.Precio.FirstOrDefault().ToString("C2"),
                    p.Vendido
                );
            }

            TotalProductos.Text = total.ToString();
            TextPagina.Text = $"{paginaActual} de {totalPaginas}";

            // Consultas async de agregados
            label16.Text = (await Productos.CalcularTotalProductosVendidosAsync()).ToString("C2").Substring(1);
            InversionTotal.Text = (await Productos.CalcularInversionAsync()).ToString("C2");
            GananciaActual.Text = (await Productos.CalcularGananciasActualesAsync()).ToString("C2");
            GananciaEsperada.Text = (await Productos.CalcularGananciasEsperadasAsync()).ToString("C2");
        }

        /// <summary>
        /// Lista productos filtrados por clave/valor con paginación.
        /// </summary>
        private async Task ListarFiltradoAsync()
        {
            // Validación básica de páginas
            if (paginaActual < 1) paginaActual = 1;

            // Llamada async con filtro
            var (productos, total) = await Productos.ListarPorPaginaAsync(
                paginaActual, ProductosPorPagina,
                clave: Clave.Text, valor: TextoABuscar.Text
            );

            long totalPaginas = Math.Max(1, (total + ProductosPorPagina - 1) / ProductosPorPagina);

            // Refrescar UI
            ListaDeProductos.Rows.Clear();
            foreach (var p in productos)
            {
                ListaDeProductos.Rows.Add(p.Id, p.Nombre);
            }

            TotalProductos.Text = total.ToString();
            TextPagina.Text = $"{paginaActual} de {totalPaginas}";

            // Estadísticas
            label16.Text = (await Productos.CalcularTotalProductosVendidosAsync()).ToString("C2").Substring(1);
            InversionTotal.Text = (await Productos.CalcularInversionAsync()).ToString("C2");
            GananciaActual.Text = (await Productos.CalcularGananciasActualesAsync()).ToString("C2");
            GananciaEsperada.Text = (await Productos.CalcularGananciasEsperadasAsync()).ToString("C2");
        }

        private async void pictureBox5_Click(object sender, EventArgs e)
            => await ListarAsync("Mas");

        private async void pictureBox6_Click(object sender, EventArgs e)
            => await ListarAsync("Menos");

        private async void TextoABuscar_TextChanged(object sender, EventArgs e)
            => await ListarFiltradoAsync();

        private async void Clave_SelectedIndexChanged(object sender, EventArgs e)
            => await ListarFiltradoAsync();

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ordenar la columna seleccionada
            var col = OrdenarPor.SelectedIndex;
            if (col >= 0 && col < ListaDeProductos.Columns.Count)
                ListaDeProductos.Sort(ListaDeProductos.Columns[col], ListSortDirection.Ascending);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            // Guardar diálogo
            using var dlg = new SaveFileDialog
            {
                Filter = "Archivos de Excel (*.xlsx)|*.xlsx",
                Title = "Exportar Productos a Excel"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                await Productos.ExportarProductosAExcelAsync(dlg.FileName);
                MessageBox.Show($"Productos exportados a {dlg.FileName}", "Exportación completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
