using System;

namespace MBS.Host.Dtos;

public class MessageDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string EncryptedText { get; set; }
    public DateTime Timestamp { get; set; }
}