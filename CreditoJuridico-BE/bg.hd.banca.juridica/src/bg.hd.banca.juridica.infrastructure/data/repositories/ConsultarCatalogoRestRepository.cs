using AutoMapper;
using bg.hd.banca.juridica.application.interfaces.repositories;
using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.application.models.dtos;
using bg.hd.banca.juridica.application.models.exceptions;
using bg.hd.banca.juridica.domain.entities;
using bg.hd.banca.juridica.domain.entities.catalogos;
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
    public class ConsultarCatalogoRestRepository: IConsultarCatalogoRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public ConsultarCatalogoRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }

        public async Task<ConsultarCatalogoResponse> ConsultarCatalogo(ConsultarCatalogoRequest request)
        {
            ConsultarCatalogoResponse consultarCatalogoResponse = new ConsultarCatalogoResponse();
            //request.producto = "cuotaMensual";
            Utilitarios utilitarios = PrimitiveDataUtils.obtenerCatalogosPermitidos(_configuration);


            int[] opcionesValidas = utilitarios.opcionesValidas;
            string[] catalogosPermitidos = utilitarios.catalogosPermitidos;
            

            if (opcionesValidas.Contains(request.opcion)) 
            {
                if (catalogosPermitidos.Contains(request.idCatalogo))
                {                    
                    ConsultarCatalogoRequestMicroServ requestMicro = new ConsultarCatalogoRequestMicroServ()
                    {
                        opcion = request.opcion,
                        idCatalogo = request.idCatalogo,
                        idCatalogoPadre = request.idCatalogoPadre,
                        Filtro = request.Filtro,
                        valorFiltro = request.valorFiltro
                    };


                    string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/catalogos/catalogo/detalle";
                    string auth = string.Format(_configuration["AzureAd:tokenName"]);
                    HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), requestMicro);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        MsDtoResponse<ConsultarCatalogoResponse> responseJson = JsonConvert.DeserializeObject<MsDtoResponse<ConsultarCatalogoResponse>>(responseBody, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.None
                        });
                        if (request.idCatalogo == "195")
                        {
                            List<CatalogoDetalle> nuevalista = new List<CatalogoDetalle>();
                            List<CatalogoDetalle> lista = responseJson.data.listaCatalogoDetalle.catalogoDetalle;
                            foreach (var item in lista)
                            {
                                if (item.strCodigoHost == "A" || item.strCodigoHost == "F")
                                {
                                    nuevalista.Add(item);
                                }
                            }
                            responseJson.data.listaCatalogoDetalle.catalogoDetalle = nuevalista;
                        }
                        else if (request.idCatalogo == "308" || request.idCatalogo == "306")
                        {
                            responseJson.data.listaCatalogoDetalle.catalogoDetalle = responseJson.data.listaCatalogoDetalle.catalogoDetalle.OrderBy(x => x.idCodigo).ToList();
                        }

                        consultarCatalogoResponse = responseJson.data;

                        PrimitiveDataUtils.saveLogsInformation(uri, "", request, consultarCatalogoResponse);
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
                        else if ((int)response.StatusCode == 401)
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
                            throw new ConsultarCatalogoException("Error Api - ", "Error Consulta de Catalogos", 1);
                        }
                    }
                }
                else
                {
                    throw new ConsultarCatalogoException("Catálogo no permitido para consulta", "Catálogo no permitido para consulta", 3);
                }
            }
            else
            {
                throw new ConsultarCatalogoException("Opción de consulta no es correcta", "Opción de consulta no es correcta", 2);
            }            
              
            return consultarCatalogoResponse;
        }
    }
}
