using bg.hd.banca.juridica.domain.entities.catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.interfaces.services
{
    public interface IConsultarCatalogoRepository
    {
        Task<ConsultarCatalogoResponse> ConsultarCatalogo(ConsultarCatalogoRequest request);
    }
}
