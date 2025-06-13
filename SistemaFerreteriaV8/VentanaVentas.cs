
using DocumentFormat.OpenXml.Drawing;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using Octetus.ConsultasDgii.Services;
using SistemaFerreteriaV8.Clases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace SistemaFerreteriaV8
{
    public partial class VentanaVentas : Form
    {
        public Empleado empleado { get; set; }

        public string codigoProducto = string.Empty;
        Productos productoActivo = null;
        Factura facturaActiva { get; set; }
        double totalActivo = 0;
        double descuentoActivo = 0;
        public bool esCargada = false;


        public VentanaVentas()
        {
            InitializeComponent();
        }

        #region Limpieza del Formulario

        /// <summary>
        /// Limpia todos los campos de la ventana de ventas y reinicia el estado.
        /// </summary>
        /// <param name="resetCliente">
        /// Si es true, también resetea los campos de cliente; si false, solo limpia la lista de compras.
        /// </param>
        public void LimpiarTodo(bool resetCliente = true)
        {
            // Fechas y horas
            Fecha.Text = DateTime.Now.ToShortDateString();
            Hora.Text = DateTime.Now.ToShortTimeString();

            // Campos de cliente
            if (resetCliente)
            {
                IdCliente.Text = "0";
                NombreCliente.Text = "Generico";
            }

            // Lista de productos
            ListaDeCompras.Rows.Clear();

            // Totales y descuentos
            totalActivo = 0;
            descuentoActivo = 0;
            SubTotal.Text = Total.Text = ADescontar.Text = string.Empty;

            // Otros campos
            direccion.Text = descripcion.Text = string.Empty;
            N.Checked = false;
            esCargada = false;
            BuscarPorNombreBox.Visible = false;

            // Tipo de factura desde configuración
            var config = new Configuraciones().ObtenerPorId(1);
            tipoFactura.Text = config?.Seleccion ?? "0";

            // Generar nuevo número de factura
            NoFactura.Text = new Factura().GenerarId().ToString();
        }

        #endregion

        #region Carga de Factura para Edición

        /// <summary>
        /// Carga los datos de una factura existente en la UI para edición.
        /// </summary>
        /// <param name="factura">Instancia de la factura a cargar.</param>
        public async Task CargarFacturaAsync(Factura factura)
        {
            if (factura == null) return;

            bool puedeEditar = empleado?.Puesto == "Administrador";
            if (!puedeEditar)
            {
                string clave = Interaction.InputBox("Necesita la clave del Administrador para editar");
                var admin = await  Empleado.BuscarPorClaveAsync("contrasena", clave);
                puedeEditar = admin?.Puesto == "Administrador";
            }

            if (!puedeEditar)
            {
                MessageBox.Show("Acceso denegado. Solo Administrador puede editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (factura.Eliminada)
            {
                MessageBox.Show("La factura ya fue eliminada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Asignar datos de la factura a los controles
            facturaActiva = factura;
            LimpiarTodo(resetCliente: false);

            NombreCliente.Text = factura.NombreCliente;
            IdCliente.Text = string.IsNullOrWhiteSpace(factura.IdCliente) || factura.IdCliente == "0"
                ? factura.RNC
                : factura.IdCliente;

            tipoFactura.Text = factura.TipoFactura;
            Fecha.Text = factura.Fecha.ToShortDateString();
            Hora.Text = factura.Fecha.ToShortTimeString();
            NoFactura.Text = factura.Id.ToString();
            SubTotal.Text = (factura.Total - factura.Descuentos).ToString("C2");
            ADescontar.Text = factura.Descuentos.ToString("C2");
            Total.Text = factura.Total.ToString("C2");
            direccion.Text = factura.Direccion;
            descripcion.Text = factura.Description;
            N.Checked = factura.Enviar;
            totalActivo = factura.Total;

            // Cargar productos
            ListaDeCompras.Rows.Clear();
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
            // Actualiza los cambios de la factura en la BD
            await facturaActiva.ActualizarFacturaAsync();
        }

        #endregion


        // 1. RegistrarFactura ahora como async Task, usando métodos async y evitando async void.
        public async Task RegistrarFacturaAsync(bool paga)
        {
            if (string.IsNullOrWhiteSpace(NoFactura.Text)) return;

            // 1. Construir la lista de productos desde la DataGridView
            var listaProducto = ListaDeCompras.Rows
                .Cast<DataGridViewRow>()
                .Where(row => row.Cells[0]?.Value != null)
                .Select(row =>
                {
                    string nombre = row.Cells[0].Value.ToString();
                    double cantidad = double.TryParse(row.Cells[4]?.Value?.ToString(), out var cant) ? cant : 0;
                    double precio = double.TryParse(row.Cells[3]?.Value?.ToString(), out var prec) ? prec : 0;

                    Productos producto;
                    if (nombre == "Generico")
                    {
                        producto = new Productos
                        {
                            Nombre = "Generico",
                            Precio = new List<double> { precio, precio, precio, precio }
                        };
                    }
                    else
                    {
                        // Usamos la versión sync para no generar demasiada concurrencia en UI
                        producto = new Productos().Buscar("nombre", nombre);
                    }

                    return producto != null
                        ? new ListProduct { Producto = producto, Cantidad = cantidad, Precio = precio }
                        : null;
                })
                .Where(lp => lp != null)
                .ToList();

            // 2. Obtener caja activa de forma asíncrona
            var cajaActiva = await  Caja.BuscarPorClaveAsync("estado", "true");

            // 3. Crear o actualizar factura
            var factura = new Factura
            {
                Id = facturaActiva.Id,
                //ObjectId = facturaActiva.ObjectId,
                NombreCliente = NombreCliente.Text,
                NombreEmpresa = cajaActiva?.Id ?? "Empresa no definida",
                RNC = IdCliente.Text,
                IdCliente = IdCliente.Text,
                Fecha = DateTime.Now.AddHours(-4),
                IdEmpleado = empleado.Id.ToString(),
                Productos = listaProducto,
                Total = totalActivo,
                Descuentos = descuentoActivo,
                Description = descripcion.Text,
                Direccion = direccion.Text.Trim(),
                Paga = paga,
                Enviar = N.Checked,
                TipoFactura = tipoFactura.Text
            };

            // 4. Verificar si existe y usar async
            var existente = await Factura.BuscarAsync(factura.Id);

            if (existente != null)
                await factura.ActualizarFacturaAsync();
            else
                await factura.InsertarFacturaAsync();

            facturaActiva = factura;
        }

        // 2. AsignarTotales mantiene cálculos en UI thread
        public void AsignarTotales()
        {
            double subtotal = 0;
            foreach (DataGridViewRow row in ListaDeCompras.Rows)
            {
                if (row.Cells[5]?.Value != null && double.TryParse(row.Cells[5].Value.ToString(), out var val))
                    subtotal += val;
            }

            totalActivo = subtotal;
            if (double.TryParse(ADescontar.Text.Replace("$", ""), out var dto))
            {
                descuentoActivo = (FiltroDescuento.SelectedIndex == 0)
                    ? subtotal * (dto / 100.0)
                    : dto;
            }
            else
            {
                descuentoActivo = 0;
            }

            // Actualizar UI
            ActualizarTotalesUI(subtotal);
        }

        // 3. ActualizarTotalesUI simplificado
        private void ActualizarTotalesUI(double subtotal)
        {
            SubTotal.Text = subtotal.ToString("C2");
            Descuento.Text = descuentoActivo.ToString("C2");
            Total.Text = (subtotal - descuentoActivo).ToString("C2");
        }

        // 4. DetectaProducto ahora async Task (si necesitas buscar en BD async)
        public async Task DetectaProductoAsync()
        {
            string codigo = string.IsNullOrWhiteSpace(this.codigoProducto) ? "0" : this.codigoProducto;
            Productos producto = null;
            if (codigo == "0000")
            {
                double precioGen = ObtenerPrecioGenerico();
                if (precioGen > 0)
                {
                    producto = new Productos { Nombre = "Generico", Precio = new List<double> { precioGen, precioGen, precioGen, precioGen } };
                    await producto.ActualizarProductosAsync();
                }
            }
            else
            {
                // Si tu método BuscarAsync existe, úsalo; si no, déjalo sync.
                producto = await  Productos.BuscarAsync(codigo);
            }

            if (producto != null)
            {
                VerificarStock(producto);
                ActualizarListaDeCompras(producto);
            }
            else
            {
                MessageBox.Show($"Producto no encontrado: {codigo}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            codigoProducto = string.Empty;
        }
        public void CambiarPrecio(string valor)
        {
            int y = ListaDeCompras.CurrentRow.Index;
            double cantidad = double.Parse(obtenerValorDeCelda(ListaDeCompras[4, y]));
            double precio = double.Parse(valor);

            ListaDeCompras[3, y].Value = valor;
            ListaDeCompras[5, y].Value = (cantidad * precio).ToString();

        }
        public string obtenerValorDeCelda(DataGridViewCell celda)
        {
            string valor = "";
            if (celda != null && celda.Value != null && celda.Value.ToString() != null)
            {
                valor = !string.IsNullOrWhiteSpace(celda.Value.ToString()) ? celda.Value.ToString() : "";
            }
            return valor;
        }
        // 5. ObtenerPrecioGenerico sin cambios (puede ser sync)
        private double ObtenerPrecioGenerico()
        {
            string input = Interaction.InputBox("Este artículo es genérico. Ingrese el precio:");
            if (double.TryParse(input, out var precio) && precio > 0) return precio;
            MessageBox.Show("Precio inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return -1;
        }

        // 6. VerificarStock queda igual
        private void VerificarStock(Productos producto)
        {
            if (producto.Cantidad < 1)
                MessageBox.Show($"Agotado: {producto.Nombre}", "Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (producto.Cantidad < 3)
                MessageBox.Show($"Poco stock ({producto.Cantidad}) de {producto.Nombre}", "Stock", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // Método auxiliar: Actualizar la lista de compras
        #region Actualizar Lista de Compras

        /// <summary>
        /// Agrega un producto al DataGridView de compras o incrementa su cantidad si ya existe.
        /// Luego recalcula totales.
        /// </summary>
        private void ActualizarListaDeCompras(Productos producto)
{
    // Busca la fila existente por nombre de producto
    var filaExistente = ListaDeCompras.Rows
        .Cast<DataGridViewRow>()
        .FirstOrDefault(r => (r.Cells["Nombre"].Value?.ToString() ?? "") == producto.Nombre);

    // Obtiene el precio actual según la configuración
    double precioBase = producto.Nombre != "Generico"
        ? ObtenerPrecioProducto(producto)
        : producto.Precio[0];

    if (filaExistente == null)
    {
        // Añade nueva fila: Nombre | Descripción | Marca | Precio Unitario | Cantidad | Subtotal
        int rowIndex = ListaDeCompras.Rows.Add(
            producto.Nombre,
            producto.Descripcion,
            producto.Marca,
            precioBase.ToString("0.00"),
            1,
            precioBase.ToString("0.00")
        );
        // Opcional: guarda el objeto de producto en Tag para futuras referencias
        ListaDeCompras.Rows[rowIndex].Tag = producto;
    }
    else
    {
        // Incrementa cantidad y actualiza sub-total
        int cantidad = Convert.ToInt32(filaExistente.Cells["Cantidad"].Value) + 1;
        filaExistente.Cells["Cantidad"].Value = cantidad;
        filaExistente.Cells["SubTotal1"].Value = (cantidad * precioBase).ToString("0.00");
    }

    // Actualiza totales de la venta
    AsignarTotales();
}

/// <summary>
/// Obtiene el precio unitario del producto según la selección en Configuraciones.
/// </summary>
private double ObtenerPrecioProducto(Productos producto)
{
    // Carga perezosa de configuración
    var config = new Configuraciones().ObtenerPorId(1);
    int indicePrecio = config?.Precio ?? 0;

    // Valida que el índice exista en la lista de precios
    if (indicePrecio < 0 || indicePrecio >= producto.Precio.Count)
        return producto.Precio.First(); // precio por defecto

    return producto.Precio[indicePrecio];
}

#endregion

#region Botón Limpiar Todo

/// <summary>
/// Evento del botón 10: limpia todo el formulario de ventas.
/// Mantiene intactos los datos de configuración y estado del empleado.
/// </summary>
private void button10_Click(object sender, EventArgs e)
{
    // (Opcional) Si quieres revertir cambios en stock o marcar algo en facturaActiva, hazlo aquí.
    LimpiarTodo();
}

#endregion

        private void GlobalKeyListener_KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    button18_Click(sender, e);
                    break;
                case Keys.F4:
                    button1_Click(sender, e);
                    break;
                case Keys.F10:
                    Cobrar_Click(sender, e);
                    break;
                case Keys.F11:
                    Eliminar_Click(sender, e);
                    break;
                case Keys.F12:
                    button3_Click(sender, e);
                    break;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            new ListaDeEnvios().Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (!esCargada)
            {
                Hora.Text = DateTime.Now.ToShortTimeString();
                NoFactura.Text = new Factura().Id.ToString();
            }
        }
        private void Eliminar_Click(object sender, EventArgs e)
        {
            if (ListaDeCompras.RowCount > 1)
            {
                try
                {

                    ListaDeCompras.Rows.RemoveAt(ListaDeCompras.CurrentRow.Index);
                }
                catch (Exception)
                {


                }
                finally
                {
                    AsignarTotales();
                }
            }
        }
        /// <summary>
        /// Al terminar de editar una celda de cantidad o precio, actualiza el subtotal y recalcula totales.
        /// </summary>
        private void ListaDeCompras_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Asegurarse de que estamos en una fila válida
            if (e.RowIndex < 0) return;

            var row = ListaDeCompras.Rows[e.RowIndex];

            // Obtener valores de cantidad y precio de forma segura
            if (row.Cells[4].Value != null
                && row.Cells[3].Value != null
                && double.TryParse(row.Cells[4].Value.ToString(), out double cantidad)
                && double.TryParse(row.Cells[3].Value.ToString(), out double precio))
            {
                // Actualizar SubTotal (columna índice 5)
                double subtotal = cantidad * precio;
                row.Cells[5].Value = subtotal.ToString("0.00");

                // Recalcular el total general
                AsignarTotales();
            }
        }

        /// <summary>
        /// Al hacer doble clic en la columna de precio (índice 3), abre la ventana de cambio de precio.
        /// </summary>
        private async void ListaDeCompras_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Validar índice de fila y columna
            if (e.RowIndex < 0 || e.ColumnIndex != 3) return;

            // Obtener el nombre del producto de la columna 0
            string nombre = ListaDeCompras.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";
            if (string.IsNullOrEmpty(nombre)) return;

            // Buscar el producto de forma asíncrona
            var producto = await  Productos.BuscarPorClaveAsync("nombre", nombre);
            if (producto == null) return;

            // Abrir la ventana de precio con el producto encontrado
            var ventana = new VentanaPrecio
            {
                dataGridView = ListaDeCompras,
                ProductoSeleccionado = producto
            };
            ventana.ShowDialog();

            // Recalcular totales después de cualquier cambio de precio
            AsignarTotales();
        }

        /// <summary>
        /// Captura el código del producto a medida que se teclean caracteres
        /// y al presionar Enter dispara la detección y agrega el producto.
        /// </summary>
        private async void Id_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Si es Enter, procesar el código acumulado
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Evita el "ding" del sistema

                // Ejecuta la detección de producto de forma asincrónica si tu método lo soporta
                await DetectaProductoAsync();

                // Limpia el buffer y el TextBox
                codigoProducto = string.Empty;
                Id.Clear();
            }
            else if (!char.IsControl(e.KeyChar))
            {
                // Acumula solo caracteres imprimibles
                codigoProducto += e.KeyChar;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            new VentanaFacturasPorCobrar().ShowDialog();
        }

        // Requiere:
        // using System.Threading.Tasks;
        // using SistemaFerreteriaV8.Clases;

        private async void IdCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;

            string textoRnc = IdCliente.Text.Trim();
            if (string.IsNullOrEmpty(textoRnc))
            {
                Id.Focus();
                return;
            }

            // 1) Intentar buscar cliente en MongoDB de forma asíncrona
            Cliente cliente = await new Cliente().BuscarAsync(textoRnc);
            if (cliente != null)
            {
                NombreCliente.Text = cliente.Nombre;
                direccion.Text = cliente.Direccion;
            }
            else
            {
                // 2) Cliente no existe: consultar DGII en segundo plano
                string NombreEncontrado = "", RNCEncontrado = "";
                var dgii = new ServicioConsultasWebDgii();

                // Llamada a DGII (bloqueante) en Task.Run para no congelar UI
                var response = await Task.Run(() => dgii.ConsultarRncRegistrados(textoRnc));
                if (response.Success)
                {
                    MessageBox.Show(
                        $"RNC: {response.RncOCedula}\n" +
                        $"Nombre/Razón Social: {response.Nombre}\n" +
                        $"Tipo: {response.Tipo}\n" +
                        $"Actividad: {response.Actividad}\n" +
                        $"Estado: {response.Estado}",
                        "RNC Registrado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    NombreEncontrado = response.Nombre;
                    RNCEncontrado = response.RncOCedula;
                }
                else
                {
                    var response2 = await Task.Run(() => dgii.ConsultarRncContribuyentes(textoRnc));
                    if (response2.Success)
                    {
                        MessageBox.Show(
                            $"RNC: {response2.CedulaORnc}\n" +
                            $"Nombre/Razón Social: {response2.NombreORazónSocial}\n" +
                            $"Nombre Comercial: {response2.NombreComercial}\n" +
                            $"Categoría: {response2.Categoría}\n" +
                            $"Actividad Económica: {response2.ActividadEconomica}\n" +
                            $"Estado: {response2.Estado}",
                            "Contribuyente",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                        NombreEncontrado = response2.NombreORazónSocial;
                        RNCEncontrado = response2.CedulaORnc;
                    }
                    else
                    {
                        // 3) Buscar en archivo local rnc.txt
                        PanelDeCarga.Visible = true;
                        await Task.Run(() => RncSearcher.DownloadAndExtractAsync(BarraDeCarga, Carga));
                        var resultado = await Task.Run(() => RncSearcher.SearchRNC(textoRnc));
                        PanelDeCarga.Visible = false;

                        if (resultado != null)
                        {
                            NombreEncontrado = resultado.Nombre; // Ajusta según estructura de RncRecord
                            RNCEncontrado = resultado.RNC;
                        }
                        else
                        {
                            MessageBox.Show("RNC no encontrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                // 4) Actualizar o insertar la factura activa con los datos del RNC
                if (!string.IsNullOrEmpty(RNCEncontrado))
                {
                    if (facturaActiva != null)
                    {
                        facturaActiva.RNC = RNCEncontrado;
                        facturaActiva.NombreCliente = NombreEncontrado;
                        NombreCliente.Text = NombreEncontrado;
                        direccion.Text = direccion.Text; // conservar dirección actual
                        await facturaActiva.ActualizarFacturaAsync();
                    }
                    else
                    {
                        facturaActiva = new Factura { RNC = RNCEncontrado, NombreCliente = NombreEncontrado };
                        NombreCliente.Text = NombreEncontrado;
                        await facturaActiva.InsertarFacturaAsync();
                    }
                }
            }

            Id.Focus();
        }

        private async void VentanaVentas_Load(object sender, EventArgs e)
        {
            // Inicialización UI
            FiltroDescuento.SelectedIndex = 0;
            GlobalKeyListener.KeyPressed += GlobalKeyListener_KeyPressed;

            // Carga ASINCRÓNICA de la lista de clientes para no bloquear la UI
            List<Cliente> clientes;
            try
            {
                clientes = await new Cliente().ListarAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clientes = new List<Cliente>();
            }

            // Preparar AutoComplete
            var nombres = clientes.Select(c => c.Nombre).ToArray();
            var autoCompleteCollection = new AutoCompleteStringCollection();
            autoCompleteCollection.AddRange(nombres);
            NombreCliente.AutoCompleteCustomSource = autoCompleteCollection;

            // Resto de inicialización
            IdCliente.Text = "0";
            NombreCliente.Text = "Generico";
            Id.Focus();
            LimpiarTodo();

            bool tieneAcceso = empleado.Puesto == "Administrador" || empleado.Puesto == "Cajera";
            VentaRapida.Enabled = tieneAcceso;
            Cobrar.Enabled = tieneAcceso;
        }

        #region Botones de cantidad y búsqueda

        private void button1_Click(object sender, EventArgs e)
        {
            // Alterna visibilidad y limpia la búsqueda
            BuscarPorNombreBox.Visible = !BuscarPorNombreBox.Visible;
            NombreABuscar.Clear();
            ListaProductos.Rows.Clear();
            NombreABuscar.Focus();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            // Disminuye la cantidad del producto seleccionado
            var row = ListaDeCompras.CurrentRow;
            if (row == null) return;

            if (double.TryParse(row.Cells[4].Value?.ToString(), out var cantidad) &&
                double.TryParse(row.Cells[3].Value?.ToString(), out var precio))
            {
                cantidad = Math.Max(0, cantidad - 1);
                row.Cells[4].Value = cantidad;
                row.Cells[5].Value = (cantidad * precio).ToString("0.00");

                // Ajusta totales internos
                totalActivo = Math.Max(0, totalActivo - precio);
                descuentoActivo = Math.Max(0, descuentoActivo - precio * (productoActivo!= null ?productoActivo.Descuento:1));
                AsignarTotales();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            // Aumenta la cantidad del producto seleccionado
            var row = ListaDeCompras.CurrentRow;
            if (row == null) return;

            if (double.TryParse(row.Cells[4].Value?.ToString(), out var cantidad) &&
                double.TryParse(row.Cells[3].Value?.ToString(), out var precio))
            {
                cantidad++;
                row.Cells[4].Value = cantidad;
                row.Cells[5].Value = (cantidad * precio).ToString("0.00");

                totalActivo += precio;
                descuentoActivo += precio * (productoActivo != null ? productoActivo.Descuento : 1);
                AsignarTotales();
            }
        }
        #endregion

        #region Procesamiento de venta

        private async void button18_Click(object sender, EventArgs e)
        {
            // Registrar productos en inventario si no está cargada
            if (!esCargada && facturaActiva != null)
                await facturaActiva.RegistrarProductosAsync(+1);

            // Registrar o actualizar la factura en BD
            await RegistrarFacturaAsync(false);

            LimpiarTodo();
        }

        private async void Cobrar_Click(object sender, EventArgs e)
        {
            if (ValidarDireccionParaEnvio())
            {
                MostrarAvisoDireccionFaltante();
                return;
            }

            // Registra la factura como pagada
            await RegistrarFacturaAsync(true);

            // Abre ventana de pago con cliente y factura actual
            var ventanaPagar = new VentanaPagar
            {
                facturaActiva = facturaActiva,
                ClienteActivo = await new Cliente().BuscarAsync(IdCliente.Text)
            };
            ventanaPagar.ShowDialog();
        }

        private bool ValidarDireccionParaEnvio()
            => N.Checked && string.IsNullOrWhiteSpace(direccion.Text);

        private void MostrarAvisoDireccionFaltante()
        {
            Aviso.Visible = true;
            MessageBox.Show(
                "La factura se marcó para enviar, debe agregar una dirección.",
                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error
            );
        }
        #endregion
        #region Navegación y foco

        // Lleva el foco al campo de producto
        private void button4_Click(object sender, EventArgs e)
            => Id.Focus();

        #endregion

        #region Cobrar y Generar Factura

        // Cobrar y registrar la factura; ahora async para await RegistrarFacturaAsync
        private async void button5_Click(object sender, EventArgs e)
        {
            Configuraciones confi = new Configuraciones().ObtenerPorId(1);
            if (confi != null)
            {
                // Validar dirección si se marcó para enviar
                if (N.Checked && string.IsNullOrWhiteSpace(direccion.Text))
            {
                Aviso.Visible = true;
                label11.ForeColor = Color.Red;
                MessageBox.Show(
                    "La factura se marcó para enviar, debe agregar una dirección.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // Registrar o actualizar la factura en BD
            await RegistrarFacturaAsync(true);

            // Restaurar UI
            label11.ForeColor = Color.White;
            Aviso.Visible = false;

            if (ListaDeCompras.Rows.Count <= 1)
            {
                MessageBox.Show(
                    "Todavía no ha registrado ningún producto para cobrar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Marcar como pagada y generar impresión según tipo
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

            // Actualizar BD e inventario
            await facturaActiva.ActualizarFacturaAsync();
            await facturaActiva.RegistrarProductosAsync(+1);

            LimpiarTodo();
            }
            else MessageBox.Show("Todavia este Sistema no se ha configurado para empezar a trabajar! Dirijase a configuraciones para configurar correctamente", "ATENCION", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        #endregion

        #region Imprimir Factura Manual

        // Botón imprime la factura activa usando PrinterClass
        private void label6_Click(object sender, EventArgs e)
        {
            var printer = new Imprimir();
            printer.ImprimirFactura(
                new Configuraciones().ObtenerPorId(1).Impresora
            );
        }

        #endregion

        #region Reporte de Ventas

        // Botón genera reporte de ventas en PDF
        private async void button5_Click_1(object sender, EventArgs e)
        {
            if (N.Checked && string.IsNullOrWhiteSpace(direccion.Text))
            {
                Aviso.Visible = true;
                label11.ForeColor = Color.Red;
                MessageBox.Show(
                    "La factura se marcó para enviar, debe agregar una dirección.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            label11.ForeColor = Color.White;
            Aviso.Visible = false;

            if (facturaActiva == null || ListaDeCompras.Rows.Count <= 1)
            {
                MessageBox.Show(
                    "Todavía no ha registrado ningún producto para cobrar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            facturaActiva.TipoFactura = tipoFactura.Text;
            facturaActiva.Paga = true;
            facturaActiva.NombreCliente = NombreCliente.Text;
            facturaActiva.Direccion = direccion.Text;
            facturaActiva.Description = descripcion.Text;

            await facturaActiva.ActualizarFacturaAsync();

            var reportes = new Reportes { FacturaActiva = facturaActiva };
            await Task.Run(() => reportes.GenerarReporteVentasPDFAsync());

            LimpiarTodo();
        }

        #endregion

        #region Cliente por nombre

        // Autocompletar y buscar cliente al presionar Enter
        private async void NombreCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;

            var cliente = await new Cliente().BuscarPorClaveAsync("nombre", NombreCliente.Text);
            if (cliente != null)
            {
                IdCliente.Text = cliente.Id;
                direccion.Text = cliente.Direccion;
            }
        }

        private void NombreCliente_Enter(object sender, EventArgs e)
        {
            // Opcional: abrir dropdown de autocompletado
            NombreCliente.SelectAll();
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

        #endregion

        #region Búsqueda RNC

        // Busca datos en un archivo rnc.txt
        private string[] BuscarPorRNC(string rnc)
        {
            const string ruta = @"rnc.txt";
            if (!File.Exists(ruta)) return null;

            foreach (var linea in File.ReadLines(ruta))
            {
                if (!linea.Contains(rnc)) continue;

                var partes = linea.Split('|');
                MessageBox.Show(
                    $"RNC: {partes[0]}\nNombre: {partes[1]}\nDescripción: {partes[3]}\nFecha: {partes[8]}\nEstado: {partes[9]}",
                    "RNC encontrado!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return new[] { partes[0], partes[1] };
            }
            return null;
        }

        #endregion

        #region Cambio de Tipo de Factura

        // Guarda la selección en configuración
        private void tipoFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            var config = new Configuraciones().ObtenerPorId(1);
            if (config != null) {
                config.Seleccion = tipoFactura.Text;
                config.Guardar();
            }
            
        }

        #endregion

        #region Acción vacía (placeholder)

        private void button6_Click(object sender, EventArgs e)
        {
            // Este botón no tiene funcionalidad asignada
        }

        #endregion
        #region Cotizar Venta

        /// <summary>
        /// Realiza una cotización sin marcar como pagada.
        /// </summary>
        public async Task CotizarAsync()
        {
            // Registra factura en BD sin marcarla pagada
            await RegistrarFacturaAsync(false);

            // Reset UI
            label11.ForeColor = Color.White;
            Aviso.Visible = false;

            if (ListaDeCompras.Rows.Count <= 1)
            {
                MessageBox.Show(
                    "Todavía no ha registrado ningún producto para cotizar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning
                );
                return;
            }

            // Configura la factura como cotización
            facturaActiva.Paga = false;
            facturaActiva.Cotizacion = true;
            facturaActiva.NombreCliente = NombreCliente.Text;
            facturaActiva.Direccion = direccion.Text;
            facturaActiva.Description = descripcion.Text;

            // Genera la cotización PDF
            facturaActiva.GenerarFactura1();

            // Actualiza en BD
            await facturaActiva.ActualizarFacturaAsync();

            LimpiarTodo();
        }

        #endregion

        #region Búsqueda Dinámica de Productos (async)

        private async void textBox1_TextChanged(object sender, EventArgs e)
        {
            ListaProductos.Rows.Clear();

            var term = NombreABuscar.Text?.Trim().ToLower();
            if (string.IsNullOrEmpty(term)) return;

            // Conexión async a MongoDB
            var client = new MongoClient(new OneKeys().URI);
            var db = client.GetDatabase("Ferreteria");
            var col = db.GetCollection<Productos>("Productos");

            var filter = Builders<Productos>.Filter.And(
                Builders<Productos>.Filter.Regex("nombre", new BsonRegularExpression(term, "i")),
                Builders<Productos>.Filter.Ne("nombre", "Generico")
            );
            var projection = Builders<Productos>.Projection
                .Include(p => p.Nombre)
                .Include(p => p.Descripcion)
                .Include(p => p.Marca)
                .Include(p => p.Precio);

            var results = await col.Find(filter).Project<Productos>(projection).ToListAsync();

            foreach (var prod in results)
            {
                ListaProductos.Rows.Add(
                    prod.Id,
                    prod.Nombre,
                    prod.Descripcion,
                    prod.Precio.FirstOrDefault()
                );
            }
        }

        #endregion

        #region Selección de Producto desde Resultado

        private async void ListaProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var id = ListaProductos.Rows[e.RowIndex].Cells[0].Value?.ToString();
            if (string.IsNullOrEmpty(id)) return;

            // Busca producto async
            var producto = await  Productos.BuscarAsync(id);
            if (producto != null)
            {
                await DetectaProductoAsync();
                AsignarTotales();
            }

            ListaProductos.Rows.Clear();
            NombreABuscar.Clear();
        }

        #endregion

        #region Imprimir Rápido con PrinterClass

        private void label11_Click(object sender, EventArgs e)
        {
            var printer = new PrinterClass(lineWidth: 48)
            {
                PrinterName = "80mm Series Printer (Copiar 1)"
            };

            // Configura logo y encabezados
            printer.SetLogo(SistemaFerreteriaV8.Properties.Resources.logo_ivan_modificado_3);
            printer.AddCenteredLine("FERRETERIA XYZ");
            printer.AddCenteredLine("RNC: 123456789");
            printer.AddCenteredLine("Tel: 809-555-1234");
            printer.AddSeparator();

            // Ejemplo de demostración
            printer.AddLine("Factura No: 00123");
            printer.AddLine($"Fecha: {DateTime.Now:dd/MM/yyyy}");
            printer.AddSeparator();
            printer.AgregaArticulo(10.00, "Martillo de construcción", 2, 20.00);
            printer.AgregaArticulo(5.00, "Destornillador", 1, 5.00);
            printer.AddSeparator();
            printer.AddRightAlignedLine("Total: $25.00");
            printer.AddSeparator();
            printer.AddCenteredLine("Gracias por su compra");
            printer.AddCenteredLine("¡Vuelva pronto!");
        }

        #endregion

        #region Actualizar Fechas Masivas

        private async void label10_Click(object sender, EventArgs e)
        {
            var listFact = new List<Factura>();
            for (int id = 6574; id < 6580; id++)
            {
                var fact = await Factura.BuscarAsync(id);
                if (fact != null)
                {
                    fact.Fecha = fact.Fecha.AddDays(10);
                    await fact.ActualizarFacturaAsync();
                    listFact.Add(fact);
                }
            }
            MessageBox.Show("Tarea Finalizada!");
            new Reportes().GenerarReportes(listFact, DateTime.Now, DateTime.Now);
        }

        #endregion

        #region Tecla de Descuento

        private void ADescontar_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permitir dígitos y punto
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;
        }

        #endregion

        #region Integración WooCommerce Async

        private async void button6_Click_1(object sender, EventArgs e)
        {
            var api = new WooCommerce.WooCommerce();
            await api.EjecutarWooCommerce();
            ListaDeCompras.Rows.Add(api.P.fee_lines.Count);
        }

        #endregion


    }
}
