using MediatR;

namespace AnimePI.Application.FavoriteAggregate.Command.AddFavorite;

public record AddFavoriteCommand(Guid UserId, int AnimeId, string AnimeTitle, string AnimeImageUrl) : IRequest<AddFavoriteCommandResponse>;