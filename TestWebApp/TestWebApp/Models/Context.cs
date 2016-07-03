using System.Data.Entity;
using TestWebApp.Models.Entities;

namespace TestWebApp.Models
{
    public class Context : DbContext
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Film> Films { get; set; }

        public Context(Type type = Type.RealDatabase) : base(type.ToString())
        {
            
        }

        public enum Type
        {
            RealDatabase,
            TestDatabase
        }
    }
}