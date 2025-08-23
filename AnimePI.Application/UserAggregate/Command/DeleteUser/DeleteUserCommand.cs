using MediatR;

namespace AnimePI.Application.UserAggregate.Command.DeleteUser;

public class DeleteUserCommand : IRequest<DeleteUserCommandResponse>
{
    public Guid Id { get; init; }
}
