namespace AnimePI.Application.UserAggregate.Command.DeleteUser;

public class DeleteUserCommandResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }

    public DeleteUserCommandResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}
