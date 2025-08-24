using AnimePI.Application.Interfaces;
using MediatR;

namespace AnimePI.Application.FavoriteAggregate.Command.RemoveFavorite;

public class RemoveFavoriteCommandHandler : IRequestHandler<RemoveFavoriteCommand, RemoveFavoriteCommandResponse>
{
    private readonly IFavoriteService _favoriteService;

    public RemoveFavoriteCommandHandler(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    public async Task<RemoveFavoriteCommandResponse> Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _favoriteService.RemoveFavoriteAsync(request.UserId, request.AnimeId);

            if (result)
            {
                return new RemoveFavoriteCommandResponse(true, "Anime removed successfully");
            }
            return new RemoveFavoriteCommandResponse(false, "Anime or user not found");
        }
        catch (Exception ex)
        {
            return new RemoveFavoriteCommandResponse(false, $"Error removing anime: {ex.Message}");
        }
    }
}