using AnimePI.Application.Interfaces;
using AnimePI.Domain.Aggregates.FavoriteAggregate;
using AnimePI.Domain.Aggregates.FavoriteAggregate.Interfaces;

namespace AnimePI.Application.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;

    public FavoriteService(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task<List<Favorite>> GetUserFavoritesAsync(Guid userId)
    {
        return await _favoriteRepository.GetUserFavoritesAsync(userId);
    }

    public async Task<Favorite?> GetUserFavoriteAsync(Guid userId)
    {
        return await _favoriteRepository.GetUserFavoriteAsync(userId);
    }

    public async Task<Favorite> AddFavoriteAsync(Guid userId, int animeId, string animeTitle, string animeImageUrl)
    {
        var favorite = await _favoriteRepository.GetUserFavoriteAsync(userId);
        if (favorite == null)
        {
            favorite = new Favorite(userId);
        }

        favorite.AddAnime(animeId, animeTitle, animeImageUrl);
        return await _favoriteRepository.AddOrUpdateFavoriteAsync(favorite);
    }

    public async Task<bool> RemoveFavoriteAsync(Guid userId, int animeId)
    {
        return await _favoriteRepository.RemoveFavoriteAsync(userId, animeId);
    }

    public async Task<bool> IsFavoriteAsync(Guid userId, int animeId)
    {
        return await _favoriteRepository.IsFavoriteAsync(userId, animeId);
    }

    public async Task<bool> ToggleFavoriteAsync(Guid userId, int animeId, string animeTitle, string animeImageUrl)
    {
        var favorite = await _favoriteRepository.GetUserFavoriteAsync(userId);
        if (favorite == null)
        {
            favorite = new Favorite(userId);
        }

        var wasAdded = favorite.ToggleAnime(animeId, animeTitle, animeImageUrl);
        await _favoriteRepository.AddOrUpdateFavoriteAsync(favorite);

        return wasAdded;
    }
}
