using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.application.models.dtos;
using bg.hd.banca.juridica.domain.entities.cotizador;
using bg.hd.banca.juridica.domain.entities.email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace bg.hd.banca.juridica.api.Controllers.v1
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class CotizadorController : BaseApiController
    {
        private readonly ICotizadorRepository _cotizadorRepository;

        public CotizadorController(ICotizadorRepository _cotizadorRepository)
        {
            this._cotizadorRepository = _cotizadorRepository;
        }

        /// <summary>
        /// Simula Credito Juridico
        /// </summary>
        [HttpPost]
        [Route("hdbancajuridica/v1/cotizador/simular-credito")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<SimularCreditoResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<SimularCreditoResponse>>> SimularCredito([FromBody][Required] SimularCreditoRequest request)
        {
            SimularCreditoResponse _response = await _cotizadorRepository.SimularCredito(request);
            return Ok(new MsDtoResponse<SimularCreditoResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));

        }


    }
}
