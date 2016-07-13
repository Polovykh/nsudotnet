using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebApp2.Backend.Entities
{
	public class CurrencyRate
	{
		[Key]
		public long ID { get; set; }
		public DateTime Date { get; set; }
		public double Value { get; set; }

		[ForeignKey("CurrencyID")]
		public virtual Currency Currency { get; set; }
		public string CurrencyID { get; set; } 
		
		public CurrencyRate()
		{
		}
	}
}