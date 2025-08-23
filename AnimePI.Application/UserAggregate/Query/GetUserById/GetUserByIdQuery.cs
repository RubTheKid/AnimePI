using MediatR;

namespace AnimePI.Application.UserAggregate.Query.GetUserById;

public record GetUserByIdQuery : IRequest<GetUserByIdQueryResponse?>
{
    public Guid Id { get; init; }
}
