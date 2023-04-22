using System;
using System.Security.Cryptography;
using System.Text;

namespace WebMessenger.Security
{
    public class PasswordUserSecurity 
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;
        
        /// <summary>
        /// Генерирует случайную соль с использованием
        /// криптографически стойкого генератора случайных чисел.
        /// </summary>
        /// <returns></returns>
        public static string GenerateSalt() 
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
        }

        /// <summary>
        /// Генерирует хеш пароля с помощью алгоритма PBKDF2,
        /// используя соль и итерации для повышения стойкости.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GenerateHash(string password, string salt)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), Iterations))
            {
                return Convert.ToBase64String(rfc2898.GetBytes(HashSize));
            }
        }

        /// <summary>
        /// Проверяет, соответствует ли предоставленный
        /// пароль хранящемуся хешу и соли, сравнивая сгенерированный хеш с хранящимся хешем.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="storedHash"></param>
        /// <param name="storedSalt"></param>
        /// <returns></returns>
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var generatedHash = GenerateHash(password, storedSalt);
            return string.Equals(storedHash, generatedHash);
        }
    }
}
