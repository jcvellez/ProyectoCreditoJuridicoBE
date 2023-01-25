using bg.hd.banca.juridica.application.interfaces.repositories;
using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.application.models.exceptions;
using bg.hd.banca.juridica.domain.entities.solicitud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.services
{
    public class SolicitudRepository: ISolicitudRepository
    {
        private readonly ISolicitudRestRepository _solicitudRestRepository;

        public SolicitudRepository(ISolicitudRestRepository _solicitudRestRepository)
        {
            this._solicitudRestRepository = _solicitudRestRepository;
        }

        public async Task<RegistrarSolicitudContactoResponse> RegistrarSolicitudContacto(RegistrarSolicitudContactoRequest request)
        {
            if (request.viaContacto != "T" && request.viaContacto != "C" && request.viaContacto != null)
                throw new RegistrarSolicitudContactoException("Error de ingreso de datos", "viaContacto debe ser T(Telefono) o C(Correo)", 2);

            request.opcion=(request.SolicitudContacto == 0) ? 1 : 2;
            return await _solicitudRestRepository.RegistrarSolicitudContacto(request);
        }
    }
}
