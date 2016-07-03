using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestWebApp.Controllers;
using TestWebApp.Models;
using TestWebApp.Models.Entities;

namespace TestWebApp.Tests.Controllers
{
    [TestClass]
    public class InfoControllerTest
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
            var film = new Film()
            {
                Name = "AppTesting",
                Actors = new[] {new Actor() {Name = "Me"}, new Actor() {Name = "TestWebApp"}},
                Year = 2016,
                Genre = Genre.Drama
            };

            using (var context = new Context(Context.Type.TestDatabase))
            {
                context.Films.Add(film);
                context.SaveChanges();

                var amountOfFilms = context.Films.Count();

                using (var infoController = new InfoController(context))
                {
                    var view = infoController.Index((int) film.ID) as ViewResult;

                    film = view?.Model as Film;

                    Assert.IsNotNull(film);
                    Assert.AreEqual("AppTesting", film.Name);
                    Assert.AreEqual((short) 2016, film.Year);
                    Assert.AreEqual(Genre.Drama, film.Genre);
                    Assert.IsNotNull(film.Actors);
                    Assert.AreEqual(2, film.Actors.Count);
                    Assert.IsTrue(film.Actors.Any(a => a.Name == "Me"));
                    Assert.IsTrue(film.Actors.Any(a => a.Name == "TestWebApp"));


                    view = infoController.Index() as ViewResult;

                    Assert.IsNotNull(view);
                    Assert.AreEqual("Error", view.ViewName);


                    view = infoController.Index(amountOfFilms + 1) as ViewResult;

                    Assert.IsNotNull(view);
                    Assert.AreEqual("Error", view.ViewName);
                }
            }
        }
    }
}
