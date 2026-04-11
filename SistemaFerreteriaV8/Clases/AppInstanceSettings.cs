using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SistemaFerreteriaV8.Clases
{
    public class AppInstanceSettings
    {
        public bool IsConfigured { get; set; }
        public string CompanyName { get; set; } = "";
        public string DatabaseName { get; set; } = "";
        public string MongoUri { get; set; } = "mongodb://localhost:27017/";
        public string NodeRole { get; set; } = "Primary";

        public static string SettingsFilePath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "SistemaFerreteriaV8",
            "appsettings.ini");

        public static AppInstanceSettings Load()
        {
            var settings = new AppInstanceSettings();

            if (!File.Exists(SettingsFilePath))
                return settings;

            var lines = File.ReadAllLines(SettingsFilePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                    continue;

                var idx = line.IndexOf('=');
                if (idx <= 0) continue;

                var key = line.Substring(0, idx).Trim();
                var value = line.Substring(idx + 1).Trim();

                switch (key)
                {
                    case "is_configured":
                        settings.IsConfigured = value.Equals("true", StringComparison.OrdinalIgnoreCase);
                        break;
                    case "company_name":
                        settings.CompanyName = value;
                        break;
                    case "database_name":
                        settings.DatabaseName = value;
                        break;
                    case "mongo_uri":
                        settings.MongoUri = value;
                        break;
                    case "mongo_uri_enc":
                        settings.MongoUri = Decrypt(value);
                        break;
                    case "node_role":
                        settings.NodeRole = value;
                        break;
                }
            }

            return settings;
        }

        public void Save()
        {
            var dir = Path.GetDirectoryName(SettingsFilePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var lines = new List<string>
            {
                $"is_configured={(IsConfigured ? "true" : "false")}",
                $"company_name={CompanyName}",
                $"database_name={DatabaseName}",
                $"mongo_uri_enc={Encrypt(MongoUri)}",
                $"node_role={NodeRole}"
            };

            File.WriteAllLines(SettingsFilePath, lines);
        }

        private static string Encrypt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
                return string.Empty;

            var bytes = Encoding.UTF8.GetBytes(plainText);
            var encrypted = ProtectedData.Protect(bytes, null, DataProtectionScope.LocalMachine);
            return Convert.ToBase64String(encrypted);
        }

        private static string Decrypt(string cipherText)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
                return "mongodb://localhost:27017/";

            try
            {
                var encrypted = Convert.FromBase64String(cipherText);
                var bytes = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.LocalMachine);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (CryptographicException)
            {
                return "mongodb://localhost:27017/";
            }
            catch (FormatException)
            {
                return "mongodb://localhost:27017/";
            }
        }
    }
}
