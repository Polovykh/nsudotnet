using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using TestWebApp2.Backend.Entities;

namespace TestWebApp2.Backend
{
	public class DbInitializer : DropCreateDatabaseAlways<AppContext>
	{
		protected override void Seed(AppContext context)
		{
			var currencies = InitializeCurrencies(context);
			InitializeCurrencyRates(context, currencies);

			base.Seed(context);
		}

		private static IEnumerable<Currency> InitializeCurrencies(AppContext context)
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
							  let currencyId = node["ParentCode"]?.InnerText
							  select new Currency()
							  {
								  ID = currencyId,
								  Name = currencyName
							  }).ToList();

			context.Currencies.AddRange(currencies);
			context.SaveChanges();

			return currencies;
		}

		private static void InitializeCurrencyRates(AppContext context, IEnumerable<Currency> currencies)
		{
			foreach (var currency in currencies)
			{
				const string date1 = "01/04/2016";
				var date2 = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

				var doc = new XmlDocument();
				var res = $"http://www.cbr.ru/scripts/XML_dynamic.asp?date_req1={date1}&date_req2={date2}&VAL_NM_RQ={currency.ID}";
				doc.Load(res);






			}
		}
	}
}
