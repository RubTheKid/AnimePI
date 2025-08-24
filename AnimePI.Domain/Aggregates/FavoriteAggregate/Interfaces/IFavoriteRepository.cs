namespace AnimePI.Domain.Aggregates.FavoriteAggregate.Interfaces;

public interface IFavoriteRepository
{
    Task<List<Favorite>> GetUserFavoritesAsync(Guid userId);
    Task<Favorite?> GetUserFavoriteAsync(Guid userId);
    Task<Favorite> AddOrUpdateFavoriteAsync(Favorite favorite);
    Task<bool> RemoveFavoriteAsync(Guid userId, int animeId);
    Task<bool> IsFavoriteAsync(Guid userId, int animeId);
}
