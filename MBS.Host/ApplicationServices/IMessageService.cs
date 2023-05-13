using MBS.Domain.Entities;
using MBS.Host.Dtos;

namespace MBS.Host.ApplicationServices;

public interface IMessageService
{
    Task<Message> GetLatestMessageAsync(string user1, string user2);
    Task<IEnumerable<Message>> GetMessagesAsync(string user1, string user2);
    Task<Message> SendMessageAsync(Message message);
    Task<string> UploadFileAsync(IFormFile file);
    Task<byte[]> DownloadFileAsync(string fileName);
    Task UpdateMessageAsync(int messageId, string updatedText);
}