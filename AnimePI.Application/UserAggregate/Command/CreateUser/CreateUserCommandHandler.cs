using AnimePI.Domain.Aggregates.UserAggregate;
using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using AnimePI.Domain.Core;
using MediatR;

namespace AnimePI.Application.UserAggregate.Command.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IUserRepository _repository;

    public CreateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userName = new UserName(request.FirstName, request.Surname);
        var email = new Email(request.Email);
        var user = new User(userName, email);

        return await _repository.CreateUserAsync(user);
    }
}
