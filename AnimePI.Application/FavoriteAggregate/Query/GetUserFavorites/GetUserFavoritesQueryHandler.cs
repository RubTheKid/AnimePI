using AnimePI.Application.Interfaces;
using MediatR;

namespace AnimePI.Application.FavoriteAggregate.Query.GetUserFavorites;

public class GetUserFavoritesQueryHandler : IRequestHandler<GetUserFavoritesQuery, List<GetUserFavoritesQueryResponse>>
{
    private readonly IFavoriteService _favoriteService;

    public GetUserFavoritesQueryHandler(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    public async Task<List<GetUserFavoritesQueryResponse>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
    {
        var favorites = await _favoriteService.GetUserFavoritesAsync(request.UserId);
        return favorites.Select(GetUserFavoritesQueryResponse.FromDomain).ToList();
    }
}