namespace TestWebApp2.Backend.Repositories
{
	public class DbUnit : IDbUnit
	{
		public DbUnit() : this(null)
		{
		}

		public DbUnit(AppContext context)
		{
			Context = context ?? new AppContext();
			CurrencyRates = new CurrencyRatesRepository(Context);
			Currencies = new CurrenciesRepository(Context);
		}

		#region Main methods

		public ICurrencyRatesRepository CurrencyRates { get; }

		public ICurrenciesRepository Currencies { get; }

		public int Complete()
		{
			return Context.SaveChanges();
		}

		public void Dispose()
		{
			Context.Dispose();
		}

		#endregion

		private AppContext Context { get; }
	}
}
