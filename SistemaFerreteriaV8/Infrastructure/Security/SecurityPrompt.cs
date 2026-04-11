using System.Windows.Forms;

namespace SistemaFerreteriaV8.Infrastructure.Security;

public static class SecurityPrompt
{
    public static string PromptPassword(string message, string title)
    {
        using var form = new Form
        {
            Width = 420,
            Height = 180,
            Text = title,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            StartPosition = FormStartPosition.CenterParent,
            MinimizeBox = false,
            MaximizeBox = false
        };

        var lbl = new Label { Left = 15, Top = 15, Width = 370, Text = message };
        var txt = new TextBox { Left = 15, Top = 45, Width = 370, UseSystemPasswordChar = true };
        var ok = new Button { Text = "Aceptar", Left = 220, Width = 80, Top = 85, DialogResult = DialogResult.OK };
        var cancel = new Button { Text = "Cancelar", Left = 305, Width = 80, Top = 85, DialogResult = DialogResult.Cancel };

        form.Controls.AddRange(new Control[] { lbl, txt, ok, cancel });
        form.AcceptButton = ok;
        form.CancelButton = cancel;

        return form.ShowDialog() == DialogResult.OK ? txt.Text?.Trim() ?? string.Empty : string.Empty;
    }
}
