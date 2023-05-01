using MBS.Domain.Entities;
using MBS.Host.Dtos;

namespace MBS.Host.ApplicationServices
{
    public interface IUserIdentityService
    {
        Task RegisterAsync(UserRegistrationDto dto);
        Task<User> AuthorizeAsync(UserAuthorizationDto dto);
    }
}