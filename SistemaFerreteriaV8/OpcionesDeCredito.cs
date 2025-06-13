using SistemaFerreteriaV8.Clases;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaFerreteriaV8
{
    public partial class OpcionesDeCredito : Form
    {
        public Cliente ClienteActivo { get; set; }

        public OpcionesDeCredito()
        {
            InitializeComponent();
        }

        private async void OpcionesDeCredito_Load(object sender, EventArgs e)
        {
            await CargarAsync();
        }

        // Registrar pago de crédito (abono)
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var row = ListaCreditos.CurrentRow;
                if (row == null || row.Index < 0 || ListaCreditos[0, row.Index]?.Value == null ||
                    string.IsNullOrWhiteSpace(ListaCreditos[0, row.Index].Value.ToString()))
                {
                    MessageBox.Show("Seleccione una factura válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = int.Parse(ListaCreditos[0, row.Index].Value.ToString());
                Factura factura = await Factura.BuscarAsync(id);

                if (factura == null)
                {
                    MessageBox.Show("No se encontró la factura seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (factura.Paga)
                {
                    MessageBox.Show("Esta factura ya se pagó.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    if (MessageBox.Show($"¿Desea pagar la factura seleccionada que tiene un valor de {factura.Total:C}?",
                        "Pago de Factura", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        factura.Paga = true;
                        await factura.ActualizarFacturaAsync();
                        await ClienteActivo.EditarAsync();

                        MessageBox.Show("La factura se ha registrado como pagada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CargarAsync(); // Refresca lista después de pagar
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Carga la información de créditos del cliente actual (ASYNC)
        public async Task CargarAsync()
        {
            if (ClienteActivo == null)
                return;

            double creditoActivo = ClienteActivo.CreditosActivo?.Sum(item => item.Total) ?? 0;

            ID.Text = ClienteActivo.Id;
            Nombre.Text = ClienteActivo.Nombre;
            LimiteCredito.Text = ClienteActivo.LimiteCredito.ToString("N2");
            CreditoUtilizado.Text = creditoActivo.ToString("N2");
            CreditoDisponible.Text = (ClienteActivo.LimiteCredito - creditoActivo).ToString("N2");

            ListaCreditos.Rows.Clear();

            var facturas = await Factura.ListarFacturasAsync("idCliente", ClienteActivo.Id);
            foreach (Factura item in facturas)
            {
                ListaCreditos.Rows.Add(item.Id, item.Fecha, item.Total.ToString("N2"), item.Paga ? "Sí" : "No");
            }
        }

        // Imprimir comprobante de abono parcial
        public async Task ImprimirComprobanteAsync(double valorAbono)
        {
            double creditoActivo = ClienteActivo.CreditosActivo?.Sum(item => item.Total) ?? 0;

            var confi = new Configuraciones().ObtenerPorId(1);
            var ticket = new CreaTicket2();

            ticket.TextoCentro(confi.Nombre ?? "FERRETERIA");
            ticket.TextoCentro(confi.Direccion ?? "");
            ticket.TextoCentro("Tel: " + (confi.Telefono ?? "809-487-1244"));
            ticket.TextoCentro("RNC:" + (confi.RNC ?? ""));

            ticket.TextoCentro("");
            ticket.TextoIzquierda("Comprobante de Pago");
            ticket.TextoExtremos("Fecha: " + DateTime.Now.ToShortDateString(), "Hora: " + DateTime.Now.ToShortTimeString());
            ticket.TextoIzquierda("Nombre/Razon social: " + ClienteActivo.Nombre);
            ticket.TextoIzquierda("Dirección: " + ClienteActivo.Direccion);

            var cl = await new Cliente().BuscarAsync(ClienteActivo.Id);
            ticket.TextoIzquierda("Tel: " + (cl?.Telefono ?? ""));

            ticket.TextoIzquierda("");
            ticket.AgregaTotales("Abono: ", valorAbono);
            ticket.AgregaTotales("Total Adeudado: ", creditoActivo);
            ticket.AgregaTotales("Deuda Actual: ", creditoActivo - valorAbono);

            // Actualiza UI
            creditoActivo -= valorAbono;
            await ClienteActivo.EditarAsync();
            CreditoDisponible.Text = (ClienteActivo.LimiteCredito - creditoActivo).ToString("N2");
            CreditoUtilizado.Text = creditoActivo.ToString("N2");

            for (int i = 0; i < 10; i++) ticket.TextoIzquierda("");
            ticket.ImprimirTiket(confi.Impresora);
        }

        // Imprimir comprobante de todos los créditos
        private async void ImprimirTotal_Click(object sender, EventArgs e)
        {
            var confi = new Configuraciones().ObtenerPorId(1);
            var ticket = new CreaTicket2();

            ticket.TextoCentro(confi.Nombre ?? "FERRETERIA");
            ticket.TextoCentro(confi.Direccion ?? "");
            ticket.TextoCentro("Tel: " + (confi.Telefono ?? "809-487-1244"));
            ticket.TextoCentro("RNC:" + (confi.RNC ?? ""));
            ticket.TextoCentro("");
            ticket.TextoIzquierda("Comprobante de Pago");

            ticket.TextoExtremos("Fecha: " + DateTime.Now.ToShortDateString(), "Hora: " + DateTime.Now.ToShortTimeString());
            ticket.TextoIzquierda("Nombre/Razon social: " + ClienteActivo.Nombre);
            ticket.TextoIzquierda("Dirección: " + ClienteActivo.Direccion);

            var cl = await new Cliente().BuscarAsync(ClienteActivo.Id);
            ticket.TextoIzquierda("Tel: " + (cl?.Telefono ?? ""));

            ticket.LineasGuion();
            ticket.TextoIzquierda("ID    Fecha                Valor    ");
            ticket.LineasGuion();

            double valorTotal = 0;
            foreach (DataGridViewRow item in ListaCreditos.Rows)
            {
                if (item?.Cells[0].Value != null)
                {
                    string id = item.Cells[0].Value.ToString().PadRight(6);
                    string fecha = item.Cells[1].Value.ToString().PadRight(21);
                    string valor = item.Cells[2].Value.ToString().PadRight(9);

                    valorTotal += double.TryParse(valor, out double val) ? val : 0;
                    ticket.TextoIzquierda(id + fecha + valor);
                }
            }

            ticket.LineasGuion();
            ticket.TextoIzquierda("");
            ticket.AgregaTotales("Total: ", valorTotal);

            for (int i = 0; i < 10; i++) ticket.TextoIzquierda("");
            ticket.ImprimirTiket(confi.Impresora);
        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // Doble clic sobre una factura: ver detalles
        private async void ListaCreditos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = int.Parse(ListaCreditos[0, e.RowIndex].Value.ToString());
            Factura factura = await Factura.BuscarAsync(id);
            if (factura != null)
            {
                new VentanaFactura() { Factura = factura }.Show();
            }
        }

        // (Vacío para posibles futuras funciones)
        private void ListaCreditos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void Editar_Click(object sender, EventArgs e)
        {

        }
    }
}
