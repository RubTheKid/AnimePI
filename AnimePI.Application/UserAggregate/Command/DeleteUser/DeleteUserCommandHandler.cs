using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using MediatR;

namespace AnimePI.Application.UserAggregate.Command.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserCommandResponse>
{
    private readonly IUserRepository _repository;
    public DeleteUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _repository.DeleteUserAsync(request.Id);

            if (success)
            {
                return new DeleteUserCommandResponse(true, "User deleted");
            }

            return new DeleteUserCommandResponse(false, "User not found or already deleted");
        }
        catch (Exception ex)
        {
            return new DeleteUserCommandResponse(false, $"Error deleting user: {ex.Message}");
        }
    }
}
