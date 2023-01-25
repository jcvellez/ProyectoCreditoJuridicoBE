using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.persona
{
    public class ConsultaRUCDNI
    {
        public string? numero { get; set; }
        public string? identificacion { get; set; }
        public string? razonSocial { get; set; }
        public string? nombreComercial { get; set; }
        public string? estadoContribuyente { get; set; }
        public string? claseContribuyente { get; set; }
        public string? fechaInicioActividades { get; set; }
        public string? fechaReinicioActividades { get; set; }
        public string? tipoContribuyente { get; set; }
        public string? nombreFantasiaComercial { get; set; }
        public string? actividadEconomica { get; set; }
        public DireccionRUCDNI? direccion { get; set; }
        public string? codigoCIUU { get; set; }
        public string? codigoCIUUSB { get; set; }
    }
}
