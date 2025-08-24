using AnimePI.Application.Interfaces;
using MediatR;

namespace AnimePI.Application.FavoriteAggregate.Command.AddFavorite;

public class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand, AddFavoriteCommandResponse>
{
    private readonly IFavoriteService _favoriteService;

    public AddFavoriteCommandHandler(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    public async Task<AddFavoriteCommandResponse> Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
    {
        var favorite = await _favoriteService.AddFavoriteAsync(
        request.UserId,
        request.AnimeId,
        request.AnimeTitle,
        request.AnimeImageUrl
    );

        return AddFavoriteCommandResponse.FromDomain(favorite);
    }
}
