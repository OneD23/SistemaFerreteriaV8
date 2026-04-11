using System.Drawing;
using System.Windows.Forms;

namespace SistemaFerreteriaV8.Clases;

internal static class UiConsistencia
{
    public static void AplicarFormularioBase(Form form)
    {
        form.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        form.Padding = new Padding(Math.Max(form.Padding.Left, 8), Math.Max(form.Padding.Top, 8), Math.Max(form.Padding.Right, 8), Math.Max(form.Padding.Bottom, 8));
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
        label.ForeColor = error ? Color.Maroon : Color.DarkGreen;
        label.Visible = true;
    }
}
