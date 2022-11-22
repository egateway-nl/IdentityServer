using Microsoft.EntityFrameworkCore;
using WebClientSample.Model;

namespace WebClientSample.DataAccess
{
	public class IdpContext : DbContext
	{
		public IdpContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<User>? AspNetUsers { get; set; }

		public DbSet<Claim>? AspNetUserClaims { get; set; }
	}
}
