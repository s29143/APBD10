using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace APBD10.Helpers;

public static class SecurityHelpers
{
    public static Tuple<string, string> GetHashAndSalt(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount:10000,
            numBytesRequested: 256/8
            ));

        string saltBase64 = Convert.ToBase64String(salt);

        return new Tuple<string, string>(hash, saltBase64);
    }

    public static string GetHashWithSalt(string password, string salt)
    {
        string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Convert.FromBase64String(salt),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount:10000,
            numBytesRequested: 256/8
        ));
        return hash;
    }

    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}