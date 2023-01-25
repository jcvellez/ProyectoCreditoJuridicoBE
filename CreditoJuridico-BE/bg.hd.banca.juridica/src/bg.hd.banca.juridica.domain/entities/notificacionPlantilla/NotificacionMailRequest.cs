using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.notificacionPlantilla
{
    public class NotificacionMailRequest
    {
        public string De { get; set; } = string.Empty;
        public string Copia { get; set; } = string.Empty;
        public string Asunto { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string NombreAdjunto { get; set; } = string.Empty;
        public string Adjunto { get; set; } = string.Empty;
        public string Para { get; set; } = string.Empty;
    }
}
