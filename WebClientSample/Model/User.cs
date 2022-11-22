namespace WebClientSample.Model
{
	public class User
	{
		public string? Id { get; set; }

		public string? UserName { get; set; }
	}

	public class Claim
	{
		public int Id { get; set; }

		public string? UserId { get; set; }

		public string? ClaimType { get; set; }

		public string? ClaimValue { get; set; }
	}
}
