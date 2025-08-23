using MediatR;

namespace AnimePI.Application.UserAggregate.Command.UpdateUser;

public record UpdateUserCommand : IRequest<UpdateUserCommandResponse>
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string Surname { get; init; }
    public string Email { get; init; }
}
