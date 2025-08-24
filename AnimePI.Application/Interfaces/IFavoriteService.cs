using AnimePI.Domain.Aggregates.FavoriteAggregate;

namespace AnimePI.Application.Interfaces;

public interface IFavoriteService
{
    Task<List<Favorite>> GetUserFavoritesAsync(Guid userId);
    Task<Favorite?> GetUserFavoriteAsync(Guid userId);
    Task<Favorite> AddFavoriteAsync(Guid userId, int animeId, string animeTitle, string animeImageUrl);
    Task<bool> RemoveFavoriteAsync(Guid userId, int animeId);
    Task<bool> IsFavoriteAsync(Guid userId, int animeId);
    Task<bool> ToggleFavoriteAsync(Guid userId, int animeId, string animeTitle, string animeImageUrl);
}
