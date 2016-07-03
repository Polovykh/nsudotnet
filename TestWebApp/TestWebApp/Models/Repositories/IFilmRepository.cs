using System.Collections.Generic;
using TestWebApp.Models.Entities;

namespace TestWebApp.Models.Repositories
{
    public interface IFilmRepository : IRepository<Film>
    {
        IEnumerable<Film> GetFilmsByName(string name);

        IEnumerable<Film> GetFilmsByGenre(Genre genre);

        IEnumerable<Film> GetFilmsByActorName(string name);
    }
}
