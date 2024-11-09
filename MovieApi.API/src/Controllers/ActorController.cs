using Microsoft.AspNetCore.Mvc;
using MovieApi.Core.src.Application.DTO.Actor;
using MovieApi.Core.src.Domain.Interfaces.Services;

namespace MovieApi.API.src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActorController : ControllerBase
{
    private readonly IActorService _actorService;

    public ActorController(
        IActorService actorService,
        ILogger<ActorController> logger)
    {
        _actorService = actorService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ActorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateActor([FromBody] CreateActorDto createActorDto)
    {
        var actor = await _actorService.CreateActorAsync(createActorDto);
        return CreatedAtAction(nameof(GetActorById), new { id = actor.Id }, actor);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ActorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateActor(
        [FromRoute] Guid id,
        [FromBody] UpdateActorDto updateActorDto)
    {
        var actor = await _actorService.UpdateActorAsync(id, updateActorDto);
        return Ok(actor);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteActor([FromRoute] Guid id)
    {
        await _actorService.DeleteActorAsync(id);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ActorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActorById([FromRoute] Guid id)
    {
        var actor = await _actorService.GetActorByIdAsync(id);
        return Ok(actor);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ActorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllActors()
    {
        var actors = await _actorService.GetAllActorsAsync();
        return Ok(actors);
    }

    [HttpGet("movie/{movieId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<ActorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActorsByMovie([FromRoute] Guid movieId)
    {
        var actors = await _actorService.GetActorByMovieAsync(movieId);
        return Ok(actors);
    }

    [HttpGet("age-range")]
    [ProducesResponseType(typeof(IEnumerable<ActorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetActorsByAgeRange(
        [FromQuery] int minAge,
        [FromQuery] int maxAge)
    {
        var actors = await _actorService.GetActorByAgeRangeAsync(minAge, maxAge);
        return Ok(actors);
    }

    [HttpGet("{actorId:guid}/movie-count")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMovieCount([FromRoute] Guid actorId)
    {
        var count = await _actorService.GetMovieCountByActorAsync(actorId);
        return Ok(count);
    }
}
