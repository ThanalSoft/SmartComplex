using System.Web.Optimization;

namespace ThanalSoft.SmartComplex.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection pBundles)
        {
            pBundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            pBundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                         "~/Scripts/jquery.unobtrusive*",
                         "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            pBundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            pBundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            pBundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            pBundles.Add(new StyleBundle("~/Content/logincss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/login.css"));
        }
    }
}