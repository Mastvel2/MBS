using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Domain.Services;
using MBS.Host.Dtos;
using MBS.Host.InfrastructureServices;

namespace MBS.Host.ApplicationServices;

public class UserIdentityService : IUserIdentityService
{
    private readonly IUserIdentityRepository _userIdentityRepository;
    private readonly IUserRepository  _userRepository;
    private readonly ITokenFactory  _tokenFactory;
    private readonly IUnitOfWork  _unitOfWork;

    public UserIdentityService(
        IUserIdentityRepository userIdentityRepository,
        IUnitOfWork unitOfWork,
        ITokenFactory tokenFactory,
        IUserRepository userRepository
        )
    {
        this. _userIdentityRepository = userIdentityRepository ?? throw new ArgumentNullException(nameof(userIdentityRepository));
        this. _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this. _tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
        this. _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task RegisterAsync(UserRegistrationDto dto)
    {
        var userExists = await this._userIdentityRepository.HasByUsernameAsync(dto.Username);
        if (userExists)
        {
            throw new Exception($"Пользователь с логином {dto.Username} уже существует.");
        }

        var userIdentity = new UserIdentity(dto.Username, dto.Password);
        await this._userIdentityRepository.Add(userIdentity);
        var user = new User(dto.Username); 
        this._userRepository.Add(user);
        await this._unitOfWork.SaveChangesAsync();
    }

    public async Task<TokenDto> AuthorizeAsync(UserAuthorizationDto dto)
    {
        var userIdentity = await this._userIdentityRepository.GetByUsernameAsync(dto.Username);
        if (userIdentity == null)
        {
            throw new Exception("Пользователь с логином {dto.Username} не существует.");
        }

        return new TokenDto()
        {
            Token = this._tokenFactory.Create(userIdentity.Username)
        };
    }
}