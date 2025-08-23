namespace AnimePI.Domain.Aggregates.UserAggregate.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserAsync(Guid id);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(Guid id);
    Task<User> GetUserByEmailAsync(string email);
    Task<bool> UserExistsAsync(string email);
}
