namespace MBS.Domain.Entities;

public class Message
{
    public int Id { get; set; }
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public string EncryptedText { get; set; }
    public DateTime Timestamp { get; set; }
}