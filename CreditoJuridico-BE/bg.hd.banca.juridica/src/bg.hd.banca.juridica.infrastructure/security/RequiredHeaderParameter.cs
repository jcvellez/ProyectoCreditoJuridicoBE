using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Security.Claims;

namespace bg.hd.banca.juridica.infrastructure.security
{
    public class RequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor)
                if (!context.ApiDescription.CustomAttributes().Any((a) => a is AllowAnonymousAttribute)
            && (context.ApiDescription.CustomAttributes().Any((a) => a is AuthorizeAttribute)
                || descriptor.ControllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>() != null))
                {
                    if (operation.Parameters == null)
                        operation.Parameters = new List<OpenApiParameter>();

                    operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                    operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
                    operation.Security = new List<OpenApiSecurityRequirement>
                    {
                        new OpenApiSecurityRequirement()
                            {
                                 {
                                    new OpenApiSecurityScheme
                                    {
                                        Scheme = "bearer",
                                        BearerFormat = "JWT",
                                        Name = "JWT Authentication",
                                        In = ParameterLocation.Header,
                                        Type = SecuritySchemeType.Http,
                                        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Put **_ONLY_** your JWT Bearer **_token_** on textbox below! \r\n\r\n\r\n Example: \"Value: **12345abcdef**\"",

                                        Reference = new OpenApiReference
                                        {
                                            Id = JwtBearerDefaults.AuthenticationScheme,
                                            Type = ReferenceType.SecurityScheme
                                        }
                                    },
                                    new List<string>()
                                }
                        }
                    };
                }

        }
    }
}
