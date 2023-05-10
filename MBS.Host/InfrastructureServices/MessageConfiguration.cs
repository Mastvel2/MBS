using MBS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBS.Host.InfrastructureServices;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
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

        builder.Property(m => m.EncryptedText)
            .HasColumnName("encrypted_text");

        builder.Property(m => m.Timestamp)
            .HasColumnName("timestamp");
    }
}