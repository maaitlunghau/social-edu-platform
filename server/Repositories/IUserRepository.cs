using shared;

namespace server.Repositories;

public interface IUserRepository
{
    public Task<ICollection<User>> GetAllUserAsync();

    public Task<User?> GetUserByIdAsync(Guid id);
    public Task<User?> GetUserByEmailAsync(string email);

    public Task<User?> CreateNewUserAsync(User u);

    public Task<User?> UpdateUserAsync(User u);

    public Task DeleteUserAsync(User user);
}
