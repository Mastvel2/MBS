using MBS.Domain.Entities;
using MBS.Host.Dtos;

namespace MBS.Host.ApplicationServices;

public interface IUserService
{
    Task UpdateUserAvatarAsync(string username, IFormFile avatarFile);
    Task UpdateUserAsync(string username, UserUpdateDto userUpdateDto);
    Task<User> GetUserAsync(string username);
    Task UpdateLastLoginTime(string username, DateTime lastLoginTime);
    Task UpdateStatus(string username, string status);

}