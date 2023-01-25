using bg.hd.banca.juridica.application.models.dtos;
using bg.hd.banca.juridica.application.models.exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Prometheus;
using Serilog;
using System.Text.Json;

namespace bg.hd.banca.juridica.infrastructure.extentions
{
    public static class ApplicationExtentions
    {

        public static IApplicationBuilder ConfigureMetricServer(this IApplicationBuilder app)
        {
            app.UseMetricServer();
            app.UseHttpMetrics();

            return app;
        }

        public static IApplicationBuilder ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    int _code = context.Response.StatusCode;
                    int _codeAPP = 0;
                    string _message = exceptionHandlerPathFeature.Error.Message;
                    string _stackTrace = null;
                    
                    try
                    {
                        _codeAPP = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).Code;
                        _message = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).Message;
                        _stackTrace = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).StackTrace;

                        switch (_codeAPP)
                        {
                            case 500:
                                context.Response.StatusCode = _codeAPP;
                                _code = _codeAPP;
                                break;
                            case 401:

                                switch (_stackTrace)
                                {
                                    case "SecurityTokenExpiredException":
                                        context.Response.Headers.Add("Token-Expired", "true");
                                        break;
                                    case "ArgumentException":
                                    case "invalid_token":
                                        context.Response.Headers.Add("Token-Valid", "false");
                                        break;
                                }

                                context.Response.StatusCode = _codeAPP;
                                _code = _codeAPP;
                                break;
                            default:
                                context.Response.StatusCode = 400;
                                _code = 400;
                                break;
                        }
                    }
                    catch (InvalidCastException)
                    {
                    }

                    MsDtoResponseError _response = new MsDtoResponseError
                    {
                        code = _code,
                        message = _message,
                        errors = new List<MsDtoError> {
                            new MsDtoError
                            {
                            code = _codeAPP == 0 ? _code : _codeAPP,
                            message = _stackTrace ??= _message
                            }
                        },
                        traceid = context?.TraceIdentifier == null ? "no-traceid" : context?.TraceIdentifier
                    };

                    Log.Error("{Proceso} {errorCode} {errorMessage}", "ExceptionHandler", context.Response.StatusCode, _message);

                    string sjson = JsonSerializer.Serialize(_response);

                    await context.Response.WriteAsync(sjson);
                    await context.Response.Body.FlushAsync();
                });
            });

            return app;
        }
    }
}
