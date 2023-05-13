using MBS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBS.Host.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Устанавливаем первичный ключ
        builder.HasKey(u => u.Username);

        // Устанавливаем имя схемы и таблицы
        builder.ToTable("users", "public");

        // Определяем свойства
        builder.Property(u => u.Username)
            .HasColumnName("username");

        builder.Property(u => u.AboutMe)
            .HasColumnName("about_me");

        builder.Property(u => u.LastActivityTime)
            .HasColumnName("last_activity_time");
    }
}