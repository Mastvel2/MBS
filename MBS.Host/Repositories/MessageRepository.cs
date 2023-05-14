using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Host.InfrastructureServices;
using Microsoft.EntityFrameworkCore;

namespace MBS.Host.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext context;

    public MessageRepository(AppDbContext contextDb)
    {
        context = contextDb;
    }

    public IAsyncEnumerable<Message> GetMessagesBetweenUsersAsync(string firstUser, string secondUser)
    {
        return context.Messages
            .Where(m => (m.Sender == firstUser && m.Receiver == secondUser)
                        || (m.Sender == secondUser && m.Receiver == firstUser))
            .OrderBy(m => m.Timestamp)
            .AsAsyncEnumerable();
    }

    public Task<Message> GetByIdAsync(Guid id)
    {
        return context.Messages.SingleOrDefaultAsync(m => m.Id == id);
    }

    public void Add(Message message)
    {
        context.Messages.Add(message);
    }

    public void Update(Message message)
    {
        context.Messages.Update(message);
    }

    public void Delete(Message message)
    {
        context.Messages.Remove(message);
    }
}