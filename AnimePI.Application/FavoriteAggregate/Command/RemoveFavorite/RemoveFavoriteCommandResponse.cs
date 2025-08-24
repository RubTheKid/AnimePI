namespace AnimePI.Application.FavoriteAggregate.Command.RemoveFavorite;

public class RemoveFavoriteCommandResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }

    public RemoveFavoriteCommandResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}
