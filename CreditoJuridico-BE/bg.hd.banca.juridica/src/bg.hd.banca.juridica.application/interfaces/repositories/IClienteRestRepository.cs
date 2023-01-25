using bg.hd.banca.juridica.domain.entities.email;
using bg.hd.banca.juridica.domain.entities.notificacionPlantilla;
using bg.hd.banca.juridica.domain.entities.persona;

namespace bg.hd.banca.juridica.application.interfaces.repositories
{
    public interface IClienteRestRepository
    {
        Task<EmailValidacionResponse> ValidarEmail(EmailValidacionRequest request);
        Task<ConsultaDatosRUCResponse> ConsultaDatosRUC(string identificacion);
        Task<ClienteBGResponse> IdentificarClienteBG(string identificacion, string tipo);
        Task<NotificacionPlantillaResponse> ConsultarPlantilla(NotificacionPlantillaRequest request);
        Task<NotificacionMailResponse> EnviarNotificacionMail(NotificacionMailRequest request);
        Task<ConsultaOficialResponse> ConsultarDatosOficial(ConsultaOficialRequest request);

    }
}
