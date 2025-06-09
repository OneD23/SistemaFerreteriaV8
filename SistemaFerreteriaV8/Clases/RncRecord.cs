using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.ComponentModel;
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

    public static void DownloadAndExtract(ProgressBar progressBar, Label statusLabel)
    {
        string txtFilePath = Path.Combine(ExtractFolder, TxtFileName);

        if (File.Exists(txtFilePath))
        {
            statusLabel.Text = "El archivo ya existe, omitiendo descarga y extracción.";
            return;
        }

        using (var client = new WebClient())
        {
            client.DownloadProgressChanged += (s, e) =>
            {
                progressBar.Value = e.ProgressPercentage;
                statusLabel.Text = $"Descargando... {e.ProgressPercentage}%";
            };

            client.DownloadFileTaskAsync(new Uri(Url), ZipPath).Wait();
        }

        if (!Directory.Exists(ExtractFolder))
            Directory.CreateDirectory(ExtractFolder);

        statusLabel.Text = "Extrayendo archivos...";
        ZipFile.ExtractToDirectory(ZipPath, ExtractFolder);

        string[] files = Directory.GetFiles(ExtractFolder, "*", SearchOption.AllDirectories);
        if (files.Length == 0)
            throw new Exception("No se extrajeron archivos. Verifique la descarga.");

        statusLabel.Text = "Archivos extraídos correctamente.";
    }

    public static RncRecord SearchRNC(string rnc)
    {
        string txtFilePath = Directory.GetFiles(ExtractFolder, TxtFileName, SearchOption.AllDirectories).FirstOrDefault();
        if (txtFilePath == null)
            throw new FileNotFoundException("No se encontró el archivo DGII_RNC.TXT después de la extracción.");

        string line = File.ReadLines(txtFilePath)
                          .FirstOrDefault(l => l.StartsWith(rnc));

        return line != null ? RncRecord.FromLine(line) : null;
    }
}
