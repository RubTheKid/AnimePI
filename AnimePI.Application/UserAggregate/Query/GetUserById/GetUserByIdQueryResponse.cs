using AnimePI.Domain.Aggregates.FavoriteAggregate;
using AnimePI.Domain.Core;

namespace AnimePI.Application.UserAggregate.Query.GetUserById;

public class GetUserByIdQueryResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime DateCreated { get; set; }
    public List<AnimeFavorite>? Favorites { get; set; }

    public GetUserByIdQueryResponse(Guid id, Email email, DateTime dateCreated, UserName userName, List<AnimeFavorite>? favorites = null)
    {
        Id = id;
        UserName = userName.FullName;
        Email = email.Mail;
        DateCreated = dateCreated;
        Favorites = favorites ?? new List<AnimeFavorite>();
    }
}