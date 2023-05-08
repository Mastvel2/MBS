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

    public async Task<User> GetByUsernameAsync(string username)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public void Add(User user)
    {
        context.Users.Add(user);
        context.SaveChanges();
    }

    public void Update(User user)
    {
        context.Users.Update(user);
    }

    public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, string currentUsername)
    {
        return await context.Users
            .Where(u => u.Username.Contains(searchTerm) && u.Username != currentUsername)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }
}