using bg.hd.banca.juridica.application.interfaces.repositories;
using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.application.models.exceptions;
using bg.hd.banca.juridica.domain.entities;
using bg.hd.banca.juridica.domain.entities.catalogos;
using bg.hd.banca.juridica.domain.entities.cotizador;
//using bg.hd.banca.juridica.infrastructure.utils;
using Microsoft.Extensions.Configuration;

namespace bg.hd.banca.juridica.application.services
{
    public class CotizadorRepository : ICotizadorRepository
    {
        private readonly ICotizadorRestRepository _cotizadorRestRepository;
        private readonly IConsultarCatalogoRestRepository _consultarCatalogoRestRepository;
        private readonly IConfiguration _configuration;

        public CotizadorRepository(ICotizadorRestRepository _cotizadorRestRepository, IConsultarCatalogoRestRepository consultarCatalogoRestRepository,
            IConfiguration Configuration)
        {
            this._cotizadorRestRepository = _cotizadorRestRepository;
            _consultarCatalogoRestRepository = consultarCatalogoRestRepository;
            this._configuration = Configuration;

        }

        public async Task<SimularCreditoResponse> SimularCredito(SimularCreditoRequest request)
        {
            ConsultarCatalogoResponse _catalogo = new();
            SimularCreditoResponse response = new();
            var TipoProducto = string.Empty;

            ConsultarCatalogoRequest requestTipoProducto = new ConsultarCatalogoRequest()
            {
                opcion = 4,
                idCatalogo = "305",
                idCatalogoPadre = "0",
                Filtro = "idCodigo",
                valorFiltro = request.tipoProducto
            };

            _catalogo = _consultarCatalogoRestRepository.ConsultarCatalogo(requestTipoProducto).GetAwaiter().GetResult();
            if (_catalogo.listaCatalogoDetalle != null)
            {
                TipoProducto = _catalogo.listaCatalogoDetalle.catalogoDetalle[0].strValor2;
            }

            if (TipoProducto.Equals("AD"))
            {
                var idSegmento = string.Empty;
                var idPeriosidad = string.Empty;
                var TipoAmortizacion = string.Empty;


                ConsultarCatalogoRequest requestCatalogo = new ConsultarCatalogoRequest()
                {
                    opcion = 4,
                    idCatalogo = "346",
                    idCatalogoPadre = "0",
                    Filtro = "idCodigo",
                    valorFiltro = request.RangoVentas
                };

                ConsultarCatalogoRequest requestCatalogoPeriocidad = new ConsultarCatalogoRequest()
                {
                    opcion = 4,
                    idCatalogo = "345",
                    idCatalogoPadre = "0",
                    Filtro = "idCodigo",
                    valorFiltro = request.PeriodoCuota
                };

                ConsultarCatalogoRequest requestCatalogoAmortizacion = new ConsultarCatalogoRequest()
                {
                    opcion = 4,
                    idCatalogo = "195",
                    idCatalogoPadre = "0",
                    Filtro = "idCodigo",
                    valorFiltro = request.TipoAmortizacion
                };

                _catalogo = _consultarCatalogoRestRepository.ConsultarCatalogo(requestCatalogo).GetAwaiter().GetResult();
                if(_catalogo.listaCatalogoDetalle != null)
                {
                    idSegmento = _catalogo.listaCatalogoDetalle.catalogoDetalle[0].strValor2;
                }
                _catalogo = _consultarCatalogoRestRepository.ConsultarCatalogo(requestCatalogoPeriocidad).GetAwaiter().GetResult();
                if (_catalogo.listaCatalogoDetalle != null)
                {
                    idPeriosidad = _catalogo.listaCatalogoDetalle.catalogoDetalle[0].strValor2;
                }

                _catalogo = _consultarCatalogoRestRepository.ConsultarCatalogo(requestCatalogoAmortizacion).GetAwaiter().GetResult();
                if (_catalogo.listaCatalogoDetalle != null)
                {
                    TipoAmortizacion = _catalogo.listaCatalogoDetalle.catalogoDetalle[0].strCodigoHost;
                }

                SimularTablaPublicaJuridicoMicroRequest requestMicro = new SimularTablaPublicaJuridicoMicroRequest()
                {
                    montoSolicitado = request.monto,
                    idSegmento = idSegmento,
                    idPeriodicidad= idPeriosidad,
                    idPlazo = request.plazo,
                    tipoTablaAmortizacion = TipoAmortizacion
                }
                ;
                response =  await _cotizadorRestRepository.SimularCredito(requestMicro);
            }
            else if (TipoProducto.Equals("ME"))
            {
                ConsultaTasaInteresRequest TasaInteresRequest = new ConsultaTasaInteresRequest()
                {
                    idCanal = Convert.ToInt32(_configuration["GeneralConfig:idCanal"]),
                    Producto = TipoProducto,
                    PeriodicidadDias = Convert.ToInt32(_configuration["GeneralConfig:PeriodicidadDias"])
                };

                ConsultaTasaInteresResponse tasaInteresResponse = await _cotizadorRestRepository.ConsultaTasaInteres(TasaInteresRequest);
                double tasa = (double)tasaInteresResponse.tasaNominal;
                response.plazo = request.plazo;
                response.codigoRetorno = 0;
                response.mensaje = "Ok-Proceso";
                int? capital_inicial = request.monto;
                double total = Convert.ToDouble(capital_inicial) + Convert.ToDouble(capital_inicial * request.plazo * tasa) / 36000;
                total = Math.Round(total, 2);
                response.totalPagar = total;
                response.tasaInteres = tasa;
                if (response.tasaInteres == 0)
                {
                    throw new CotizadorExeption("Tasa de interes no puede ser 0", "Tasa de interes no puede ser 0", 2);
                }
                response.cuota = 0;

            }

            return response;
        }
    }
}
