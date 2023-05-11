using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Domain.Services;
using MBS.Host.Dtos;

namespace MBS.Host.ApplicationServices;

public class UserService:IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IWebHostEnvironment webHostEnvironment;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.webHostEnvironment = webHostEnvironment;
    }

    public async Task UpdateUserAvatarAsync(string username, IFormFile avatarFile)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        string newAvatarUrl = await UploadFileAndGetUrl(username, avatarFile);

        user.ProfilePictureUrl = newAvatarUrl;
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync();
    }

    private async Task<string> UploadFileAndGetUrl(string username, IFormFile file)
    {
        string fileName = $"{username}{Path.GetExtension(file.FileName)}";
        string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "avatars");

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        string filePath = Path.Combine(uploadFolder, fileName);

        // Overwrite the existing file
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        await using var fileStream = new FileStream(filePath, FileMode.CreateNew);
        await file.CopyToAsync(fileStream);

        // Return the relative URL of the file
        return $"/avatars/{fileName}";
    }

    public async Task UpdateUserAsync(string username, UserUpdateDto userUpdateDto)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception($"Пользователь с логином {username} не найден.");
        }

        user.AboutMe = userUpdateDto.AboutMe;
        user.Status = userUpdateDto.Status;
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync();
    }


    public async Task<User> GetUserAsync(string username)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception($"Пользователь с логином {username} не найден.");
        }

        return user;
    }
    public async Task UpdateStatus(string username, string status)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception($"Пользователь с логином {username} не найден.");
        }

        user.Status = status;
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateLastLoginTime(string username, DateTime lastLoginTime)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception($"Пользователь с логином {username} не найден.");
        }

        user.LastLoginTime = lastLoginTime;
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync();
    }
}