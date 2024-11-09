using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using MovieApi.API.src.DependencyInjection;
using MovieApi.API.src.Middlewares;
using MovieApi.Core.src.DependencyInjection;
using MovieApi.Infraestructure.src.InfrastructureDependencyInjection;
using System.Text.Json.Serialization;

namespace MovieApi.API.src;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container
        ConfigureServices(builder.Services, builder.Configuration);
        var app = builder.Build();
        // Configure the HTTP request pipeline
        ConfigureMiddleware(app);
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Add layers
        services
            .AddApiLayer()
            .AddCoreLayer()
            .AddInfraestructureLayer(configuration, true);

        // Add controllers with JSON options
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        // Add FluentValidation
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Movie API",
                Version = "v1",
                Description = "A simple movie management API"
            });
        });

        // Add CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        // Configure Swagger - siempre habilitado en este caso ya que estamos en desarrollo
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie API V1");
            // Cambiamos esto para que Swagger esté en /swagger
            c.RoutePrefix = "swagger";
        });

        // Global error handling
        app.UseCustomExceptionHandler();

        // CORS - debe ir antes de la autenticación y autorización
        app.UseCors("AllowAll");

        // HTTP to HTTPS redirect - comentado para desarrollo local con Docker
        app.UseHttpsRedirection();

        // Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Controllers
        app.MapControllers();
    }
}