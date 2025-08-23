using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using AnimePI.Domain.Core;
using MediatR;

namespace AnimePI.Application.UserAggregate.Command.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse>
{
    private readonly IUserRepository _repository;

    public UpdateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _repository.GetUserAsync(request.Id);

        if (existingUser == null)
        {
            throw new ArgumentException("User not found");
        }

        var userName = new UserName(request.FirstName, request.Surname);
        var email = new Email(request.Email);

        existingUser.UserName = userName;
        existingUser.Email = email;
        existingUser.DateUpdated = DateTime.UtcNow;

        var updatedUser = await _repository.UpdateUserAsync(existingUser);

        return new UpdateUserCommandResponse(
            updatedUser.Id,
            updatedUser.UserName,
            updatedUser.Email.Mail,
            updatedUser.DateCreated,
            updatedUser.DateUpdated.Value
        );
    }
}