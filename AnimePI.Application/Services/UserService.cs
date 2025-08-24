using AnimePI.Application.Interfaces;
using AnimePI.Domain.Aggregates.UserAggregate;
using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using AnimePI.Domain.Core;

namespace AnimePI.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        return await _userRepository.GetUserAsync(id);
    }

    public async Task<User> CreateUserAsync(string firstName, string surname, string email)
    {
        var userName = new UserName(firstName, surname);
        var emailObj = new Email(email);
        var user = new User(userName, emailObj);

        return await _userRepository.CreateUserAsync(user);
    }

    public async Task<User> UpdateUserAsync(Guid id, string firstName, string surname, string email)
    {
        var user = await _userRepository.GetUserAsync(id);
        if (user == null)
            throw new ArgumentException("User not found");

        user.UserName = new UserName(firstName, surname);
        user.Email = new Email(email);
        user.DateUpdated = DateTime.Now;

        return await _userRepository.UpdateUserAsync(user);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        return await _userRepository.DeleteUserAsync(id);
    }
}
