using MBS.Domain.Services;

namespace MBS.Domain.Entities;

/// <summary>
/// Сообщение.
/// </summary>
public class Message
{
    private string encryptedText;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Message"/>.
    /// </summary>
    /// <param name="sender">Отправитель.</param>
    /// <param name="receiver">Получатель.</param>
    /// <param name="clearText">Текст.</param>
    public Message(string sender, string receiver, string clearText)
    {
        this.Id = Guid.NewGuid();
        this.Sender = sender;
        this.Receiver = receiver;
        this.encryptedText = AesEncryption.Encrypt(clearText);
        this.Timestamp = DateTime.Now;
    }

    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Отправитель.
    /// </summary>
    public string Sender { get; protected set; }

    /// <summary>
    /// Получатель.
    /// </summary>
    public string Receiver { get; protected set; }

    /// <summary>
    /// Текст.
    /// </summary>
    public string Text => AesEncryption.Decrypt(this.encryptedText);

    /// <summary>
    /// Время.
    /// </summary>
    public DateTime Timestamp { get; protected set; }

    /// <summary>
    /// Редактирует текст сообщения.
    /// </summary>
    /// <param name="newText">Новый текст.</param>
    public void EditText(string newText)
    {
        this.encryptedText = AesEncryption.Encrypt(newText);
    }
}