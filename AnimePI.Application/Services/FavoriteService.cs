
using AnimePI.Domain.Aggregates.FavoriteAggregate;
using AnimePI.Domain.Aggregates.FavoriteAggregate.Interfaces;
using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;

namespace AnimePI.Application.Services;

public class FavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IUserRepository _userRepository;

    public FavoriteService(IFavoriteRepository favoriteRepository, IUserRepository userRepository)
    {
        _favoriteRepository = favoriteRepository;
        _userRepository = userRepository;
    }

    public async Task<List<Favorite>> GetUserFavoritesAsync(Guid userId)
    {
        return await _favoriteRepository.GetUserFavoritesAsync(userId);
    }

    public async Task<Favorite> AddFavoriteAsync(Guid userId, int animeId, string animeTitle, string animeImageUrl)
    {
        var favorite = await _favoriteRepository.GetUserFavoriteAsync(userId);
        if (favorite == null)
        {
            favorite = new Favorite(userId);
        }
        
        favorite.AddAnime(animeId, animeTitle, animeImageUrl);
        return await _favoriteRepository.AddFavoriteAsync(favorite);
    }

    public async Task<bool> RemoveFavoriteAsync(Guid userId, int animeId)
    {
        return await _favoriteRepository.RemoveFavoriteAsync(userId, animeId);
    }
}
