using AutoMapper;
using bg.hd.banca.juridica.application.interfaces.repositories;
using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.application.models.dtos;
using bg.hd.banca.juridica.application.models.exceptions;
using bg.hd.banca.juridica.domain.entities.persona;
using bg.hd.banca.juridica.domain.entities.solicitud;
using bg.hd.banca.juridica.infrastructure.utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.infrastructure.data.repositories
{
    public class SolicitudRestRepository: ISolicitudRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;
        private readonly IClienteRestRepository _clienteRestRepository;

        public SolicitudRestRepository(IAuthenticationServiceRepository Authentication, IConfiguration Configuration, IMapper Mapper, IClienteRestRepository clienteRestRepository)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
            _clienteRestRepository = clienteRestRepository;
        }

        public async Task<RegistrarSolicitudContactoResponse> RegistrarSolicitudContacto(RegistrarSolicitudContactoRequest request)
        {
            IdentificarClienteResponse identificarClienteResponse = new();
            RegistrarSolicitudContactoResponse solicitudResponse = new RegistrarSolicitudContactoResponse();
            int esCliente = 0;
            string razonSocial = "";
            if (request.numeroIdentificacion.Length == 10 || request.numeroIdentificacion.Length==13)
            {
                ClienteBGResponse clienteBGResponse = await _clienteRestRepository.IdentificarClienteBG(request.numeroIdentificacion, "R");
                esCliente = (clienteBGResponse==null) ?0: clienteBGResponse.id;
                ConsultaDatosRUCResponse consultaDatosRUCResponse = await _clienteRestRepository.ConsultaDatosRUC(request.numeroIdentificacion);
                //razonSocial = clienteBGResponse.nombres;                
                identificarClienteResponse.Mensaje = "PROCESO OK";
                identificarClienteResponse.RazonSocial = (identificarClienteResponse.RazonSocial == null) ? "" : consultaDatosRUCResponse.data[0].razonSocial;
            }                

            RegistrarSolicitudContactoRequestMicro requestMicro= new RegistrarSolicitudContactoRequestMicro();
            requestMicro.opcion = request.opcion;
            requestMicro.idSolicitudContacto = request.SolicitudContacto;
            requestMicro.numeroIdentificacion = request.numeroIdentificacion;
            requestMicro.razonSocial = identificarClienteResponse.RazonSocial;//request.razonSocial;
            requestMicro.nombreContacto = request.nombreContacto;
            requestMicro.cargoContacto = request.cargoContacto;
            requestMicro.telefonoContacto = request.telefonoContacto;
            requestMicro.correoContacto = request.correoContacto;
            requestMicro.viaContacto = request.viaContacto;
            requestMicro.idRangoVenta = request.RangoVenta;
            requestMicro.idProvincia = request.Provincia;
            requestMicro.idCiudad = request.Ciudad;
            requestMicro.oficial = request.Oficial;
            requestMicro.envioCorreo = request.envioCorreo;
            requestMicro.montoSolicitado = request.montoSolicitado.ToString();
            requestMicro.idFormaPago = Convert.ToInt32( string.IsNullOrEmpty(request.FormaPago) ?0: request.FormaPago );
            requestMicro.idDestino = Convert.ToInt32( string.IsNullOrEmpty(request.Destino) ? 0 : request.Destino);
            requestMicro.idPlazo = Convert.ToInt32( string.IsNullOrEmpty(request.Plazo) ? 0 : request.Plazo );
            requestMicro.idPeriodicidad = Convert.ToInt32( string.IsNullOrEmpty(request.Periodicidad) ? 0 : request.Periodicidad );
            requestMicro.idTipoAmortizacion = Convert.ToInt32( string.IsNullOrEmpty(request.TipoAmortizacion) ? 0 : request.TipoAmortizacion);
            requestMicro.idClteHost = esCliente;//id de host

            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/solicitudes/solicitudContacto";
            string auth = string.Format(_configuration["AzureAd:tokenName"]);
            HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), requestMicro);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                MsDtoResponse<RegistrarSolicitudContactoResponse> responseJson = JsonConvert.DeserializeObject<MsDtoResponse<RegistrarSolicitudContactoResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });                

                solicitudResponse.CodigoRetorno = responseJson.data.CodigoRetorno;
                solicitudResponse.Mensaje = responseJson.data.Mensaje;
                solicitudResponse.idSolicitudContacto = responseJson.data.datosConsulta.idSolicitudContacto;

                PrimitiveDataUtils.saveLogsInformation(uri, "", request, solicitudResponse);
            }
            else
            {
                if ((int)response.StatusCode == 400)
                {
                    if (responseBody.Contains("code") && responseBody.Contains("message"))
                    {
                        MsDtoResponseError responseJson1 = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.None
                        });
                        throw new ConsultarCatalogoException(responseJson1.errors[0].message.ToString(), responseJson1.errors[0].message.ToString() + "(" + responseJson1.errors[0].code.ToString() + ")", 3);
                    }
                }                
                else
                {
                    throw new SolicitudExeption("Error Api - ", "Error en registrar o actualizar contacto ", 1);
                }
            }                
            return solicitudResponse;
        }

        public async Task<ConsultarSolicitudContactoResponse> ConsutalSolicitudContacto(ConsultarSolicitudContactoRequest request)
        {
            ConsultarSolicitudContactoResponse solicitudResponse = new ConsultarSolicitudContactoResponse();


            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/solicitudes/solicitudContacto";
            string auth = string.Format(_configuration["AzureAd:tokenName"]);
            HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), request);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                MsDtoResponse<ConsultarSolicitudContactoResponse> responseJson = JsonConvert.DeserializeObject<MsDtoResponse<ConsultarSolicitudContactoResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                solicitudResponse = responseJson.data;

            }
            else
            {
                if ((int)response.StatusCode == 400)
                {
                    if (responseBody.Contains("code") && responseBody.Contains("message"))
                    {
                        MsDtoResponseError responseJson1 = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.None
                        });
                        throw new SolicitudExeption(responseJson1.errors[0].message.ToString(), responseJson1.errors[0].message.ToString() + "(" + responseJson1.errors[0].code.ToString() + ")", 3);
                    }
                }
                else
                {
                    throw new SolicitudExeption("Error Api - ", "Error en registrar o actualizar contacto ", 1);
                }
            }
            PrimitiveDataUtils.saveLogsInformation(uri, "", request, solicitudResponse);

            return solicitudResponse;
        }
    }
}
