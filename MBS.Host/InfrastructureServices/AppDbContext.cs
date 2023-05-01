using MBS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MBS.Host.InfrastructureServices;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Message> Messages { get; set; }
    
    public DbSet<UserIdentity> UserIdentities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("YourConnectionString");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserIdentityConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}