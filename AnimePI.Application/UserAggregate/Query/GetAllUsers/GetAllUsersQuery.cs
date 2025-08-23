using AnimePI.Domain.Aggregates.UserAggregate;
using MediatR;

namespace AnimePI.Application.UserAggregate.Query.GetAllUsers;

public record GetAllUsersQuery : IRequest<List<User>>;
