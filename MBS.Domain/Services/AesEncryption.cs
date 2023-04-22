using System.Security.Cryptography;

namespace MBS.Domain;

/// <summary>
/// Класс AesEncryption для шифрования и дешифрования сообщений
/// </summary>
public class AesEncryption
{
    private static readonly int KeySize = 256;
    private static readonly int BlockSize = 128;
    private static readonly string EncryptionKey = "YourEncryptionKey";

    public static string Encrypt(string plainText)
    {
        using (var aes = new AesManaged { KeySize = KeySize, BlockSize = BlockSize })
        {
            var key = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, 1000);
            aes.Key = key.GetBytes(KeySize / 8);
            aes.IV = key.GetBytes(BlockSize / 8);

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }

    public static string Decrypt(string encryptedText)
    {
        using (var aes = new AesManaged { KeySize = KeySize, BlockSize = BlockSize })
        {
            var key = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, 1000);
            aes.Key = key.GetBytes(KeySize / 8);
            aes.IV = key.GetBytes(BlockSize / 8);

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}