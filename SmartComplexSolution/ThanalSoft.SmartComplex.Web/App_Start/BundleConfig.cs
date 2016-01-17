using System.Web.Optimization;

namespace ThanalSoft.SmartComplex.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection pBundles)
        {
            pBundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/nprogress.js"));

            pBundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                         "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            pBundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            pBundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/jquery.unobtrusive*",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/jquery.nicescroll.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/site.js"));

            pBundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/animate.css",
                      "~/Content/site.css"));

            pBundles.Add(new StyleBundle("~/Content/logincss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/login.css"));
        }
    }
}