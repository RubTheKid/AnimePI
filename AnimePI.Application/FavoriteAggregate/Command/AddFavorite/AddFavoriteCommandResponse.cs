using AnimePI.Application.DTOs;
using AnimePI.Domain.Aggregates.FavoriteAggregate;

namespace AnimePI.Application.FavoriteAggregate.Command.AddFavorite;

public class AddFavoriteCommandResponse
{
    public Guid UserId { get; set; }
    public List<AnimeFavoriteResponse> Animes { get; set; } = new List<AnimeFavoriteResponse>();
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Message { get; set; } = string.Empty;

    public static AddFavoriteCommandResponse FromDomain(Favorite favorite, string message = "Favorite added successfully")
    {
        return new AddFavoriteCommandResponse
        {
            UserId = favorite.UserId,
            Animes = favorite.Animes.Select(AnimeFavoriteResponse.FromDomain).ToList(),
            DateCreated = favorite.DateCreated,
            DateUpdated = favorite.DateUpdated,
            Message = message
        };
    }
}
