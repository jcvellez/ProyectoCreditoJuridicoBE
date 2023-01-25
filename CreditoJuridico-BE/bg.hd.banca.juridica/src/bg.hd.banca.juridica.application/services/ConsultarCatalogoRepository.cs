using bg.hd.banca.juridica.application.interfaces.repositories;
using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.domain.entities.catalogos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.services
{
    public class ConsultarCatalogoRepository: IConsultarCatalogoRepository
    {
        private readonly IConsultarCatalogoRestRepository _consultarCatalogoRestRepository;
        private readonly IConfiguration _configuration;
        public ConsultarCatalogoRepository(IConfiguration Configuration,IConsultarCatalogoRestRepository consultarCatalogoRestRepository)
        {
            _consultarCatalogoRestRepository = consultarCatalogoRestRepository;
            _configuration = Configuration;
        }

        public async Task<ConsultarCatalogoResponse> ConsultarCatalogo(ConsultarCatalogoRequest request)
        {
            ConsultarCatalogoResponse _response = new();            
            if (request.opcion == 1 && request.idCatalogo == "307" && request.idCatalogoPadre == "0")
            {                
                request.valorFiltro = "0";
            }
            if (request.opcion == 1 && request.idCatalogo == "1" && request.idCatalogoPadre == "0")
            {
                request.idCatalogo ="307";
                request.valorFiltro = "0";
            }
            if ((request.opcion == 1 && request.idCatalogo == "1" && request.idCatalogoPadre == "12176") || (request.opcion == 1 && request.idCatalogo == "1" && request.idCatalogoPadre == "12177"))
            {
                request.idCatalogo = "307";
                request.valorFiltro = request.idCatalogoPadre;
                request.idCatalogoPadre = "0";
            }

            _response = await _consultarCatalogoRestRepository.ConsultarCatalogo(request);

            if (request.opcion == 1 && request.idCatalogo == "307" && request.idCatalogoPadre == "0")
            {
                foreach (var item in _response.listaCatalogoDetalle.catalogoDetalle)
                {
                    if (item.idCodigo == int.Parse(request.valorFiltro))
                    {
                        List<CatalogoDetalle> lista = new List<CatalogoDetalle>();                                                
                        for (int i = 1; i <= int.Parse(item.strValor2); i++)
                        {
                            CatalogoDetalle catalogo = new CatalogoDetalle();
                            catalogo.idCodigo = i;
                            catalogo.strValor = i.ToString() + " años";
                            lista.Add(catalogo);
                        }
                        _response.listaCatalogoDetalle.catalogoDetalle = lista;
                    }
                }
                
            }
            return _response;
        }
    }
}
