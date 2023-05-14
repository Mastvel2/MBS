using MBS.Domain.Entities;
using MBS.Host.Dtos;

namespace MBS.Host.ApplicationServices;

public interface IMessageService
{
    Task<IEnumerable<MessageDto>> GetMessagesBetweenUsersAsync(string firstUser, string secondUser);
    Task SendMessageAsync(string sender, SendMessageDto dto);
    Task<string> UploadFileAsync(IFormFile file);
    Task<byte[]> DownloadFileAsync(string fileName);
    Task EditMessageTextAsync(EditMessageTextDto dto);
    Task DeleteMessageAsync(Guid id);
}