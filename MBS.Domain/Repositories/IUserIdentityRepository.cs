using MBS.Domain.Entities;

namespace MBS.Domain.Repositories
{
    public interface IUserIdentityRepository
    {
        Task<UserIdentity> GetByUsernameAsync(string name);
        Task<bool> HasByUsernameAsync(string username);
        void Add(UserIdentity userIdentity);
    }
}