using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.solicitud
{
    public class ConsultarSolicitudContactoResponse
    {
        public int codigoRetorno { get; set; }
        public string mensaje { get; set; }
        public DetalleSolicitudContacto datosConsulta { get; set; }

    }

}
