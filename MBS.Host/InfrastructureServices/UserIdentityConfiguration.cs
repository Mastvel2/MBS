using MBS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBS.Host.InfrastructureServices;

public class UserIdentityConfiguration : IEntityTypeConfiguration<UserIdentity>
{
    public void Configure(EntityTypeBuilder<UserIdentity> builder)
    {
        // Устанавливаем первичный ключ
        builder.HasKey(ui => ui.Username);
        // Устанавливаем имя схемы и таблицы
        builder.ToTable("UserIdentities", "public");
        
        builder.Property(ui => ui.Username);
        // Маппим приватные поля hash и salt для EF Core
        builder.Property<string>("hash")
            .HasColumnName("Hash");
        builder.Property<string>("salt")
            .HasColumnName("Salt");
    }
}

