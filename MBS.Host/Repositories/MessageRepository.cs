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

    public async Task<Message> GetLatestMessageAsync(string user1, string user2)
    {
        return await context.Messages
            .Where(m => (m.Sender == user1 && m.Receiver == user2) || (m.Sender == user2 && m.Receiver == user1))
            .OrderByDescending(m => m.Timestamp)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(string user1, string user2)
    {
        return await context.Messages
            .Where(m => (m.Sender == user1 && m.Receiver == user2) || (m.Sender == user2 && m.Receiver == user1))
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task<Message> GetMessageByIdAsync(int id)
    {
        return await context.Messages.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddMessageAsync(Message message)
    {
        await context.Messages.AddAsync(message);
        await context.SaveChangesAsync();
    }

    public async Task UpdateMessageAsync(Message message)
    {
        context.Messages.Update(message);
        await context.SaveChangesAsync();
    }

    public async Task DeleteMessageAsync(int id)
    {
        var message = await context.Messages.FindAsync(id);
        if (message != null)
        {
            context.Messages.Remove(message);
            await context.SaveChangesAsync();
        }
    }
}