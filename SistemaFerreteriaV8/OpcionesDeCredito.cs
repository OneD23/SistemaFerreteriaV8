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
            SistemaFerreteriaV8.Clases.ThemeManager.ApplyToForm(this);
            AutoScroll = true;
            MinimumSize = new System.Drawing.Size(760, 560);
            ModernizarUI();
            Resize += (_, __) => ReorganizarLayout();
        }

        private void ModernizarUI()
        {
            ListaCreditos.BorderStyle = BorderStyle.None;
            ListaCreditos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ListaCreditos.RowHeadersVisible = false;
            ListaCreditos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ListaCreditos.MultiSelect = false;
            ListaCreditos.EnableHeadersVisualStyles = false;
            ListaCreditos.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(24, 36, 60);
            ListaCreditos.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            ListaCreditos.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(64, 85, 122);
            ListaCreditos.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            ListaCreditos.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(46, 74, 125);
            ListaCreditos.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            foreach (var btn in new[] { Editar, PagarTotal, ImprimirTotal, Cancelar })
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Height = 44;
            }
            Editar.Text = "Abonar";
            PagarTotal.Text = "Pagar total";
            ImprimirTotal.Text = "Imprimir historial";

            ReorganizarLayout();
        }

        private void ReorganizarLayout()
        {
            int margen = 16;
            label1.Left = (ClientSize.Width - label1.Width) / 2;
            label1.Top = 24;

            int baseX = 26;
            int yInfo = 86;
            label2.Location = new System.Drawing.Point(baseX, yInfo);
            ID.Location = new System.Drawing.Point(baseX + 90, yInfo);
            label3.Location = new System.Drawing.Point(baseX, yInfo + 36);
            Nombre.Location = new System.Drawing.Point(baseX + 90, yInfo + 36);
            label4.Location = new System.Drawing.Point(baseX, yInfo + 72);
            LimiteCredito.Location = new System.Drawing.Point(baseX + 160, yInfo + 72);
            label5.Location = new System.Drawing.Point(baseX, yInfo + 108);
            CreditoUtilizado.Location = new System.Drawing.Point(baseX + 160, yInfo + 108);
            label6.Location = new System.Drawing.Point(baseX, yInfo + 144);
            CreditoDisponible.Location = new System.Drawing.Point(baseX + 160, yInfo + 144);

            label8.Left = (ClientSize.Width - label8.Width) / 2;
            label8.Top = yInfo + 180;

            ListaCreditos.Location = new System.Drawing.Point(margen, label8.Bottom + 10);
            ListaCreditos.Size = new System.Drawing.Size(ClientSize.Width - (margen * 2), ClientSize.Height - 320);

            int yBotones = ClientSize.Height - 66;
            int spacing = 14;
            int buttonWidth = (ListaCreditos.Width - (spacing * 3)) / 4;
            int x = ListaCreditos.Left;
            foreach (var btn in new[] { Editar, PagarTotal, ImprimirTotal, Cancelar })
            {
                btn.SetBounds(x, yBotones, buttonWidth, 44);
                x += buttonWidth + spacing;
            }
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
