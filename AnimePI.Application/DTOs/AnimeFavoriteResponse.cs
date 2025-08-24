using AnimePI.Domain.Aggregates.FavoriteAggregate;

namespace AnimePI.Application.DTOs;

public class AnimeFavoriteResponse
{
    public int AnimeId { get; set; }
    public string AnimeTitle { get; set; } = string.Empty;
    public string AnimeImageUrl { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }

    public static AnimeFavoriteResponse FromDomain(AnimeFavorite animeFavorite)
    {
        return new AnimeFavoriteResponse
        {
            AnimeId = animeFavorite.AnimeId,
            AnimeTitle = animeFavorite.AnimeTitle,
            AnimeImageUrl = animeFavorite.AnimeImageUrl,
            DateAdded = animeFavorite.DateAdded
        };
    }
}