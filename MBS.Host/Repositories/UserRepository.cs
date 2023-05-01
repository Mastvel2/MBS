using MBS.Domain.Entities;
using MBS.Domain.Repositories;

namespace MBS.Host.Repositories;

public class UserRepository : IUserRepository
{
    public Task<User> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }
}