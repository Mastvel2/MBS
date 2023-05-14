namespace MBS.Infrastructure.Repositories;

using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

/// <inheritdoc />
public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext context;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MessageRepository"/>.
    /// </summary>
    /// <param name="context">Контекст БД.</param>
    public MessageRepository(AppDbContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public IAsyncEnumerable<Message> GetMessagesBetweenUsersAsync(string firstUser, string secondUser)
    {
        return this.context.Messages
            .Where(m => (m.Sender == firstUser && m.Receiver == secondUser)
                        || (m.Sender == secondUser && m.Receiver == firstUser))
            .OrderBy(m => m.Timestamp)
            .AsAsyncEnumerable();
    }

    /// <inheritdoc />
    public Task<Message> GetByIdAsync(Guid id)
    {
        return this.context.Messages.SingleOrDefaultAsync(m => m.Id == id);
    }

    /// <inheritdoc />
    public void Add(Message message)
    {
        this.context.Messages.Add(message);
    }

    /// <inheritdoc />
    public void Update(Message message)
    {
        this.context.Messages.Update(message);
    }

    /// <inheritdoc />
    public void Delete(Message message)
    {
        this.context.Messages.Remove(message);
    }
}