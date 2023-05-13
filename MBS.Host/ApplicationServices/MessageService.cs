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

    public MessageService(IMessageRepository messageRepository,
        IWebHostEnvironment webHostEnvironment,IUnitOfWork unitOfWork)
    {
        this.messageRepository = messageRepository;
        this.webHostEnvironment = webHostEnvironment;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Message> GetLatestMessageAsync(string user1, string user2)
    {
        var latestMessage = await messageRepository.GetLatestMessageAsync(user1, user2);

        if (latestMessage != null)
        {
            latestMessage.EncryptedText = AesEncryption.Decrypt(latestMessage.EncryptedText);
        }

        return latestMessage;
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(string user1, string user2)
    {
        var messages = (await messageRepository.GetMessagesAsync(user1, user2)).ToList();

        foreach (var message in messages)
        {
            message.EncryptedText = AesEncryption.Decrypt(message.EncryptedText);
        }

        return messages;
    }

    public async Task<Message> SendMessageAsync(Message message)
    {
        message.EncryptedText = AesEncryption.Encrypt(message.EncryptedText);
        await messageRepository.AddMessageAsync(message);
        await unitOfWork.SaveChangesAsync();
        return message;
    }

    public async Task UpdateMessageAsync(int messageId, string updatedText)
    {
        var message = await messageRepository.GetMessageByIdAsync(messageId);
        if (message != null)
        {
            message.EncryptedText = updatedText;
            await messageRepository.UpdateMessageAsync(message);
            await unitOfWork.SaveChangesAsync();
        }
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

    private MessageDto GetMessageDto(Message message)
    {
        return new MessageDto
        {
            Id = message.Id,
            Sender = message.Sender,
            Receiver = message.Receiver,
            EncryptedText = AesEncryption.Decrypt(message.EncryptedText),
            Timestamp = message.Timestamp,
        };
    }
}