using System.Web.Mvc;
using TestWebApp2.Models;

namespace TestWebApp2.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View(CurrenciesRatesModel.GetCurrenciesRates());
		}
	}
}