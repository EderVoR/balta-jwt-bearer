using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using MediatR;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate
{
	public class Handler : IRequestHandler<Request, Response>
	{
		private readonly IRepository _repository;

        public Handler(IRepository repository) => _repository = repository;

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
		{
			#region 01. Valida Requisição

			try
			{
				var res = Specification.Ensure(request);
				if (!res.IsValid)
					return new Response("Requisição inválida", 400, res.Notifications);
			}
			catch
			{
				return new Response("Não foi possivel validar sua requisição", 500);
			}

			#endregion
			
			#region 02.Recuperar perfil

			User? user;
			try
			{
				user = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);
				if (user == null)
					return new Response("Perfil não encontrado", 400);
			}
			catch
			{
				return new Response("Não foi possivel realizar a requisição", 500);
			}

			#endregion

			#region 03. Validar password

			if (!user.Password.Challenge(request.Password))
				return new Response("Usuário ou senha inválidos", 400);

			#endregion

			#region 04. Conta ativa
			
			try
			{
				if (!user.Email.Verification.IsActive)
					return new Response("Conta inativa", 400);
			}
			catch
			{
				return new Response("Não foi possivel verificar seu perfil", 500);
			}

			#endregion

			#region 05. Retornar dados
			
			try
			{
				var data = new ResponseData
				{
					Id = user.Id.ToString(),
					Name = user.Name,
					Email = user.Email,
					Roles = user.Roles.Select(c => c.Name).ToArray(),
				};

				return new Response(string.Empty, data);
			}
			catch
			{
				return new Response("Não foi possivel obter os dados do perfil", 500);
			}


			#endregion
		}
	}
}
