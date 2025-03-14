using Flunt.Notifications;
using Flunt.Validations;


namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate
{
	public static class Specification
	{
		public static Contract<Notification> Ensure(Request request) =>
			new Contract<Notification>()
				.Requires()
				.IsLowerThan(request.Password.Length, 40, "Password", "A senha deve conter menos que 40 caracteres")
				.IsGreaterThan(request.Password.Length, 5, "Password", "A senha deve conter mais que 5 caracteres")
				.IsEmail(request.Email, "Email", "Email inválido");
	}
}
