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

        public byte[] StringHashingWithSalt(string input, byte[] salt)
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
    }
}
