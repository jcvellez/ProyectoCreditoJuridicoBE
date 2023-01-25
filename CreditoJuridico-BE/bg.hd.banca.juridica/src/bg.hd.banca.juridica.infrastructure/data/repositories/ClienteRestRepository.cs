using AutoMapper;
using bg.hd.banca.juridica.application.interfaces.repositories;
using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.application.models.dtos;
using bg.hd.banca.juridica.application.models.exceptions;
using bg.hd.banca.juridica.application.models.ms;
using bg.hd.banca.juridica.domain.entities.email;
using bg.hd.banca.juridica.domain.entities.notificacionPlantilla;
using bg.hd.banca.juridica.domain.entities.persona;
using bg.hd.banca.juridica.infrastructure.utils;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.infrastructure.data.repositories
{
    public class ClienteRestRepository : IClienteRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public ClienteRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;

        }

        public async Task<ConsultaDatosRUCResponse> ConsultaDatosRUC(string identificacion)
        {
            ConsultaDatosRUCResponse generarResponse = new ConsultaDatosRUCResponse();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("identificacion", identificacion);

            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroPersonas:url"]) + "v1/ruc";
            response = await client.GetAsync(uri);
            string responseBody = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {

                ConsultaDatosRUCResponse responseJson = JsonConvert.DeserializeObject<ConsultaDatosRUCResponse>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                generarResponse = _mapper.Map<ConsultaDatosRUCResponse>(responseJson);
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
                        throw new ClienteExeption(responseJson1.errors[0].message.ToString(), responseJson1.errors[0].message.ToString() + "(" + responseJson1.errors[0].code.ToString() + ")", 3);
                    }
                }
                else
                {
                    throw new ClienteExeption("Error Api - ", "Error Consulta Datos Ruc", 1);
                }
            }

            return generarResponse;
        }

        public async Task<EmailValidacionResponse> ValidarEmail(EmailValidacionRequest request)
        {
            EmailValidacionResponse emailValidacionResponse = null;

            if (!PrimitiveDataUtils.ValidarEstructuraEmail(request.correo))
            {
                throw new ClienteExeption("Correo no valido", "Correo no valido", 1);
            }

            EmailValidacionRequest requestMicroEmail = new EmailValidacionRequest()
            {
                codigoAppBg = _configuration["GeneralConfig:codigoAppBg"],
                identificacion = request.identificacion,
                correo = request.correo,

            };


            var clientMicroMail = new HttpClient();
            clientMicroMail.DefaultRequestHeaders.Accept.Clear();
            clientMicroMail.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var responseMicro = new HttpResponseMessage();
            string uri =_configuration["InfraConfig:MicroAutotizaciones:urlService"] + "v1/email/validacion";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestMicroEmail), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            clientMicroMail.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());
            responseMicro = await clientMicroMail.PostAsync(uri, httpContent);
            string response = await responseMicro.Content.ReadAsStringAsync();


            if (responseMicro.IsSuccessStatusCode)
            {
                MsResponse<EmailValidacionResponse>  responseJson = JsonConvert.DeserializeObject<MsResponse<EmailValidacionResponse>>(response, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });                

                emailValidacionResponse = _mapper.Map<EmailValidacionResponse>(responseJson.data);
                emailValidacionResponse.CodigoRetorno = 0;
                emailValidacionResponse.Mensaje = emailValidacionResponse.descripcionValidacion;

                if (!emailValidacionResponse.estadoValidacion.Equals("V"))
                {
                    throw new ClienteExeption(emailValidacionResponse.descripcionValidacion, emailValidacionResponse.descripcionValidacion.ToString(), 1);
                }

            }
            else
            {
                throw new ClienteExeption(responseMicro.ReasonPhrase, responseMicro.RequestMessage.ToString(), 1);
            }

            PrimitiveDataUtils.saveLogsInformation(uri, request.identificacion, request, emailValidacionResponse);

            return emailValidacionResponse;

        }

        public async Task<ClienteBGResponse> IdentificarClienteBG(string identificacion, string tipo)
        {

            ClienteBGResponse clienteBGResponse = null;
            var query = new Dictionary<string, string>()
            {
                ["identificacion"] = identificacion,
                ["tipoIdentificacion"] = tipo
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroClientes:urlServiceCliente"]) + "v1/informacion";
            response = await client.GetAsync(QueryHelpers.AddQueryString(uri, query));
            string responseBody = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {
                MsResponse<ClienteBGResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ClienteBGResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                clienteBGResponse = _mapper.Map<ClienteBGResponse>(responseJson.data);

            }
            else
            {
                if ((int)response.StatusCode == 500)
                {
                    throw new ClienteExeption("Error Api - ", "Error Consulta Cliente Ruc", 1);

                }
              
            }

            return clienteBGResponse;
        }
        public async Task<NotificacionPlantillaResponse> ConsultarPlantilla(NotificacionPlantillaRequest request) 
        {
            NotificacionPlantillaResponse responsePlantilla = new();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/catalogos/notificacionesPlantilla";
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                MsResponse<NotificacionPlantillaResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<NotificacionPlantillaResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                responsePlantilla = _mapper.Map<NotificacionPlantillaResponse>(responseJson.data);
            }
            else
            {
                if ((int)response.StatusCode == 400)
                {
                    if (responseBody.Contains("code") && responseBody.Contains("message"))
                    {
                        MsDtoResponseError responseJsonMicro = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.None
                        });
                        string mensajeError; int cont = 0; int code = 2;
                        if (responseJsonMicro.errors.Count == 1)
                        {
                            code = responseJsonMicro.errors[0].code;
                            mensajeError = responseJsonMicro.errors[0].message;
                        }
                        else
                        {
                            mensajeError = "";
                            foreach (MsDtoError error in responseJsonMicro.errors)
                            {
                                cont += 1;
                                code = error.code;
                                mensajeError += $"Error {code}: {error.message} \n";
                            }
                        }

                        throw new CotizadorExeption(mensajeError, mensajeError, code);

                    }
                }
                else
                {
                    throw new CotizadorExeption(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, "", request, responsePlantilla);
            return responsePlantilla;    
        }
        public async Task<NotificacionMailResponse> EnviarNotificacionMail(NotificacionMailRequest request)
        {
            NotificacionMailResponse responseMail = new();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = new HttpResponseMessage();
            string uri = _configuration["InfraConfig:MicroNotificaciones:urlService"] + "v1/email/address";
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                MsResponse<NotificacionMailResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<NotificacionMailResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                responseMail = _mapper.Map<NotificacionMailResponse>(responseJson.data);
                responseMail.mensaje = "PROCESO OK";
            }
            else
            {
                if ((int)response.StatusCode == 400)
                {
                    if (responseBody.Contains("code") && responseBody.Contains("message"))
                    {
                        MsDtoResponseError responseJsonMicro = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.None
                        });
                        string mensajeError; int cont = 0; int code = 2;
                        if (responseJsonMicro.errors.Count == 1)
                        {
                            code = responseJsonMicro.errors[0].code;
                            mensajeError = responseJsonMicro.errors[0].message;
                        }
                        else
                        {
                            mensajeError = "";
                            foreach (MsDtoError error in responseJsonMicro.errors)
                            {
                                cont += 1;
                                code = error.code;
                                mensajeError += $"Error {code}: {error.message} \n";
                            }
                        }

                        throw new CotizadorExeption(mensajeError, mensajeError, code);

                    }
                }
                else
                {
                    throw new CotizadorExeption(response.ReasonPhrase, response.RequestMessage.ToString(), 1);
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, "", request, responseMail);
            return responseMail;
        }
        public async Task<ConsultaOficialResponse> ConsultarDatosOficial(ConsultaOficialRequest parameter)
        {
            ConsultaOficialResponse responseConsulta = new ConsultaOficialResponse();
            bool generarException = false;
            parameter.TipoIdentificacion = Int32.Parse(string.Format(_configuration["GeneralConfig:tipoIdentificacion"]));
            responseConsulta.CodigoRetorno = 1;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("identificacion", parameter.Identificacion);
            client.DefaultRequestHeaders.Add("tipoIdentificacion", parameter.TipoIdentificacion.ToString());
            client.DefaultRequestHeaders.Add(string.Format(_configuration["AzureAd:tokenName"]), await _authentication.GetAccessToken());

            var response = new HttpResponseMessage();


            string uri = string.Format(_configuration["InfraConfig:MicroCompositeNeo:urlService"]) + "v2/personas/persona/oficial";
            response = await client.GetAsync(uri);


            string responseBody = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {
                MsResponse<ConsultaOficialResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultaOficialResponse>>(responseBody, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                responseConsulta = _mapper.Map<ConsultaOficialResponse>((ConsultaOficialResponse)responseJson.data);
                responseConsulta.Mensaje = "PROCESO OK";
                responseConsulta.CodigoRetorno = 0;
            }
            else
            {
                generarException = true;
                if (responseBody.Contains("code") && responseBody.Contains("message"))
                {

                    MsDtoResponseError responseJsonError = JsonConvert.DeserializeObject<MsDtoResponseError>(responseBody, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.None
                    });
                    responseConsulta.Mensaje = responseJsonError.errors[0].message.ToString() + "(" + responseJsonError.errors[0].code.ToString() + ")";

                }
                else
                {
                    responseConsulta.Mensaje = response.RequestMessage.ToString();
                }
            }

            PrimitiveDataUtils.saveLogsInformation(uri, parameter.Identificacion, parameter, responseConsulta);




            return responseConsulta;
        }
    }
}
