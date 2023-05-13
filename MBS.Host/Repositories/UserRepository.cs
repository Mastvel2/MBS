using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Host.InfrastructureServices;
using Microsoft.EntityFrameworkCore;

namespace MBS.Host.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext context;

    public UserRepository(AppDbContext context)
    {
        this.context = context;
    }

    public IAsyncEnumerable<User> GetAllAsync()
    {
        return context.Users.AsAsyncEnumerable();
    }

    public IAsyncEnumerable<User> SearchUsersAsync(string searchTerm, string currentUsername)
    {
        return context.Users
            .Where(u => u.Username.Contains(searchTerm) && u.Username != currentUsername)
            .AsAsyncEnumerable();
    }

    public IAsyncEnumerable<User> GetByUsernamesAsync(IEnumerable<string> usernames)
    {
        return context.Users
            .Where(u => usernames.Contains(u.Username)).AsAsyncEnumerable();
    }

    public Task<User> GetByUsernameAsync(string username)
    {
        return context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public void Add(User user)
    {
        context.Users.Add(user);
    }

    public void Update(User user)
    {
        context.Users.Update(user);
    }

}