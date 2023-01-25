using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.notificacionPlantilla
{
    public class NotificacionPlantillaResponse
    {
        public int codigoRetorno { get; set; } = 0;
        public string mensaje { get; set; } = string.Empty;
        public DetalleNotificacion datosNotificacion { get; set; }
    }

    public class DetalleNotificacion
    {
        public string idCodigo { get; set; }
        public string nombrePlantilla { get; set; }
        public string remitente { get; set; }
        public string asunto { get; set; }
        public string mensaje { get; set; }
        public string archivosAdjuntos { get; set; }
        public string tipoMensaje { get; set; }
        public string rutaAdjuntos { get; set; }
    }
}
