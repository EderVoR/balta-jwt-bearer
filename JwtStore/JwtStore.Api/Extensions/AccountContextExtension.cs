﻿using JwtStore.Core.Contexts.AccountContext.UseCases.Create;
using MediatR;

namespace JwtStore.Api.Extensions
{
	public static class AccountContextExtension
	{
		public static void AddAccountContext(this WebApplicationBuilder builder)
		{
			#region Create

			builder.Services.AddTransient<
				JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts.IRepository,
				JwtStore.Infra.Contexts.AccountContext.UseCases.Create.Repository>();

			builder.Services.AddTransient<
				JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts.IService,
				JwtStore.Infra.Contexts.AccountContext.UseCases.Create.Service>();

			#endregion


		}

		public static void MapAccountEndpoinsts(this WebApplication app)
		{
			#region Create

			app.MapPost("api/v1/users", async (Request request, IRequestHandler<
				JwtStore.Core.Contexts.AccountContext.UseCases.Create.Request,
				JwtStore.Core.Contexts.AccountContext.UseCases.Create.Response> handler) =>
			{
				var result = await handler.Handle(request, new CancellationToken());
				return result.IsSucess
					? Results.Created("", result)
					: Results.Json(result, statusCode: result.Status);
			});

			#endregion
		}
	}
}
