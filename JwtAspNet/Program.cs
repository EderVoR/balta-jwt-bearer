using JwtAspNet;
using JwtAspNet.Extensions;
using JwtAspNet.Models;
using JwtAspNet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TokenService>();
builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
	x.TokenValidationParameters = new TokenValidationParameters
	{
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.PrivateKey)),
		ValidateIssuer = false,
		ValidateAudience = false
	};
});
builder.Services.AddAuthorization(x =>
{
	x.AddPolicy("Admin", x => x.RequireRole("admin"));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("", () => "Voce esta online");

app.MapGet("/login", (TokenService token) =>
{
	var user = new User(
		1,
		"Eder",
		"eder.tes",
		"https://",
		"13546",
		["teacher", "estudante"]
	);
	return token.Create(user);
});

app.MapGet("/restrito", (ClaimsPrincipal claim) =>
{
	return new
	{
		id = claim.Id(),
		name = claim.Name(),
		email = claim.Email(),
		givenName = claim.GivenName(),
		image = claim.Image(),
	};
}).RequireAuthorization();

app.MapGet("/admin", () =>
{
	return "Você tem acesso admin";
}).RequireAuthorization("admin");

app.Run();
