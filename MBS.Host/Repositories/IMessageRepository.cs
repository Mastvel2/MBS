using System.Collections.Generic;
using System.Threading.Tasks;
using MBS.Domain;

namespace MBS.Host.Repositories;

public interface IMessageRepository
{
    Task<Message> GetByIdAsync(int id);
    Task<List<Message>> GetByUserIdAsync(int userId);
    Task AddAsync(Message message);
}