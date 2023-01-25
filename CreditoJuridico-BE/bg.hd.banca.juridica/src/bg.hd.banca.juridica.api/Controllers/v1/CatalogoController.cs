using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.application.models.dtos;
using bg.hd.banca.juridica.domain.entities.catalogos;
using Microsoft.AspNetCore.Mvc;

namespace bg.hd.banca.juridica.api.Controllers.v1
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class CatalogoController : BaseApiController
    {
        private readonly IConsultarCatalogoRepository _consultarcatalogoRepository;
        private readonly IConfiguration _configuration;
        //private string clienteToken = String.Empty;

        public CatalogoController(IConsultarCatalogoRepository _consultarcatalogoRepository, IConfiguration Configuration)
        {
            this._consultarcatalogoRepository = _consultarcatalogoRepository;
            this._configuration = Configuration;
        }
        [HttpPost]
        [Route("hdbancajuridica/v1/catalogo/consultar-catalogo")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<ConsultarCatalogoResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<ConsultarCatalogoResponse>>> ConsultarCatalogo([FromBody] ConsultarCatalogoRequest request)
        {
            ConsultarCatalogoResponse _response = await _consultarcatalogoRepository.ConsultarCatalogo(request);
            return new MsDtoResponse<ConsultarCatalogoResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower());
        }
    }
}
