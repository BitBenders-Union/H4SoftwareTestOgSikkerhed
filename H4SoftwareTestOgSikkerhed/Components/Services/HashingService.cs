using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace H4SoftwareTestOgSikkerhed.Components.Services
{
    public class HashingService
    {
        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // without salting
        public byte[] StringHashing(string input)
        {
            Argon2id argon2 = new(Encoding.UTF8.GetBytes(input))
            {
                DegreeOfParallelism = 4,
                Iterations = 2,
                MemorySize = 1024
            };


            byte[] hash = argon2.GetBytes(32);

            return hash;
        }


        // with salting
        public byte[] StringHashing(string input, byte[] salt)
        {
            Argon2id argon2 = new(Encoding.UTF8.GetBytes(input))
            {
                Salt = salt,
                DegreeOfParallelism = 4,
                Iterations = 2,
                MemorySize = 1024
            };


            byte[] hash = argon2.GetBytes(32);

            return hash;
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
            // if input is a string
            if (input is string)
            {
                return (T)(object)BCrypt.Net.BCrypt.HashPassword(input as string);
            }
            // if input is byte[]
            else
            {
                string inputBytes = Convert.ToBase64String(input as byte[]);
                return (T)(object)BCrypt.Net.BCrypt.HashPassword(inputBytes);

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
