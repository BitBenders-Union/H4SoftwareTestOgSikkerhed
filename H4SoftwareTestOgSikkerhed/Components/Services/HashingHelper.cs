using H4SoftwareTestOgSikkerhed.Components.Interfaces;
using Org.BouncyCastle.Crypto.Generators;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace H4SoftwareTestOgSikkerhed.Components.Services;

public class HashingHelper : IHashingHelper
{
    public dynamic HashSHA2(dynamic input)
    {
        // Tjekker, om input er null, og smider en undtagelse, hvis det er tilfældet
        if (input == null)
            throw new ArgumentNullException(nameof(input), "Input kan ikke være null");

        // Hvis input er en string
        if (input is string inputString)
        {
            // Konverterer string til byte[]
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);

            // Bruger SHA256 til at beregne hash
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                // Returnerer hash som Base64 string for string input
                return Convert.ToBase64String(hashBytes);
            }
        }
        // Hvis input er en byte[]
        else if (input is byte[] inputBytes)
        {
            // Bruger SHA256 til at beregne hash
            using (var sha256 = SHA256.Create())
            {
                // Returnerer hash som byte[] for byte[] input
                return sha256.ComputeHash(inputBytes);
            }
        }
        else
        {
            // Hvis input ikke er en string eller byte[], smides en undtagelse
            throw new ArgumentException("Input skal være af typen string eller byte[]", nameof(input));
        }
    }

    public dynamic HashHMAC(dynamic input)
    {
        // Tjekker, om input er null, og smider en undtagelse, hvis det er tilfældet
        if (input == null)
            throw new ArgumentNullException(nameof(input), "Input kan ikke være null");

        // Hardkodet nøgle til HMAC (kan ændres eller erstattes med en sikrere metode)
        string key = "Passw0rd!";
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);

        // Hvis input er en string
        if (input is string inputString)
        {
            // Konverterer string til byte[]
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
            using (var hmac = new HMACSHA256(keyBytes)) // Bruger HMAC med SHA256 algoritme
            {
                // Beregner HMAC og returnerer resultatet som Base64 string
                byte[] hashBytes = hmac.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
        // Hvis input er en byte[]
        else if (input is byte[] inputBytes)
        {
            using (var hmac = new HMACSHA256(keyBytes)) // Bruger HMAC med SHA256 algoritme
            {
                // Beregner HMAC og returnerer resultatet som byte[]
                return hmac.ComputeHash(inputBytes);
            }
        }
        else
        {
            // Hvis input ikke er en string eller byte[], smides en undtagelse
            throw new ArgumentException("Input skal være af typen string eller byte[]", nameof(input));
        }
    }




    public dynamic HashPBKDF2(dynamic input)
    {
        // Tjekker, om input er null, og smider en undtagelse, hvis det er tilfældet
        if (input == null)
            throw new ArgumentNullException(nameof(input), "Input kan ikke være null");

        // Hardcoded salt
        string salt = "po1hg12nabg626peg876nbag";

        // Hvis input er en string
        if (input is string inputString)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);

            // Bruger PBKDF2 (Rfc2898DeriveBytes) til at generere hash
            using (var pbkdf2 = new Rfc2898DeriveBytes(inputBytes, Encoding.UTF8.GetBytes(salt), 10000, HashAlgorithmName.SHA256))
            {
                // Returnerer hash som byte[] for string input
                return pbkdf2.GetBytes(32); // 32 byte længde
            }
        }
        // Hvis input er en byte[]
        else if (input is byte[] inputBytes)
        {
            // Bruger PBKDF2 (Rfc2898DeriveBytes) til at generere hash
            using (var pbkdf2 = new Rfc2898DeriveBytes(inputBytes, Encoding.UTF8.GetBytes(salt), 10000, HashAlgorithmName.SHA256))
            {
                // Returnerer hash som byte[] for byte[] input
                return pbkdf2.GetBytes(32); // 32 byte længde
            }
        }
        else
        {
            // Hvis input ikke er en string eller byte[], smides en undtagelse
            throw new ArgumentException("Input skal være af typen string eller byte[]", nameof(input));
        }
    }
    public string HashBcrypt(string input)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentNullException(nameof(input), "Input kan ikke være null eller tom");

        // Hash adgangskoden med bcrypt, som automatisk genererer salt
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(input);
        return hashedPassword;
    }

    public bool VerifyBcrypt(string input, string storedHash)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(storedHash))
            throw new ArgumentNullException("Input og storedHash kan ikke være null eller tom");

        // Verificer om den indtastede adgangskode matcher den lagrede bcrypt-hash
        return BCrypt.Net.BCrypt.Verify(input, storedHash);
    }

}
