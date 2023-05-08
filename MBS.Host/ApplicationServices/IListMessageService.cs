using MBS.Domain.Entities;

namespace MBS.Host.ApplicationServices;

public interface IListMessageService
{
    Task<IEnumerable<User>> GetAllUsersAsync(string currentUser);
    Task<IEnumerable<User>> SearchUsersAsync(string currentUser, string searchTerm);
}