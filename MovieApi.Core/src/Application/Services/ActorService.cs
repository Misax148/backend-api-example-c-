using FluentValidation;
using MovieApi.Core.src.Application.DTO.Actor;
using MovieApi.Core.src.Application.DTO.Movie;
using MovieApi.Core.src.Domain.Entities;
using MovieApi.Core.src.Domain.Exceptions;
using MovieApi.Core.src.Domain.Interfaces.Repositories;
using MovieApi.Core.src.Domain.Interfaces.Services;

namespace MovieApi.Core.src.Application.Services;

public class ActorService : IActorService
{
    private readonly IActorRepository _actorRepository;
    private readonly IValidator<CreateActorDto> _createActorValidator;
    private readonly IValidator<UpdateActorDto> _updateActorValidator;

    public ActorService(
        IActorRepository actorRepository,
        IValidator<CreateActorDto> createActorValidator,
        IValidator<UpdateActorDto> updateActorValidator)
    {
        _actorRepository = actorRepository;
        _createActorValidator = createActorValidator;
        _updateActorValidator = updateActorValidator;
    }
    
    public async Task<ActorDto> CreateActorAsync(CreateActorDto actorDto)
    {
        var validationResult = await _createActorValidator.ValidateAsync(actorDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var actor = MapToEntity(actorDto);
        var created = await _actorRepository.CreateAsync(actor);
        
        if (!created)
        {
            throw new InternalServerErrorException("Failed to create actor");
        }

        Console.WriteLine(actor.Id);

        return await GetActorByIdAsync(actor.Id);
    }

    public async Task<bool> DeleteActorAsync(Guid id)
    {
        var actor = await _actorRepository.GetByIdAsync(id);
        if (actor == null)
        {
            throw new NotFoundException($"Actor with ID {id} not found");
        }

        return await _actorRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ActorDto>> GetActorByAgeRangeAsync(int minAge, int maxAge)
    {
        if (minAge < 0 || maxAge < minAge)
        {
            throw new BadRequestException("Invalid age range");
        }

        var actors = await _actorRepository.GetActorByAgeRangeAsync(minAge, maxAge);
        return actors.Select(MapToDto);
    }

    public async Task<ActorDto> GetActorByIdAsync(Guid id)
    {
        var actor = await _actorRepository.GetByIdAsync(id);
        if (actor == null)
        {
            throw new NotFoundException($"Actor with ID {id} not found");
        }

        return MapToDto(actor);
    }

    public async Task<IEnumerable<ActorDto>> GetActorByMovieAsync(Guid movieId)
    {
        var actors = await _actorRepository.GetActorsByMovieAsync(movieId);
        return actors.Select(MapToDto);
    }

    public async Task<IEnumerable<ActorDto>> GetAllActorsAsync()
    {
        var actors = await _actorRepository.GetAllAsync();
        return actors.Select(MapToDto);
    }

    public async Task<int> GetMovieCountByActorAsync(Guid actorId)
    {
        var actor = await _actorRepository.GetByIdAsync(actorId);
        if (actor == null)
        {
            throw new NotFoundException($"Actor with ID {actorId} not found");
        }

        return await _actorRepository.GetMovieCountByActorAsync(actorId);
    }

    public async Task<ActorDto> UpdateActorAsync(Guid id, UpdateActorDto updateActorDto)
    {
        var validationResult = await _updateActorValidator.ValidateAsync(updateActorDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var actor = await _actorRepository.GetByIdAsync(id);
        if (actor == null)
        {
            throw new NotFoundException($"Actor with ID {id} not found");
        }

        UpdateEntityFromDto(actor, updateActorDto);
        var updated = await _actorRepository.UpdateAsync(actor);
        
        if (!updated)
        {
            throw new InternalServerErrorException("Failed to update actor");
        }

        return await GetActorByIdAsync(id);
    }

    private Actor MapToEntity(CreateActorDto dto)
    {
        return new Actor
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Biography = dto.Biography,
            Birthdate = dto.Birthdate
        };
    }

    private void UpdateEntityFromDto(Actor actor, UpdateActorDto dto)
    {
        actor.Name = dto.Name;
        actor.Biography = dto.Biography;
        actor.Birthdate = dto.Birthdate;
    }

    private ActorDto MapToDto(Actor actor)
    {
        return new ActorDto(
            actor.Id,
            actor.Name,
            actor.Biography,
            actor.Birthdate,
            actor.Movies?.Select(movie => new MovieSimpleDto(
                movie.Id,
                movie.Title,
                movie.Genre,
                movie.Rating
            )).ToList() ?? new List<MovieSimpleDto>()
        );
    }
}
