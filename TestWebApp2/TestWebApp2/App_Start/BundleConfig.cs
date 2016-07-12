using System.Web;
using System.Web.Optimization;

namespace TestWebApp2
{
	public class BundleConfig
	{
		//Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
				"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
				"~/Scripts/jquery.validate*"));

			// Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
			// используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
				"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
				"~/Scripts/bootstrap.js",
				"~/Scripts/respond.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqchart").Include(
				"~/Scripts/jquery-1.11.1.min.js",
				"~/Scripts/jquery.mousewheel.js",
				"~/Scripts/jquery.jqChart.min.js",
				"~/Scripts/jquery.jqRangeSlider.min.js",
				"~/Scripts/excanvas.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/Content/bootstrap.css",
				"~/Content/site.css", 
				"~/Content/jquery.jqChart.css", 
				"~/Content/jquery.jqRangeSlider.css", 
				"~/Content/themes/smoothness/jquery-ui-1.10.4.css"));
		}
	}
}