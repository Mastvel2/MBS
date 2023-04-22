using MBS.Domain;

namespace MBS.Host.Host_services;

public interface IMessageService
{
    Task<IEnumerable<Message>> GetMessagesForUserAsync(int userId);
    Task<Message> SendMessageAsync(int senderId, int recipientId, string content);
}