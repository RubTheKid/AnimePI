using AnimePI.Domain.Aggregates.FavoriteAggregate;
using AnimePI.Domain.Core;

namespace AnimePI.Domain.Aggregates.UserAggregate;

public class User : BaseEntity
{
    public UserName UserName { get; set; }
    public Email Email { get; set; }
    public virtual Favorite? Favorites { get; set; }

    protected User() { }

    public User(UserName userName, Email email)
    {
        UserName = userName;
        Email = email;
    }

    public void AddFavorite(int animeId, string animeTitle, string animeImageUrl)
    {
        if (Favorites == null)
        {
            Favorites = new Favorite(Id);
        }

        Favorites.AddAnime(animeId, animeTitle, animeImageUrl);
    }

    public void RemoveFavorite(int animeId)
    {
        Favorites?.RemoveAnime(animeId);
    }

    public bool HasFavorite(int animeId)
    {
        return Favorites?.HasAnime(animeId) ?? false;
    }

    public List<AnimeFavorite> GetFavorites()
    {
        return Favorites?.Animes ?? new List<AnimeFavorite>();
    }

}
