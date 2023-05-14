namespace MBS.Infrastructure;

using MBS.Domain.Entities;
using MBS.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Контекст БД приложения.
/// </summary>
public class AppDbContext : DbContext
{
    /// <inheritdoc />
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Пользователи.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Сообщения.
    /// </summary>
    public DbSet<Message> Messages { get; set; }

    /// <summary>
    /// Идентификационные данные пользователей.
    /// </summary>
    public DbSet<UserIdentity> UserIdentities { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserIdentityConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
    }
}