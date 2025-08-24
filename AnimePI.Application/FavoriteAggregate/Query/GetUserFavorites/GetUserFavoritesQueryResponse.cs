using AnimePI.Application.DTOs;
using AnimePI.Domain.Aggregates.FavoriteAggregate;

namespace AnimePI.Application.FavoriteAggregate.Query.GetUserFavorites;

public class GetUserFavoritesQueryResponse
{
    public Guid UserId { get; set; }
    public List<AnimeFavoriteResponse> Animes { get; set; } = new List<AnimeFavoriteResponse>();
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }

    public static GetUserFavoritesQueryResponse FromDomain(Favorite favorite)
    {
        return new GetUserFavoritesQueryResponse
        {
            UserId = favorite.UserId,
            Animes = favorite.Animes.Select(AnimeFavoriteResponse.FromDomain).ToList(),
            DateCreated = favorite.DateCreated,
            DateUpdated = favorite.DateUpdated
        };
    }
}