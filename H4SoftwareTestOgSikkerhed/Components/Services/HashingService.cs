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

        
        public T SHAHashing<T>(T input)
        {
            // if input is a string
            if (input is string)
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input as string);
                return (T)(object)SHA256.Create().ComputeHash(inputBytes);

            }
            // if input is byte[]
            else
            {
                return (T)(object)SHA256.Create().ComputeHash(input as byte[]);
            }
        }

        public T HMACHashing<T>(T input)
        {
            // if input is a string
            if (input is string)
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input as string);
                return (T)(object)new HMACSHA256(Encoding.UTF8.GetBytes("key")).ComputeHash(inputBytes); ;

            }
            // if input is byte[]
            else
            {
                return (T)(object)new HMACSHA256(Encoding.UTF8.GetBytes("po1hg12nabg626peg876nbag")).ComputeHash(input as byte[]);
            }
        }

        public T PBKDF2Hashing<T>(T input)
        {
            // if input is a string
            if (input is string)
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input as string);

                return (T)(object)Rfc2898DeriveBytes.Pbkdf2(inputBytes, Encoding.UTF8.GetBytes("po1hg12nabg626peg876nbag"), 2, HashAlgorithmName.SHA256, 32);

            }
            // if input is byte[]
            else
            {
                return (T)(object)Rfc2898DeriveBytes.Pbkdf2(
                    input as byte[], 
                    Encoding.UTF8.GetBytes("po1hg12nabg626peg876nbag"), 
                    2, 
                    HashAlgorithmName.SHA256, 
                    32);
            }
        }

        public T BCRYPTHashing<T>(T input)
        {
            if(input == null)
                throw new ArgumentNullException("input must not be null");

            // if input is a string
            if (input is string)
            {
                return (T)(object)BCrypt.Net.BCrypt.HashPassword(input as string);
            }
            // if input is byte[]
            else if(input is byte[])
            {
                string inputBytes = Convert.ToBase64String(input as byte[]);
                return (T)(object)BCrypt.Net.BCrypt.HashPassword(inputBytes);

            }
            else
            {
                throw new ArgumentException("Input must be of type string or byte[]");
            }
        }

        public bool BCRYPTVerify<T>(T input, string hash)
        {
            // if input is a string
            if (input is string)
            {
                return BCrypt.Net.BCrypt.Verify(input as string, hash);
            }
            // if input is byte[]
            else
            {
                string inputBytes = Convert.ToBase64String(input as byte[]);
                return BCrypt.Net.BCrypt.Verify(inputBytes, hash);
            }
        }

        public T BCRYPTHashingSalted<T>(T input)
        {
            // if input is a string
            if (input is string)
            {
                return (T)(object)BCrypt.Net.BCrypt.HashPassword(input as string, BCrypt.Net.BCrypt.GenerateSalt(), true, BCrypt.Net.HashType.SHA256);
            }
            // if input is byte[]
            else
            {
                string inputBytes = Convert.ToBase64String(input as byte[]);
                return (T)(object)BCrypt.Net.BCrypt.HashPassword(inputBytes, BCrypt.Net.BCrypt.GenerateSalt(), true, BCrypt.Net.HashType.SHA256);

            }
        }


        public bool BCRYPTVerifySalted<T>(T input, string hash)
        {
            // if input is a string
            if (input is string)
            {
                return BCrypt.Net.BCrypt.Verify(input as string, hash, true, BCrypt.Net.HashType.SHA256);
            }
            // if input is byte[]
            else
            {
                string inputBytes = Convert.ToBase64String(input as byte[]);
                return BCrypt.Net.BCrypt.Verify(inputBytes, hash, true, BCrypt.Net.HashType.SHA256);
            }
        }



    }
}
