namespace MBS.Application.Services;

using MBS.Application.Dtos;
using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Domain.Services;
using Microsoft.Extensions.Hosting;

/// <inheritdoc />
public class MessageService : IMessageService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IUserRepository userRepository;
    private readonly IMessageRepository messageRepository;
    private readonly IHostEnvironment hostEnvironment;
    private readonly IMessageNotificationService notificationService;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MessageService"/>.
    /// </summary>
    /// <param name="unitOfWork">Единица работы.</param>
    /// <param name="userRepository">Репозиторий пользователей.</param>
    /// <param name="messageRepository">Репозиторий сообщений.</param>
    /// <param name="hostEnvironment">Окружение приложения.</param>
    /// <param name="notificationService">Сервис уведомлений о сообщениях.</param>
    public MessageService(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IMessageRepository messageRepository,
        IHostEnvironment hostEnvironment,
        IMessageNotificationService notificationService)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
        this.hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<MessageDto>> GetMessagesBetweenUsersAsync(string firstUser, string secondUser)
    {
        return await this.messageRepository.GetMessagesBetweenUsersAsync(firstUser, secondUser)
            .Select(this.GetMessageDto).ToListAsync();
    }

    /// <inheritdoc />
    public async Task SendMessageAsync(string sender, SendMessageDto dto)
    {
        if (!await this.userRepository.HasByUsernameAsync(dto.Receiver))
        {
            throw new Exception($"Отправитель с именем пользователя {dto.Receiver} не существует");
        }

        var message = new Message(sender, dto.Receiver, dto.Text);
        this.messageRepository.Add(message);
        await this.unitOfWork.SaveChangesAsync();
        await this.notificationService.NotifyBySendMessage(message.Receiver, new SentMessageNotificationDto
        {
            Id = message.Id,
            Sender = sender,
            Text = dto.Text,
            Timestamp = message.Timestamp,
        });
    }

    /// <inheritdoc />
    public async Task EditMessageTextAsync(string sender, EditMessageTextDto dto)
    {
        var message = await this.messageRepository.GetByIdAsync(dto.Id);
        if (message == null)
        {
            throw new Exception($"Сообщение с идентификатором {dto.Id} не существует");
        }

        if (message.Sender != sender)
        {
            throw new Exception("Сообщение принадлежит другому отправителю. Изменение невозможно!");
        }

        message.EditText(dto.Text);
        this.messageRepository.Update(message);
        await this.unitOfWork.SaveChangesAsync();
        await this.notificationService.NotifyByEditMessage(message.Receiver, new EditMessageNotificationDto
        {
            Id = message.Id,
            Sender = sender,
            Text = dto.Text,
        });
    }

    /// <inheritdoc />
    public async Task<string> UploadFileAsync(string originalFileName, Stream file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
        var uploadFolder = Path.Combine(this.hostEnvironment.ContentRootPath, "uploads");

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var filePath = Path.Combine(uploadFolder, fileName);

        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
        return fileName;
    }

    /// <inheritdoc />
    public async Task<byte[]> DownloadFileAsync(string fileName)
    {
        var filePath = Path.Combine(this.hostEnvironment.ContentRootPath, "uploads", fileName);
        if (File.Exists(filePath))
        {
            return await File.ReadAllBytesAsync(filePath);
        }

        throw new FileNotFoundException($"Файл с именем {fileName} не найден.");
    }

    /// <inheritdoc />
    public async Task DeleteMessageAsync(Guid id)
    {
        var message = await this.messageRepository.GetByIdAsync(id);
        if (message == null)
        {
            return;
        }

        this.messageRepository.Delete(message);
        await this.unitOfWork.SaveChangesAsync();
    }

    private MessageDto GetMessageDto(Message message)
    {
        return new MessageDto
        {
            Id = message.Id,
            Sender = message.Sender,
            Receiver = message.Receiver,
            Text = message.Text,
            Timestamp = message.Timestamp,
        };
    }
}