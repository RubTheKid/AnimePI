using MediatR;

namespace AnimePI.Application.FavoriteAggregate.Query.GetUserFavorites;

public record GetUserFavoritesQuery(Guid UserId) : IRequest<List<GetUserFavoritesQueryResponse>>;