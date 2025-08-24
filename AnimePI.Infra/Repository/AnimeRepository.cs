using AnimePI.Domain.Aggregates.AnimeAggregate;
using AnimePI.Domain.Aggregates.AnimeAggregate.Interfaces;
using AnimePI.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimePI.Infra.Repository;

public class AnimeRepository : IAnimeRepository
{
    private readonly AppDbContext _dbContext;

    public AnimeRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Anime> AddAsync(Anime anime)
    {
        await _dbContext.Animes.AddAsync(anime);
        await _dbContext.SaveChangesAsync();
        return anime;
    }

    public async Task<bool> ExistsByMalIdAsync(int malId)
    {
        return await _dbContext.Animes
            .AnyAsync(a => a.MalId == malId && !a.IsDeleted);
    }
    public async Task<Anime?> GetByMalIdAsync(int malId)
    {
        return await _dbContext.Animes
            .FirstOrDefaultAsync(a => a.MalId == malId && !a.IsDeleted);
    }

    public async Task<List<Anime>> GetAllAsync()
    {
        return await _dbContext.Animes
           .Where(a => !a.IsDeleted)
           .OrderByDescending(a => a.Score)
           .ToListAsync();
    }

    public async Task<Anime?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Animes
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
    }

    public async Task<List<Anime>> GetTopRankedAsync(int limit = 10)
    {
        return await _dbContext.Animes
            .Where(a => !a.IsDeleted && a.Rank > 0)
            .OrderBy(a => a.Rank)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Anime>> SearchByTitleAsync(string title)
    {
        return await _dbContext.Animes
           .Where(a => (a.Title.Contains(title) ||
                       a.TitleEnglish.Contains(title) ||
                       a.TitleJapanese.Contains(title)) &&
                      !a.IsDeleted)
           .OrderByDescending(a => a.Score)
           .ToListAsync();
    }

    public async Task<Anime> UpdateAsync(Anime anime)
    {
        _dbContext.Animes.Update(anime);
        await _dbContext.SaveChangesAsync();
        return anime;
    }
}
