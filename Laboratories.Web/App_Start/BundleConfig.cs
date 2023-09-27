using System.Web;
using System.Web.Optimization;

namespace Laboratories.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));


            //bootstrap RightToLeft css
            bundles.Add(new StyleBundle("~/assets/css-rtl").Include
                ("~/assets/css-rtl/app.min.css"));

            //bootstrap LeftToRight css
            bundles.Add(new StyleBundle("~/assets/css").Include(
                "~/assets/css/app.min.css"));


            //bundles.Add(new StyleBundle("~/assets-dark/css-rtl").Include
            // ("~/assets/css-rtl/app-dark.min.css"));

            ////bootstrap LeftToRight css
            //bundles.Add(new StyleBundle("~/assets-dark/css").Include
            //    ("~/assets/css/app-dark.min.css"));


        }
    }
}
