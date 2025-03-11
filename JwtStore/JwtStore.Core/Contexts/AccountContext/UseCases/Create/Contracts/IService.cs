using JwtStore.Core.Contexts.AccountContext.Entities;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts
{
	public interface IService
	{
		Task SendVerificarionEmailAsync(User user, CancellationToken cancellationToken);
	}
}
