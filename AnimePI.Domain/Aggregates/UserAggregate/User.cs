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

}
