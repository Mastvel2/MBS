using MBS.Domain.Services;

namespace MBS.Host.InfrastructureServices;

/// <inheritdoc />
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext context;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UnitOfWork"/>.
    /// </summary>
    /// <param name="context">Контекст БД.</param>
    public UnitOfWork(AppDbContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }
    /// <inheritdoc />
    public int SaveChanges()
    {
        return this.context.SaveChanges();
    }

    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return this.context.SaveChangesAsync(cancellationToken);
    }
}