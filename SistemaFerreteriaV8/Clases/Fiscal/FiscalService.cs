using System;
using System.Collections.Generic;

namespace SistemaFerreteriaV8.Clases
{
    public enum TipoComprobanteFiscal
    {
        Consumo,
        CreditoFiscal,
        Gubernamental,
        Desconocido
    }

    public sealed class FiscalService
    {
        private static bool TryParseDouble(string value, out double parsed)
            => double.TryParse(value, out parsed);

        public TipoComprobanteFiscal ResolverTipo(string tipoFactura)
        {
            return tipoFactura switch
            {
                "Consumo" => TipoComprobanteFiscal.Consumo,
                "Comprobante Fiscal" => TipoComprobanteFiscal.CreditoFiscal,
                "Comprobante Gubernamental" => TipoComprobanteFiscal.Gubernamental,
                _ => TipoComprobanteFiscal.Desconocido
            };
        }

        public bool ValidarRangos(Configuraciones config, out List<string> errores)
        {
            errores = new List<string>();
            if (config == null)
            {
                errores.Add("No hay configuración cargada.");
                return false;
            }

            ValidarSecuencia("Consumo", config.SCCI, config.SCCA, config.SCCF, errores);
            ValidarSecuencia("Crédito Fiscal", config.NFCInicial, config.NFCActual, config.NFCFinal, errores);
            ValidarSecuencia("Gubernamental", config.SGI, config.SGA, config.SGF, errores);

            return errores.Count == 0;
        }

        public bool TryAsignarNcf(Factura factura, Configuraciones config, out string prefijo, out string error)
        {
            prefijo = string.Empty;
            error = string.Empty;

            var tipo = ResolverTipo(factura?.TipoFactura);
            if (tipo == TipoComprobanteFiscal.Desconocido)
                return true;

            if (factura == null || config == null)
            {
                error = "No se pudo generar NCF porque faltan datos de factura o configuración.";
                return false;
            }

            string inicio;
            string actual;
            string fin;

            switch (tipo)
            {
                case TipoComprobanteFiscal.Consumo:
                    prefijo = "B02";
                    inicio = config.SCCI;
                    actual = config.SCCA;
                    fin = config.SCCF;
                    break;
                case TipoComprobanteFiscal.CreditoFiscal:
                    prefijo = "B01";
                    inicio = config.NFCInicial;
                    actual = config.NFCActual;
                    fin = config.NFCFinal;
                    break;
                case TipoComprobanteFiscal.Gubernamental:
                    prefijo = "B15";
                    inicio = config.SGI;
                    actual = config.SGA;
                    fin = config.SGF;
                    break;
                default:
                    return true;
            }

            if (!TryParseDouble(inicio, out var valorInicio) || !TryParseDouble(fin, out var valorFinal))
            {
                error = $"La secuencia fiscal para {factura.TipoFactura} no está configurada correctamente.";
                return false;
            }

            if (!TryParseDouble(actual, out var valorActual))
                valorActual = valorInicio;

            if (valorActual > valorFinal)
            {
                error = $"Ya alcanzó la secuencia máxima para {factura.TipoFactura}.";
                return false;
            }

            var siguiente = (valorActual + 1).ToString("0").PadLeft(8, '0');
            factura.NFC = siguiente;

            switch (tipo)
            {
                case TipoComprobanteFiscal.Consumo:
                    config.SCCA = siguiente;
                    break;
                case TipoComprobanteFiscal.CreditoFiscal:
                    config.NFCActual = siguiente;
                    break;
                case TipoComprobanteFiscal.Gubernamental:
                    config.SGA = siguiente;
                    break;
            }

            return true;
        }

        private static void ValidarSecuencia(string nombre, string inicio, string actual, string fin, List<string> errores)
        {
            if (!TryParseDouble(inicio, out var valorInicio))
            {
                errores.Add($"{nombre}: inicio inválido.");
                return;
            }

            if (!TryParseDouble(fin, out var valorFin))
            {
                errores.Add($"{nombre}: final inválido.");
                return;
            }

            if (valorFin < valorInicio)
                errores.Add($"{nombre}: final no puede ser menor que inicio.");

            if (!string.IsNullOrWhiteSpace(actual) && TryParseDouble(actual, out var valorActual))
            {
                if (valorActual < valorInicio || valorActual > valorFin)
                    errores.Add($"{nombre}: actual está fuera del rango [{valorInicio:0}..{valorFin:0}].");
            }
        }
    }
}
