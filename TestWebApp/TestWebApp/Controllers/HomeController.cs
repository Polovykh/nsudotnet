using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TestWebApp.Models;
using TestWebApp.Models.Entities;
using TestWebApp.Models.Repositories;

namespace TestWebApp.Controllers
{
    public class HomeController : Controller
    {
        private IFilmRepository Repository { get; }
        private Dictionary<SearchCriterionType, Func<string, IEnumerable<Film>>> Handlers { get; }

        public HomeController() : this(null)
        {

        }

        public HomeController(Context context)
        {
            Repository = new FilmRepository(context);
            Handlers = new Dictionary<SearchCriterionType, Func<string, IEnumerable<Film>>>()
            {
                {SearchCriterionType.SearchByName, Repository.GetFilmsByName},
                {SearchCriterionType.SearchByActorName, Repository.GetFilmsByActorName},
                {
                    SearchCriterionType.SearchByGenre, (str) =>
                    {
                        Genre genre;
                        return Enum.TryParse(str, out genre) ? Repository.GetFilmsByGenre(genre) : Repository.GetAll();
                    }
                }
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Repository?.Dispose();
            }

            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Films = Repository.GetAll();
            return View(new SearchCriterion());
        }

        [HttpPost]
        public ActionResult Index(SearchCriterion criterion)
        {
            ViewBag.Films = Handlers[criterion.Type](criterion.String);
            return View(criterion);
        }
    }
}