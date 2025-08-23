using AnimePI.Domain.Aggregates.FavoriteAggregate;
using AnimePI.Domain.Aggregates.FavoriteAggregate.Interfaces;
using AnimePI.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimePI.Infra.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _dbContext;

        public FavoriteRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Favorite> AddFavoriteAsync(Favorite favorite)
        {
            await _dbContext.Favorites.AddAsync(favorite);
            await _dbContext.SaveChangesAsync();
            return favorite;
        }

        public async Task<Favorite> GetUserFavoriteAsync(Guid userId)
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
               .AnyAsync(f =>
                   f.UserId == userId &&
                   f.Animes.Any(a => a.AnimeId == animeId) &&
                   !f.IsDeleted);
        }

        public async Task<bool> RemoveFavoriteAsync(Guid userId, int animeId)
        {
            var favorite = await _dbContext.Favorites
                .FirstOrDefaultAsync(f =>
                    f.UserId == userId &&
                    f.Animes.Any(a => a.AnimeId == animeId) &&
                    !f.IsDeleted);

            if (favorite == null) return false;

            favorite.RemoveAnime(animeId);

            // Se não houver mais animes, marca como deletado
            if (!favorite.Animes.Any())
            {
                favorite.IsDeleted = true;
                favorite.DateDeleted = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
