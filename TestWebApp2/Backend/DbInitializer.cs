using System.Data.Entity;

namespace TestWebApp2.Backend
{
	public class DbInitializer : DropCreateDatabaseAlways<AppContext>
	{
		protected override void Seed(AppContext context)
		{
			CurrencyManager.Initialize(context);

			base.Seed(context);
		}
	}
}
