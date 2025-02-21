using JwtStore.Core.SharedContext.Extensions;
using JwtStore.Core.SharedContext.ValueObjects;

namespace JwtStore.Core.AccountContext.ValueObjects
{
	internal class Email : ValueObject
	{
        private const string Pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w)*\.\w+([-.]\w+)*$";

        public string Adress { get; }
        public string Hash => Adress.ToBase64();
    }
}
