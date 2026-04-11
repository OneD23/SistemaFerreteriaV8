using System.Drawing;
using System.Windows.Forms;

namespace SistemaFerreteriaV8.Clases;

internal static class UiConsistencia
{
    public static readonly Color FondoPrincipal = Color.FromArgb(15, 23, 42);
    public static readonly Color PanelClaro = Color.FromArgb(241, 245, 249);
    public static readonly Color Superficie = Color.White;
    public static readonly Color TextoPrincipal = Color.FromArgb(15, 23, 42);
    public static readonly Color BordeSuave = Color.FromArgb(203, 213, 225);
    public static readonly Color EstadoExito = Color.DarkGreen;
    public static readonly Color EstadoAdvertencia = Color.DarkGoldenrod;
    public static readonly Color EstadoError = Color.Maroon;

    public static void AplicarFormularioBase(Form form)
    {
        form.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        form.Padding = new Padding(Math.Max(form.Padding.Left, 8), Math.Max(form.Padding.Top, 8), Math.Max(form.Padding.Right, 8), Math.Max(form.Padding.Bottom, 8));
        form.BackColor = FondoPrincipal;
        form.ForeColor = Color.White;
    }

    public static void AplicarBotonPrimario(Button button) => AplicarBoton(button, Color.FromArgb(14, 116, 144));
    public static void AplicarBotonAccion(Button button) => AplicarBoton(button, Color.FromArgb(59, 130, 246));
    public static void AplicarBotonExito(Button button) => AplicarBoton(button, Color.FromArgb(16, 185, 129));
    public static void AplicarBotonPeligro(Button button) => AplicarBoton(button, Color.FromArgb(220, 38, 38));

    public static void AplicarBoton(Button button, Color backColor)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.BackColor = backColor;
        button.ForeColor = Color.White;
        button.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        button.Height = Math.Max(34, button.Height);
        button.Width = Math.Max(120, button.Width);
    }

    public static void AplicarInput(TextBox textBox)
    {
        textBox.BorderStyle = BorderStyle.FixedSingle;
        textBox.BackColor = Color.White;
        textBox.ForeColor = TextoPrincipal;
    }

    public static void AplicarInput(ComboBox comboBox)
    {
        comboBox.BackColor = Color.White;
        comboBox.ForeColor = TextoPrincipal;
    }

    public static void AplicarPanelContenedor(Control control)
    {
        control.BackColor = PanelClaro;
        control.ForeColor = TextoPrincipal;
    }

    public static void AplicarGrupo(GroupBox groupBox)
    {
        groupBox.BackColor = PanelClaro;
        groupBox.ForeColor = TextoPrincipal;
        groupBox.Padding = new Padding(8);
    }

    public static void AplicarGrid(DataGridView grid)
    {
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        grid.DefaultCellStyle.BackColor = Superficie;
        grid.DefaultCellStyle.ForeColor = TextoPrincipal;
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(191, 219, 254);
        grid.DefaultCellStyle.SelectionForeColor = TextoPrincipal;
        grid.GridColor = BordeSuave;
        grid.RowTemplate.Height = Math.Max(28, grid.RowTemplate.Height);
        grid.BackgroundColor = Superficie;
    }

    public static void AplicarStatusLabel(Label label, int top, int left = 12)
    {
        label.Left = left;
        label.Top = top;
        label.AutoSize = true;
        label.Visible = false;
    }

    public static void MostrarEstado(Label label, string message, bool error)
    {
        label.Text = message;
        label.ForeColor = error ? EstadoError : EstadoExito;
        label.BackColor = Color.Transparent;
        label.Visible = true;
    }

    public static void MostrarAdvertencia(Label label, string message)
    {
        label.Text = message;
        label.ForeColor = EstadoAdvertencia;
        label.BackColor = Color.Transparent;
        label.Visible = true;
    }
}
