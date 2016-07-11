using TestWebApp2.Backend.Entities;

namespace TestWebApp2.Backend.Repositories
{
	public class CurrenciesRepository : Repository<Currency, string>, ICurrenciesRepository
	{
		internal CurrenciesRepository(AppContext context) : base(context)
		{
		}
	}
}
