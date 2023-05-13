using MBS.Domain.Entities;

namespace MBS.Domain.Repositories;

public interface IUserRepository
{
    IAsyncEnumerable<User> GetAllAsync();
    IAsyncEnumerable<User> SearchUsersAsync(string searchTerm, string currentUsername);
    IAsyncEnumerable<User> GetByUsernamesAsync(IEnumerable<string> usernames);
    Task<User> GetByUsernameAsync(string username);
    void Add(User user);
    void Update(User user);

}