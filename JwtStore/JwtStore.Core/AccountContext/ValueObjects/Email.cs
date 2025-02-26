using JwtStore.Core.SharedContext.Extensions;
using JwtStore.Core.SharedContext.ValueObjects;
using System.Text.RegularExpressions;

namespace JwtStore.Core.AccountContext.ValueObjects
{
	public partial class Email : ValueObject
	{
        public Email(string adress)
        {
            if (string.IsNullOrEmpty(adress))
                throw new Exception("Email inválido");

            Adress = adress.Trim().ToLower();

            if (Adress.Length < 5)
                throw new Exception("Email inválido");

            if (!EmailRegex().IsMatch(Adress)) 
                throw new Exception("Email inválido");
        }

        private const string Pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w)*\.\w+([-.]\w+)*$";

        public string Adress { get; }
        public string Hash => Adress.ToBase64();
        public Verification Verification { get; private set; } = new();

        public void ResentVerification()
            => Verification = new Verification();

        public static implicit operator string(Email email)
            => email.ToString();

        public static implicit operator Email(string email)
            => new Email(email);

        public override string ToString()
            => Adress;

		[GeneratedRegex(Pattern)]
        private static partial Regex EmailRegex();
    }
}