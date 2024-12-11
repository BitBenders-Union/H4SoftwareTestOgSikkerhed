using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace H4SoftwareTestOgSikkerhed.Components.Services
{
    public class HashingService
    {
        // Generate a global salt
        private static readonly byte[] GlobalSalt = GenerateGlobalSalt();

        private static byte[] GenerateGlobalSalt()
        {
            byte[] salt = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public class HashingMethods
        {
            // SHA2 Hashing
            public static object SHA2Hash(object input)
            {
                using (var sha256 = SHA256.Create())
                {
                    if (input is string strInput)
                    {
                        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(strInput));
                        return Convert.ToBase64String(hashBytes);
                    }
                    else if (input is byte[] byteInput)
                    {
                        return sha256.ComputeHash(byteInput);
                    }
                    else
                    {
                        throw new ArgumentException("Input must be of type string or byte[]");
                    }
                }
            }

            // HMAC Hashing
            public static object HMACHash(object input, byte[] key)
            {
                using (var hmac = new HMACSHA256(key))
                {
                    if (input is string strInput)
                    {
                        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(strInput));
                        return Convert.ToBase64String(hashBytes);
                    }
                    else if (input is byte[] byteInput)
                    {
                        return hmac.ComputeHash(byteInput);
                    }
                    else
                    {
                        throw new ArgumentException("Input must be of type string or byte[]");
                    }
                }
            }

            // PBKDF2 Hashing
            public static object PBKDF2Hash(object input, byte[] salt, int iterations = 10000, int hashSize = 32)
            {
                if (input is string strInput)
                {
                    using (var rfc2898 = new Rfc2898DeriveBytes(strInput, salt, iterations))
                    {
                        byte[] hashBytes = rfc2898.GetBytes(hashSize);
                        return Convert.ToBase64String(hashBytes);
                    }
                }
                else if (input is byte[] byteInput)
                {
                    using (var rfc2898 = new Rfc2898DeriveBytes(byteInput, salt, iterations))
                    {
                        return rfc2898.GetBytes(hashSize);
                    }
                }
                else
                {
                    throw new ArgumentException("Input must be of type string or byte[]");
                }
            }

            public static string BcryptHashWithGlobalSalt<TEntity>(TEntity input, int workFactor = 10)
            {
                if (input == null)
                {
                    throw new ArgumentException("Input cannot be null");
                }

                string inputString;

                // Determine the input type and convert to string
                if (input is string strInput)
                {
                    inputString = strInput;
                }
                else if (input is byte[] byteInput)
                {
                    inputString = Encoding.UTF8.GetString(byteInput);
                }
                else
                {
                    throw new ArgumentException("Input must be of type string or byte[]");
                }

                // Hash the input string using Bcrypt
                string hash = BCrypt.Net.BCrypt.HashPassword(inputString, workFactor);

                // Return the hash
                return hash;
            }


            public static bool VerifyBcryptHash(string input, string storedHash)
            {
                if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(storedHash))
                {
                    throw new ArgumentException("Input and stored hash cannot be null or empty");
                }

                // Use Bcrypt's built-in salt handling to verify the hash
                return BCrypt.Net.BCrypt.Verify(input, storedHash);
            }

            public static bool VerifyBcryptCprNumber(string input, string storedHash)
            {
                if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(storedHash))
                {
                    throw new ArgumentException("Input and stored hash cannot be null or empty");
                }

                // Use Bcrypt's built-in salt handling to verify the hash
                return BCrypt.Net.BCrypt.Verify(input, storedHash);
            }
        }
    }
}
