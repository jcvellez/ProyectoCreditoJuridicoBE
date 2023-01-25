using bg.hd.banca.juridica.application.models.ords;
using bg.hd.banca.juridica.domain.entities.email;
using bg.hd.banca.juridica.domain.entities.persona;

namespace bg.hd.banca.juridica.application.interfaces.services
{
    public interface IClienteRepository
    {
        Task<EmailValidacionResponse> ValidarEmail(EmailValidacionRequest request);
        Task<IdentificarClienteResponse> IdentificarCliente(string identificacion);
        Task<EnviarNotificacionResponse> EnviarNotificacion(EnviarNotificacionRequest request);

    }
}
