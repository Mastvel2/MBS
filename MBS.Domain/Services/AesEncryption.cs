using System.Security.Cryptography;

namespace MBS.Domain.Services;

public static class AesEncryption
{
    private const int KeySize = 256;
    private const int BlockSize = 128;

    public static string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.KeySize = KeySize;
        aes.BlockSize = BlockSize;

        using (var rng = RandomNumberGenerator.Create())
        {
            aes.Key = new byte[KeySize / 8];
            rng.GetBytes(aes.Key);
            aes.IV = new byte[BlockSize / 8];
            rng.GetBytes(aes.IV);
        }

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

    public static string Decrypt(string encryptedText)
    {
        using var aes = Aes.Create();
        aes.KeySize = KeySize;
        aes.BlockSize = BlockSize;

        using (var rng = RandomNumberGenerator.Create())
        {
            aes.Key = new byte[KeySize / 8];
            rng.GetBytes(aes.Key);
            aes.IV = new byte[BlockSize / 8];
            rng.GetBytes(aes.IV);
        }

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