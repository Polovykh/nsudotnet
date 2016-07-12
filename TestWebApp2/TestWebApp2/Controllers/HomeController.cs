using System.Web.Mvc;
using TestWebApp2.Backend.Repositories;

namespace TestWebApp2.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			using (var dbUnit = new DbUnit())
			{
				var currencyRates = dbUnit.CurrencyRates.Find(r => r.Currency.Name == "US Dollar");

				return View(currencyRates);
			}
		}
	}
}