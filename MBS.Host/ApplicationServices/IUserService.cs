using MBS.Domain.Entities;
using MBS.Host.Dtos;

namespace MBS.Host.ApplicationServices;

public interface IUserService
{
    Task UpdateUserAvatarAsync(string username, string avatarUrl);
    Task UpdateUserAsync(string username, UserUpdateDto userUpdateDto);
    Task<User> GetUserAsync(string username);
}