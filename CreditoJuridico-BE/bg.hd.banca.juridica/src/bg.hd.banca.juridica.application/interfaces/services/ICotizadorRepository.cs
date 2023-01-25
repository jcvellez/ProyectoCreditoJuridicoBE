using bg.hd.banca.juridica.domain.entities.cotizador;

namespace bg.hd.banca.juridica.application.interfaces.services
{
    public interface ICotizadorRepository
    {
        Task<SimularCreditoResponse> SimularCredito(SimularCreditoRequest request);

    }
}
