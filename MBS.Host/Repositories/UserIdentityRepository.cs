using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Host.InfrastructureServices;
using Microsoft.EntityFrameworkCore;

namespace MBS.Host.Repositories;

public class UserIdentityRepository : IUserIdentityRepository
{
    private readonly AppDbContext _context;

    public UserIdentityRepository(AppDbContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<UserIdentity> GetByUsernameAsync(string username)
    {
        return await this._context.UserIdentities.SingleOrDefaultAsync(identity => identity.Username == username);
    }

    public Task<bool> HasByUsernameAsync(string username)
    {
        return this._context.UserIdentities.AnyAsync(identity => identity.Username == username);
    }

    public async Task Add(UserIdentity userIdentity)
    {
        await _context.UserIdentities.AddAsync(userIdentity);
    }
}