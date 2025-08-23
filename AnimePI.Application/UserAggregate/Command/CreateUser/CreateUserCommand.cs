using MediatR;

namespace AnimePI.Application.UserAggregate.Command.CreateUser;

public record CreateUserCommand : IRequest<CreateUserCommandResponse>
{
    public string FirstName { get; init; }
    public string Surname { get; init; }
    public string Email { get; init; }
}
