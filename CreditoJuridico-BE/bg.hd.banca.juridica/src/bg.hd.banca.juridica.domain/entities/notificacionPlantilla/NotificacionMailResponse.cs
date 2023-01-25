using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.notificacionPlantilla
{
    public class NotificacionMailResponse
    {
        public int codigoRetorno { get; set; } = 0;
        public string mensaje { get; set; } = string.Empty;
    }
}
