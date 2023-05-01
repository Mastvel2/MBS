using MBS.Domain.Entities;

namespace MBS.Domain.Repositories;

public interface IMessageRepository
{
    Task<Message> GetByIdAsync(int id);
    Task<List<Message>> GetByUserIdAsync(int userId);
    Task AddAsync(Message message);
}