using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using TestWebApp2.Backend.Entities;

namespace TestWebApp2.Backend
{
	public static class CurrencyManager
	{
		public static void Initialize(AppContext context)
		{
			var currencies = GetCurrencies(context);
			var currentDateTime = GetCurrencyRates(context, currencies);
			var timer = new Timer((s) =>
			                      {
				                      var state = (State) s;
				                      state.LastUpdateTime = GetCurrencyRates(new AppContext(), state.Currencies,
				                                                              state.LastUpdateTime);
			                      },
			                      new State()
			                      {
				                      Currencies = currencies,
				                      LastUpdateTime = currentDateTime
			                      },
			                      new DateTime(0, 0, 1).Millisecond,
			                      new DateTime(0, 0, 1).Millisecond);
		}

		#region Main functions

		private static ICollection<Currency> GetCurrencies(AppContext context)
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

			context.Currencies.AddRange(currencies);
			context.SaveChanges();

			return currencies;
		}

		private static DateTime GetCurrencyRates(AppContext context, IEnumerable<Currency> currencies,
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

				context.CurrencyRates.AddRange(from node in nodes.Cast<XmlNode>()
				                               let date = DateTime.Parse(node?.Attributes?["Date"].Value)
				                               let nominal = int.Parse(node["Nominal"]?.InnerText ?? "1")
				                               let value = node["Value"]?.InnerText
				                               where null != value
				                               select new CurrencyRate()
				                                      {
					                                      CurrencyID = currency.ID,
					                                      Currency = currency,
					                                      Date = date,
					                                      Value = decimal.Parse(value)/nominal
				                                      });
			}

			context.SaveChanges();

			return currentDateTime;
		}

		#endregion

		private class State
		{
			internal ICollection<Currency> Currencies { get; set; }
			internal DateTime? LastUpdateTime { get; set; }
		}
	}
}