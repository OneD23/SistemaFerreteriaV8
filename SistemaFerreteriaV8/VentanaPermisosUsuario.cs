using SistemaFerreteriaV8.AppCore.Abstractions;
using SistemaFerreteriaV8.Clases;
using SistemaFerreteriaV8.Domain.Security;
using SistemaFerreteriaV8.Infrastructure.Security;
using System.Drawing;

namespace SistemaFerreteriaV8;

public sealed class VentanaPermisosUsuario : Form
{
    private readonly TextBox _txtBuscar = new() { Text = "" };
    private readonly ListBox _lstUsuarios = new();
    private readonly ListBox _lstRol = new();
    private readonly ListBox _lstEfectivos = new();
    private readonly CheckedListBox _chkAllow = new();
    private readonly CheckedListBox _chkDeny = new();
    private readonly Button _btnGuardar = new() { Text = "Guardar overrides" };
    private readonly Button _btnCerrar = new() { Text = "Cerrar" };
    private readonly Label _lblEstado = new() { AutoSize = true };
    private readonly Label _lblUsuarioRol = new() { AutoSize = true, Text = "Usuario: - | Rol: -" };
    private readonly Label _lblPendiente = new() { AutoSize = true, Text = "Sin cambios pendientes", ForeColor = Color.DarkGreen };

    private List<Empleado> _usuarios = new();
    private UserPermissionSnapshot? _snapshotActual;
    private bool _cambiosPendientes;
    private bool _syncingChecks;

    public VentanaPermisosUsuario()
    {
        Text = "Permisos por Usuario (Mínimo)";
        Width = 980;
        Height = 620;
        StartPosition = FormStartPosition.CenterParent;
        KeyPreview = true;

        BuildLayout();
        WireEvents();
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await CargarUsuariosAsync();
    }

    private void BuildLayout()
    {
        UiConsistencia.AplicarFormularioBase(this);
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 3,
            Padding = new Padding(8)
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));

        _txtBuscar.Dock = DockStyle.Fill;
        UiConsistencia.AplicarInput(_txtBuscar);
        _lstUsuarios.Dock = DockStyle.Fill;
        _lstRol.Dock = DockStyle.Fill;
        _lstEfectivos.Dock = DockStyle.Fill;
        _chkAllow.Dock = DockStyle.Fill;
        _chkDeny.Dock = DockStyle.Fill;

        var splitLeft = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
        splitLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        splitLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        splitLeft.Controls.Add(Wrap("Permisos por rol", _lstRol), 0, 0);
        splitLeft.Controls.Add(Wrap("Permisos efectivos", _lstEfectivos), 0, 1);

        var splitRight = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
        splitRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        splitRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        splitRight.Controls.Add(Wrap("Allow directos", _chkAllow), 0, 0);
        splitRight.Controls.Add(Wrap("Deny directos", _chkDeny), 0, 1);

        var footer = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        UiConsistencia.AplicarBotonPrimario(_btnGuardar);
        UiConsistencia.AplicarBotonAccion(_btnCerrar);
        _btnGuardar.Width = 180;
        _btnCerrar.Width = 120;
        footer.Controls.Add(_btnGuardar);
        footer.Controls.Add(_btnCerrar);
        footer.Controls.Add(_lblUsuarioRol);
        footer.Controls.Add(_lblPendiente);
        footer.Controls.Add(_lblEstado);

        root.Controls.Add(_txtBuscar, 0, 0);
        root.SetColumnSpan(_txtBuscar, 3);
        root.Controls.Add(Wrap("Usuarios", _lstUsuarios), 0, 1);
        root.Controls.Add(splitLeft, 1, 1);
        root.Controls.Add(splitRight, 2, 1);
        root.Controls.Add(footer, 0, 2);
        root.SetColumnSpan(footer, 3);

        Controls.Add(root);

        foreach (var permission in AppPermissions.All.OrderBy(x => x))
        {
            _chkAllow.Items.Add(permission);
            _chkDeny.Items.Add(permission);
        }

        _lstRol.BackColor = Color.FromArgb(243, 246, 255);
        _chkAllow.BackColor = Color.FromArgb(236, 253, 245);
        _chkDeny.BackColor = Color.FromArgb(254, 242, 242);
        _lstEfectivos.BackColor = Color.FromArgb(239, 246, 255);

        AcceptButton = _btnGuardar;
        CancelButton = _btnCerrar;
    }

    private static Control Wrap(string title, Control child)
    {
        var panel = new GroupBox { Text = title, Dock = DockStyle.Fill };
        UiConsistencia.AplicarGrupo(panel);
        child.Dock = DockStyle.Fill;
        panel.Controls.Add(child);
        return panel;
    }

    private void WireEvents()
    {
        _txtBuscar.TextChanged += (_, _) => FiltrarUsuarios();
        _lstUsuarios.SelectedIndexChanged += async (_, _) => await CargarSnapshotSeleccionadoAsync();
        _btnGuardar.Click += async (_, _) => await GuardarOverridesAsync();
        _btnCerrar.Click += (_, _) => Close();
        _chkAllow.ItemCheck += (_, _) => MarkPendingChanges();
        _chkDeny.ItemCheck += (_, _) => MarkPendingChanges();
        KeyDown += VentanaPermisosUsuario_KeyDown;
    }

    private async Task CargarUsuariosAsync()
    {
        var all = await SecurityServices.EmployeeRepository.ListAsync();
        _usuarios = all.OrderBy(u => u.Nombre).ToList();
        FiltrarUsuarios();
        _txtBuscar.Focus();
    }

    private void FiltrarUsuarios()
    {
        var term = _txtBuscar.Text?.Trim() ?? string.Empty;
        var filtered = _usuarios
            .Where(u => string.IsNullOrWhiteSpace(term) || (u.Nombre?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();

        _lstUsuarios.DisplayMember = nameof(Empleado.Nombre);
        _lstUsuarios.ValueMember = nameof(Empleado.Id);
        _lstUsuarios.DataSource = filtered;
    }

    private async Task CargarSnapshotSeleccionadoAsync()
    {
        if (_lstUsuarios.SelectedItem is not Empleado empleado)
            return;

        _snapshotActual = await SecurityServices.UserPermissionService.GetSnapshotAsync(empleado.Id.ToString());
        if (_snapshotActual == null)
            return;

        _lstRol.Items.Clear();
        _lstEfectivos.Items.Clear();

        foreach (var p in _snapshotActual.RolePermissions.OrderBy(x => x)) _lstRol.Items.Add(p);
        foreach (var p in _snapshotActual.EffectivePermissions.OrderBy(x => x)) _lstEfectivos.Items.Add(p);

        _syncingChecks = true;
        try
        {
            SyncChecks(_chkAllow, _snapshotActual.AllowOverrides);
            SyncChecks(_chkDeny, _snapshotActual.DenyOverrides);
        }
        finally
        {
            _syncingChecks = false;
        }

        _lblUsuarioRol.Text = $"Usuario: {_snapshotActual.EmployeeName} | Rol: {(empleado.Puesto ?? "sin_rol")}";
        _lblEstado.Text = $"Snapshot cargado: {_snapshotActual.EffectivePermissions.Count} permisos efectivos";
        SetPendingChanges(false);
    }

    private static void SyncChecks(CheckedListBox list, IReadOnlyCollection<string> selected)
    {
        for (var i = 0; i < list.Items.Count; i++)
        {
            var key = list.Items[i]?.ToString() ?? string.Empty;
            list.SetItemChecked(i, selected.Contains(key, StringComparer.OrdinalIgnoreCase));
        }
    }

    private async Task GuardarOverridesAsync()
    {
        if (_snapshotActual == null)
        {
            MessageBox.Show("Seleccione un usuario.", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var allow = _chkAllow.CheckedItems.Cast<object>().Select(x => x.ToString() ?? string.Empty).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var deny = _chkDeny.CheckedItems.Cast<object>().Select(x => x.ToString() ?? string.Empty).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        var conflict = allow.Intersect(deny, StringComparer.OrdinalIgnoreCase).ToList();
        if (conflict.Count > 0)
        {
            MessageBox.Show($"No puede existir el mismo permiso en allow y deny: {string.Join(", ", conflict)}", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        await SecurityServices.UserPermissionService.SetOverridesAsync(_snapshotActual.EmployeeId, allow, deny);
        await CargarSnapshotSeleccionadoAsync();
        _lblEstado.Text = $"Overrides guardados correctamente ({DateTime.Now:HH:mm:ss})";
        SetPendingChanges(false);
        MessageBox.Show("Permisos guardados correctamente.", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void MarkPendingChanges()
    {
        if (_syncingChecks || _snapshotActual == null)
            return;

        SetPendingChanges(true);
    }

    private void SetPendingChanges(bool hasPendingChanges)
    {
        _cambiosPendientes = hasPendingChanges;
        _lblPendiente.Text = hasPendingChanges ? "Cambios pendientes (Ctrl+S para guardar)" : "Sin cambios pendientes";
        _lblPendiente.ForeColor = hasPendingChanges ? Color.DarkOrange : Color.DarkGreen;
    }

    private async void VentanaPermisosUsuario_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.S)
        {
            e.SuppressKeyPress = true;
            if (_cambiosPendientes)
            {
                await GuardarOverridesAsync();
            }
            return;
        }

        if (e.Control && e.KeyCode == Keys.W)
        {
            e.SuppressKeyPress = true;
            Close();
        }
    }
}
