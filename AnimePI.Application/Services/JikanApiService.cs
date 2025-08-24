using AnimePI.Application.DTOs;
using AnimePI.Domain.Aggregates.AnimeAggregate;
using AnimePI.Domain.Aggregates.AnimeAggregate.Interfaces;
using System.Text.Json;

namespace AnimePI.Application.Services;

public class JikanApiService
{
    private readonly HttpClient _httpClient;
    private readonly IAnimeRepository _animeRepository;
    private const string BaseUrl = "https://api.jikan.moe/v4";

    public JikanApiService(HttpClient httpClient, IAnimeRepository animeRepository)
    {
        _httpClient = httpClient;
        _animeRepository = animeRepository;
    }

    public async Task<List<Anime>> FetchAndStoreTopAnimesAsync(int limit = 25)
    {
        try
        {
            var url = $"{BaseUrl}/top/anime?limit={limit}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<JikanApiResponse<List<JikanAnimeDto>>>(json);

            if (apiResponse?.Data == null) return new List<Anime>();

            var animes = new List<Anime>();

            foreach (var dto in apiResponse.Data)
            {
                var existingAnime = await _animeRepository.GetByMalIdAsync(dto.MalId);

                if (existingAnime != null)
                {
                    UpdateAnimeFromDto(existingAnime, dto);
                    await _animeRepository.UpdateAsync(existingAnime);
                    animes.Add(existingAnime);
                }
                else
                {
                    var newAnime = CreateAnimeFromDto(dto);
                    await _animeRepository.AddAsync(newAnime);
                    animes.Add(newAnime);
                }
            }

            return animes;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching top animes: {ex.Message}");
            return new List<Anime>();
        }
    }

    public async Task<List<Anime>> FetchAndStoreTopAnimesAsync(int page = 1, int limit = 25)
    {
        try
        {
            var url = $"{BaseUrl}/top/anime?page={page}&limit={limit}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<JikanApiResponse<List<JikanAnimeDto>>>(json);

            if (apiResponse?.Data == null) return new List<Anime>();

            var animes = new List<Anime>();

            foreach (var dto in apiResponse.Data)
            {
                var existingAnime = await _animeRepository.GetByMalIdAsync(dto.MalId);

                if (existingAnime != null)
                {
                    UpdateAnimeFromDto(existingAnime, dto);
                    await _animeRepository.UpdateAsync(existingAnime);
                    animes.Add(existingAnime);
                }
                else
                {
                    var newAnime = CreateAnimeFromDto(dto);
                    await _animeRepository.AddAsync(newAnime);
                    animes.Add(newAnime);
                }
            }

            return animes;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching top animes: {ex.Message}");
            return new List<Anime>();
        }
    }

    public async Task<List<Anime>> FetchAndStoreTopAnimesMultiplePagesAsync(int startPage = 1, int endPage = 1, int limit = 25)
    {
        var allAnimes = new List<Anime>();

        for (int page = startPage; page <= endPage; page++)
        {
            try
            {
                Console.WriteLine($"Fetching page {page} of top animes...");
                var animes = await FetchAndStoreTopAnimesAsync(page, limit);
                allAnimes.AddRange(animes);

                if (page < endPage)
                {
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching page {page}: {ex.Message}");
            }
        }

        return allAnimes;
    }

    public async Task<Anime?> FetchAndStoreAnimeByIdAsync(int malId)
    {
        try
        {
            var url = $"{BaseUrl}/anime/{malId}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<JikanApiResponse<JikanAnimeDto>>(json);

            if (apiResponse?.Data == null) return null;

            var existingAnime = await _animeRepository.GetByMalIdAsync(malId);

            if (existingAnime != null)
            {
                UpdateAnimeFromDto(existingAnime, apiResponse.Data);
                return await _animeRepository.UpdateAsync(existingAnime);
            }
            else
            {
                var newAnime = CreateAnimeFromDto(apiResponse.Data);
                return await _animeRepository.AddAsync(newAnime);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching anime {malId}: {ex.Message}");
            return null;
        }
    }

    private Anime CreateAnimeFromDto(JikanAnimeDto dto)
    {
        var anime = new Anime(dto.MalId, dto.Title);
        UpdateAnimeFromDto(anime, dto);
        return anime;
    }

    private void UpdateAnimeFromDto(Anime anime, JikanAnimeDto dto)
    {
        anime.UpdateFromExternalData(
            titleEnglish: dto.TitleEnglish ?? string.Empty,
            titleJapanese: dto.TitleJapanese ?? string.Empty,
            synopsis: dto.Synopsis ?? string.Empty,
            imageUrl: dto.Images?.Jpg?.ImageUrl ?? string.Empty,
            trailerUrl: dto.Trailer?.Url ?? string.Empty,
            score: dto.Score ?? 0,
            rank: dto.Rank ?? 0,
            status: dto.Status ?? string.Empty,
            type: dto.Type ?? string.Empty,
            episodes: dto.Episodes ?? 0,
            year: dto.Year ?? 0,
            genres: dto.Genres?.Select(g => g.Name).ToList() ?? new List<string>()
        );
    }
}