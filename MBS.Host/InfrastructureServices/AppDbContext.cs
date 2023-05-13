using MBS.Domain.Entities;
using MBS.Host.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace MBS.Host.InfrastructureServices;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }

    public DbSet<Message> Messages { get; set; }

    public DbSet<UserIdentity> UserIdentities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserIdentityConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
    }
}