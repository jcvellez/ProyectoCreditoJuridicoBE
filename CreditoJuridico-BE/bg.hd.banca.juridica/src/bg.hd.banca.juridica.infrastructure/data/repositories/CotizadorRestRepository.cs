using AutoMapper;
using bg.hd.banca.juridica.application.interfaces.repositories;
using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.application.models.dtos;
using bg.hd.banca.juridica.application.models.exceptions;
using bg.hd.banca.juridica.application.models.ms;
using bg.hd.banca.juridica.domain.entities;
using bg.hd.banca.juridica.domain.entities.cotizador;
using bg.hd.banca.juridica.domain.entities.email;
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
    public class CotizadorRestRepository : ICotizadorRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public CotizadorRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;

        }

        public async Task<SimularCreditoResponse> SimularCredito(SimularTablaPublicaJuridicoMicroRequest request)
        {
            SimularCreditoResponse simularCreditoResponse = new();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroCreditos:url"]) + "v1/tabla-amortizacion-publica/simular-tabla-juridica";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                MsResponse<SimularCreditoResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<SimularCreditoResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                simularCreditoResponse = _mapper.Map<SimularCreditoResponse>(responseJson.data);
                if (simularCreditoResponse.detalleTabla.Count >= 2)
                {
                    simularCreditoResponse.plazo = simularCreditoResponse.detalleTabla.Count - 2;
                }
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

            PrimitiveDataUtils.saveLogsInformation(uri, "", request, simularCreditoResponse);

            return simularCreditoResponse;

        }

        public async Task<ConsultaTasaInteresResponse> ConsultaTasaInteres(ConsultaTasaInteresRequest request)
        {
            Producto producto = PrimitiveDataUtils.obtenerProducto(request.Producto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new CotizadorExeption("Producto ingresado no es correcto", "Producto ingresado no es correcto", 2);
            }
            request.idProducto = Convert.ToInt32(producto.idProducto);

            ConsultaTasaInteresResponse tasainteresResponse = new ConsultaTasaInteresResponse();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = new HttpResponseMessage();
            string uri = string.Format(_configuration["InfraConfig:MicroCreditos:url"]) + "v2/tasa";
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            response = await client.PostAsync(uri, httpContent);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                MsResponse<ConsultaTasaInteresResponse> responseJson = JsonConvert.DeserializeObject<MsResponse<ConsultaTasaInteresResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });

                tasainteresResponse = _mapper.Map<ConsultaTasaInteresResponse>(responseJson.data);
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

            PrimitiveDataUtils.saveLogsInformation(uri, "", request, tasainteresResponse);
            return tasainteresResponse;
        }
    }
}
