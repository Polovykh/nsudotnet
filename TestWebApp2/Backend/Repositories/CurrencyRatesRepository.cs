using TestWebApp2.Backend.Entities;

namespace TestWebApp2.Backend.Repositories
{
	public class CurrencyRatesRepository : Repository<CurrencyRate, long>, ICurrencyRatesRepository
	{
		internal CurrencyRatesRepository(AppContext context) : base(context)
		{
		}
	}
}
