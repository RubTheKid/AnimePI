using AnimePI.Domain.Aggregates.UserAggregate;
using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using AnimePI.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimePI.Infra.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await GetUserAsync(id);
        if (user == null) return false;

        user.IsDeleted = true;
        user.DateDeleted = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _dbContext.Users
           .Where(u => !u.IsDeleted)
           .Include(u => u.Favorites)
           .ToListAsync();
    }

    public async Task<User> GetUserAsync(Guid id)
    {
        return await _dbContext.Users
            .Where(u => u.Id == id && !u.IsDeleted)
            .Include(u => u.Favorites)
            .FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users
            .Where(u => u.Email.Mail == email && !u.IsDeleted)
            .Include(u => u.Favorites)
            .FirstOrDefaultAsync();
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _dbContext.Users
            .AnyAsync(u => u.Email.Mail == email && !u.IsDeleted);
    }
}
