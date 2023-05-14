namespace MBS.Infrastructure.EntityConfigurations;

using MBS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Конфигурация сущности <see cref="Message"/>.
/// </summary>
public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        // Установливаем первичный ключ для сущности Message
        builder.HasKey(m => m.Id);

        // Установливаем имя схемы и таблицы
        builder.ToTable("messages", "public");

        builder.Property(m => m.Id)
            .HasColumnName("id");

        builder.Property(m => m.Sender)
            .HasColumnName("sender");

        builder.Property(m => m.Receiver)
            .HasColumnName("receiver");

        builder.Ignore(m => m.Text);
        builder.Property<string>("encryptedText")
            .HasColumnName("encrypted_text");

        builder.Property(m => m.Timestamp)
            .HasColumnName("timestamp");
    }
}