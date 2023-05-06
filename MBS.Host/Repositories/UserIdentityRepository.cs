using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Host.InfrastructureServices;
using Microsoft.EntityFrameworkCore;

namespace MBS.Host.Repositories;

public class UserIdentityRepository : IUserIdentityRepository
{
    private readonly AppDbContext context;

    public UserIdentityRepository(AppDbContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<UserIdentity> GetByUsernameAsync(string username)
    {
        return this.context.UserIdentities
            .SingleOrDefaultAsync(identity => identity.Username == username);
    }

    public Task<bool> HasByUsernameAsync(string username)
    {
        return this.context.UserIdentities
            .AnyAsync(identity => identity.Username == username);
    }

    public void Add(UserIdentity userIdentity)
    {
        this.context.UserIdentities.Add(userIdentity);
    }
}