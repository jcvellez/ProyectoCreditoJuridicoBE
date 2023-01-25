
using bg.hd.banca.juridica.domain.entities.cotizador;

namespace bg.hd.banca.juridica.application.interfaces.repositories
{
    public interface ICotizadorRestRepository
    {
        Task<SimularCreditoResponse> SimularCredito(SimularTablaPublicaJuridicoMicroRequest request);
        Task<ConsultaTasaInteresResponse> ConsultaTasaInteres(ConsultaTasaInteresRequest request);

    }
}
