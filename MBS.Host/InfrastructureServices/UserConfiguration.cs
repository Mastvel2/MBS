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

        // Определяем свойства
        builder.Property(u => u.Username);

        builder.Property(u => u.AboutMe);

        builder.Property(u => u.ProfilePictureUrl);
    }
}