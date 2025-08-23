using AnimePI.Domain.Core;

namespace AnimePI.Application.UserAggregate.Command.CreateUser;

public class CreateUserCommandResponse
{
    public Guid Id { get; set; }
    public UserNameDto UserName { get; set; }
    public string Email { get; set; }
    public DateTime DateCreated { get; set; }

    public CreateUserCommandResponse(Guid id, UserName userName, string email, DateTime dateCreated)
    {
        Id = id;
        UserName = new UserNameDto(userName.FirstName, userName.Surname, userName.FullName);
        Email = email;
        DateCreated = dateCreated;
    }
}

public class UserNameDto
{
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string FullName { get; set; }

    public UserNameDto(string firstName, string surname, string fullName)
    {
        FirstName = firstName;
        Surname = surname;
        FullName = fullName;
    }
}