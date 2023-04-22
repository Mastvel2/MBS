using MBS.Domain;

namespace MBS.Host.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}