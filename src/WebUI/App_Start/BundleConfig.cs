using System.Web;
using System.Web.Optimization;

namespace WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                        "~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/lib/bootstrap/dist/css/bootstrap.min.css",
                        "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                        "~/Scripts/js/site.js",
                        "~/Scripts/js/Admin.js"));


            //bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
            //"~/Scripts/dataTables.js",
            //"~/Scripts/dataTables.min.js"));

            #region Bootstrap Select

            bundles.Add(new StyleBundle("~/lib/bootstrap-select").Include(
                "~/lib/bootstrap-select/dist/css/bootstrap-select.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-select").Include(
                "~/lib/bootstrap-select/dist/js/bootstrap-select.js"));

            #endregion

            bundles.Add(new StyleBundle("~/lib/jquery-datatables").Include(
                "~/lib/jquery-datatables/css/jquery.dataTables.css",
            "~/lib/jquery-contextmenu/dist/jquery.contextMenu.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
            "~/lib/jquery-datatables/js/jquery.dataTables.js",
            "~/lib/jquery-contextmenu/dist/jquery.contextMenu.min.js",
            "~/lib/jquery-contextmenu/dist/jquery.ui.position.min.js"));



        }
    }
}
