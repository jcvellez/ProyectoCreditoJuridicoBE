using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using bg.hd.banca.juridica.application.models.exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace bg.hd.banca.juridica.infrastructure.security
{
    public static class TokenValidationHandler
    {
        public static IServiceCollection SetupAuthenticationServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Audience = builder.Configuration["AzureAdAudience"];
                    options.Authority = builder.Configuration["AzureAdInstance"] + builder.Configuration["AzureAdTenantId"];
                    options.TokenValidationParameters.ValidAudiences = new string?[] { options.Audience, $"api://{options.Audience}" };

                    options.TokenValidationParameters.ValidateIssuer = true;
                    options.TokenValidationParameters.ValidateAudience = true;
                    options.TokenValidationParameters.ValidateLifetime = true;
                    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                    options.Events = new JwtBearerEvents();
                });
            return services;
        }
    }
}
