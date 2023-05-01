using MBS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBS.Host.InfrastructureServices;

public class UserIdentityConfiguration : IEntityTypeConfiguration<UserIdentity>
{
    public void Configure(EntityTypeBuilder<UserIdentity> builder)
    {
        // Устанавливаем первичный ключ
        builder.HasKey(u => u.Username);
        builder.Property(ui => ui.Username);
        // Маппим приватные свойства hash и salt для EF Core
        builder.Property<string>("hash");
        builder.Property<string>("salt");
        // Устанавливаем индексы
        builder.HasIndex(ui => ui.Username);
    }
}

