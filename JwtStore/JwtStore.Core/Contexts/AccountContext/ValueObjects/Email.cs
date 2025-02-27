using JwtStore.Core.Contexts.AccountContext.ValueObjects;
using JwtStore.Core.Contexts.SharedContext.Extensions;
using JwtStore.Core.Contexts.SharedContext.ValueObjects;
using System.Text.RegularExpressions;

namespace JwtStore.Core.AccountContext.ValueObjects
{
    public partial class Email : ValueObject
	{
		#region Construtor

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

        protected Email()
        {
        }

		#endregion

		#region Propriedades

		private const string Pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w)*\.\w+([-.]\w+)*$";

        public string Adress { get; }
        public string Hash => Adress.ToBase64();
        public Verification Verification { get; private set; } = new();

		#endregion

		#region Métodos

		public void ResentVerification()
            => Verification = new Verification();

        public static implicit operator string(Email email)
            => email.ToString();

        public static implicit operator Email(string email)
            => new Email(email);

        public override string ToString()
            => Adress;

		#endregion

		[GeneratedRegex(Pattern)]
        private static partial Regex EmailRegex();
    }
}