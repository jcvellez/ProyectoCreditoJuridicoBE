using bg.hd.banca.juridica.domain.entities.catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.interfaces.repositories
{
    public interface IConsultarCatalogoRestRepository
    {
        Task<ConsultarCatalogoResponse> ConsultarCatalogo(ConsultarCatalogoRequest request);
    }
}
