using AnimePI.Domain.Aggregates.UserAggregate;
using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using AnimePI.Domain.Core;
using MediatR;

namespace AnimePI.Application.UserAggregate.Command.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
{
    private readonly IUserRepository _repository;

    public CreateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userName = new UserName(request.FirstName, request.Surname);
        var email = new Email(request.Email);
        var user = new User(userName, email);

        var createdUser = await _repository.CreateUserAsync(user);

        return new CreateUserCommandResponse(
            createdUser.Id,
            createdUser.UserName,
            createdUser.Email.Mail,
            createdUser.DateCreated
        );
    }
}