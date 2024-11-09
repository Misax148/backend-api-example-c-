using FluentValidation;
using MovieApi.Core.src.Application.DTO.Actor;
using MovieApi.Core.src.Application.DTO.Movie;
using MovieApi.Core.src.Domain.Entities;
using MovieApi.Core.src.Domain.Exceptions;
using MovieApi.Core.src.Domain.Interfaces.Repositories;
using MovieApi.Core.src.Domain.Interfaces.Services;

namespace MovieApi.Core.src.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<CreateMovieDto> _createMovieValidator;
    private readonly IValidator<UpdateMovieDto> _updateMovieValidator;

    public MovieService(
        IMovieRepository movieRepository,
        IValidator<CreateMovieDto> createMovieValidator,
        IValidator<UpdateMovieDto> updateMovieValidator)
    {
        _movieRepository = movieRepository;
        _createMovieValidator = createMovieValidator;
        _updateMovieValidator = updateMovieValidator;
    }

    public async Task<MovieDto> CreateMovieAsync(CreateMovieDto movieDto)
    {
        var validationResult = await _createMovieValidator.ValidateAsync(movieDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }


        var movie = MapToEntity(movieDto);
        var created = await _movieRepository.CreateAsync(movie);
        
        if (!created)
        {
            throw new InternalServerErrorException("Failed to create movie");
        }

        return await GetMovieByIdAsync(movie.Id);
    }

    public async Task<bool> DeteleMovieAsync(Guid id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        if (movie == null)
        {
            throw new NotFoundException($"Movie with ID {id} not found");
        }

        return await _movieRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<MovieDto>> GetAllMovies()
    {
        var movies = await _movieRepository.GetAllAsync();
        return movies.Select(MapToDto);
    }

    public async Task<MovieDto> GetMovieByIdAsync(Guid id)
    {
         var movie = await _movieRepository.GetByIdAsync(id);
        if (movie == null)
        {
            throw new NotFoundException($"Movie with ID {id} not found");
        }

        return MapToDto(movie);
    }

    public async Task<IEnumerable<MovieDto>> GetMoviesByGenreAsync(string genre)
    {
        if (string.IsNullOrWhiteSpace(genre))
        {
            throw new BadRequestException("Genre cannot be empty");
        }

        var movies = await _movieRepository.GetMoviesByGenreAsync(genre);
        return movies.Select(MapToDto);
    }

    public async Task<IEnumerable<MovieDto>> GetMoviesByRatingAsync(decimal rating)
    {
        if (rating < 0 || rating > 10)
        {
            throw new BadRequestException("Rating must be between 0 and 10");
        }

        var movies = await _movieRepository.GetMoviesByRatingAsync(rating);
        return movies.Select(MapToDto);
    }

    public async Task<MovieDto> GetMovieWithActorsAsync(Guid movieId)
    {
        var movie = await _movieRepository.GetMovieWithActorAsync(movieId);
        if (movie == null)
        {
            throw new NotFoundException($"Movie with ID {movieId} not found");
        }

        return MapToDto(movie);
    }

    public async Task<MovieDto> UpdateMovieAsync(Guid id, UpdateMovieDto updateMovieDto)
    {
        var validationResult = await _updateMovieValidator.ValidateAsync(updateMovieDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var movie = await _movieRepository.GetByIdAsync(id);
        if (movie == null)
        {
            throw new NotFoundException($"Movie with ID {id} not found");
        }

        UpdateEntityFromDto(movie, updateMovieDto);
        var updated = await _movieRepository.UpdateAsync(movie);
        
        if (!updated)
        {
            throw new InternalServerErrorException("Failed to update movie");
        }

        return await GetMovieByIdAsync(id);
    }

    private Movie MapToEntity(CreateMovieDto dto)
    {
        return new Movie
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            ReleaseDate = dto.ReleaseDate,
            Description = dto.Description,
            Genre = dto.Genre,
            Rating = dto.Rating
        };
    }

    private void UpdateEntityFromDto(Movie movie, UpdateMovieDto dto)
    {
        movie.Title = dto.Title;
        movie.ReleaseDate = dto.ReleaseDate;
        movie.Description = dto.Description;
        movie.Genre = dto.Genre;
        movie.Rating = dto.Rating;
    }

    private MovieDto MapToDto(Movie movie)
    {
        return new MovieDto(
            movie.Id,
            movie.Title,
            movie.ReleaseDate,
            movie.Description,
            movie.Genre,
            movie.Rating,
            movie.Actors?.Select(actor => new ActorDto(
                actor.Id,
                actor.Name,
                actor.Biography,
                actor.Birthdate,
                new List<MovieSimpleDto>()
            )).ToList() ?? new List<ActorDto>()
        );
    }
}
