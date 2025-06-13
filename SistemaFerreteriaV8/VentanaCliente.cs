using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.VisualBasic;
using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8
{
    public partial class VentanaCliente : Form
    {
        public Cliente ClienteActivo { get; set; }
        Cuenta CreditoActivo { get; set; }
        public VentanaCliente()
        {
            InitializeComponent();
        }

        private async void VentanaCliente_Load(object sender, EventArgs e)
        {// Asignar el color de texto de una vez para toda la lista
            ListaDeClientes.DefaultCellStyle.ForeColor = Color.Black;

            // Obtener la lista de clientes de manera eficiente
            List<Cliente> clientes = await new Cliente().ListarAsync();

            // Crear una lista para almacenar las filas a agregar
            List<DataGridViewRow> filas = new List<DataGridViewRow>();

            foreach (Cliente cliente in clientes)
            {
                double totalDeCliente = 0;

                // Crear una fila y agregarla a la lista
                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(ListaDeClientes, cliente.Id, cliente.Nombre, cliente.Cedula, cliente.Direccion, cliente.Correo, cliente.Telefono, totalDeCliente);
                filas.Add(fila);
            }

            // Agregar todas las filas a la lista de clientes de una sola vez
            ListaDeClientes.Rows.AddRange(filas.ToArray());

        }

        private async void Nuevo_Click(object sender, EventArgs e)
        {
            Id.Enabled = Nombre.Enabled = Cedula.Enabled = Direccion.Enabled = Correo.Enabled = Telefono.Enabled = LimiteCredito.Enabled = true;
            Id.Text = Nombre.Text = Cedula.Text = Direccion.Text = Correo.Text = Telefono.Text = LimiteCredito.Text ="";
            Cliente nuevoCliente = new Cliente();
            Id.Text = await nuevoCliente.GenerarNuevoIdAsync();
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            Cliente nuevoCliente = new Cliente();

            nuevoCliente.Id = Id.Text;
            nuevoCliente.Nombre = Nombre.Text;
            nuevoCliente.Direccion = Direccion.Text;
            nuevoCliente.Cedula = Cedula.Text;  
            nuevoCliente.Telefono = Telefono.Text;
            nuevoCliente.Correo = Correo.Text;
            nuevoCliente.LimiteCredito = double.Parse(LimiteCredito.Text);

            if (new Cliente().BuscarAsync(Id.Text) == null)
            {
                nuevoCliente.CrearAsync();
                MessageBox.Show("Cliente creado correctamente!");
            }
            else
            {
                nuevoCliente.EditarAsync();
                MessageBox.Show("Cliente actualizado correctamente!");
            }

            ClienteActivo = nuevoCliente;

            
            Id.Text = Nombre.Text = Cedula.Text = Direccion.Text = Correo.Text = Telefono.Text = LimiteCredito.Text= "";
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (ClienteActivo != null)
            {
                if (DialogResult.Yes == MessageBox.Show($"Esta seguro que desea eliminar a {ClienteActivo.Nombre} de su lista de cuenta","Aviso",MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                {
                    await ClienteActivo.EliminarAsync();
                }
            }           
        }

        private async void BuscarTodo_Click(object sender, EventArgs e)
        {
            string idTemporal = Interaction.InputBox("Ingrese el Id: ", "Buscar por Id");
            if (!String.IsNullOrWhiteSpace(idTemporal))
            {
                Cliente nuevoCliente = await new Cliente().BuscarAsync(idTemporal);

                double creditoActivo = 0;
                foreach (var item in nuevoCliente.CreditosActivo)
                {
                    creditoActivo += item.Total;
                }


                if (nuevoCliente!=null)
                {
                    Id.Text = nuevoCliente.Id;
                    Nombre.Text = nuevoCliente.Nombre;
                    Direccion.Text = nuevoCliente.Direccion;
                    Cedula.Text = nuevoCliente.Cedula;
                    Telefono.Text = nuevoCliente.Telefono;
                    Correo.Text = nuevoCliente.Correo;
                    LimiteCredito.Text = nuevoCliente.LimiteCredito.ToString();
                    CreditoActivo2.Text = creditoActivo.ToString();

                    ClienteActivo = nuevoCliente;
                }
                else
                {
                    MessageBox.Show("No existe ningun cliente con el Id: " + idTemporal);
                }
               
            }
            Id.Enabled = Nombre.Enabled = Cedula.Enabled = Direccion.Enabled = Correo.Enabled = Telefono.Enabled = LimiteCredito.Enabled = false;
        }

        private async void ListaDeClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string id = ListaDeClientes[0, e.RowIndex].Value.ToString();
            string nombre = "";

            if (!String.IsNullOrWhiteSpace(id))
            {
                Cliente nuevoCliente =await new Cliente().BuscarAsync(id);

                double creditoActivo = 0;
                if (nuevoCliente.CreditosActivo != null)
                {


                    foreach (var item in nuevoCliente.CreditosActivo)
                    {
                        creditoActivo += item.Total;
                    }
                }

                Id.Text = nuevoCliente.Id;
                nombre =  Nombre.Text = nuevoCliente.Nombre;
                Direccion.Text = nuevoCliente.Direccion;
                Cedula.Text = nuevoCliente.Cedula;
                Telefono.Text = nuevoCliente.Telefono;
                Correo.Text = nuevoCliente.Correo;
                LimiteCredito.Text = nuevoCliente.LimiteCredito.ToString();
                CreditoActivo2.Text = creditoActivo.ToString();

                ClienteActivo = nuevoCliente;
            }

            Id.Enabled = Nombre.Enabled = Cedula.Enabled = Direccion.Enabled = Correo.Enabled = Telefono.Enabled = LimiteCredito.Enabled = false;          
        }

        private void Editar_Click(object sender, EventArgs e)
        {
            Id.Enabled = Nombre.Enabled = Cedula.Enabled = Direccion.Enabled = Correo.Enabled = Telefono.Enabled = LimiteCredito.Enabled = true;
        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            Id.Text = Nombre.Text = Cedula.Text = Direccion.Text = Correo.Text  = Telefono.Text = "";
        }      
        /*private Cuenta BuscarCreditos(string  id)
        {
            Cuenta credito = new Cuenta().BuscarPorIdFactura(id);           

            if (credito != null) { 
            
                if(credito.Abono <=  credito.Valor)
                {
                    if (ClienteActivo.Id == credito.IdCliente)
                    {
                        return credito;
                    }                                      
                }
                else
                {
                    MessageBox.Show("Este credito ya esta pago");
                }
            }
            else
            {
                MessageBox.Show($"El id {id} no pertenece a un credito");
            }
            return null;
        }*/
        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            new OpcionesDeCredito() {ClienteActivo = ClienteActivo }.ShowDialog();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
