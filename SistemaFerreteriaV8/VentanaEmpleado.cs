using MongoDB.Bson;
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
    public partial class VentanaEmpleado : Form
    {
        Empleado EmpleadoActivo { set; get; }
        private readonly TextBox txtSueldo = new TextBox();
        private readonly Label lblSueldo = new Label();
        public VentanaEmpleado()
        {
            InitializeComponent();
            SistemaFerreteriaV8.Clases.ThemeManager.ApplyToForm(this);
            InicializarControlSueldo();
            button1.Click += BuscarTodo_Click;
            ReestructurarInterfaz();
            Resize += (_, __) => ReestructurarInterfaz();
        }

        private void InicializarControlSueldo()
        {
            lblSueldo.AutoSize = true;
            lblSueldo.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            lblSueldo.ForeColor = Color.White;
            lblSueldo.Location = new Point(12, 297);
            lblSueldo.Name = "lblSueldo";
            lblSueldo.Size = new Size(123, 20);
            lblSueldo.Text = "Sueldo mensual:";

            txtSueldo.Enabled = false;
            txtSueldo.Location = new Point(142, 299);
            txtSueldo.Name = "txtSueldo";
            txtSueldo.Size = new Size(186, 20);

            groupBox1.Controls.Add(lblSueldo);
            groupBox1.Controls.Add(txtSueldo);
        }

        private void ReestructurarInterfaz()
        {
            int margen = 12;
            int espacio = 12;
            int formularioH = 440;

            groupBox1.Location = new Point(margen, margen);
            groupBox1.Size = new Size(430, formularioH);

            groupBox3.Location = new Point(groupBox1.Right + espacio, margen);
            groupBox3.Size = new Size(Math.Max(420, ClientSize.Width - groupBox1.Width - (margen * 2) - espacio), formularioH);

            groupBox2.Location = new Point(margen, groupBox1.Bottom + espacio);
            groupBox2.Size = new Size(ClientSize.Width - (margen * 2), Math.Max(190, ClientSize.Height - groupBox2.Location.Y - margen));

            ListaDeCompras.Location = new Point(12, 24);
            ListaDeCompras.Size = new Size(groupBox3.Width - 24, groupBox3.Height - 36);
            ListaDeEmpleado.Location = new Point(12, 24);
            ListaDeEmpleado.Size = new Size(groupBox2.Width - 24, groupBox2.Height - 36);

            int xLabel = 20;
            int wLabel = 120;
            int xInput = 150;
            int wInput = 250;
            int y = 32;
            int h = 30;
            int gap = 38;
            label4.Text = "Dirección:";
            label6.Text = "Código:";
            button1.Text = "Buscar ID";

            ConfigurarCampo(label5, Id, xLabel, wLabel, xInput, wInput, y, h);
            button1.Location = new Point(xInput + wInput - 96, y);
            button1.Size = new Size(96, h);
            y += gap;
            ConfigurarCampo(label1, Nombre, xLabel, wLabel, xInput, wInput, y, h);
            y += gap;
            ConfigurarCampo(label2, Cedula, xLabel, wLabel, xInput, wInput, y, h);
            y += gap;
            ConfigurarCampo(label4, Direccion, xLabel, wLabel, xInput, wInput, y, h);
            y += gap;
            ConfigurarCampo(label3, Telefono, xLabel, wLabel, xInput, wInput, y, h);
            y += gap;
            ConfigurarCampo(label7, Puesto, xLabel, wLabel, xInput, wInput, y, h);
            y += gap;
            ConfigurarCampo(label6, Codigo, xLabel, wLabel, xInput, wInput, y, h);
            y += gap;

            lblSueldo.AutoSize = false;
            lblSueldo.TextAlign = ContentAlignment.MiddleRight;
            lblSueldo.Location = new Point(xLabel, y);
            lblSueldo.Size = new Size(wLabel, h);
            txtSueldo.Location = new Point(xInput, y + 4);
            txtSueldo.Size = new Size(wInput, 24);

            int yBotones = groupBox1.Height - 96;
            int wBoton = 124;
            int hBoton = 36;
            int sepBoton = 10;
            Nuevo.Location = new Point(20, yBotones);
            Editar.Location = new Point(20 + wBoton + sepBoton, yBotones);
            Guardar.Location = new Point(20 + ((wBoton + sepBoton) * 2), yBotones);
            BuscarTodo.Location = new Point(20, yBotones + hBoton + 8);
            button3.Location = new Point(20 + wBoton + sepBoton, yBotones + hBoton + 8);
            Cancelar.Location = new Point(20 + ((wBoton + sepBoton) * 2), yBotones + hBoton + 8);

            foreach (var btn in new[] { Nuevo, Editar, Guardar, BuscarTodo, button3, Cancelar, button1 })
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
            }
        }

        private static void ConfigurarCampo(Label label, Control input, int xLabel, int wLabel, int xInput, int wInput, int y, int h)
        {
            label.AutoSize = false;
            label.TextAlign = ContentAlignment.MiddleRight;
            label.Location = new Point(xLabel, y);
            label.Size = new Size(wLabel, h);

            input.Location = new Point(xInput, y + 4);
            input.Size = new Size(wInput, 24);
        }

        private void Nuevo_Click(object sender, EventArgs e)
        {

            Id.Enabled = Nombre.Enabled = Cedula.Enabled = Direccion.Enabled = Codigo.Enabled = Puesto.Enabled = Telefono.Enabled = txtSueldo.Enabled = true;
            Empleado nuevoCliente = new Empleado();
            Id.Text = nuevoCliente.Id.ToString();
        }

        private void Editar_Click(object sender, EventArgs e)
        {


            Id.Enabled = Nombre.Enabled = Cedula.Enabled = Direccion.Enabled = Codigo.Enabled= Puesto.Enabled = Telefono.Enabled = txtSueldo.Enabled = true;
        }

        private async void Guardar_Click(object sender, EventArgs e)
        {

            Empleado nuevoEmpleado = new Empleado();

            nuevoEmpleado.Id = ObjectId.Parse(Id.Text);
            nuevoEmpleado.Nombre = Nombre.Text;
            nuevoEmpleado.Direccion = Direccion.Text;
            nuevoEmpleado.Cedula = Cedula.Text;
            nuevoEmpleado.Telefono = Telefono.Text;
            nuevoEmpleado.Puesto = Puesto.Text;
            nuevoEmpleado.Contrasena =Codigo.Text;
            nuevoEmpleado.SueldoMensual = double.TryParse(txtSueldo.Text, out var sueldo) ? sueldo : 0;

            

            if (await Empleado.BuscarAsync(Id.Text) == null)
            {
                MessageBox.Show("El usuario se ha creado corrextamente!!!");
                nuevoEmpleado.CrearAsync();
            }
            else
            {
                MessageBox.Show("El usuario se ha Actualizado corrextamente!!!");
                await nuevoEmpleado.EditarAsync();
            }

            EmpleadoActivo = nuevoEmpleado;
        }

        private  async void BuscarTodo_Click(object sender, EventArgs e)
        {

            if (!String.IsNullOrWhiteSpace(Id.Text))
            {
                Empleado nuevoCliente = await Empleado.BuscarAsync(Id.Text);

                Id.Text = nuevoCliente.Id.ToString();
                Nombre.Text = nuevoCliente.Nombre;
                Direccion.Text = nuevoCliente.Direccion;
                Cedula.Text = nuevoCliente.Cedula;
                Telefono.Text = nuevoCliente.Telefono;
                Puesto.Text = nuevoCliente.Puesto;
                Codigo.Text = nuevoCliente.Contrasena;
                txtSueldo.Text = nuevoCliente.SueldoMensual.ToString("0.##");

                EmpleadoActivo = nuevoCliente;
            }

            Id.Enabled = Nombre.Enabled = Cedula.Enabled = Direccion.Enabled = Puesto.Enabled = Codigo.Enabled = Telefono.Enabled = txtSueldo.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Estas seguro que quieres eliminar el empleado llamado: " + EmpleadoActivo.Nombre, "Aviso!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                EmpleadoActivo.EliminarAsync();
                Id.Text = Nombre.Text = Cedula.Text = Direccion.Text = Puesto.Text = Codigo.Text = Telefono.Text = txtSueldo.Text = " ";
            }
        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            Id.Text = Nombre.Text = Cedula.Text = Direccion.Text = Puesto.Text = Codigo.Text = Telefono.Text = txtSueldo.Text = " ";

        }

        private async void VentanaEmpleado_Load(object sender, EventArgs e)
        {

            ListaDeEmpleado.DefaultCellStyle.ForeColor = Color.Black;
            ListaDeEmpleado.Columns[6].HeaderText = "Sueldo";
            List<Empleado> clientes =  await Empleado.ListarAsync();

            foreach (Empleado cliente in clientes)
            {
                ListaDeEmpleado.Rows.Add(cliente.Id, cliente.Nombre, cliente.Cedula, cliente.Direccion, cliente.Correo, cliente.Telefono, cliente.SueldoMensual.ToString("C2"));
            }
        }

        private async void ListaDeEmpleado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Validar índice y contenido de la celda seleccionada
                if (e.RowIndex < 0 || ListaDeEmpleado[0, e.RowIndex]?.Value == null ||
                    string.IsNullOrWhiteSpace(ListaDeEmpleado[0, e.RowIndex].Value.ToString()))
                {
                    MessageBox.Show("Seleccione un empleado válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el ID del empleado seleccionado
                string id = ListaDeEmpleado[0, e.RowIndex].Value.ToString();

                // Buscar empleado de manera asincrónica
                Empleado nuevoEmpleado = await  Empleado.BuscarAsync(id);

                if (nuevoEmpleado == null)
                {
                    MessageBox.Show("No se encontró el empleado seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Mostrar la información del empleado
                Id.Text = nuevoEmpleado.Id.ToString();
                Nombre.Text = nuevoEmpleado.Nombre;
                Direccion.Text = nuevoEmpleado.Direccion;
                Cedula.Text = nuevoEmpleado.Cedula;
                Telefono.Text = nuevoEmpleado.Telefono;
                Puesto.Text = nuevoEmpleado.Puesto;
                txtSueldo.Text = nuevoEmpleado.SueldoMensual.ToString("0.##");

                // Deshabilitar campos de entrada
                Id.Enabled = Nombre.Enabled = Cedula.Enabled = Direccion.Enabled = Puesto.Enabled = Telefono.Enabled = txtSueldo.Enabled = false;

                // Actualizar el empleado activo
                EmpleadoActivo = nuevoEmpleado;

                // Obtener las facturas asociadas al empleado
                List<Factura> facturas =  await Factura.ListarFacturasAsync("IdEmpleado", nuevoEmpleado.Id.ToString());

                // Mostrar facturas en la lista
                ListaDeCompras.Rows.Clear();
                foreach (var item in facturas)
                {
                    ListaDeCompras.Rows.Add(item.Id, item.NombreCliente, item.Fecha.ToString("dd/MM/yyyy"), item.Total.ToString("C2"));
                }
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
