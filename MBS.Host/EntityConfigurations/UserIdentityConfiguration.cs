using MBS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBS.Host.EntityConfigurations;

public class UserIdentityConfiguration : IEntityTypeConfiguration<UserIdentity>
{
    public void Configure(EntityTypeBuilder<UserIdentity> builder)
    {
        // Устанавливаем первичный ключ
        builder.HasKey(ui => ui.Username);

        // Устанавливаем имя схемы и таблицы
        builder.ToTable("user_identities", "public");

        builder.Property(ui => ui.Username).HasColumnName("username");

        builder.Property(ui => ui.IsAdmin).HasColumnName("is_admin");

        builder.OwnsOne(ui => ui.Password,
            b =>
            {
                // Маппим приватные поля hash и salt для EF Core
                b.Property<string>("hash")
                    .IsRequired()
                    .HasColumnName("hash");
                b.Property<string>("salt")
                    .IsRequired()
                    .HasColumnName("salt");
            });
    }
}