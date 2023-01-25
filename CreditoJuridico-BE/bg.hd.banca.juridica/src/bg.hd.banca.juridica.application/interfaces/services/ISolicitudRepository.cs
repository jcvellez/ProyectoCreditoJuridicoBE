using bg.hd.banca.juridica.domain.entities.solicitud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.interfaces.services
{
    public interface ISolicitudRepository
    {
        Task<RegistrarSolicitudContactoResponse> RegistrarSolicitudContacto(RegistrarSolicitudContactoRequest request);
    }
}
