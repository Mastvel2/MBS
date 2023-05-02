using MBS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBS.Host.InfrastructureServices;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Устанавливаем первичный ключ
        builder.HasKey(u => u.Username);
        
        // Устанавливаем имя схемы и таблицы
        builder.ToTable("Users", "dbo");
        
        // Определяем свойства
        builder.Property(u => u.Username)
            .HasColumnName("Username");

        builder.Property(u => u.AboutMe)
            .HasColumnName("AboutMe");
        
        builder.Property(u => u.DateOfBirth)
            .HasColumnName("DateOfBirth");

        builder.Property(u => u.ProfilePictureUrl)
            .HasColumnName("ProfilePictureUrl");
        
        builder.Property(u => u.LastLogin)
            .HasColumnName("LastLogin");
    }
}