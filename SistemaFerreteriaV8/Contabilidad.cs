using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private Button btnExportacionDatos;
        private readonly ContextMenuStrip menuExportacion = new ContextMenuStrip();
        public Contabilidad()
        {
            InitializeComponent();
            SistemaFerreteriaV8.Clases.ThemeManager.ApplyToForm(this);
            InicializarBotonTendencia();
            InicializarBotonExportacionDatos();
            AlinearBotonesTop();
            panel1.Resize += (_, __) => AlinearBotonesTop();
        }
        private void InicializarBotonTendencia()
        {
            btnTendencia = new Button
            {
                Text = "Tendencia / Gastos",
                Size = new Size(180, 42),
                Location = new Point(588, 12),
                BackColor = Color.FromArgb(211, 47, 47),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnTendencia.FlatAppearance.BorderSize = 0;
            btnTendencia.Click += (_, __) => AbrirFormulario(new VentanaTendenciaGastos());
            panel1.Controls.Add(btnTendencia);
        }
        private void InicializarBotonExportacionDatos()
        {
            btnExportacionDatos = new Button
            {
                Text = "Exportación de datos",
                Size = new Size(180, 42),
                Location = new Point(776, 12),
                BackColor = Color.FromArgb(46, 74, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnExportacionDatos.FlatAppearance.BorderSize = 0;
            btnExportacionDatos.Click += (_, __) =>
            {
                menuExportacion.Show(btnExportacionDatos, new Point(0, btnExportacionDatos.Height));
            };

            ConfigurarMenuExportacion();
            panel1.Controls.Add(btnExportacionDatos);
        }

        private void AlinearBotonesTop()
        {
            var botones = new List<Button> { Estadistica, Inventario, btnTendencia, button4, btnExportacionDatos };
            const int separacion = 12;
            int cantidad = botones.Count;
            int anchoDisponible = panel1.Width - ((cantidad + 1) * separacion);
            int anchoBoton = Math.Max(140, anchoDisponible / cantidad);
            int altoBoton = 42;
            int y = 12;
            int x = separacion;

            foreach (var btn in botones)
            {
                btn.Size = new Size(anchoBoton, altoBoton);
                btn.Location = new Point(x, y);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                x += anchoBoton + separacion;
            }

            btnExportacionDatos.BackColor = Color.FromArgb(46, 74, 125);
            btnExportacionDatos.ForeColor = Color.White;
            foreach (var btn in botones.Where(b => b != btnExportacionDatos))
            {
                btn.BackColor = Color.FromArgb(211, 47, 47);
                btn.ForeColor = Color.White;
            }
        }

        private void ConfigurarMenuExportacion()
        {
            menuExportacion.ShowImageMargin = false;
            menuExportacion.Items.Clear();

            menuExportacion.Items.Add(CrearOpcionExportacion("Exportar facturas", ExportarFacturasAsync));
            menuExportacion.Items.Add(CrearOpcionExportacion("Exportar productos", ExportarProductosAsync));
            menuExportacion.Items.Add(CrearOpcionExportacion("Exportar clientes", ExportarClientesAsync));
            menuExportacion.Items.Add(CrearOpcionExportacion("Exportar empleados", ExportarEmpleadosAsync));
        }

        private ToolStripMenuItem CrearOpcionExportacion(string texto, Func<Task> accion)
        {
            var item = new ToolStripMenuItem
            {
                Text = texto,
                BackColor = Color.FromArgb(46, 74, 125),
                ForeColor = Color.White
            };

            item.Click += async (_, __) => await accion();
            return item;
        }
        private async Task ExportarFacturasAsync()
        {
            var data = await Factura.ListarTodasAsync(true);
            GuardarCsv("facturas_todas.csv",
                "Id,Fecha,Cliente,Tipo,Total,Eliminada,MotivoEliminacion,EliminadaPor,FechaEliminacion",
                data.Select(f => $"{f.Id},{f.Fecha:yyyy-MM-dd HH:mm:ss},{Esc(f.NombreCliente)},{Esc(f.TipoFactura)},{f.Total:F2},{f.Eliminada},{Esc(f.MotivoEliminacion)},{Esc(f.EliminadaPorNombre)},{f.FechaEliminacion:yyyy-MM-dd HH:mm:ss}"));
        }
        private async Task ExportarProductosAsync()
        {
            var data = await Productos.ListarAsync();
            GuardarCsv("productos_todos.csv",
                "Id,Nombre,Descripcion,Costo,Cantidad,Vendido",
                data.Select(p => $"{Esc(p.Id)},{Esc(p.Nombre)},{Esc(p.Descripcion)},{p.Costo:F2},{p.Cantidad:F2},{p.Vendido:F2}"));
        }
        private async Task ExportarClientesAsync()
        {
            var data = await new Cliente().ListarAsync();
            GuardarCsv("clientes_todos.csv",
                "Id,Nombre,Cedula,Telefono,Correo,LimiteCredito",
                data.Select(c => $"{Esc(c.Id)},{Esc(c.Nombre)},{Esc(c.Cedula)},{Esc(c.Telefono)},{Esc(c.Correo)},{c.LimiteCredito:F2}"));
        }
        private async Task ExportarEmpleadosAsync()
        {
            var data = await Empleado.ListarAsync();
            GuardarCsv("empleados_todos.csv",
                "Id,Nombre,Puesto,Cedula,Telefono,Correo",
                data.Select(e => $"{Esc(e.Id.ToString())},{Esc(e.Nombre)},{Esc(e.Puesto)},{Esc(e.Cedula)},{Esc(e.Telefono)},{Esc(e.Correo)}"));
        }
        private void GuardarCsv(string fileName, string header, IEnumerable<string> lines)
        {
            using var dialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = fileName
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var sb = new StringBuilder();
            sb.AppendLine(header);
            foreach (var line in lines)
                sb.AppendLine(line);
            File.WriteAllText(dialog.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("Exportación completada.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private string Esc(string value)
        {
            value ??= "";
            return "\"" + value.Replace("\"", "\"\"") + "\"";
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
