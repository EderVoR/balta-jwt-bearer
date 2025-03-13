namespace JwtStore.Core
{
	public static class Configuration
	{
		public static SecretsConfiguration Secrets { get; set; } = new();
		public static EmailConfiguration Email { get; set; } = new();
		public static SendgridConfiguration SendGrid { get; set; } = new();
        public static DatabaseConfiguration Database { get; set; } = new();


		public class DatabaseConfiguration
		{
			public string ConnectionString { get; set; } = string.Empty;
		}

		public class EmailConfiguration
		{
			public string DefaultFromEmail { get; set; } = "eder_edy@hotmail.com";
			public string DefaultFromName { get; set; } = "Eder Teste";
        }

		public class SendgridConfiguration
		{
			public string ApiKey { get; set; } = string.Empty;
        }

		public class SecretsConfiguration
		{
			public string ApiKey { get; set; } = string.Empty;
			public string JwtPrivateKey { get; set; } = string.Empty;
			public string PasswordSaltKey { get; set; } = string.Empty;
		}
	}
}
