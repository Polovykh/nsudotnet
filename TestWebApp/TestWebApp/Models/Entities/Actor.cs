using System.Collections.Generic;

namespace TestWebApp.Models.Entities
{
    public class Actor : Entity
    {
        public string Name { get; set; }
        public virtual ICollection<Film> Films { get; set; }

        public Actor() : base()
        {
            
        }
    }
}