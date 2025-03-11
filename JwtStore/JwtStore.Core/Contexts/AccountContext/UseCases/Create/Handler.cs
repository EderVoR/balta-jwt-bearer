using JwtStore.Core.AccountContext.ValueObjects;
using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using JwtStore.Core.Contexts.AccountContext.ValueObjects;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Create
{
    public class Handler
    {
        private readonly IRepository _repository;
        private readonly IService _service;

        public Handler(IRepository repository, IService service)
        {
            _repository = repository;
            _service = service; 
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            #region 01. Valida a requisição

            try
            {
                var res = Specification.Ensure(request);
                if(!res.IsValid)
                    return new Response("Requisição inválida", 400, res.Notifications);
            }
            catch (Exception ex)
            {
				return new Response("Não foi possível validar sua requisição.", 500);
			}

            #endregion

            #region 02. Gerar os objetos

            Email email;
            Password password;
            User user;

            try
            {
                email = new Email(request.Email);
                password = new Password(request.Password);

                user = new User(request.Name, email, password);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, 400);
            }

            #endregion

            #region 03. Verifica cadastro de usuario

            try
            {
				var exists = await _repository.AnyAsync(request.Email, cancellationToken);
				if (exists)
					return new Response("Email já cadastrado.", 400);
			}
            catch
            {
                return new Response("Falha ao verificar email cadastrado", 500);
            }

            #endregion

            #region 04. Persistir os dados

            try
            {
                await _repository.SaveAsync(user, cancellationToken);
            }
            catch
            {
                return new Response("Falha ao persistir os dados", 500);
            }

            #endregion

            #region 05. Enviar Email

            try
            {
                await _service.SendVerificarionEmailAsync(user, cancellationToken);
            }
            catch
            {

            }

            #endregion

            return new Response("Conta criada", new ResponseData(user.Id, user.Name, user.Email));
		}
	}
}
