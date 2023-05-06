using System.Security.Cryptography;
namespace MBS.Domain.Entities
{
    public class Password
    {
        private const int SaltSize = 8;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        private readonly string salt;
        private readonly string hash;

        // Конструктор, принимающий только пароль в открытом виде
        public Password(string clearPassword)
        {
            if (string.IsNullOrWhiteSpace(clearPassword))
            {
                throw new ArgumentException("Password is required");
            }

            var saltArray = GenerateSalt();
            var hashArray = GenerateHash(clearPassword, saltArray);
            this.salt = Convert.ToBase64String(saltArray);
            this.hash = Convert.ToBase64String(hashArray);
        }
        
        // Конструктор, принимающий соль и хеш
        public Password(string salt, string hash)
        {
            this.salt = salt;
            this.hash = hash;
        }
        
        /// <summary>
        /// Проверяет, соответствует ли предоставленный
        /// пароль хранящемуся хешу и соли, сравнивая сгенерированный хеш с хранящимся хешем.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="storedHash"></param>
        /// <param name="storedSalt"></param>
        /// <returns></returns>
        public bool Verify(string password)
        {
            var saltArray = Convert.FromBase64String(salt);
            var hashArray = GenerateHash(password, saltArray);
            var generatedHash = Convert.ToBase64String(hashArray);
            return hash.Equals(generatedHash);
        }

        // Генерирует случайную соль с использованием
        // криптографически стойкого генератора случайных чисел.
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
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private static byte[] GenerateHash(string password, byte[] salt)
        {
            using var rfc2898 = new Rfc2898DeriveBytes(password, salt, Iterations);
            return rfc2898.GetBytes(HashSize);
        }
    }
}
