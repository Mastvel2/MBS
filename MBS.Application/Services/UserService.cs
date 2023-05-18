namespace MBS.Application.Services;

using MBS.Application.Dtos;
using MBS.Application.Providers;
using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Domain.Services;
using Microsoft.Extensions.Hosting;

/// <inheritdoc />
public class UserService : IUserService
{
    private static readonly TimeSpan MaxInactiveTime = new(0, 5, 0);
    private readonly IUnitOfWork unitOfWork;
    private readonly IUserRepository userRepository;
    private readonly IHostEnvironment hostEnvironment;
    private readonly IUserActivityProvider userActivityProvider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MessageService"/>.
    /// </summary>
    /// <param name="unitOfWork">Единица работы.</param>
    /// <param name="userRepository">Репозиторий пользователей.</param>
    /// <param name="hostEnvironment">Окружение приложения.</param>
    /// <param name="userActivityProvider">Провайдер активности пользователей.</param>
    public UserService(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IHostEnvironment hostEnvironment,
        IUserActivityProvider userActivityProvider)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        this.userActivityProvider = userActivityProvider ?? throw new ArgumentNullException(nameof(userActivityProvider));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserDto>> GetAvailableUsersAsync(string currentUser)
    {
        var users = this.userRepository.GetAllAsync();
        return await users.Where(u => u.Username != currentUser)
            .Select(this.GetUserDto).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<UserDto> GetUserAsync(string username)
    {
        var user = await this.userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception($"Пользователь с логином {username} не найден.");
        }

        return this.GetUserDto(user);
    }

    /// <inheritdoc />
    public async Task UpdateUserAsync(string username, UserUpdateDto userUpdateDto)
    {
        var user = await this.userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception($"Пользователь с логином {username} не найден.");
        }

        user.AboutMe = userUpdateDto.AboutMe;
        user.DisplayName = userUpdateDto.DisplayName;
        this.userRepository.Update(user);
        await this.unitOfWork.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateUserAvatarAsync(string username, string fileName, Stream avatarFile)
    {
        if (!await this.userRepository.HasByUsernameAsync(username))
        {
            throw new Exception($"Пользователь c логином {username} не найден.");
        }

        await this.UploadFile(username, fileName, avatarFile);
    }

    /// <inheritdoc />
    public async Task UpdateLastActiveTimesAsync()
    {
        var userActivities = this.userActivityProvider.GetActivities();
        var users = this.userRepository.GetByUsernamesAsync(userActivities.Keys);
        var currentTime = DateTime.Now;
        await foreach (var user in users)
        {
            var lastActivityTime = userActivities[user.Username];
            if (currentTime - lastActivityTime >= MaxInactiveTime)
            {
                this.userActivityProvider.DeleteActivity(user.Username);
            }

            user.LastActivityTime = lastActivityTime;
            this.userRepository.Update(user);
        }

        await this.unitOfWork.SaveChangesAsync();
    }

    private UserDto GetUserDto(User user)
    {
        return new UserDto
        {
            Username = user.Username,
            AboutMe = user.AboutMe,
            Status = this.GetUserStatus(user.Username),
            DisplayName = user.DisplayName,
            LastActivityTime = user.LastActivityTime,
        };
    }

    private UserStatus GetUserStatus(string username)
    {
        var activityTime = this.userActivityProvider.GetActivity(username);
        if (activityTime == null)
        {
            return UserStatus.Offline;
        }

        return DateTime.Now - activityTime.Value < MaxInactiveTime ? UserStatus.Online : UserStatus.Offline;
    }

    private async Task UploadFile(string username, string originalFileName, Stream avatarFile)
    {
        var fileName = $"{username}{Path.GetExtension(originalFileName)}";
        var uploadFolder = Path.Combine(this.hostEnvironment.ContentRootPath, "avatars");

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var filePath = Path.Combine(uploadFolder, fileName);
        var existedFile = Directory.GetFiles(uploadFolder)
            .SingleOrDefault(f => Path.GetFileNameWithoutExtension(f) == username);
        if (existedFile != null)
        {
            File.Delete(existedFile);
        }

        await using var fileStream = new FileStream(filePath, FileMode.CreateNew);
        await avatarFile.CopyToAsync(fileStream);
    }
}