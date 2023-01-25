using bg.hd.banca.juridica.api.Controllers;
using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.application.models.dtos;
using bg.hd.banca.juridica.domain.entities.recaptcha;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;


namespace bg.hd.juridica.pyme.api.Controllers.v1
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class Seguridad : BaseApiController
    {
        private readonly IRecaptchaRepository _recaptchaRepository;

        public Seguridad(IRecaptchaRepository _recaptchaRepository)
        {
            this._recaptchaRepository = _recaptchaRepository;
        }
        /// <summary>
        /// Valida Recaptcha
        /// </summary>
        [HttpPost]
        [Route("hdbancajuridica/v1/seguridad/validacion-recaptcha")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MsDtoResponse<RecaptchaResponse>), 200)]
        [ProducesResponseType(typeof(MsDtoResponseError), 400)]
        [ProducesResponseType(typeof(MsDtoResponseError), 500)]
        public async Task<ActionResult<MsDtoResponse<RecaptchaResponse>>> validarRecaptcha([FromBody][Required] RecaptchaRequest request)
        {
            RecaptchaResponse response = await _recaptchaRepository.validarRecaptcha(request);
            return Ok(new MsDtoResponse<RecaptchaResponse>(response, HttpContext?.TraceIdentifier.Split(":")[0].ToLower()));
        }


    }
}
