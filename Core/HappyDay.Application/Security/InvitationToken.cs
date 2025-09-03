using System;
using System.Security.Cryptography;
using System.Text;

public static class InvitationToken
{
    // URL-safe random token üret (tek kullanımlık)
    public static string Generate(int byteLen = 32)
    {
        var bytes = RandomNumberGenerator.GetBytes(byteLen);
        return Convert.ToBase64String(bytes)
            .Replace('+','-')
            .Replace('/','_')
            .TrimEnd('='); // URL-safe
    }

    // Token SHA256 hash (DB’de bunu saklayacağız)
    public static string Hash(string token)
    {
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(hash); // uppercase hex
    }
}