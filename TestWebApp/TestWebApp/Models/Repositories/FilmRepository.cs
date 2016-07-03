using System.Collections.Generic;
using System.Linq;
using TestWebApp.Models.Entities;

namespace TestWebApp.Models.Repositories
{
    public class FilmRepository : Repository<Film>, IFilmRepository
    {
        public FilmRepository(Context context = null) : base(context)
        {

        }

        public IEnumerable<Film> GetFilmsByName(string name) => (
            from film in Context.Films
            where film.Name == name
            select film).ToList();

        public IEnumerable<Film> GetFilmsByGenre(Genre genre) => (
            from film in Context.Films
                where film.Genre == genre
                select film).ToList();

        public IEnumerable<Film> GetFilmsByActorName(string name) => (
            from film in Context.Films
            where film.Actors.Any(a => a.Name == name)
            select film).ToList();
    }
}