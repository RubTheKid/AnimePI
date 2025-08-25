using AnimePI.Application.Services;
using AnimePI.Domain.Aggregates.AnimeAggregate.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimePI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnimeController : ControllerBase
{
    private readonly JikanApiService _jikanApiService;
    private readonly IAnimeRepository _animeRepository;

    public AnimeController(JikanApiService jikanApiService, IAnimeRepository animeRepository)
    {
        _jikanApiService = jikanApiService;
        _animeRepository = animeRepository;
    }


    [HttpPost("jikan/fetch/top")]
    public async Task<ActionResult> FetchTopAnimes([FromQuery] int limit = 25)
    {
        try
        {
            var animes = await _jikanApiService.FetchAndStoreTopAnimesAsync(limit);
            return Ok(new
            {
                message = $"Fetched and stored {animes.Count} animes",
                data = animes
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("jikan/fetch/top/multiple-pages")]
    public async Task<ActionResult> FetchTopAnimesMultiplePages([FromQuery] int startPage = 1, [FromQuery] int endPage = 2, [FromQuery] int limit = 25)
    {
        try
        {
            if (startPage < 1) startPage = 1;
            if (endPage < startPage) endPage = startPage;
            if (limit < 1 || limit > 25) limit = 25;


            if (endPage - startPage + 1 > 10)
            {
                return BadRequest("Cannot fetch more than 10 pages at once");
            }

            var animes = await _jikanApiService.FetchAndStoreTopAnimesMultiplePagesAsync(startPage, endPage, limit);

            var startRank = (startPage - 1) * limit + 1;

            return Ok(new
            {
                message = $"Fetched and stored {animes.Count} top animes from pages {startPage}-{endPage})",
                startPage = startPage,
                endPage = endPage,
                limit = limit,
                totalPages = endPage - startPage + 1,
                totalFetched = animes.Count,
                data = animes
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("jikan/fetch/{malId:int}")]
    public async Task<ActionResult> FetchAnimeById(int malId)
    {
        try
        {
            var anime = await _jikanApiService.FetchAndStoreAnimeByIdAsync(malId);
            if (anime == null) return NotFound("Anime not found in external API");

            return Ok(new
            {
                message = "Successfully fetched and stored anime",
                data = anime
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpGet]
    public async Task<ActionResult> GetAllAnimes()
    {
        try
        {
            var animes = await _animeRepository.GetAllAsync();
            return Ok(animes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetAnimeById(Guid id)
    {
        try
        {
            var anime = await _animeRepository.GetByIdAsync(id);
            if (anime == null) return NotFound();
            return Ok(anime);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("mal/{malId:int}")]
    public async Task<ActionResult> GetAnimeByMalId(int malId)
    {
        try
        {
            var anime = await _animeRepository.GetByMalIdAsync(malId);
            if (anime == null) return NotFound();
            return Ok(anime);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("top")]
    public async Task<ActionResult> GetTopAnimes([FromQuery] int limit = 10)
    {
        try
        {
            var animes = await _animeRepository.GetTopRankedAsync(limit);
            return Ok(animes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult> SearchAnimes([FromQuery] string title)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest("Title parameter is required");

            var animes = await _animeRepository.SearchByTitleAsync(title);
            return Ok(animes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("analytics/top-genres")]
    public async Task<ActionResult> GetTopGenres([FromQuery] int limit = 10)
    {
        try
        {
            var animes = await _animeRepository.GetAllAsync();

            var genreStats = animes
                .SelectMany(a => a.Genres)
                .Where(g => !string.IsNullOrEmpty(g))
                .GroupBy(g => g)
                .Select(g => new
                {
                    Genre = g.Key,
                    Count = g.Count(),
                    AverageScore = animes.Where(a => a.Genres.Contains(g.Key)).Average(a => a.Score)
                })
                .OrderByDescending(g => g.Count)
                .Take(limit)
                .ToList();

            return Ok(genreStats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}