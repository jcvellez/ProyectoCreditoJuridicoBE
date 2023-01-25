using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.application.models.dtos;
using bg.hd.banca.juridica.domain.entities.solicitud;
using Microsoft.AspNetCore.Mvc;

namespace bg.hd.banca.juridica.api.Controllers.v1
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class SolicitudController : BaseApiController
    {
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly IConfiguration _configuration;        

        public SolicitudController(ISolicitudRepository _solicitudRepository, IConfiguration Configuration)
        {
            this._solicitudRepository = _solicitudRepository;
            this._configuration = Configuration;
        }

        [HttpPost]
        [Route("hdbancajuridica/v1/solicitud/solicitudContacto")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<RegistrarSolicitudContactoResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<RegistrarSolicitudContactoResponse>>> ConsultarCatalogo([FromBody] RegistrarSolicitudContactoRequest request)
        {
            RegistrarSolicitudContactoResponse _response = await _solicitudRepository.RegistrarSolicitudContacto(request);
            return new MsDtoResponse<RegistrarSolicitudContactoResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower());
        }
    }
}
