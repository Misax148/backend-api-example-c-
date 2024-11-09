using Microsoft.Extensions.DependencyInjection;
using MovieApi.Core.src.Application.Services;
using MovieApi.Core.src.Domain.Interfaces.Services;

namespace MovieApi.Core.src.DependencyInjection;

public static class CoreDependencyInjection
{
    public static IServiceCollection AddCoreLayer(this IServiceCollection services)
    {
        services.AddServices();        
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IUserService, UserService>();
        
        return services;
    }
}
