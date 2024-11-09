using FluentValidation;
using MovieApi.API.src.Validators;
using MovieApi.Core.src.Application.DTO.Actor;
using MovieApi.Core.src.Application.DTO.Movie;
using MovieApi.Core.src.Application.DTO.User;

namespace MovieApi.API.src.DependencyInjection;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddApiLayer(this IServiceCollection services)
    {
        services.AddValidators();

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateMovieDto>, CreateMovieValidator>();
        services.AddScoped<IValidator<UpdateMovieDto>, UpdateMovieValidator>();
        services.AddScoped<IValidator<CreateActorDto>, CreateActorValidator>();
        services.AddScoped<IValidator<UpdateActorDto>, UpdateActorValidator>();
        services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();
        services.AddScoped<IValidator<UpdateUserDto>, UpdateUserValidator>();

        return services;
    }
}
