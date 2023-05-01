using MBS.Domain.Services;

namespace MBS.Host.InfrastructureServices;

/// <inheritdoc />
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UnitOfWork"/>.
    /// </summary>
    /// <param name="context">Контекст БД.</param>
    public UnitOfWork(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    /// <inheritdoc />
    public int SaveChanges()
    {
        return this._context.SaveChanges();
    }

    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return this._context.SaveChangesAsync(cancellationToken);
    }
}