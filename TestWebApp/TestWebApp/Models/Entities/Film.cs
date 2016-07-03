using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models.Entities
{
    public class Film : Entity
    {
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Слоган")]
        public string Tagline { get; set; }
        [Display(Name = "Жанр")]
        public Genre Genre { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Год")]
        public short? Year { get; set; }
        [Display(Name = "Актёры")]
        public virtual ICollection<Actor> Actors { get; set; }

        public Film() : base()
        {

        }
    }
}