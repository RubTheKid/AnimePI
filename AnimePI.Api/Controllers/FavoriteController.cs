using AnimePI.Application.FavoriteAggregate.Command.AddFavorite;
using AnimePI.Application.FavoriteAggregate.Command.RemoveFavorite;
using AnimePI.Application.FavoriteAggregate.Query.GetUserFavorites;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AnimePI.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FavoriteController : ControllerBase
{
    private readonly IMediator _mediator;

    public FavoriteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<List<GetUserFavoritesQueryResponse>>> GetUserFavorites(Guid userId)
    {
        try
        {
            var query = new GetUserFavoritesQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<AddFavoriteCommandResponse>> AddFavorite([FromBody] AddFavoriteCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserFavorites), new { userId = command.UserId }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{userId:guid}/anime/{animeId:int}")]
    public async Task<ActionResult<RemoveFavoriteCommandResponse>> RemoveFavorite(Guid userId, int animeId)
    {
        try
        {
            var command = new RemoveFavoriteCommand(userId, animeId);
            var result = await _mediator.Send(command);

            if (result.Success)
                return Ok(result);
            else
                return NotFound(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
