using AnimePI.Domain.Core;

namespace AnimePI.Domain.Aggregates.FavoriteAggregate;

public class Favorite : BaseEntity
{
    public Guid UserId { get; set; }
    public List<AnimeFavorite> Animes { get; set; } = new List<AnimeFavorite>();

    protected Favorite() { }

    public Favorite(Guid userId)
    {
        UserId = userId;
    }

    public void AddAnime(int animeId, string animeTitle, string animeImageUrl)
    {
        if (!Animes.Any(a => a.AnimeId == animeId))
        {
            Animes.Add(new AnimeFavorite(animeId, animeTitle, animeImageUrl));
        }
    }

    public void RemoveAnime(int animeId)
    {
        var anime = Animes.FirstOrDefault(a => a.AnimeId == animeId);
        if (anime != null)
        {
            Animes.Remove(anime);
            DateUpdated = DateTime.UtcNow;
        }
    }

    public bool HasAnime(int animeId)
    {
        return Animes.Any(a => a.AnimeId == animeId);
    }

    public bool ToggleAnime(int animeId, string animeTitle, string animeImageUrl)
    {
        if (HasAnime(animeId))
        {
            RemoveAnime(animeId);
            return false;
        }
        else
        {
            AddAnime(animeId, animeTitle, animeImageUrl);
            return true;
        }
    }

}

public class AnimeFavorite
{
    public int AnimeId { get; set; }
    public string AnimeTitle { get; set; }
    public string AnimeImageUrl { get; set; }
    public DateTime DateAdded { get; set; }

    public AnimeFavorite(int animeId, string animeTitle, string animeImageUrl)
    {
        AnimeId = animeId;
        AnimeTitle = animeTitle;
        AnimeImageUrl = animeImageUrl;
        DateAdded = DateTime.Now;
    }

}