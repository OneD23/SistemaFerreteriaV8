using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SistemaFerreteriaV8.Clases
{   
    public  class OneKeys
    {
        public Color Naranja = Color.FromArgb(240, 111, 49);
        public Color Gris = Color.FromArgb(48, 48, 45);
        public Color Negro = Color.FromArgb(255, 255, 255);
        public Color Rojo = Color.FromArgb(255, 0, 0);
        public Color Azul = Color.FromArgb(0, 0, 255);
        public Color Verde = Color.FromArgb(0, 255, 0);

        private static string _uri = "mongodb://localhost:27017/";
        private static string _databaseName = "ferreteria_default";

        public string URI => _uri;
        public string DatabaseName => _databaseName;

        public static void ApplySettings(AppInstanceSettings settings)
        {
            if (settings == null) return;

            if (!string.IsNullOrWhiteSpace(settings.MongoUri))
                _uri = settings.MongoUri.Trim();

            if (!string.IsNullOrWhiteSpace(settings.DatabaseName))
                _databaseName = BuildDatabaseName(settings.DatabaseName);
        }

        public static string BuildDatabaseName(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName))
                return "ferreteria_default";

            var normalized = companyName.Trim().ToLowerInvariant();
            normalized = Regex.Replace(normalized, @"[^a-z0-9]+", "_");
            normalized = Regex.Replace(normalized, @"_+", "_").Trim('_');
            return string.IsNullOrWhiteSpace(normalized) ? "ferreteria_default" : normalized;
        }
    }
}
