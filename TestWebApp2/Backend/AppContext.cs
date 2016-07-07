using System.Data.Entity;
using TestWebApp2.Backend.Entities;

namespace TestWebApp2.Backend
{
	public class AppContext : DbContext
	{
		public DbSet<Currency> Currencies { get; set; }
		public DbSet<CurrencyRate> CurrencyRates { get; set; }

		public AppContext() : base("TestWebApp2DB")
		{
		}
	}
}