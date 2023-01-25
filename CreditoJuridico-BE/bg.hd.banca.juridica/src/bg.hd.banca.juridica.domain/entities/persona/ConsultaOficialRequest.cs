using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.persona
{
    public class ConsultaOficialRequest
    {
        public string Identificacion { get; set; } = string.Empty;
        public int TipoIdentificacion { get; set; } = 0;
        public int Canal { get; set; } = 0;
    }
}
