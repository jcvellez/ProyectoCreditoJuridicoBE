using bg.hd.banca.juridica.api.Extentions;
using bg.hd.banca.juridica.application.ioc;
using bg.hd.banca.juridica.infrastructure.extentions;
using bg.hd.banca.juridica.infrastructure.ioc;
using bg.hd.banca.juridica.infrastructure.security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Collections;

var builder = WebApplication.CreateBuilder(args);

//Parametros Entorno
builder = builder.ConfigureEnvironmentVariable("BGBANCAPYMEJURIDICA");

IConfigurationSection myArraySection = builder.Configuration.GetSection("AuthorizeSite:SiteUrl");
string[] corsURL = myArraySection.Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("myAllowSpecificOrigins",
    builder =>
    {
        builder.AllowAnyHeader()
               .AllowAnyMethod()
               .WithOrigins(corsURL)
               .AllowCredentials();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var version = builder.Configuration["OpenApi:info:version"];
    var title = builder.Configuration["OpenApi:info:title"];
    var description = builder.Configuration["OpenApi:info:description"];
    var termsOfService = new Uri(builder.Configuration["OpenApi:info:termsOfService"]);
    var contact = new OpenApiContact
    {
        Name = builder.Configuration["OpenApi:info:contact:name"],
        Url = new Uri(builder.Configuration["OpenApi:info:contact:url"]),
        Email = builder.Configuration["OpenApi:info:contact:email"]
    };
    var license = new OpenApiLicense
    {
        Name = builder.Configuration["OpenApi:info:License:name"],
        Url = new Uri(builder.Configuration["OpenApi:info:License:url"])
    };
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = title,
        Description = description,
        TermsOfService = termsOfService,
        Contact = contact,
        License = license
    });
    options.SwaggerDoc(builder.Configuration["OpenApi:info:version"], new OpenApiInfo
    {
        Version = builder.Configuration["OpenApi:info:version"],
        Title = title,
        Description = description,
        TermsOfService = termsOfService,
        Contact = contact,
        License = license
    });
    var jwtSecurityScheme = new OpenApiSecurityScheme
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
    };
    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.OperationFilter<RequiredHeaderParameter>();
    List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
    xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));

});


//Dependencias propias de Servicio
builder.Services.RegisterDependencies();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();


builder.Services.AddHealthChecks();


//Config Authentication Tokes
//builder.Services.SetupAuthenticationServices(builder);


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1");
        c.SwaggerEndpoint($"/swagger/v2/swagger.json", $"v2");
    });
}


app.ConfigureMetricServer();
app.ConfigureExceptionHandler();


app.UseRouting();
app.UseCors("myAllowSpecificOrigins");

app.UseAuthentication();
//app.UseAuthorization();



app.MapHealthChecks("/health/readiness", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
});

app.MapHealthChecks("/health/liveness", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
    Predicate = _ => false
});



app.MapControllers();




app.Run();
