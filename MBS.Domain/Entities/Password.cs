using System.Security.Cryptography;
namespace MBS.Domain.Entities;

/// <summary>
/// Пароль.
/// </summary>
public class Password
{
    private const int SaltSize = 8;
    private const int HashSize = 20;
    private const int Iterations = 10000;

    private readonly string salt;
    private readonly string hash;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="Password"/>.
    /// </summary>
    /// <param name="clearPassword">Пароль в чистом виде.</param>
    public Password(string clearPassword)
    {
        if (string.IsNullOrWhiteSpace(clearPassword))
        {
            throw new ArgumentException("Не задан пароль");
        }

        var saltArray = GenerateSalt();
        var hashArray = GenerateHash(clearPassword, saltArray);
        this.salt = Convert.ToBase64String(saltArray);
        this.hash = Convert.ToBase64String(hashArray);
    }

    /// <summary>
    /// Проверяет, соответствует ли предоставленный
    /// пароль хранящемуся хешу и соли, сравнивая сгенерированный хеш с хранящимся хешем.
    /// </summary>
    /// <param name="password">Сравниваемый пароль.</param>
    /// <returns>Результат сравнения паролей.</returns>
    public bool Verify(string password)
    {
        var saltArray = Convert.FromBase64String(salt);
        var hashArray = GenerateHash(password, saltArray);
        var generatedHash = Convert.ToBase64String(hashArray);
        return this.hash.Equals(generatedHash);
    }

    /// <summary>
    /// Генерирует случайную соль с использованием криптографически стойкого генератора случайных чисел.
    /// </summary>
    /// <returns>Массив байтов, представляющих соль.</returns>
    private static byte[] GenerateSalt()
    {
        using var rng = RandomNumberGenerator.Create();
        var saltArray = new byte[SaltSize];
        rng.GetBytes(saltArray);
        return saltArray;
    }

    /// <summary>
    /// Генерирует хеш пароля с помощью алгоритма PBKDF2,
    /// используя соль и итерации для повышения стойкости.
    /// </summary>
    /// <param name="clearPassword">Пароль в чистом виде.</param>
    /// <param name="salt">Соль.</param>
    /// <returns>Массив байтов, представляющих хэш.</returns>
    private static byte[] GenerateHash(string clearPassword, byte[] salt)
    {
        using var rfc2898 = new Rfc2898DeriveBytes(clearPassword, salt, Iterations);
        return rfc2898.GetBytes(HashSize);
    }
}