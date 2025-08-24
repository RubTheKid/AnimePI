using AnimePI.Domain.Aggregates.FavoriteAggregate;
using AnimePI.Domain.Aggregates.FavoriteAggregate.Interfaces;
using AnimePI.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimePI.Infra.Repository;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly AppDbContext _dbContext;

    public FavoriteRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Favorite> AddOrUpdateFavoriteAsync(Favorite favorite)
    {
        var existingFavorite = await _dbContext.Favorites
            .FirstOrDefaultAsync(f => f.UserId == favorite.UserId && !f.IsDeleted);

        if (existingFavorite != null)
        {
            existingFavorite.Animes = favorite.Animes;
            existingFavorite.DateUpdated = DateTime.UtcNow;
            _dbContext.Favorites.Update(existingFavorite);
            await _dbContext.SaveChangesAsync();
            return existingFavorite;
        }
        else
        {
            await _dbContext.Favorites.AddAsync(favorite);
            await _dbContext.SaveChangesAsync();
            return favorite;
        }
    }

    public async Task<Favorite?> GetUserFavoriteAsync(Guid userId)
    {
        return await _dbContext.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && !f.IsDeleted);
    }

    public async Task<List<Favorite>> GetUserFavoritesAsync(Guid userId)
    {
        return await _dbContext.Favorites
             .Where(f => f.UserId == userId && !f.IsDeleted)
             .ToListAsync();
    }

    public async Task<bool> IsFavoriteAsync(Guid userId, int animeId)
    {
        return await _dbContext.Favorites
           .AnyAsync(f => f.UserId == userId && f.Animes.Any(a => a.AnimeId == animeId) && !f.IsDeleted);
    }

    public async Task<bool> RemoveFavoriteAsync(Guid userId, int animeId)
    {
        var favorite = await _dbContext.Favorites
        .FirstOrDefaultAsync(f => f.UserId == userId && !f.IsDeleted);

        if (favorite == null) return false;

        if (!favorite.HasAnime(animeId)) return false;

        try
        {
            favorite.RemoveAnime(animeId);
            favorite.DateUpdated = DateTime.UtcNow;
            _dbContext.Favorites.Update(favorite);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing anime: {ex.Message}");
            return false;
        }
    }
}
