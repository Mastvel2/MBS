using MBS.Domain;
using MBS.Host.Repositories;
using MBS.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBS.Host.Host_services;

namespace MBS.Host.Services
{
     public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        // Получение списка сообщений для текущего пользователя
        public async Task<IEnumerable<Message>> GetMessagesForUserAsync(int userId)
        {
            // Получаем зашифрованные сообщения из репозитория
            var encryptedMessages = await _messageRepository.GetByUserIdAsync(userId);

            var decryptedMessages = new List<Message>();

            // Расшифровываем каждое сообщение с использованием AesEncryption
            foreach (var message in encryptedMessages)
            {
                // Расшифровываем содержимое сообщения с использованием статического метода AesEncryption.Decrypt
                var decryptedContent = AesEncryption.Decrypt(message.EncryptedText);
                
                // Создаем новый экземпляр Message с расшифрованным содержимым
                var decryptedMessage = new Message
                {
                    Id = message.Id,
                    SenderId = message.SenderId,
                    ReceiverId = message.ReceiverId,
                    EncryptedText = decryptedContent,
                    Timestamp = message.Timestamp
                };

                decryptedMessages.Add(decryptedMessage);
            }

            return decryptedMessages;
        }

        // Отправка сообщения от одного пользователя другому
        public async Task<Message> SendMessageAsync(int senderId, int receiverId, string content)
        {
            // Шифруем содержимое сообщения с использованием статического метода AesEncryption.Encrypt
            var encryptedContent = AesEncryption.Encrypt(content);

            // Создаем новый экземпляр Message с зашифрованным содержимым
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                EncryptedText = encryptedContent,
                Timestamp = DateTime.UtcNow
            };

            // Сохраняем зашифрованное сообщение в репозитории
            await _messageRepository.AddAsync(message);

            return message;
        }
    }
}