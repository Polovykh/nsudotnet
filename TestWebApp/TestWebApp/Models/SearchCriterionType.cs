using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models
{
    public enum SearchCriterionType
    {
        [Display(Name="По названию")]
        SearchByName,
        [Display(Name = "По жанру")]
        SearchByGenre,
        [Display(Name = "По имени одного из актёров")]
        SearchByActorName
    }
}