using System.Security.Cryptography;
using System.Text;

namespace DAL.Infrastructure.Helpers;
public record SaltAndPassword(string Salt, string PasswordHash);

public static class HashHelper
{
    private const int INTERVAL = 181081;
    private static readonly Random random = new((int)(DateTime.Now.Ticks % INTERVAL));
    private static string symbols => "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm[]'/.,{}:\"<>?`1234567890-=~!@#$%^&*()_+\\|";
    private static string defaultSalt => "98+_)+(_+a?}\">?\\\"kf98bvsocn01234-)(U^)QWEJOFkn9uwe0tj)ASDJF)(H0INHO$%uh09hj";

    private const int ITERATION_COUNT = 1000;
    private const int KEY_SIZE = 16;
    private static byte[] key = GenerateKeyFromSalt(Encoding.UTF8.GetBytes(symbols), ITERATION_COUNT, KEY_SIZE);

    private static byte[] GenerateKeyFromSalt(byte[] salt, int iterationCount, int keySize)
    {
        using var deriveBytes = new Rfc2898DeriveBytes(defaultSalt, salt, iterationCount, HashAlgorithmName.SHA1);
            return deriveBytes.GetBytes(keySize);
    }

    public static byte[] Encrypt(byte[] fileBytes)
    {
        using var aes = new AesGcm(key, AesGcm.TagByteSizes.MaxSize);
        var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(nonce);

        var encryptedData = new byte[fileBytes.Length];
        var tag = new byte[AesGcm.TagByteSizes.MaxSize];

        aes.Encrypt(nonce, fileBytes, encryptedData, tag);

        var encryptedBytes = new byte[nonce.Length + encryptedData.Length + tag.Length];
        Buffer.BlockCopy(nonce, 0, encryptedBytes, 0, nonce.Length);
        Buffer.BlockCopy(encryptedData, 0, encryptedBytes, nonce.Length, encryptedData.Length);
        Buffer.BlockCopy(tag, 0, encryptedBytes, nonce.Length + encryptedData.Length, tag.Length);

        return encryptedBytes;
    }

    public static byte[] Decrypt(byte[] encryptedBytes)
    {
        using var aes = new AesGcm(key, AesGcm.TagByteSizes.MaxSize);
        var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
        var tag = new byte[AesGcm.TagByteSizes.MaxSize];
        var encryptedData = new byte[encryptedBytes.Length - nonce.Length - tag.Length];

        Buffer.BlockCopy(encryptedBytes, 0, nonce, 0, nonce.Length);
        Buffer.BlockCopy(encryptedBytes, nonce.Length, encryptedData, 0, encryptedData.Length);
        Buffer.BlockCopy(encryptedBytes, nonce.Length + encryptedData.Length, tag, 0, tag.Length);

        var decryptedData = new byte[encryptedData.Length];
        aes.Decrypt(nonce, encryptedData, tag, decryptedData);

        return decryptedData;
    }

    public static SaltAndPassword GenerateNewPasswordHash(string newPassword)
    {
        const int SALT_LENGTH = 16;
        var salt = new string(Enumerable.Repeat(symbols, SALT_LENGTH)
            .Select(s => s[random.Next(s.Length)]).ToArray()
        );

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(newPassword + salt + defaultSalt);

        return new SaltAndPassword(salt, passwordHash);
    }

    public static bool VerifyPassword(string password, string salt, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password + salt + defaultSalt, passwordHash);
    }
}
