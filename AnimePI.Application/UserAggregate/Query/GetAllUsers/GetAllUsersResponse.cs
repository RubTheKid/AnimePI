namespace AnimePI.Application.UserAggregate.Query.GetAllUsers;

public class GetAllUsersResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    public GetAllUsersResponse(Guid id, string userName, string email)
    {
        Id = id;
        UserName = userName;
        Email = email;
    }
}
