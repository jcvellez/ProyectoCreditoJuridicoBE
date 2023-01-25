using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.cotizador
{
    public class ConsultaTasaInteresResponse
    {
        public int? codigoRetorno { get; set; }
        public string? mensaje { get; set; }
        public double? tasaNominal { get; set; }
        public double? tasaEfectiva { get; set; }
        public double? factorReajuste { get; set; }
    }
}
