using System.Security.Cryptography;

namespace MBS.Domain.Entities
{
    public class Password
    {
        private const int SaltSize = 8;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        private byte[] _salt;
        private byte[] _hash;

        // Конструктор, принимающий только пароль в открытом виде
        public Password(string clearPassword)
        {
            if (string.IsNullOrWhiteSpace(clearPassword))
            {
                throw new ArgumentException("Password is required");
            }

            this._salt = GenerateSalt();
            this._hash = GenerateHash(clearPassword, _salt);
        }
        
        // Конструктор, принимающий соль и хеш
        public Password(string salt, string hash)
        {
            this._salt = Convert.FromBase64String(salt);
            this._hash = Convert.FromBase64String(hash);
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
            var generatedHash = GenerateHash(password, _salt);
            return _hash.SequenceEqual(generatedHash);
        }

        // Генерирует случайную соль с использованием
        // криптографически стойкого генератора случайных чисел.
        private byte[] GenerateSalt() 
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);
                return salt;
            }
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
            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                return rfc2898.GetBytes(HashSize);
            }
        }

    }
}
