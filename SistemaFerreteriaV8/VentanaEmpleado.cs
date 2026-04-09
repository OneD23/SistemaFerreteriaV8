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
