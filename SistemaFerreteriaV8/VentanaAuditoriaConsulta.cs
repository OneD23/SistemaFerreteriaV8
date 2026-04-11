using System.Drawing;
using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Infrastructure.Services;

namespace SistemaFerreteriaV8;

public sealed class VentanaAuditoriaConsulta : Form
{
    private readonly DateTimePicker _from = new() { Format = DateTimePickerFormat.Short };
    private readonly DateTimePicker _to = new() { Format = DateTimePickerFormat.Short };
    private readonly CheckBox _chkFrom = new() { Text = "Desde", Checked = true, AutoSize = true };
    private readonly CheckBox _chkTo = new() { Text = "Hasta", Checked = true, AutoSize = true };
    private readonly TextBox _txtActor = new();
    private readonly ComboBox _cmbModule = new() { DropDownStyle = ComboBoxStyle.DropDown };
    private readonly TextBox _txtEvent = new();
    private readonly TextBox _txtOperation = new();
    private readonly NumericUpDown _numLimit = new() { Minimum = 20, Maximum = 1000, Value = 300 };
    private readonly Button _btnBuscar = new() { Text = "Consultar" };
    private readonly Button _btnLimpiar = new() { Text = "Limpiar filtros" };
    private readonly Label _lblResumen = new() { AutoSize = true, Text = "Resultados: 0" };
    private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, RowHeadersVisible = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

    public VentanaAuditoriaConsulta()
    {
        Text = "Consulta Operativa de Auditoría";
        Width = 1200;
        Height = 640;
        StartPosition = FormStartPosition.CenterParent;
        KeyPreview = true;

        BuildLayout();
        WireEvents();
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var filters = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 8, Padding = new Padding(8) };
        for (var i = 0; i < 8; i++) filters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));

        filters.Controls.Add(_chkFrom, 0, 0);
        filters.Controls.Add(_from, 1, 0);
        filters.Controls.Add(_chkTo, 2, 0);
        filters.Controls.Add(_to, 3, 0);
        filters.Controls.Add(new Label { Text = "Usuario", AutoSize = true, Top = 6 }, 4, 0);
        filters.Controls.Add(_txtActor, 5, 0);
        filters.Controls.Add(new Label { Text = "Módulo", AutoSize = true, Top = 6 }, 6, 0);
        filters.Controls.Add(_cmbModule, 7, 0);

        filters.Controls.Add(new Label { Text = "Evento", AutoSize = true, Top = 6 }, 0, 1);
        filters.Controls.Add(_txtEvent, 1, 1);
        filters.Controls.Add(new Label { Text = "OperationId", AutoSize = true, Top = 6 }, 2, 1);
        filters.Controls.Add(_txtOperation, 3, 1);
        filters.Controls.Add(new Label { Text = "Límite", AutoSize = true, Top = 6 }, 4, 1);
        filters.Controls.Add(_numLimit, 5, 1);
        filters.Controls.Add(_btnBuscar, 6, 1);
        filters.Controls.Add(_btnLimpiar, 7, 1);

        _grid.Columns.Add("TimestampUtc", "Fecha UTC");
        _grid.Columns.Add("Actor", "Usuario");
        _grid.Columns.Add("Module", "Módulo");
        _grid.Columns.Add("EventType", "Evento");
        _grid.Columns.Add("Result", "Resultado");
        _grid.Columns.Add("Message", "Mensaje");
        _grid.Columns.Add("OperationId", "OperationId");
        _grid.Columns[0].FillWeight = 18;
        _grid.Columns[1].FillWeight = 14;
        _grid.Columns[2].FillWeight = 10;
        _grid.Columns[3].FillWeight = 16;
        _grid.Columns[4].FillWeight = 8;
        _grid.Columns[5].FillWeight = 26;
        _grid.Columns[6].FillWeight = 18;
        _grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
        _grid.Columns[5].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        _grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;

        _cmbModule.Items.AddRange(new object[] { "", "ventas", "caja", "inventario", "security", "permissions" });

        var gridPanel = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
        gridPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        gridPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var hdr = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(8, 4, 8, 0) };
        hdr.Controls.Add(_lblResumen);
        gridPanel.Controls.Add(hdr, 0, 0);
        gridPanel.Controls.Add(_grid, 0, 1);

        root.Controls.Add(filters, 0, 0);
        root.Controls.Add(gridPanel, 0, 1);
        Controls.Add(root);

        AcceptButton = _btnBuscar;
    }

    private void WireEvents()
    {
        _btnBuscar.Click += async (_, _) => await ConsultarAsync();
        _btnLimpiar.Click += (_, _) => LimpiarFiltros();
        KeyDown += VentanaAuditoriaConsulta_KeyDown;
    }

    private async Task ConsultarAsync()
    {
        _btnBuscar.Enabled = false;
        try
        {
            var query = new AuditQuery(
                FromUtc: _chkFrom.Checked ? _from.Value.Date.ToUniversalTime() : null,
                ToUtc: _chkTo.Checked ? _to.Value.Date.AddDays(1).AddSeconds(-1).ToUniversalTime() : null,
                Actor: _txtActor.Text.Trim(),
                Module: _cmbModule.Text.Trim(),
                EventType: _txtEvent.Text.Trim(),
                OperationId: _txtOperation.Text.Trim(),
                Limit: (int)_numLimit.Value);

            var results = await AppServices.Audit.QueryAsync(query);

            _grid.Rows.Clear();
            foreach (var item in results)
            {
                var rowIndex = _grid.Rows.Add(
                    item.TimestampUtc.ToString("yyyy-MM-dd HH:mm:ss"),
                    string.IsNullOrWhiteSpace(item.ActorName) ? item.ActorId : item.ActorName,
                    item.Module,
                    item.EventType,
                    item.Result,
                    item.Message,
                    ExtractOperationId(item.MetadataJson));

                var row = _grid.Rows[rowIndex];
                if (string.Equals(item.Result, "error", StringComparison.OrdinalIgnoreCase) || string.Equals(item.Result, "unexpected_error", StringComparison.OrdinalIgnoreCase))
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                else if (string.Equals(item.Result, "warning", StringComparison.OrdinalIgnoreCase) || string.Equals(item.Result, "stock_error", StringComparison.OrdinalIgnoreCase))
                    row.DefaultCellStyle.BackColor = Color.LemonChiffon;
                else if (string.Equals(item.Result, "ok", StringComparison.OrdinalIgnoreCase) || string.Equals(item.Result, "confirmed", StringComparison.OrdinalIgnoreCase))
                    row.DefaultCellStyle.BackColor = Color.Honeydew;
            }

            _lblResumen.Text = $"Resultados: {results.Count}";
            if (_grid.Rows.Count > 0)
            {
                _grid.ClearSelection();
                _grid.Rows[0].Selected = true;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al consultar auditoría: {ex.Message}", "Auditoría", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _btnBuscar.Enabled = true;
        }
    }

    private void LimpiarFiltros()
    {
        _chkFrom.Checked = true;
        _chkTo.Checked = true;
        _from.Value = DateTime.Today.AddDays(-7);
        _to.Value = DateTime.Today;
        _txtActor.Text = string.Empty;
        _cmbModule.Text = string.Empty;
        _txtEvent.Text = string.Empty;
        _txtOperation.Text = string.Empty;
        _numLimit.Value = 300;
        _txtActor.Focus();
    }

    private async void VentanaAuditoriaConsulta_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.F)
        {
            e.SuppressKeyPress = true;
            _txtActor.Focus();
            return;
        }

        if (e.KeyCode == Keys.F5)
        {
            e.SuppressKeyPress = true;
            await ConsultarAsync();
        }
    }

    private static string ExtractOperationId(string metadataJson)
    {
        if (string.IsNullOrWhiteSpace(metadataJson)) return string.Empty;

        var op = FindJsonValue(metadataJson, "operationId");
        return op;
    }

    private static string FindJsonValue(string json, string key)
    {
        var marker = $"\"{key}\"";
        var idx = json.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (idx < 0) return string.Empty;

        var colon = json.IndexOf(':', idx);
        if (colon < 0) return string.Empty;

        var start = json.IndexOf('"', colon + 1);
        if (start < 0) return string.Empty;

        var end = json.IndexOf('"', start + 1);
        if (end < 0) return string.Empty;

        return json.Substring(start + 1, end - start - 1);
    }
}
