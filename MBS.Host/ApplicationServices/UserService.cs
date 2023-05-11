using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Domain.Services;
using MBS.Host.Dtos;
using MBS.Host.Providers;

namespace MBS.Host.ApplicationServices;

public class UserService:IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly IUserStatusProvider userStatusProvider;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IUserStatusProvider userStatusProvider)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        this.userStatusProvider = userStatusProvider ?? throw new ArgumentNullException(nameof(userStatusProvider));
    }

    public async Task<IEnumerable<UserDto>> GetAvailableUsersAsync(string currentUser)
    {
        if (string.IsNullOrWhiteSpace(currentUser))
        {
            throw new ArgumentException("Требуется текущий пользователь.");
        }

        var users = await userRepository.GetAllAsync();
        return users.Where(u => u.Username != currentUser).Select(this.GetUserDto);
    }

    public async Task<UserDto> GetUserAsync(string username)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception($"Пользователь с логином {username} не найден.");
        }

        return this.GetUserDto(user);
    }

    public async Task UpdateUserAsync(string username, UserUpdateDto userUpdateDto)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception($"Пользователь с логином {username} не найден.");
        }

        user.AboutMe = userUpdateDto.AboutMe;
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateUserAvatarAsync(string username, IFormFile avatarFile)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception("Пользователь не найден.");
        }

        await UploadFile(username, avatarFile);
    }

    public async Task UpdateLastActiveTime(string username, DateTime lastActiveTime)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception($"Пользователь с логином {username} не найден.");
        }

        user.LastActiveTime = lastActiveTime;
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync();
    }

    public void UpdateStatus(string username)
    {
        this.userStatusProvider.UpdateStatus(username);
    }

    private UserDto GetUserDto(User user)
    {
        return new UserDto
        {
            Username = user.Username,
            AboutMe = user.AboutMe,
            Status = this.userStatusProvider.GetStatus(user.Username),
        };
    }

    private async Task UploadFile(string username, IFormFile file)
    {
        var fileName = $"{username}{Path.GetExtension(file.FileName)}";
        var uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "avatars");

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var filePath = Path.Combine(uploadFolder, fileName);

        // Overwrite the existing file
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        await using var fileStream = new FileStream(filePath, FileMode.CreateNew);
        await file.CopyToAsync(fileStream);
    }
}