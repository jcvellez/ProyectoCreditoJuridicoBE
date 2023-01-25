using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.application.models.dtos;
using bg.hd.banca.juridica.domain.entities.email;
using bg.hd.banca.juridica.domain.entities.persona;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace bg.hd.banca.juridica.api.Controllers.v1
{
    //[Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ClienteController : BaseApiController
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteController(IClienteRepository _clienteRepository)
        {
            this._clienteRepository = _clienteRepository;
        }
        /// <summary>
        /// Identifica Cliente Juridico
        /// </summary>
        /// <param name="identificacion"> Identificación de persona.</param>
        [HttpGet]
        [Route("hdbancajuridica/v1/cliente/identificar-cliente")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<IdentificarClienteResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<List<ConsultaRUCDNI>>>> ConsultaGeneralRucDNI([FromHeader][Required] string identificacion)
        {
            IdentificarClienteResponse response = await _clienteRepository.IdentificarCliente(identificacion);
            return Ok(new MsDtoResponse<IdentificarClienteResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }


        /// <summary>
        /// Valida correo electronico correcto
        /// </summary>
        [HttpPost]
        [Route("hdbancajuridica/v1/email/validacion")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<EmailValidacionResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<EmailValidacionResponse>>> ValidarEmail([FromBody][Required] EmailValidacionRequest request)
        {
            EmailValidacionResponse _response = await _clienteRepository.ValidarEmail(request);
            return Ok(new MsDtoResponse<EmailValidacionResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));

        }
        /// <summary>
        /// Envia notificacion a cliente
        /// </summary>
        [HttpPost]
        [Route("hdbancajuridica/v1/cliente/notificacion")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<EnviarNotificacionResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<EnviarNotificacionResponse>>> EnviarNotificacion([FromBody][Required] EnviarNotificacionRequest request)
        {
            EnviarNotificacionResponse _response = await _clienteRepository.EnviarNotificacion(request);
            return Ok(new MsDtoResponse<EnviarNotificacionResponse>(_response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));

        }
    }
}
