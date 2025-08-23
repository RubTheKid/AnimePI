using AnimePI.Domain.Aggregates.UserAggregate;
using MediatR;

namespace AnimePI.Application.Commands;

public record CreateUserCommand : IRequest<User>
{
    public string FirstName { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}