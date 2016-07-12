﻿using System;
using System.Collections.Generic;
using System.Linq;
using TestWebApp2.Backend.Repositories;

namespace TestWebApp2.Models
{
	public class CurrenciesRatesModel
	{
		public static IEnumerable<CurrenciesRatesModel> GetCurrenciesRates()
		{
			using (var dbUnit = new DbUnit())
			{
				var usDollarRates = dbUnit.CurrencyRates.Find(rate => rate.Currency.Name == "US Dollar");
				var chinaYuanRates = dbUnit.CurrencyRates.Find(rate => rate.Currency.Name == "China Yuan");
				var japaneseYenRates = dbUnit.CurrencyRates.Find(rate => rate.Currency.Name == "Japanese Yen");

				return (from dollarRate in usDollarRates
				        join japaneseYenRate in japaneseYenRates on dollarRate.Date equals japaneseYenRate.Date
				        join chinaYuanRate in chinaYuanRates on dollarRate.Date equals chinaYuanRate.Date
				        select new CurrenciesRatesModel()
				               {
					               Date = dollarRate.Date,
					               USDollarRate = (double) dollarRate.Value,
					               ChinaYuanRate = (double) chinaYuanRate.Value,
					               JapaneseYenRate = (double) japaneseYenRate.Value
				               }).ToList();
			}
		}

		public DateTime Date { get; set; }
		// ReSharper disable once InconsistentNaming
		public double USDollarRate { get; set; }
		public double ChinaYuanRate { get; set; }
		public double JapaneseYenRate { get; set; }
	}
}