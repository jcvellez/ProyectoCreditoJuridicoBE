using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.juridica.domain.entities.cotizador
{
    public class SimularCreditoResponse
    {
        public int codigoRetorno { get; set; } = 0;
        public string mensaje { get; set; } = "";
        [JsonProperty("dividendo")]
        public double cuota { get; set; } = 0;
        [JsonProperty("tasaNominal")]
        public double tasaInteres { get; set; } = 0;
        [JsonProperty("totalAPagar")]
        public double totalPagar { get; set; } = 0;
        public int plazo { get; set; } = 0;
        public List<Tabla> detalleTabla { get; set; } = new List<Tabla>();

    }

    public class Tabla
    {
        [JsonProperty("Periodos")]
        public string Periodo { get; set; } = "";
        public string Dias { get; set; } = "";
        public string Saldo { get; set; } = "";
        public string Capital { get; set; } = "";
        public string Interes { get; set; } = "";
        public string CapitalSeguro { get; set; } = "";
        public string InteresSeguro { get; set; } = "";
        [JsonProperty("dividendoFinal")]
        public string dividendo { get; set; } = "";
    }
}
