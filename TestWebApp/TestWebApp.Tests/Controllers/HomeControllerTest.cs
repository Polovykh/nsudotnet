using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestWebApp.Controllers;
using TestWebApp.Models;
using TestWebApp.Models.Entities;

namespace TestWebApp.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestInitialize]
        public void Initialize()
        {
            using (var context = new Context(Context.Type.TestDatabase))
            {
                context.Database.Delete();
                context.Database.Create();
            }
        }

        [TestMethod]
        public void IndexGet()
        {
            using (var context = new Context(Context.Type.TestDatabase))
            {
                var films = context.Films.ToList();

                using (var homeController = new HomeController(context))
                {
                    var view = homeController.Index() as ViewResult;
                    Assert.IsNotNull(view?.Model as SearchCriterion);

                    var filmsInBag = homeController.ViewBag.Films;
                    Assert.IsNotNull(filmsInBag);
                    Assert.IsTrue(Enumerable.SequenceEqual(films, filmsInBag, new FilmComparer()));
                }
            }
        }

        [TestMethod]
        public void IndexPost()
        {
            using (var context = new Context(Context.Type.TestDatabase))
            {
                var actors = new[]
                {
                    new Actor() {Name = "First"},
                    new Actor() {Name = "Second"},
                    new Actor() {Name = "Third"}
                };
                context.Films.AddRange(new[]
                {
                    new Film()
                    {
                        Year = 2015,
                        Name = "FilmSearching",
                        Genre = Genre.Comedy,
                        Actors = new[] {actors[0], actors[1]}
                    },
                    new Film()
                    {
                        Year = 1995,
                        Name = "FilmSearching",
                        Genre = Genre.Action,
                        Actors = new[] {actors[1], actors[2]}
                    },
                    new Film()
                    {
                        Year = 1992,
                        Name = "AnotherOne",
                        Genre = Genre.Comedy,
                        Actors = new[] {actors[0], actors[2]}
                    }
                });
                context.SaveChanges();

                var films = context.Films.ToList();

                using (var homeController = new HomeController(context))
                {
                    // by name searching
                    var view = homeController.Index(new SearchCriterion()
                    {
                        Type = SearchCriterionType.SearchByName,
                        String = "FilmSearching"
                    }) as ViewResult;
                    Assert.IsNotNull(view?.Model as SearchCriterion);

                    IEnumerable<Film> selectedFilms = homeController.ViewBag.Films;
                    Assert.IsNotNull(selectedFilms);

                    var desiredFilms = from film in films
                        where film.Name == "FilmSearching"
                        select film;

                    Assert.AreEqual(2, selectedFilms.Count());
                    Assert.IsTrue(desiredFilms.SequenceEqual(selectedFilms, new FilmComparer()));

                    // by genre searching
                    view = homeController.Index(new SearchCriterion()
                    {
                        Type = SearchCriterionType.SearchByGenre,
                        String = "Comedy"
                    }) as ViewResult;

                    selectedFilms = homeController.ViewBag.Films;
                    Assert.IsNotNull(selectedFilms);

                    desiredFilms = from film in films
                        where film.Genre.ToString() == "Comedy"
                        select film;

                    Assert.AreEqual(2, selectedFilms.Count());
                    Assert.IsTrue(desiredFilms.SequenceEqual(selectedFilms, new FilmComparer()));

                    // by actor name searching
                    view = homeController.Index(new SearchCriterion()
                    {
                        Type = SearchCriterionType.SearchByActorName,
                        String = "First"
                    }) as ViewResult;

                    selectedFilms = homeController.ViewBag.Films;
                    Assert.IsNotNull(selectedFilms);

                    desiredFilms = from film in films
                        where film.Actors.Any(a => a.Name == "First")
                        select film;

                    Assert.AreEqual(2, selectedFilms.Count());
                    Assert.IsTrue(desiredFilms.SequenceEqual(selectedFilms, new FilmComparer()));
                }
            }
        }
    }

    internal class FilmComparer : IEqualityComparer<Film>
    {
        internal const int DefaultSeed = 17;
        internal const int DefaultDivisor = 19193731;


        public bool Equals(Film x, Film y)
        {
            if (x == y)
            {
                return true;
            }

            if (null == x || null == y)
            {
                return false;
            }

            return x.Name == y.Name &&
                   x.Genre == y.Genre &&
                   x.Tagline == y.Tagline &&
                   x.Year == y.Year;
        }

        public int GetHashCode(Film x)
        {
            var hashCode = DefaultSeed;

            hashCode = (hashCode*x.Name.GetHashCode() + DefaultSeed)%DefaultDivisor;
            hashCode = (hashCode*x.Description.GetHashCode() + DefaultSeed)%DefaultDivisor;
            hashCode = (hashCode*x.Genre.GetHashCode() + DefaultSeed)%DefaultDivisor;
            hashCode = (hashCode*x.Tagline.GetHashCode() + DefaultSeed)%DefaultDivisor;
            hashCode = (hashCode*x.Year.GetHashCode() + DefaultSeed)%DefaultDivisor;

            return hashCode;
        }
    }
}