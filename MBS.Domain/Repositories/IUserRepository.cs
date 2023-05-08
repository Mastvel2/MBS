using MBS.Domain.Entities;

namespace MBS.Domain.Repositories;

public interface IUserRepository
{
    void Add(User user);
    public Task<User> GetByUsernameAsync(string username);
    public void Update(User user);
    public Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, string currentUsername);
    public Task<IEnumerable<User>> GetAllAsync();
}