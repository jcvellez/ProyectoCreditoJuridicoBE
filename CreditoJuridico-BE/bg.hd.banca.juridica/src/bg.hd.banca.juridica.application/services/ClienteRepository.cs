using bg.hd.banca.juridica.application.interfaces.repositories;
using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.application.models.exceptions;
using bg.hd.banca.juridica.domain.entities.catalogos;
using bg.hd.banca.juridica.domain.entities.email;
using bg.hd.banca.juridica.domain.entities.notificacionPlantilla;
using bg.hd.banca.juridica.domain.entities.persona;
using bg.hd.banca.juridica.domain.entities.solicitud;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace bg.hd.banca.juridica.application.services
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IClienteRestRepository clienteRestRepository;
        private readonly IConfiguration _configuration;
        private readonly IConsultarCatalogoRestRepository _consultarCatalogoRestRepository;
        private readonly ISolicitudRestRepository _solicitudRestRepository;

        public ClienteRepository(IConsultarCatalogoRestRepository consultarCatalogoRestRepository, IClienteRestRepository clienteRestRepository,
            IConfiguration Configuration, ISolicitudRestRepository _solicitudRestRepository)
        {
            this.clienteRestRepository = clienteRestRepository;
            this._configuration = Configuration;
            _consultarCatalogoRestRepository = consultarCatalogoRestRepository;
            this._solicitudRestRepository = _solicitudRestRepository;

        }

        public async Task<IdentificarClienteResponse> IdentificarCliente(string identificacion)
        {
            IdentificarClienteResponse identificarClienteResponse = new();

            ConsultaDatosRUCResponse consultaDatosRUCResponse = await clienteRestRepository.ConsultaDatosRUC(identificacion);

            List<string> Arreglo_codigoCIUU = new List<string>();
            var respuesta = consultaDatosRUCResponse.data
            .Where(elem => elem.estadoContribuyente.Trim().Equals("ACTIVO"))
                                    .Select(elem => elem).ToList();
            respuesta.ForEach(elem => Arreglo_codigoCIUU.Add(elem.codigoCIUU));

            if (respuesta.Count() == 0)
            {
                throw new ClienteExeption("Cliente no contiene actividad economica en estado activo ", "Cliente no contiene actividad economica en estado activo", 9);
            }

            identificarClienteResponse.Mensaje = "PROCESO OK";
            identificarClienteResponse.RazonSocial = consultaDatosRUCResponse.data[0].razonSocial;


            ClienteBGResponse clienteBGResponse = await clienteRestRepository.IdentificarClienteBG(identificacion, "R");
            identificarClienteResponse.EsCliente = clienteBGResponse != null ? true : false;

            return identificarClienteResponse;
        }
        public async Task<EmailValidacionResponse> ValidarEmail(EmailValidacionRequest request)
        {
            if (request.identificacion is null)
            {
                throw new ClienteExeption("Identificación no valida", "Identificación no valida", 2);
            }

            if (request.correo is null)
            {
                throw new ClienteExeption("Correo no valido", "Correo no valido", 5);
            }

            return await clienteRestRepository.ValidarEmail(request);


        }
        public async Task<EnviarNotificacionResponse> EnviarNotificacion(EnviarNotificacionRequest request)
        {
            NotificacionPlantillaResponse responsePlantilla = new();
            NotificacionMailResponse notificacionMailResponse = new();
            EnviarNotificacionResponse enviarNotificacionResponse = new();
            var MailOficial = string.Empty;
            var Dominio = _configuration["GeneralConfig:emailDominio"];
            #region Consulta la solicitud
            ConsultarSolicitudContactoRequest consultarSolicitudContactoRequest = new ConsultarSolicitudContactoRequest()
            {
                Opcion = 3,
                IdSolicitudContacto = request.IdSolicitud
            };

            ConsultarSolicitudContactoResponse consultarSolicitudContactoResponse = await _solicitudRestRepository.ConsutalSolicitudContacto(consultarSolicitudContactoRequest);
            #endregion

            ConsultaOficialRequest oficialRequest = new ConsultaOficialRequest()
            {
                Identificacion = consultarSolicitudContactoResponse.datosConsulta.numeroIdentificacion
            };

            ConsultaOficialResponse consultaOficialResponse = await clienteRestRepository.ConsultarDatosOficial(oficialRequest);

            if (consultaOficialResponse.CodigoRetorno != 0) throw new ClienteExeption("Errorr Aplicacion - API", "Consulta oficial", 2);

            ConsultarCatalogoRequest requestCatalogo = new ConsultarCatalogoRequest()
            {
                opcion = 4,
                idCatalogo = "346",
                idCatalogoPadre = "0",
                Filtro = "idCodigo",
                valorFiltro = consultarSolicitudContactoResponse.datosConsulta.idRangoVenta.ToString(),
            };

            ConsultarCatalogoResponse catalogo = _consultarCatalogoRestRepository.ConsultarCatalogo(requestCatalogo).GetAwaiter().GetResult();
            var Oficial = string.Empty;
            if (consultarSolicitudContactoResponse.datosConsulta.esCliente.ToUpper().Equals("S"))
            {
                Oficial = catalogo.listaCatalogoDetalle.catalogoDetalle[0].strValor3;
            }
            else
            {
                MailOficial = catalogo.listaCatalogoDetalle.catalogoDetalle[0].strValor4;
            }

            if (Oficial.ToUpper().Equals("OF")) MailOficial = string.IsNullOrEmpty(consultaOficialResponse.UsuarioOficial) ? consultaOficialResponse.UsuarioJefeAencia : consultaOficialResponse.UsuarioOficial;
            if (Oficial.ToUpper().Equals("JA")) MailOficial = consultaOficialResponse.UsuarioJefeAencia;


            NotificacionPlantillaRequest requestNotificacion = new()
            {
                IdCodigo = Convert.ToInt32(_configuration["GeneralConfig:notificacionPlantillaCodigo"]),
                TipoOperacion = _configuration["GeneralConfig:notificacionPlantillaTipoOperacion"]
            };

            responsePlantilla = await clienteRestRepository.ConsultarPlantilla(requestNotificacion);


            NotificacionMailRequest notificacionMailRequest = new()
            {
                De = responsePlantilla.datosNotificacion.remitente,
                Asunto = responsePlantilla.datosNotificacion.asunto,
                Mensaje = replacePlatillaNotificacion(responsePlantilla.datosNotificacion.mensaje, consultarSolicitudContactoResponse.datosConsulta),
                Copia = MailOficial + Dominio,
                Para = consultarSolicitudContactoResponse.datosConsulta.correoContacto
            };

            notificacionMailResponse = await clienteRestRepository.EnviarNotificacionMail(notificacionMailRequest);

            RegistrarSolicitudContactoRequest consultarSolicitudContactoActRequest = new RegistrarSolicitudContactoRequest()
            {
                opcion = 2,
                SolicitudContacto = request.IdSolicitud,
                envioCorreo = "S",
                Oficial = MailOficial,
                montoSolicitado = 0
            };

            RegistrarSolicitudContactoResponse consultarSolicitudContactoActResponse = await _solicitudRestRepository.RegistrarSolicitudContacto(consultarSolicitudContactoActRequest);

            if (consultarSolicitudContactoActResponse.CodigoRetorno != 0) throw new ClienteExeption("Errorr Aplicacion - API", "Actualiza Solicitud", 2);

            enviarNotificacionResponse.Mensaje = "PROCESO OK";
            enviarNotificacionResponse.CodigoRetorno = 0;

            return enviarNotificacionResponse;
        }

        public string replacePlatillaNotificacion(string mensaje, DetalleSolicitudContacto datos)
        {
            string[] subs = datos.nombreContacto.Split(' ');
            var Nombre = subs[0];
            var ViaContacto = datos.viaContacto.ToUpper().Equals("C") ? "Correo electrónico" : "Teléfono ";
            var RazonSocial = datos.razonSocial;
            var Telefono = datos.telefonoContacto;
            var Correo = datos.correoContacto;
            return mensaje.Replace("@@nombre", Nombre).Replace("@@viaContacto", ViaContacto)
                .Replace("@@razonSocial", RazonSocial).Replace("@@nombreCompleto", datos.nombreContacto)
                .Replace("@@telefono", Telefono).Replace("@@correo", Correo);
        }
    }
}
