﻿using JwtStore.Core.Contexts.SharedContext.ValueObjects;
using System.ComponentModel;
using System.Security.Cryptography;

namespace JwtStore.Core.Contexts.AccountContext.ValueObjects
{
    public class Password : ValueObject
    {
        #region Propriedade

        public const string Valid = "abcdefghijklnopqrstuvxyzwABCDEFGHIJKLMNOPQRSTUVXYZ0123456789";
        public const string Special = "!@#$%&*(){}[];";

        public string Hash { get; } = string.Empty;
        public string ResetCode { get; } = Guid.NewGuid().ToString()[..8].ToUpper();

        #endregion

        #region Construtor

        protected Password()
        {
        }

        public Password(string? text = null)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
                text = Generate();

            Hash = Hashing(text);
        }

        #endregion

        #region Métodos

        private static string Generate(short length = 16, bool incluseSpecialChars = true,
            bool upperCase = false)
        {
            var chars = incluseSpecialChars ? Valid + Special : Valid;
            var startRandom = upperCase ? 26 : 0;
            var index = 0;
            var res = new char[length];
            var rnd = new Random(startRandom);

            while (index < length)
                res[index++] = chars[rnd.Next(startRandom, chars.Length)];

            return new string(res);
        }

        private static string Hashing(string password, short saltSize = 16, short keySize = 32,
            int iterations = 10000, char splitChar = '.')
        {
            if (string.IsNullOrEmpty(password))
                throw new Exception("A senha não pode ser nula ou vazia");

            password += Configuration.Secrets.PasswordSaltKey;

            using var algorithm = new Rfc2898DeriveBytes(password, saltSize, iterations, HashAlgorithmName.SHA256);
            var key = Convert.ToBase64String(algorithm.GetBytes(keySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{iterations}{splitChar}{salt}{splitChar}{key}";
        }

        private static bool Verify(string hash, string password, short keySize = 32,
            int iterations = 10000, char splitChar = '.')
        {
            password += Configuration.Secrets.PasswordSaltKey;

            var parts = hash.Split(splitChar, 3);
            if (parts.Length != 3)
                return false;

            var hashIterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            if (hashIterations != iterations)
                return false;

            using var algorithm = new Rfc2898DeriveBytes(
                password, salt, iterations, HashAlgorithmName.SHA256);
            var keyToChech = algorithm.GetBytes(keySize);

            return keyToChech.SequenceEqual(key);
        }

        public bool Challenge(string plainTextPassword)
            => Verify(Hash, plainTextPassword);

		#endregion
	}
}