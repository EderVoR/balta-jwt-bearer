using JwtAspNet.Models;
using JwtAspNet.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TokenService>();

var app = builder.Build();

app.MapGet("/", (TokenService token) =>
{
	var user = new User(
		1,
		"Eder",
		"eder.tes",
		"https://",
		"13546",
		new[] { "admin", "estudante" }
	);
	token.Create(user);
});

//app.UseAuthentication();
//app.UseAuthorization();

app.Run();
