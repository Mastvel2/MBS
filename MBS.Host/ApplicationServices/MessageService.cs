using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Domain.Services;
using MBS.Host.Dtos;

namespace MBS.Host.ApplicationServices;

public class MessageService : IMessageService
{
    private readonly IMessageRepository messageRepository;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly IUnitOfWork unitOfWork;
    private readonly IUserRepository userRepository;

    public MessageService(
        IMessageRepository messageRepository,
        IWebHostEnvironment webHostEnvironment,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository)
    {
        this.messageRepository = messageRepository;
        this.webHostEnvironment = webHostEnvironment;
        this.unitOfWork = unitOfWork;
        this.userRepository = userRepository;
    }

    public async Task<IEnumerable<MessageDto>> GetMessagesBetweenUsersAsync(string firstUser, string secondUser)
    {
        return await messageRepository.GetMessagesBetweenUsersAsync(firstUser, secondUser)
            .Select(this.GetMessageDto).ToListAsync();
    }

    public async Task SendMessageAsync(string sender, SendMessageDto dto)
    {
        var receiverUser = await this.userRepository.GetByUsernameAsync(dto.Receiver);
        if (receiverUser == null)
        {
            throw new Exception($"Отправитель с именем пользователя {dto.Receiver} не существует");
        }

        var message = new Message(sender, dto.Receiver, dto.Text);
        messageRepository.Add(message);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task EditMessageTextAsync(EditMessageTextDto dto)
    {
        var message = await messageRepository.GetByIdAsync(dto.Id);
        if (message == null)
        {
            throw new Exception($"Сообщение с идентификатором {dto.Id} не существует");
        }

        message.EditText(dto.Text);
        messageRepository.Update(message);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var filePath = Path.Combine(uploadFolder, fileName);

        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);

        return fileName;
    }

    public async Task<byte[]> DownloadFileAsync(string fileName)
    {
        var filePath = Path.Combine(webHostEnvironment.WebRootPath, "uploads", fileName);

        if (File.Exists(filePath))
        {
            return await File.ReadAllBytesAsync(filePath);
        }
        else
        {
            throw new FileNotFoundException($"Файл с именем {fileName} не найден.");
        }
    }

    public async Task DeleteMessageAsync(Guid id)
    {
        var message = await messageRepository.GetByIdAsync(id);
        if (message == null)
        {
            return;
        }

        messageRepository.Delete(message);
        await unitOfWork.SaveChangesAsync();
    }

    private MessageDto GetMessageDto(Message message)
    {

        return new MessageDto
        {
            Id = message.Id,
            Sender = message.Sender,
            Receiver = message.Receiver,
            EncryptedText = AesEncryption.Decrypt(message.Text),
            Timestamp = message.Timestamp,
        };
    }
}