using MBS.Domain;
using Microsoft.EntityFrameworkCore;

namespace MBS.Host.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Message> GetByIdAsync(int id)
    {
        return await _context.Messages.FindAsync(id);
    }

    public async Task<List<Message>> GetByUserIdAsync(int userId)
    {
        return await _context.Messages
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(Message message)
    {
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
    }
}
