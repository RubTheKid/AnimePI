using AnimePI.Domain.Aggregates.UserAggregate;

namespace AnimePI.Application.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(Guid id);
    Task<User> CreateUserAsync(string firstName, string surname, string email);
    Task<User> UpdateUserAsync(Guid id, string firstName, string surname, string email);
    Task<bool> DeleteUserAsync(Guid id);
}
