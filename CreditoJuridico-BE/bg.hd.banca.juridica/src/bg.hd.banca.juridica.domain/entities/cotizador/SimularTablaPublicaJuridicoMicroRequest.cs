using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.cotizador
{
    public class SimularTablaPublicaJuridicoMicroRequest
    {
        public decimal precio { get; set; } = 0;
        public int montoSolicitado { get; set; } = 0;
        public string idSegmento { get; set; }
        public string idPeriodicidad { get; set; }       
        public int idPlazo { get; set; }
        public string tipoTablaAmortizacion { get; set; }
        public int tasaEfectiva { get; set; } = 0;
        public int tasaNominal { get; set; } = 0;
        public int dividendo { get; set; } = 0;
        public int totalAPagar { get; set; } = 0;
    }
}
