using System.Web.Mvc;
using TestWebApp.Models;
using TestWebApp.Models.Repositories;

namespace TestWebApp.Controllers
{
    public class InfoController : Controller
    {
        private IFilmRepository Repository { get; }

        public InfoController() : this(null)
        {

        }

        public InfoController(Context context)
        {
            Repository = new FilmRepository(context);
        }

        [HttpGet]
        public ActionResult Index(int id = 0)
        {
            var film = Repository.GetById(id);

            return null != film ? View(film) : View("Error");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Repository?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}