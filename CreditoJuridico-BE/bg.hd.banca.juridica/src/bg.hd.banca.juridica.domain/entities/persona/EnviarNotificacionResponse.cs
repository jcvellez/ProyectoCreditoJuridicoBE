using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.persona
{
    public class EnviarNotificacionResponse
    {
        public int CodigoRetorno { get; set; } = 0;
        public string Mensaje { get; set; } = string.Empty;
    }
}
