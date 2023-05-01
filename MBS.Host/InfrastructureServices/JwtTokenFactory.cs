using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MBS.Host.InfrastructureServices;

public class JwtTokenFactory : ITokenFactory
{
    private readonly string secretKey;
    private readonly int expirationMinutes;

    public JwtTokenFactory(string secretKey, int expirationMinutes)
    {
        this.secretKey = secretKey;
        this.expirationMinutes = expirationMinutes;
    }

    public string Create(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler(); // Создаем обработчик токенов
        var key = Encoding.ASCII.GetBytes(this.secretKey); // Конвертируем секретный ключ в байты

        var now = DateTime.UtcNow; // Получаем текущую дату один раз

        // Описываем параметры токена
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Устанавливаем имя пользователя в виде утверждения
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
            Expires = now.AddMinutes(this.expirationMinutes), // Устанавливаем срок действия токена
            NotBefore = now, // Устанавливаем время начала действия токена
            // Устанавливаем алгоритм подписи и ключ
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor); // Создаем токен на основе параметров
        return tokenHandler.WriteToken(token); // Возвращаем сериализованный токен в виде строки
    }
}