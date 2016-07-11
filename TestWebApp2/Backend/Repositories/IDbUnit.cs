using System;

namespace TestWebApp2.Backend.Repositories
{
	internal interface IDbUnit : IDisposable
	{
		ICurrencyRatesRepository CurrencyRates { get; }
		ICurrenciesRepository Currencies { get; }

		int Complete();
	}
}
