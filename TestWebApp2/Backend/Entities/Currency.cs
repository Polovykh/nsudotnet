using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebApp2.Backend.Entities
{
	public class Currency
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string ID { get; set; }
		public string Name { get; set; }
		public virtual ICollection<CurrencyRate> Rates { get; set; } = new List<CurrencyRate>();

		public Currency()
		{
		}
	}
}