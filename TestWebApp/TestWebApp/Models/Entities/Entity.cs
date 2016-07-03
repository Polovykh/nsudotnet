using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models.Entities
{
    public class Entity
    {
        [Key]
        public long ID { get; set; }

        public Entity()
        {
            
        }
    }
}