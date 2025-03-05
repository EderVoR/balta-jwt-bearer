using Flunt.Notifications;
using Flunt.Validations;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Create
{
    public static class Specification
    {
        public static Contract<Notification> Ensure(Request request)
            => new Contract<Notification>()
                .Requires()
                .IsLowerThan(request.Name.Length, 160, "Name", "O nome deve ser minusculo")
                .IsGreaterThan(request.Name.Length, 3, "Name", "O nome deve ser maior que 3 caracteres")
                .IsLowerThan(request.Password.Length, 40, "Password", "A senha não deve ser superior a 40 caracteres")
                .IsLowerThan(request.Password.Length, 8, "Password", "A senha deve conter no minimo 8 caracteres")
                .IsEmail(request.Email, "Email", "Email inválido");
    }
}
