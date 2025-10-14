using System;
using Konscious.Security.Cryptography;
using System.Text;
namespace EAITMApp.Infrastructure.Security
{
    public class Argon2EncryptionService : IEncryptionService
    {
        // إعدادات افتراضية يمكن تعديلها لاحقاً
        private const int SaltSize = 16; // 128-bit salt
        private const int HashSize = 32; // 256-bit hash
        private const int Iterations = 4;
        private const int MemoryCost = 65536; // 64 MB
        private const int DegreeOfParallelism = 2;


        /// <summary>
        /// Hashes a plain-text password using Argon2id with a random salt.
        /// </summary>
        /// <param name="password">Plain-text password</param>
        /// <returns>Base64-encoded string containing salt + hash</returns>
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            byte[] salt = new byte[SaltSize];
            using(var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                Iterations = Iterations,
                MemorySize = MemoryCost,
                DegreeOfParallelism = DegreeOfParallelism
            };

            byte[] hash = argon2.GetBytes(HashSize);


            byte[] result = new byte[SaltSize + HashSize];
            Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, result, SaltSize, HashSize);

            return Convert.ToBase64String(result);
        }

        public bool VerifyPassword(string plainText, string hashedPassword)
        {
            throw new NotImplementedException();
        }
    }
}
