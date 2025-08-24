namespace AnimePI.Domain.Aggregates.AnimeAggregate.Interfaces;

public interface IAnimeRepository
{
    Task<List<Anime>> GetAllAsync();
    Task<Anime?> GetByIdAsync(Guid id);
    Task<List<Anime>> GetTopRankedAsync(int limit = 10);
    Task<List<Anime>> SearchByTitleAsync(string title);
    Task<Anime?> GetByMalIdAsync(int malId);
    Task<Anime> AddAsync(Anime anime);
    Task<Anime> UpdateAsync(Anime anime);
    Task<bool> ExistsByMalIdAsync(int malId);
}