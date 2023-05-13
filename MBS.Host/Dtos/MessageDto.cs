namespace MBS.Host.Dtos;

public class MessageDto
{
    public int Id { get; set; }
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public string EncryptedText { get; set; }
    public DateTime Timestamp { get; set; }
}