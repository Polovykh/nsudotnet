using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using TestWebApp2.Backend.Entities;
using TestWebApp2.Backend.Repositories;

namespace TestWebApp2.Backend
{
	public static class CurrencyManager
	{
		public static void Initialize(AppContext context)
		{
			var dbUnit = new DbUnit(context);
			var currencies = GetCurrencies(dbUnit);
			var currentDateTime = GetCurrencyRates(dbUnit, currencies);
			var state = new State()
			            {
				            Currencies = currencies,
				            LastUpdateTime = currentDateTime
			            };
			Timer = new Timer(UpdateCurrencyRates,
			                  state,
			                  (long) TimeSpan.FromDays(1)
			                                 .TotalMilliseconds,
			                  (long) TimeSpan.FromDays(1)
			                                 .TotalMilliseconds);
		}

		#region Main functions

		private static ICollection<Currency> GetCurrencies(IDbUnit dbUnit)
		{
			var doc = new XmlDocument();
			doc.Load("http://www.cbr.ru/scripts/XML_val.asp?d=0");
			var nodes = doc.SelectNodes("//Item");
			if (null == nodes)
			{
				throw new XPathException();
			}

			var supportedCurrencies = ConfigurationManager.AppSettings["supported_currencies"].Split(';');
			var currencies = (from node in nodes.Cast<XmlNode>()
			                  let currencyName = node["EngName"]?.InnerText
			                  where supportedCurrencies.Contains(currencyName)
			                  let currencyId = node["ParentCode"]?.InnerText.Trim()
			                  select new Currency()
			                         {
				                         ID = currencyId,
				                         Name = currencyName
			                         }).ToList();

			dbUnit.Currencies.AddRange(currencies);
			dbUnit.Complete();

			return currencies;
		}

		private static DateTime GetCurrencyRates(IDbUnit dbUnit, IEnumerable<Currency> currencies,
		                                         DateTime? startsWith = null)
		{
			var currentDateTime = DateTime.Now;
			var date1 = (startsWith ?? new DateTime(2016, 4, 1)).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
			var date2 = currentDateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

			foreach (var currency in currencies)
			{
				var doc = new XmlDocument();
				doc.Load($"http://www.cbr.ru/scripts/XML_dynamic.asp?date_req1={date1}&date_req2={date2}&VAL_NM_RQ={currency.ID}");
				var nodes = doc.SelectNodes("//Record");
				if (null == nodes)
				{
					throw new XPathException();
				}

				dbUnit.CurrencyRates.AddRange(from node in nodes.Cast<XmlNode>()
				                              let date = DateTime.Parse(node?.Attributes?["Date"].Value, new CultureInfo("ru-Ru"))
				                              let nominal = int.Parse(node["Nominal"]?.InnerText ?? "1")
				                              let value = node["Value"]?.InnerText
				                              where null != value
				                              select new CurrencyRate()
				                                     {
					                                     CurrencyID = currency.ID,
					                                     Currency = currency,
					                                     Date = date,
					                                     Value = double.Parse(value, new CultureInfo("ru-Ru"))/nominal
				                                     });
			}

			dbUnit.Complete();

			return currentDateTime;
		}

		#endregion

		#region Support members

		private static Timer Timer { get; set; }

		private static void UpdateCurrencyRates(object s)
		{
			var state = (State) s;
			using (var dbUnit = new DbUnit())
			{
				state.LastUpdateTime = GetCurrencyRates(dbUnit, state.Currencies,
				                                        state.LastUpdateTime);
			}
		}

		#endregion

		private class State
		{
			internal ICollection<Currency> Currencies { get; set; }
			internal DateTime? LastUpdateTime { get; set; }
		}
	}
}