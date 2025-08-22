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