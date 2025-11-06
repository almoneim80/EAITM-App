using EAITMApp.Application.Interfaces;
using EAITMApp.Infrastructure.Memory;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
namespace EAITMApp.Infrastructure.Security
{
    public class Argon2EncryptionService : IEncryptionService
    {
        private readonly Argon2Settings _settings;
        private readonly ISecureMemoryService _secureMemory;
        public Argon2EncryptionService(IOptions<Argon2Settings> options, ISecureMemoryService secureMemory)
        {
            _settings = options.Value;
            _secureMemory = secureMemory;
        }


        /// <inheritdoc/>
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            byte[] salt = new byte[_settings.SaltSize];
            using(var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            try
            {
                Argon2 argon2 = _settings.Type switch
                {
                    "Argon2i" => new Argon2i(passwordBytes),
                    "Argon2d" => new Argon2d(passwordBytes),
                    _ => new Argon2id(passwordBytes)
                };

                argon2.Salt = salt;
                argon2.AssociatedData = string.IsNullOrEmpty(_settings.AssociatedData) ? null : Encoding.UTF8.GetBytes(_settings.AssociatedData);
                argon2.KnownSecret = string.IsNullOrEmpty(_settings.SecretKey) ? null : Encoding.UTF8.GetBytes(_settings.SecretKey);
                argon2.Iterations = _settings.Iterations;
                argon2.MemorySize = _settings.MemoryCost;
                argon2.DegreeOfParallelism = _settings.DegreeOfParallelism;

                byte[] hash = argon2.GetBytes(_settings.HashSize);

                byte[] result = new byte[_settings.SaltSize + _settings.HashSize];
                Buffer.BlockCopy(salt, 0, result, 0, _settings.SaltSize);
                Buffer.BlockCopy(hash, 0, result, _settings.SaltSize, _settings.HashSize);

                return Convert.ToBase64String(result);
            }
            finally
            {
                _secureMemory.ClearBytes(passwordBytes);
            }
        }

        /// <inheritdoc/>
        public bool VerifyPassword(string plainText, string hashedPassword)
        {
            byte[] decoded = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[_settings.SaltSize];
            byte[] hash = new byte[_settings.HashSize];

            Buffer.BlockCopy(decoded, 0,  salt, 0, _settings.SaltSize);
            Buffer.BlockCopy(decoded, _settings.SaltSize, hash, 0, _settings.HashSize);


            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            try
            {
                Argon2 argon2 = _settings.Type switch
                {
                    "Argon2i" => new Argon2i(plainBytes),
                    "Argon2d" => new Argon2d(plainBytes),
                    _ => new Argon2id(plainBytes)
                };

                argon2.Salt = salt;
                argon2.AssociatedData = string.IsNullOrEmpty(_settings.AssociatedData) ? null : Encoding.UTF8.GetBytes(_settings.AssociatedData);
                argon2.KnownSecret = string.IsNullOrEmpty(_settings.SecretKey) ? null : Encoding.UTF8.GetBytes(_settings.SecretKey);
                argon2.Iterations = _settings.Iterations;
                argon2.MemorySize = _settings.MemoryCost;
                argon2.DegreeOfParallelism = _settings.DegreeOfParallelism;

                byte[] computedHash = argon2.GetBytes(_settings.HashSize);
                return CryptographicOperations.FixedTimeEquals(hash, computedHash);
            }
            finally
            {
                _secureMemory.ClearBytes(plainBytes);
            }
        }
    }
}
