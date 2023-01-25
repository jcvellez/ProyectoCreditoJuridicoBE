using bg.hd.banca.juridica.application.interfaces.repositories;
using bg.hd.banca.juridica.infrastructure.data.repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System.Reflection;


namespace bg.hd.banca.juridica.infrastructure.ioc
{
    public static class DependencyInyection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            // Se agregan Logs ELK
            Log.Logger = new LoggerConfiguration()
                  .ReadFrom
                  .Configuration(configuration).CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            // Se agrega Telemetria
            _ = int.TryParse(configuration["Jaeger:Telemetry:Port"], out int portNumber);

            services.AddOpenTelemetryTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                .AddSource(configuration["Serilog:Properties:Application"])
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName: configuration["Serilog:Properties:Application"]))
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.Enrich = (activity, eventName, rawObject) =>
                    {

                        string? traceid = string.Empty;

                        if (rawObject is HttpRequest httpRequest)
                        {
                            traceid = httpRequest.HttpContext?.TraceIdentifier;
                            activity.SetTag("Log-Traceid", traceid);
                        }

                    };
                })
                .AddJaegerExporter(opts =>
                {
                    opts.AgentHost = configuration["Jaeger:Telemetry:Host"];
                    opts.AgentPort = portNumber;
                });
                

            });


            // Se agrega Librerias de Mapeos
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Se agregan los servicios       
            services.AddScoped<IAuthenticationRestRepository, AuthenticationRestRepository>();
            services.AddScoped<IClienteRestRepository, ClienteRestRepository>();
            services.AddScoped<IConsultarCatalogoRestRepository, ConsultarCatalogoRestRepository>();
            services.AddScoped<ICotizadorRestRepository, CotizadorRestRepository>();
            services.AddScoped<ISolicitudRestRepository, SolicitudRestRepository>();
            services.AddScoped<IRecaptchaRestRepository, RecaptchaRestRepository>();
            return services;
        }
    }
}