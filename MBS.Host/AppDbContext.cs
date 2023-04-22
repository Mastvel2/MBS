using MBS.Domain;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("YourConnectionString");
    }
}