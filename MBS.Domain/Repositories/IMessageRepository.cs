using MBS.Domain.Entities;

namespace MBS.Domain.Repositories;

public interface IMessageRepository
{
    IAsyncEnumerable<Message> GetMessagesBetweenUsersAsync(string firstUser, string secondUser);
    Task<Message> GetByIdAsync(Guid id);
    void Add(Message message);
    void Update(Message message);
    void Delete(Message message);
}