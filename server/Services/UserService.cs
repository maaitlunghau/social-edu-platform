using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Repositories;
using shared;

namespace server.Services;

public class UserService : IUserRepository
{
    private readonly DataContext _dbContext;

    public UserService(DataContext dbContext) => _dbContext = dbContext;


    public async Task<ICollection<User>> GetAllUserAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByEmailAsync(string Email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == Email);
    }

    public async Task<User?> CreateNewUserAsync(User u)
    {
        await _dbContext.Users.AddAsync(u);
        await _dbContext.SaveChangesAsync();

        return u;
    }

    public async Task<User?> UpdateUserAsync(User u)
    {
        _dbContext.Users.Update(u);
        await _dbContext.SaveChangesAsync();

        return u;
    }

    public async Task DeleteUserAsync(User u)
    {
        _dbContext.Users.Remove(u);
        await _dbContext.SaveChangesAsync();
    }
}
