using System.Drawing;
using System.Windows.Forms;

namespace SistemaFerreteriaV8.Clases
{
    public static class ThemeManager
    {
        public static void ApplyToForm(Form form)
        {
            var config = new Configuraciones().ObtenerPorId(1);
            var background = ParseColor(config?.ColorFondo, Color.FromArgb(21, 34, 56));
            var panel = ParseColor(config?.ColorPanel, Color.FromArgb(36, 52, 77));
            var primary = ParseColor(config?.ColorPrimario, Color.FromArgb(255, 137, 0));
            var text = Color.FromArgb(236, 240, 245);

            form.BackColor = background;
            form.ForeColor = text;
            ApplyRecursive(form, panel, primary, text);
        }

        private static void ApplyRecursive(Control parent, Color panel, Color primary, Color text)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is GroupBox groupBox)
                {
                    groupBox.BackColor = panel;
                    groupBox.ForeColor = text;
                }
                else if (control is Label label)
                {
                    label.ForeColor = text;
                    label.BackColor = Color.Transparent;
                    label.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
                }
                else if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = Color.FromArgb(243, 244, 246);
                    textBox.ForeColor = Color.FromArgb(15, 23, 42);
                    textBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.FlatStyle = FlatStyle.Flat;
                    comboBox.BackColor = Color.FromArgb(243, 244, 246);
                    comboBox.ForeColor = Color.FromArgb(15, 23, 42);
                    comboBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                }
                else if (control is Button button)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.BackColor = primary;
                    button.ForeColor = Color.White;
                    button.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
                }
                else if (control is DataGridView grid)
                {
                    grid.EnableHeadersVisualStyles = false;
                    grid.BackgroundColor = Color.FromArgb(58, 76, 107);
                    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(226, 232, 240);
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 41, 59);
                    grid.DefaultCellStyle.BackColor = Color.White;
                    grid.DefaultCellStyle.ForeColor = Color.FromArgb(15, 23, 42);
                    grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(191, 219, 254);
                    grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
                    grid.GridColor = Color.FromArgb(148, 163, 184);
                }
                else if (control is Panel panelControl)
                {
                    panelControl.BackColor = panel;
                }

                if (control.HasChildren)
                {
                    ApplyRecursive(control, panel, primary, text);
                }
            }
        }

        private static Color ParseColor(string hex, Color fallback)
        {
            if (string.IsNullOrWhiteSpace(hex))
                return fallback;

            try
            {
                return ColorTranslator.FromHtml(hex);
            }
            catch
            {
                return fallback;
            }
        }
    }
}
