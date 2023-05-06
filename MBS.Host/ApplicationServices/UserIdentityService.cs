using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Domain.Services;
using MBS.Host.Dtos;
using MBS.Host.InfrastructureServices;

namespace MBS.Host.ApplicationServices;

public class UserIdentityService : IUserIdentityService
{
    private readonly IUserIdentityRepository userIdentityRepository;
    private readonly IUserRepository  userRepository;
    private readonly ITokenFactory  tokenFactory;
    private readonly IUnitOfWork  unitOfWork;

    public UserIdentityService(
        IUserIdentityRepository userIdentityRepository,
        IUnitOfWork unitOfWork,
        ITokenFactory tokenFactory,
        IUserRepository userRepository
        )
    {
        this.userIdentityRepository = userIdentityRepository ?? throw new ArgumentNullException(nameof(userIdentityRepository));
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task RegisterAsync(UserRegistrationDto dto)
    {
        var userExists = await this.userIdentityRepository
            .HasByUsernameAsync(dto.Username);
        if (userExists)
        {
            throw new Exception($"Пользователь с логином {dto.Username} уже существует.");
        }

        var userIdentity = new UserIdentity(dto.Username, dto.Password);
        this.userIdentityRepository.Add(userIdentity);
        var user = new User(dto.Username);
        this.userRepository.Add(user);
        await this.unitOfWork.SaveChangesAsync();
    }

    public async Task<TokenDto> AuthorizeAsync(UserAuthorizationDto dto)
    {
        var userIdentity = await this.userIdentityRepository
            .GetByUsernameAsync(dto.Username);
        if (userIdentity == null || !userIdentity.Password.Verify(dto.Password))
        {
            throw new Exception("Неверный логин или пароль");
        }

        return new TokenDto
        {
            Token = this.tokenFactory.Create(userIdentity.Username)
        };
    }
}