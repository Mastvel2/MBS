namespace MBS.Infrastructure.Repositories;

using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

/// <inheritdoc />
public class UserRepository : IUserRepository
{
    private readonly AppDbContext context;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserRepository"/>.
    /// </summary>
    /// <param name="context">Контекст БД.</param>
    public UserRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc />
    public IAsyncEnumerable<User> GetAllAsync()
    {
        return this.context.Users.AsAsyncEnumerable();
    }

    /// <inheritdoc />
    public IAsyncEnumerable<User> SearchUsersAsync(string searchTerm)
    {
        return this.context.Users
            .Where(u => u.Username.Contains(searchTerm))
            .AsAsyncEnumerable();
    }

    /// <inheritdoc />
    public IAsyncEnumerable<User> GetByUsernamesAsync(IEnumerable<string> usernames)
    {
        return this.context.Users
            .Where(u => usernames.Contains(u.Username)).AsAsyncEnumerable();
    }

    /// <inheritdoc />
    public Task<User> GetByUsernameAsync(string username)
    {
        return this.context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    /// <inheritdoc />
    public Task<bool> HasByUsernameAsync(string username)
    {
        return this.context.Users.AnyAsync(u => u.Username == username);
    }

    /// <inheritdoc />
    public void Add(User user)
    {
        this.context.Users.Add(user);
    }

    /// <inheritdoc />
    public void Update(User user)
    {
        this.context.Users.Update(user);
    }
}