using MBS.Domain.Entities;

namespace MBS.Domain.Repositories;

public interface IMessageRepository
{
    Task<Message> GetLatestMessageAsync(string user1, string user2);
    Task<IEnumerable<Message>> GetMessagesAsync(string user1, string user2);
    Task<Message> GetMessageByIdAsync(int id);
    Task AddMessageAsync(Message message);
    Task UpdateMessageAsync(Message message);
    Task DeleteMessageAsync(int id);
}