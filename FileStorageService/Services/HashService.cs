using System.Security.Cryptography;

namespace FileStorageService.Services;

public static class HashService
{
    public static string ComputeSha256(byte[] data)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(data);
        return Convert.ToHexString(hashBytes);
    }
}
