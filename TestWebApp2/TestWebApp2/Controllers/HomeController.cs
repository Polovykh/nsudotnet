using System.Web.Mvc;
using TestWebApp2.Backend.Repositories;
using TestWebApp2.Models;

namespace TestWebApp2.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			using (var dbUnit = new DbUnit())
			{
				var currencies = dbUnit.Currencies.GetAll();
			}

			return View(DateTimeXAxisChartData.GetLineChartData());
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}