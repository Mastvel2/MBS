using MBS.Domain.Entities;
using MBS.Domain.Repositories;

namespace MBS.Host.ApplicationServices;

public class ListMessageService: IListMessageService
{

    private readonly IUserRepository userRepository;
    private readonly IMessageService messageService;

    public ListMessageService(IUserRepository userRepository,IMessageService messageService)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(string currentUser)
    {
        if (string.IsNullOrWhiteSpace(currentUser))
        {
            throw new ArgumentException("Требуется текущий пользователь.");
        }

        var users = await userRepository.GetAllAsync();
        return users.Where(u => u.Username != currentUser);
    }

    public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, string currentUsername)
    {
        searchTerm = searchTerm ?? string.Empty;
        return await userRepository.SearchUsersAsync(searchTerm, currentUsername);
    }
    public async Task<IEnumerable<ListMessage>> GetListMessagesAsync(string currentUsername)
    {
        var users = await userRepository.GetAllAsync();
        var listMessages = new List<ListMessage>();

        foreach (var user in users)
        {
            if (user.Username == currentUsername) continue;
            var latestMessage = await messageService.GetLatestMessageAsync(currentUsername, user.Username);
            var listMessage = new ListMessage
            {
                Username = user.Username,
                LastMessage = latestMessage?.EncryptedText ?? "Начните диалог прямо сейчас!",
                Timestamp = latestMessage?.Timestamp ?? default(DateTime)
            };

            listMessages.Add(listMessage);
        }

        return listMessages;
    }
}