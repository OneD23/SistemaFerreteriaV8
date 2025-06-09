using DocumentFormat.OpenXml.Drawing.Diagrams;
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
    public partial class VentanaFactura : Form
    {
        public Factura Factura { get; set; }
        public VentanaFactura()
        {
            InitializeComponent();
        }

        private void VentanaFactura_Load(object sender, EventArgs e)
        {
            if(Factura != null)
            {
                Configuraciones config = new Configuraciones().ObtenerPorId(1);

                Titulo.Text = config.Nombre;
                DireccionNegocio.Text = config.Direccion;
                tel.Text = config.Telefono;

                string serie = "B02";

                if (Factura.tipoFactura == "Comprobante Fiscal") {
                    serie = "B01";
                } else if (Factura.tipoFactura == "Comprobante Gubernamental")
                {
                    serie = "B15";
                }

                IdFactura.Text = this.Factura.Id.ToString();
                NFC.Text = serie + (this.Factura.NFC != null ? this.Factura.NFC.ToString() : "");
                Valido.Text = config.FechaExpiracion != null ? config.FechaExpiracion.ToString() : "";
                RNC.Text = config.RNC != null ? config.RNC.ToString() : "";

                TipoFactura.Text = Factura.tipoFactura.ToString();

                RNCCliente.Text = Factura.RNC != null ? Factura.RNC.ToString() : "";
                Cliente.Text = Factura.NombreCliente != null ? Factura.NombreCliente.ToString(): "";
                Direccion.Text = Factura.Direccion != null ? Factura.Direccion.ToString(): "";
                Fecha.Text = Factura.Fecha!= null ? Factura.Fecha.ToString(): "";

                foreach (var item in Factura.Productos)
                {
                    dataGridView1.Rows.Add(item.Cantidad, item.Producto.Nombre, item.Precio, item.Cantidad * item.Precio);
                }
                total.Text = Factura.Total.ToString("c2");
            }
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Esta seguro que desea eliminar esta factura", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            Factura.EliminarFactura(Factura.Id);
        }

        public async void RegistrarFactura(/*bool paga*/)
        {
            if (!string.IsNullOrWhiteSpace(IdFactura.Text))
            {
                List<ListProduct> listaProducto = new List<ListProduct>();
                List<double> listaCantidad = new List<double>();

                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    if (item.Cells[0] != null && item.Cells[0].Value != null)
                    {
                        string nombre = !string.IsNullOrWhiteSpace(item.Cells[1].Value.ToString()) ? item.Cells[1].Value.ToString() : "0";
                        string cantidad = !string.IsNullOrWhiteSpace(item.Cells[0].Value.ToString()) ? item.Cells[0].Value.ToString() : "0";
                        string precio = !string.IsNullOrWhiteSpace(item.Cells[2].Value.ToString()) ? item.Cells[2].Value.ToString() : "0";

                        Productos productoActual = new Productos().Buscar("nombre", nombre);

                        ListProduct productos = new ListProduct()
                        {
                            Producto = productoActual,
                            Cantidad = double.Parse(cantidad),
                            Precio = double.Parse(precio)
                        };
                        productoActual.Vendido += double.Parse(cantidad);
                        productoActual.ActualizarProductos();
                        listaProducto.Add(productos);
                    }
                }

                Factura factura = new Factura();
               
                if (Factura.Buscar(factura.Id) != null)
                {
                    factura.NombreCliente = Cliente.Text;
                    factura.NombreEmpresa = new Caja().BuscarPorClave("estado", "true").Id;
                    //IdCliente = !string.IsNullOrWhiteSpace(IdCliente.Text) ? IdCliente.Text : " ",
                   // factura.Fecha = 
                    factura.Productos = listaProducto;
                    factura.Total = double.Parse(total.Text);
                    //Descuentos = descuentoActivo,
                    //Description = descripcion.Text,
                    //Direccion = direccion.Text,
                   // MetodoDePago = tipoFactura.Text,
                    //factura.Paga = paga;
                    //Enviar = N.Checked,
                    //tipoFactura = tipoFactura.Text,

                }
                // facturaActiva = factura;
            }
        }

        private void Actualizar_Click(object sender, EventArgs e)
        {
            RegistrarFactura();
        }

        private void IdFactura_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<ListProduct> listaProducto = new List<ListProduct>();

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Cells[0] != null && item.Cells[0].Value != null)
                {
                    string nombre = !string.IsNullOrWhiteSpace(item.Cells[1].Value.ToString()) ? item.Cells[1].Value.ToString() : "0";
                    string cantidad = !string.IsNullOrWhiteSpace(item.Cells[0].Value.ToString()) ? item.Cells[0].Value.ToString() : "0";
                    string precio = !string.IsNullOrWhiteSpace(item.Cells[2].Value.ToString()) ? item.Cells[2].Value.ToString() : "0";
                    
                    Productos productoActual = new Productos().Buscar("nombre", nombre.Trim());

                    ListProduct productos = new ListProduct()
                    {
                        Producto = new Productos() { Nombre = nombre},
                        Cantidad = double.Parse(cantidad),
                        Precio = double.Parse(precio)
                    };
                    
                    listaProducto.Add(productos);
                }
            }
            Reportes n = new Reportes() {FacturaActiva= Factura, productos = listaProducto };
            n.GenerarConducePDF();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Factura.tipoFactura == "Consumo")
            {
                Factura.GenerarFactura();
            }
            else if (Factura.tipoFactura == "Comprobante Fiscal")
            {
                Factura.GenerarFacturaComprobante();
            }
            else if (Factura.tipoFactura == "Comprobante Gubernamental")
            {
                Factura.GenerarFacturaGubernamental();
            }
            else
            {
                Factura.GenerarFactura1();
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double cantidad = 0;
            double precio = 0;
            double totalActivo1 = 0;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item != null && item.Cells[0].Value != null && item.Cells[2].Value != null)
                {
                    cantidad = double.Parse(item.Cells[0].Value.ToString()); 
                    precio = double.Parse(item.Cells[2].Value.ToString());
                    item.Cells[3].Value = cantidad * precio;
                    totalActivo1 += cantidad * precio;
                    
                }
            }
            total.Text = totalActivo1.ToString("c2");
        }
    }
}
