namespace MBS.Infrastructure.Repositories;

using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

/// <inheritdoc />
public class UserIdentityRepository : IUserIdentityRepository
{
    private readonly AppDbContext context;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserIdentityRepository"/>.
    /// </summary>
    /// <param name="context">Контекст БД.</param>
    public UserIdentityRepository(AppDbContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public Task<UserIdentity> GetByUsernameAsync(string username)
    {
        return this.context.UserIdentities
            .SingleOrDefaultAsync(identity => identity.Username == username);
    }

    /// <inheritdoc />
    public Task<bool> HasByUsernameAsync(string username)
    {
        return this.context.UserIdentities
            .AnyAsync(identity => identity.Username == username);
    }

    /// <inheritdoc />
    public void Add(UserIdentity userIdentity)
    {
        this.context.UserIdentities.Add(userIdentity);
    }
}