using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Common.Configs;
using Common.Exceptions;
using Common.IoC;
using DAL.DbContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text.Json;
using Common.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using WebAPI.Infrastructure.Enums;

namespace WebAPI.Middleware;
public static class MiddlewareExtensions
{
    public static WebApplicationBuilder DefaultConfiguration(
        this WebApplicationBuilder builder)
    {
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>((hostContext, containerBuilder) =>
        {
            containerBuilder.RegisterBuildCallback(ctx => IoC.Container = ctx.Resolve<ILifetimeScope>());

            BLL.Startup.BootStrapper.Bootstrap(containerBuilder);
        });

        builder.Configuration.AddJsonFile($@"{Path.GetFullPath("../")}/config.json");

        var connectionModel = builder.Configuration.GetSection("Connection").Get<ConnectionModel>()!;
        builder.Services.AddSingleton(connectionModel);

        var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? connectionModel.Host;
        var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? connectionModel.Database;
        var dbPassword = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD") ?? connectionModel.Password;

        var connection = string.Format(connectionModel.ConnectionString, dbHost, dbName, dbPassword);

        builder.Services.AddDbContext<DbContextBase>(options => options.UseSqlServer(connection));

        var authOptions = builder.Configuration.GetSection("Auth").Get<AuthOptions>()!;
        builder.Services.AddSingleton(authOptions);
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = authOptions.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = authOptions.SymmetricSecurityKey,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
            });

        var emailCreds = builder.Configuration.GetSection("EmailCreds").Get<EmailCreds>()!;
        builder.Services.AddSingleton(emailCreds);

        var googleMapsCreds = builder.Configuration.GetSection("GoogleMapsCreds").Get<GoogleMapsCreds>()!;
        builder.Services.AddSingleton(googleMapsCreds);

        #region Init Mapper Profiles

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(new[] {
                "DAL",
                "BLL",
            });
        });

        var mapper = mapperConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);

        #endregion

        #region Localization

        builder.Services.AddLocalization();
        builder.Services.Configure<RequestLocalizationOptions>(
            options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US")
                    {
                        DateTimeFormat = {
                            LongTimePattern = "MM/DD/YYYY",
                            ShortTimePattern = "MM/DD/YYYY "
                        }
                    },
                    new CultureInfo("uk-UA")
                    {
                        DateTimeFormat = {
                            LongTimePattern = "DD/MM/YYYY",
                            ShortTimePattern = "DD/MM/YYYY"
                        }
                    },
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "uk-UA", uiCulture: "uk-UA");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

            });

        builder.Services.AddMvc()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(Resources));
            });

        #endregion

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });


        builder.Services.AddCors(option =>
        {
            option.AddPolicy(name: MyAllowSpecificOrigins, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return builder;
    }

    public static IApplicationBuilder UseDefaultErrorHandler(this IApplicationBuilder appBuilder)
    {
        return appBuilder.Use(DefaultHandleError);
    }

    #region Helpers
    private static async Task DefaultHandleError(HttpContext httpContext, Func<Task> next)
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            var errorType = ApiErrorType.Unknown;
            string message;
            object data;

            switch (ex)
            {
                case UnauthorizedAccessException _:
                    errorType = ApiErrorType.Forbidden;
                    message = ex.Message;
                    data = ex.Data;
                    break;
                case InvalidRefreshTokenException _:
                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    errorType = ApiErrorType.Forbidden;
                    message = ex.Message;
                    data = ex.Data;
                    break;
                default:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    message = ex.Message;
                    data = ex.ToString();
                    break;
            }

            var error = new
            {
                ErrorType = errorType,
                Message = message,
                Error = data
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(error, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            }));
        }
    }

    #endregion
}
