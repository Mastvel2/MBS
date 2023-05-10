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

    public async Task UpdateUserAvatarAsync(string username, string avatarUrl)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception($"Пользователь с логином {username} не найден.");
        }

        user.ProfilePictureUrl = avatarUrl;
        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateUserAvatarAsync(string username, IFormFile avatarFile)
    {
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        // Загрузите файл на сервер и получите URL-адрес сохраненного файла
        string newAvatarUrl = await UploadFileAndGetUrl(avatarFile);

        // Обновите URL-адрес аватара пользователя и сохраните изменения в базе данных
        user.ProfilePictureUrl = newAvatarUrl;
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync();
    }

    private async Task<string> UploadFileAndGetUrl(IFormFile file)
    {
        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "avatars");

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        string filePath = Path.Combine(uploadFolder, fileName);

        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);

        // Возвращает относительный URL файла
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
}