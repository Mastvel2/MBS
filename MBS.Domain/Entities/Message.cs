using MBS.Domain.Services;

namespace MBS.Domain.Entities;

public class Message
{
    public Message(string sender, string receiver, string text)
    {
        Id = Guid.NewGuid();
        Sender = sender;
        Receiver = receiver;
        Text = AesEncryption.Encrypt(text);
        Timestamp = DateTime.Now;
    }

    public Guid Id { get; protected set; }
    public string Sender { get; protected set; }
    public string Receiver { get; protected set; }
    public string Text { get; protected set; }
    public DateTime Timestamp { get; protected set; }

    public void EditText(string text)
    {
        Text = AesEncryption.Encrypt(text);
    }
}