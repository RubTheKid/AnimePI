using AnimePI.Domain.Aggregates.UserAggregate;
using MediatR;

namespace AnimePI.Application.UserAggregate.Query.GetUserById;

public record GetUserByIdQuery : IRequest<User>
{
    public Guid Id { get; init; }
}
