using bg.hd.banca.juridica.application.interfaces.services;
using bg.hd.banca.juridica.application.services;
using Microsoft.Extensions.DependencyInjection;

namespace bg.hd.banca.juridica.application.ioc
{
    public static class DependencyInyection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IConsultarCatalogoRepository, ConsultarCatalogoRepository>();
            services.AddScoped<IAuthenticationServiceRepository, AuthenticationServiceRepository>();
            services.AddScoped<IRecaptchaRepository, RecaptchaRepository>();
            services.AddScoped<ICotizadorRepository, CotizadorRepository>();
            services.AddScoped<ISolicitudRepository, SolicitudRepository>();
            return services;
        }
    }
}
