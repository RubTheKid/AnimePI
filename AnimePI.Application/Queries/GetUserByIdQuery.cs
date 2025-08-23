using AnimePI.Domain.Aggregates.UserAggregate;
using MediatR;

namespace AnimePI.Application.Queries;

public record GetUserByIdQuery : IRequest<User>
{
    public Guid Id { get; init; }
}