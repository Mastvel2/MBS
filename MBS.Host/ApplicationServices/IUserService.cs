using MBS.Domain.Entities;
using MBS.Host.Dtos;

namespace MBS.Host.ApplicationServices;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAvailableUsersAsync(string currentUser);
    Task<UserDto> GetUserAsync(string username);
    Task UpdateUserAsync(string username, UserUpdateDto userUpdateDto);
    Task UpdateUserAvatarAsync(string username, IFormFile avatarFile);
    Task UpdateLastActiveTime(string username, DateTime lastActiveTime);
    void UpdateStatus(string username);

}