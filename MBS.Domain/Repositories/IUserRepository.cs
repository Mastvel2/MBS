using MBS.Domain.Entities;

namespace MBS.Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> GetByNameAsync(string name);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}