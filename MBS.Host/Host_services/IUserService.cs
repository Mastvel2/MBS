using System.Collections.Generic;
using System.Threading.Tasks;
using MBS.Host;
using MBS.Host.Dtos;

namespace MBS.Domain.Services;


public interface IUserService
{
        Task<User> AuthenticateAsync(string email, string password);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> CreateAsync(User user, string password);
        Task UpdateAsync(User user, string password = null);
        Task DeleteAsync(int id);
        string GenerateJwtToken(User user);
}