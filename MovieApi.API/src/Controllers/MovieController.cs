using Microsoft.AspNetCore.Mvc;
using MovieApi.Core.src.Application.DTO.Movie;
using MovieApi.Core.src.Domain.Interfaces.Services;

namespace MovieApi.API.src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDto createMovieDto)
    {
        var movie = await _movieService.CreateMovieAsync(createMovieDto);
        return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMovie(
        [FromRoute] Guid id,
        [FromBody] UpdateMovieDto updateMovieDto)
    {
        var movie = await _movieService.UpdateMovieAsync(id, updateMovieDto);
        return Ok(movie);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMovie([FromRoute] Guid id)
    {
        await _movieService.DeteleMovieAsync(id);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMovieById([FromRoute] Guid id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        return Ok(movie);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MovieDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMovies()
    {
        var movies = await _movieService.GetAllMovies();
        return Ok(movies);
    }

    [HttpGet("with-actors/{movieId:guid}")]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMovieWithActors([FromRoute] Guid movieId)
    {
        var movie = await _movieService.GetMovieWithActorsAsync(movieId);
        return Ok(movie);
    }

    [HttpGet("by-genre")]
    [ProducesResponseType(typeof(IEnumerable<MovieDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMoviesByGenre([FromQuery] string genre)
    {
        var movies = await _movieService.GetMoviesByGenreAsync(genre);
        return Ok(movies);
    }

    [HttpGet("by-rating")]
    [ProducesResponseType(typeof(IEnumerable<MovieDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMoviesByRating([FromQuery] decimal rating)
    {
        var movies = await _movieService.GetMoviesByRatingAsync(rating);
        return Ok(movies);
    }
}
