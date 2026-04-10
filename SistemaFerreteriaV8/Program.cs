using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;
using SistemaFerreteriaV8.Clases;

namespace SistemaFerreteriaV8
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            WinFormsApp.EnableVisualStyles();
            WinFormsApp.SetCompatibleTextRenderingDefault(false);

            var settings = AppInstanceSettings.Load();
            if (!settings.IsConfigured)
            {
                if (!RunFirstTimeSetup(settings))
                    return;

                settings.IsConfigured = true;
                settings.Save();
            }

            OneKeys.ApplySettings(settings);
            EnsureInitialConfigurationDocument(settings);
            WinFormsApp.Run(new Form1());
        }

        private static bool RunFirstTimeSetup(AppInstanceSettings settings)
        {
            MessageBox.Show(
                "Bienvenido. Esta es la primera ejecución.\n\nVamos a configurar Empresa y conexión MongoDB.",
                "Configuración inicial",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            var company = Prompt("Nombre de la empresa:", "Configuración inicial", "Mi Empresa");
            if (string.IsNullOrWhiteSpace(company))
            {
                MessageBox.Show("Debe indicar el nombre de la empresa para continuar.", "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var isPrimaryResult = MessageBox.Show(
                "¿Esta PC será la PRINCIPAL (servidor MongoDB)?\n\nSí = Principal\nNo = Secundaria",
                "Modo de instalación",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (isPrimaryResult == DialogResult.Cancel)
                return false;

            var localIps = string.Join(", ", GetLocalIPv4());
            string hostOrIp;
            string nodeRole;

            if (isPrimaryResult == DialogResult.Yes)
            {
                nodeRole = "Primary";
                hostOrIp = Prompt(
                    "Host/IP donde corre MongoDB en esta PC (ej: localhost o 192.168.1.20):\n\nIPs detectadas: " + localIps,
                    "Servidor principal",
                    "localhost");
            }
            else
            {
                nodeRole = "Secondary";
                hostOrIp = Prompt(
                    "IP/Host de la PC PRINCIPAL donde corre MongoDB (ej: 192.168.1.20):",
                    "Servidor secundario",
                    "");
            }

            if (string.IsNullOrWhiteSpace(hostOrIp))
            {
                MessageBox.Show("Debe indicar una IP/host válida.", "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var mongoUri = $"mongodb://{hostOrIp.Trim()}:27017/";
            var databaseName = OneKeys.BuildDatabaseName(company);

            if (!TryValidateMongoConnection(mongoUri))
            {
                var continuar = MessageBox.Show(
                    $"No fue posible validar conexión a MongoDB en:\n{mongoUri}\n\n¿Desea guardar esta configuración de todas formas?",
                    "Conexión MongoDB",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (continuar != DialogResult.Yes)
                    return false;
            }

            settings.CompanyName = company.Trim();
            settings.DatabaseName = databaseName;
            settings.MongoUri = mongoUri;
            settings.NodeRole = nodeRole;

            MessageBox.Show(
                $"Configuración guardada.\n\nEmpresa: {settings.CompanyName}\nBase de datos: {settings.DatabaseName}\nMongoDB: {settings.MongoUri}\nRol: {settings.NodeRole}",
                "Configuración completada",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            if (settings.NodeRole == "Primary")
            {
                MessageBox.Show(
                    $"Comparte esta dirección con las PCs secundarias:\n\n{settings.MongoUri}",
                    "Dirección para clientes secundarios",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            return true;
        }

        private static bool TryValidateMongoConnection(string uri)
        {
            try
            {
                var client = new MongoClient(uri);
                client.ListDatabaseNames().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static IEnumerable<string> GetLocalIPv4()
        {
            try
            {
                return Dns.GetHostEntry(Dns.GetHostName())
                    .AddressList
                    .Where(a => a.AddressFamily == AddressFamily.InterNetwork)
                    .Select(a => a.ToString())
                    .Distinct()
                    .ToList();
            }
            catch
            {
                return new[] { "127.0.0.1" };
            }
        }

        private static void EnsureInitialConfigurationDocument(AppInstanceSettings settings)
        {
            try
            {
                var client = new MongoClient(settings.MongoUri);
                var db = client.GetDatabase(new OneKeys().DatabaseName);
                var col = db.GetCollection<Configuraciones>("configuraciones");

                var exists = col.Find(c => c.Id == 1).Any();
                if (exists) return;

                var config = new Configuraciones
                {
                    Id = 1,
                    Nombre = settings.CompanyName,
                    Direccion = "",
                    Telefono = "",
                    Correo = "",
                    RNC = "",
                    Precio = 0,
                    Seleccion = "0",
                    Impresora = ""
                };
                col.InsertOne(config);
            }
            catch
            {
                // Si falla en este punto, la app seguirá y el usuario puede completar configuración después.
            }
        }

        private static string Prompt(string text, string caption, string defaultValue)
        {
            using var form = new Form()
            {
                Width = 560,
                Height = 180,
                Text = caption,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MinimizeBox = false,
                MaximizeBox = false
            };

            var label = new Label() { Left = 15, Top = 15, Width = 510, Text = text };
            var box = new TextBox() { Left = 15, Top = 45, Width = 510, Text = defaultValue ?? string.Empty };
            var ok = new Button() { Text = "Aceptar", Left = 360, Width = 80, Top = 80, DialogResult = DialogResult.OK };
            var cancel = new Button() { Text = "Cancelar", Left = 445, Width = 80, Top = 80, DialogResult = DialogResult.Cancel };

            form.Controls.AddRange(new Control[] { label, box, ok, cancel });
            form.AcceptButton = ok;
            form.CancelButton = cancel;

            return form.ShowDialog() == DialogResult.OK ? box.Text?.Trim() ?? string.Empty : string.Empty;
        }
    }
}
