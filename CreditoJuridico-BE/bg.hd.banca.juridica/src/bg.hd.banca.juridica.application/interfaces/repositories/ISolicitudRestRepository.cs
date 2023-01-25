using bg.hd.banca.juridica.domain.entities.solicitud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.interfaces.repositories
{
    public interface ISolicitudRestRepository
    {
        Task<RegistrarSolicitudContactoResponse> RegistrarSolicitudContacto(RegistrarSolicitudContactoRequest request);
        Task<ConsultarSolicitudContactoResponse> ConsutalSolicitudContacto(ConsultarSolicitudContactoRequest request);
    }
}
