namespace MBS.Domain.Services;

/// <summary>
/// Единица работы.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Сохраняет изменения.
    /// </summary>
    /// <returns>Количество сохраненных данных.</returns>
    int SaveChanges();

    /// <summary>
    /// Сохраняет изменения.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>>
    /// <returns>Количество сохраненных данных.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}