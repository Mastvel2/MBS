namespace MBS.Application.Services;

using MBS.Application.Dtos;
using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Domain.Services;

/// <inheritdoc />
public class UserIdentityService : IUserIdentityService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IUserIdentityRepository userIdentityRepository;
    private readonly IUserRepository userRepository;
    private readonly ITokenFactory tokenFactory;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserIdentityService"/>.
    /// </summary>
    /// <param name="unitOfWork">Единица работы.</param>
    /// <param name="userIdentityRepository">Репозиторий идентификационных данных пользователей.</param>
    /// <param name="userRepository">Репозиторий пользователей.</param>
    /// <param name="tokenFactory">Фабрика токенов.</param>
    public UserIdentityService(
        IUnitOfWork unitOfWork,
        IUserIdentityRepository userIdentityRepository,
        IUserRepository userRepository,
        ITokenFactory tokenFactory)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.userIdentityRepository = userIdentityRepository ?? throw new ArgumentNullException(nameof(userIdentityRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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
            Token = this.tokenFactory.Create(userIdentity.Username, userIdentity.IsAdmin)
        };
    }
}