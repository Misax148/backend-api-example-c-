using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieApi.Core.src.Domain.Interfaces.Repositories;
using MovieApi.Infraestructure.src.Data.DbConnection;
using MovieApi.Infraestructure.src.Data.DbConnection.Concretes;
using MovieApi.Infraestructure.src.Data.DbConnection.Interfaces;
using MovieApi.Infraestructure.src.Data.Memory;
using MovieApi.Infraestructure.src.Repositories.BaseMemoryRepository;
using MovieApi.Infraestructure.src.Repositories.DBRepository;

namespace MovieApi.Infraestructure.src.InfrastructureDependencyInjection;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfraestructureLayer(this IServiceCollection services, IConfiguration configuration, bool useMemoryStage = false)
    {
        if (useMemoryStage)
        {
            services.AddMemoryStorage();
        }
        else
        {
            services.AddDatabase(configuration);
        }
        
        services.AddRepositories(useMemoryStage);
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DataBaseOptions>(configuration.GetSection(DataBaseOptions.ConnectionStrings));
        services.AddScoped<IDbConnectionFactory, DbConnection>();
        return services;
    }

    private static IServiceCollection AddMemoryStorage(this IServiceCollection services)
    {
        services.AddSingleton<MemoryContext>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services, bool useMemoryStage)
    {
        if (useMemoryStage)
        {
            services.AddScoped<IMovieRepository, MovieMemoryRepository>();
            services.AddScoped<IActorRepository, ActorMemoryRepository>();
            services.AddScoped<IUserRepository, UserMemoryRepository>();
        }
        else
        {
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        return services;
    }
}
