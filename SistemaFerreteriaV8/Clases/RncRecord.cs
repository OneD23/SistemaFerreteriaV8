using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

class RncRecord
{
    public string RNC { get; set; }
    public string Nombre { get; set; }
    public string ActividadEconomica { get; set; }
    public string FechaInicio { get; set; }
    public string Estado { get; set; }
    public string TipoContribuyente { get; set; }

    public static RncRecord FromLine(string line)
    {
        var parts = line.Split('|');
        return new RncRecord
        {
            RNC = parts[0],
            Nombre = parts[1],
            ActividadEconomica = parts[3],
            FechaInicio = parts.Length > 8 ? parts[8] : "",
            Estado = parts.Length > 9 ? parts[9] : "",
            TipoContribuyente = parts.Length > 10 ? parts[10] : ""
        };
    }

    public override string ToString()
    {
        return $"RNC: {RNC}, Nombre: {Nombre}, Actividad: {ActividadEconomica}, Fecha Inicio: {FechaInicio}, Estado: {Estado}, Tipo: {TipoContribuyente}";
    }
}

class RncSearcher
{
    private const string Url = "https://www.dgii.gov.do/app/WebApps/Consultas/RNC/DGII_RNC.zip";
    private const string ZipPath = "DGII_RNC.zip";
    private const string ExtractFolder = "DGII_RNC_Extracted";
    private const string TxtFileName = "DGII_RNC.TXT";

    public static async Task DownloadAndExtractAsync(
        ProgressBar progressBar,
        Label statusLabel,
        int maxRetries = 3)
    {
        void SafeUpdate(Action action)
        {
            if (statusLabel.InvokeRequired)
                statusLabel.Invoke(action);
            else
                action();
        }

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                SafeUpdate(() => statusLabel.Text = $"Descargando (intento {attempt})...");

                using var http = new HttpClient();
                using var response = await http.GetAsync(Url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                var canReport = totalBytes > 0;

                using var stream = await response.Content.ReadAsStreamAsync();
                using var fileStream = new FileStream(ZipPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

                var buffer = new byte[8192];
                long totalRead = 0;
                int read;
                while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, read);
                    totalRead += read;
                    if (canReport)
                    {
                        int percent = (int)(totalRead * 100 / totalBytes);
                        SafeUpdate(() =>
                        {
                            progressBar.Value = percent;
                            statusLabel.Text = $"Descargando... {percent}%";
                        });
                    }
                }
            }
            catch (HttpRequestException) when (attempt < maxRetries)
            {
                // Fallback a WebClient si falla streaming
                SafeUpdate(() => statusLabel.Text = $"Streaming falló, reintentando con WebClient ({attempt})...");
                try
                {
                    using var wc = new WebClient();
                    wc.DownloadProgressChanged += (s, e) =>
                    {
                        SafeUpdate(() =>
                        {
                            progressBar.Value = e.ProgressPercentage;
                            statusLabel.Text = $"Descargando (WebClient)... {e.ProgressPercentage}%";
                        });
                    };
                    await wc.DownloadFileTaskAsync(Url, ZipPath);
                }
                catch
                {
                    await Task.Delay(500);
                    continue;
                }
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                SafeUpdate(() => statusLabel.Text = $"Error ({ex.Message}), reintentando ({attempt})...");
                await Task.Delay(500);
                continue;
            }

            // Extracción tras descarga exitosa
            SafeUpdate(() => statusLabel.Text = "Extrayendo archivos...");
            if (!Directory.Exists(ExtractFolder))
                Directory.CreateDirectory(ExtractFolder);

            // Usamos el overload que permite overwrite (disponible desde .NET 6)
            ZipFile.ExtractToDirectory(ZipPath, ExtractFolder, overwriteFiles: true);

            SafeUpdate(() => statusLabel.Text = "Archivos extraídos correctamente.");
            return;
        }

        // Falló todos los intentos
        SafeUpdate(() => MessageBox.Show(
            $"No se pudo descargar el archivo tras {maxRetries} intentos.",
            "Error de descarga",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error));
    }

    public static RncRecord SearchRNC(string rnc)
    {
        var txtPath = Directory
            .GetFiles(ExtractFolder, TxtFileName, SearchOption.AllDirectories)
            .FirstOrDefault();
        if (txtPath == null)
            throw new FileNotFoundException("No se encontró DGII_RNC.TXT tras la extracción.");

        var line = File.ReadLines(txtPath)
                       .FirstOrDefault(l => l.StartsWith(rnc));
        return line != null
            ? RncRecord.FromLine(line)
            : null;
    }
}
