using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Drawing;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.VisualBasic;

namespace SistemaFerreteriaV8
{
    public partial class VentanaVentas : Form
    {
        public Empleado empleado { get; set; }
        public string codigoProducto = string.Empty;
        Productos productoActivo = null;
        Factura facturaActiva { get; set; }
        public EventHandler<KeyEventArgs> GlobalKeyListener_KeyPressed { get; private set; }

        double totalActivo = 0;
        double descuentoActivo = 0;
        public bool esCargada = false;

        public VentanaVentas()
        {
            InitializeComponent();
        }

        #region Form Events

        private async void VentanaVentas_Load(object sender, EventArgs e)
        {
            FiltroDescuento.SelectedIndex = 0;
            GlobalKeyListener.KeyPressed += GlobalKeyListener_KeyPressed;

            var clientes = await new Cliente().ListarAsync();
            var autoCompleteCollection = new AutoCompleteStringCollection();
            autoCompleteCollection.AddRange(clientes.Select(c => c.Nombre).ToArray());
            NombreCliente.AutoCompleteCustomSource = autoCompleteCollection;

            IdCliente.Text = "0";
            NombreCliente.Text = "Generico";
            Id.Focus();
            LimpiarTodo();

            VentaRapida.Enabled = empleado.Puesto == "Administrador" || empleado.Puesto == "Cajera";
            Cobrar.Enabled = VentaRapida.Enabled;
        }

        #endregion

        #region Helpers: Limpiar y totales

        public void LimpiarTodo()
        {
            Fecha.Text = DateTime.Now.ToShortDateString();
            Hora.Text = DateTime.Now.ToShortTimeString();
            NombreCliente.Text = "Generico";
            IdCliente.Text = "0";
            ListaDeCompras.Rows.Clear();
            totalActivo = 0;
            descuentoActivo = 0;
            Total.Text = "";
            SubTotal.Text = "";
            ADescontar.Text = "";
            direccion.Text = "";
            descripcion.Text = "";
            tipoFactura.Text = new Configuraciones().ObtenerPorId(1)?.Seleccion ?? "0";
            N.Checked = false;
            esCargada = false;
            NoFactura.Text = new Factura().Id.ToString();
            facturaActiva = null;
            BuscarPorNombreBox.Visible = false;
        }

        public void AsignarTotales()
        {
            double totalActivo1 = 0;
            foreach (DataGridViewRow item in ListaDeCompras.Rows)
            {
                if (item?.Cells[5]?.Value != null && double.TryParse(item.Cells[5].Value.ToString(), out double valor))
                    totalActivo1 += valor;
            }
            totalActivo = totalActivo1;
            if (!string.IsNullOrEmpty(ADescontar.Text))
            {
                if (FiltroDescuento.SelectedIndex == 0)
                {
                    string textoDescuento = ADescontar.Text.Replace("$", "");
                    descuentoActivo = totalActivo * (double.Parse(textoDescuento) / 100.00);
                }
                else if (FiltroDescuento.SelectedIndex == 1)
                {
                    string textoDescuento = ADescontar.Text.Replace("$", "");
                    descuentoActivo = double.Parse(textoDescuento);
                }
            }
            SubTotal.Text = totalActivo1.ToString("C2");
            Descuento.Text = descuentoActivo.ToString("C2");
            Total.Text = (totalActivo1 - descuentoActivo).ToString("C2");
        }

        #endregion

        #region Factura: Cargar, registrar

        public async Task CargarFacturaAsync(Factura factura)
        {
            bool puedeEditar = empleado.Puesto == "Administrador";
            if (!puedeEditar)
            {
                string codigo = Interaction.InputBox("Nesecita la clave del Administrador para editar");
                var admin =  new Empleado().BuscarPorClave("contrasena", codigo);
                puedeEditar = admin?.Puesto == "Administrador";
            }

            if (factura != null && puedeEditar && !factura.Eliminada)
            {
                facturaActiva = factura;
                ListaDeCompras.Rows.Clear();
                NombreCliente.Text = factura.NombreCliente;
                IdCliente.Text = string.IsNullOrWhiteSpace(factura.IdCliente) || factura.IdCliente == "0"
                                 ? factura.RNC ?? ""
                                 : factura.IdCliente;
                tipoFactura.Text = factura.TipoFactura;
                Fecha.Text = factura.Fecha.ToShortDateString();
                Hora.Text = factura.Fecha.ToShortTimeString();
                NoFactura.Text = factura.Id.ToString();
                SubTotal.Text = (factura.Total - factura.Descuentos).ToString("c2");
                ADescontar.Text = factura.Descuentos.ToString("c2");
                Total.Text = factura.Total.ToString("c2");
                totalActivo = factura.Total;
                direccion.Text = factura.Direccion;
                descripcion.Text = factura.Description;
                N.Checked = factura.Enviar;

                if (factura.Productos != null)
                {
                    foreach (var item in factura.Productos)
                    {
                        ListaDeCompras.Rows.Add(
                            item.Producto.Nombre,
                            item.Producto.Descripcion,
                            item.Producto.Marca,
                            item.Precio,
                            item.Cantidad,
                            item.Cantidad * item.Precio
                        );
                    }
                }

                facturaActiva.Editada = true;
                await facturaActiva.ActualizarFacturaAsync();
            }
            else if (factura != null && factura.Eliminada)
            {
                MessageBox.Show("La factura fue eliminada del sistema.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Número de factura incorrecto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public async Task RegistrarFacturaAsync(bool paga)
        {
            if (string.IsNullOrWhiteSpace(NoFactura.Text)) return;

            var listaProducto = ListaDeCompras.Rows
                .Cast<DataGridViewRow>()
                .Where(row => row.Cells[0]?.Value != null)
                .Select(row =>
                {
                    string nombre = row.Cells[0].Value?.ToString();
                    double cantidad = double.TryParse(row.Cells[4]?.Value?.ToString(), out var cant) ? cant : 0;
                    double precio = double.TryParse(row.Cells[3]?.Value?.ToString(), out var prec) ? prec : 0;
                    Productos producto;
                    if (nombre == "Generico")
                    {
                        producto = new Productos
                        {
                            Id = "0000",
                            Nombre = "Generico",
                            Precio = new List<double> { precio, precio, precio, precio }
                        };
                    }
                    else
                    {
                        producto = new Productos().Buscar("nombre", nombre);
                    }
                    return producto != null
                        ? new ListProduct { Producto = producto, Cantidad = cantidad, Precio = precio }
                        : null;
                })
                .Where(x => x != null)
                .ToList();

            var cajaActiva = await new Caja().BuscarPorClaveAsync("estado", "true");

            Factura factura = new Factura
            {
                NombreCliente = NombreCliente.Text,
                NombreEmpresa = cajaActiva?.Id ?? "Empresa no definida",
                RNC = IdCliente.Text,
                IdCliente = IdCliente.Text,
                Fecha = DateTime.Now,
                IdEmpleado = empleado.Id,
                Productos = listaProducto,
                Total = totalActivo,
                Descuentos = descuentoActivo,
                Description = descripcion.Text,
                Direccion = direccion.Text,
                Paga = paga,
                Enviar = N.Checked,
                TipoFactura = tipoFactura.Text
            };

            var facturaExistente = await Factura.BuscarAsync(factura.Id);
            if (facturaExistente != null)
                await factura.ActualizarFacturaAsync();
            else
                await factura.InsertarFacturaAsync();

            facturaActiva = factura;
        }

        #endregion

        #region Botones y eventos principales

        private async void Cobrar_Click(object sender, EventArgs e)
        {
            if (N.Checked && string.IsNullOrWhiteSpace(direccion.Text))
            {
                Aviso.Visible = true;
                MessageBox.Show("La factura se marcó para enviar, debe agregar una dirección.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await RegistrarFacturaAsync(true);

            var clienteActivo = await new Cliente().BuscarAsync(IdCliente.Text);
            var ventanaPagar = new VentanaPagar
            {
                facturaActiva = facturaActiva,
                ClienteActivo = clienteActivo
            };
            ventanaPagar.ShowDialog();
        }

        private async void button18_Click(object sender, EventArgs e)
        {
            if (!esCargada && facturaActiva != null)
            {
                await facturaActiva.RegistrarProductosAsync(+1);
            }
            await RegistrarFacturaAsync(false);
            LimpiarTodo();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            if (N.Checked && string.IsNullOrWhiteSpace(direccion.Text))
            {
                Aviso.Visible = true;
                Aviso.ForeColor = Color.Red;
                label11.ForeColor = Color.Red;
                MessageBox.Show("La factura se marcó para enviar, debe agregar una dirección.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                label11.ForeColor = Color.White;
                Aviso.Visible = false;
                if (ListaDeCompras.Rows.Count > 1)
                {
                    await RegistrarFacturaAsync(true);
                    facturaActiva.Paga = true;
                    facturaActiva.NombreCliente = NombreCliente.Text;
                    facturaActiva.Direccion = direccion.Text;
                    facturaActiva.Description = descripcion.Text;

                    switch (tipoFactura.Text)
                    {
                        case "Comprobante Gubernamental":
                             facturaActiva.GenerarFacturaGubernamental();
                            break;
                        case "Comprobante Fiscal":
                             facturaActiva.GenerarFacturaComprobante();
                            break;
                        case "Consumo":
                             facturaActiva.GenerarFacturaAsync();
                            break;
                        default:
                             facturaActiva.GenerarFactura1();
                            break;
                    }

                    await facturaActiva.ActualizarFacturaAsync();
                    await facturaActiva.RegistrarProductosAsync(+1);
                    LimpiarTodo();
                }
                else
                {
                    MessageBox.Show("Todavía no ha registrado ningún producto para cobrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        #endregion

        #region Campos y eventos de Cliente

        private async void NombreCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                var cliente = await new Cliente().BuscarPorClaveAsync("nombre", NombreCliente.Text);
                if (cliente != null)
                    IdCliente.Text = cliente.Id;
            }
        }

        private async void NombreCliente_TextChanged(object sender, EventArgs e)
        {
            var cliente = await new Cliente().BuscarPorClaveAsync("nombre", NombreCliente.Text);
            if (cliente != null)
            {
                IdCliente.Text = cliente.Id;
                direccion.Text = cliente.Direccion;
            }
        }

        private async void IdCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                var cliente = await new Cliente().BuscarAsync(IdCliente.Text);
                if (cliente != null)
                {
                    NombreCliente.Text = cliente.Nombre;
                    direccion.Text = cliente.Direccion;
                }
                else
                {
                    // Lógica async para DGII aquí si aplica
                }
                Id.Focus();
            }
        }

        #endregion

        #region Lógica de productos

        public void DetectaProducto()
        {
            codigoProducto = !string.IsNullOrWhiteSpace(this.codigoProducto) ? this.codigoProducto : "0";
            bool productoEncontrado = false;
            if (codigoProducto == "0000")
            {
                double precioGenerico = ObtenerPrecioGenerico();
                if (precioGenerico > 0)
                {
                    Productos productoGenerico = new Productos
                    {
                        Nombre = "Generico",
                        Precio = new List<double> { precioGenerico, precioGenerico, precioGenerico, precioGenerico }
                    };
                    productoGenerico.ActualizarProductos();
                    productoActivo = productoGenerico;
                    productoEncontrado = true;
                }
            }
            else
            {
                Productos producto = new Productos().Buscar(codigoProducto);
                if (producto != null)
                {
                    productoActivo = producto;
                    productoEncontrado = true;
                }
            }

            if (productoEncontrado)
            {
                VerificarStock(productoActivo);
                ActualizarListaDeCompras(productoActivo);
            }
            else
            {
                MessageBox.Show(
                    codigoProducto != "0" ? $"No existe ningún producto con el código: {codigoProducto}" : "No se ha ingresado ningún código.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            codigoProducto = string.Empty;
        }

        private double ObtenerPrecioGenerico()
        {
            string precioTemporal = Interaction.InputBox("Este artículo es genérico. Ingrese el precio:");
            if (!string.IsNullOrWhiteSpace(precioTemporal) && double.TryParse(precioTemporal, out double precioGenerico))
            {
                return precioGenerico;
            }
            else
            {
                MessageBox.Show("El precio ingresado no es válido. Inténtelo de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private void VerificarStock(Productos producto)
        {
            if (producto.Cantidad < 3 && producto.Cantidad > 0)
            {
                MessageBox.Show($"Quedan menos de 3 {producto.Nombre}.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (producto.Cantidad < 1)
            {
                MessageBox.Show($"Ya no quedan {producto.Nombre}.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ActualizarListaDeCompras(Productos producto)
        {
            DataGridViewRow filaExistente = ListaDeCompras.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(row => row.Cells[0].Value?.ToString() == producto.Nombre);

            if (filaExistente == null)
            {
                double precioProducto = producto.Nombre != "Generico"
                    ? producto.Precio[ObtenerIndicePrecio()]
                    : producto.Precio[0];
                ListaDeCompras.Rows.Add(producto.Nombre, producto.Descripcion, producto.Marca, precioProducto, 1, precioProducto);
                totalActivo += precioProducto;
                descuentoActivo += precioProducto * producto.Descuento;
                AsignarTotales();
            }
            else
            {
                int cantidadActual = Convert.ToInt32(filaExistente.Cells[4].Value) + 1;
                filaExistente.Cells[4].Value = cantidadActual;
                filaExistente.Cells[5].Value = cantidadActual * Convert.ToDouble(filaExistente.Cells[3].Value);
                AsignarTotales();
            }
        }

        private int ObtenerIndicePrecio()
        {
            Configuraciones configuracion = new Configuraciones();
            return configuracion.ObtenerPorId(1).Precio;
        }

        #endregion

        // El resto de eventos (botones para sumar/restar productos, etc.) los puedes dejar como estaban si no interactúan con la DB.

        // Agrega aquí cualquier otro evento que requiera async y acceso a base de datos, solo cambia Buscar() por BuscarAsync, etc.

    }
}
