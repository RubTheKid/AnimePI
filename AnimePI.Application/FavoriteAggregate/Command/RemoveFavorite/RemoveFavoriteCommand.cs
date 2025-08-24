using MediatR;

namespace AnimePI.Application.FavoriteAggregate.Command.RemoveFavorite;

public record RemoveFavoriteCommand(Guid UserId, int AnimeId) : IRequest<RemoveFavoriteCommandResponse>;
