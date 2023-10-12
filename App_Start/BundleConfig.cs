using System.Web.Optimization;

namespace Shopee_Food
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/plugins/jquery/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new Bundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
           "~/Content/plugins/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/plugins/fullcalendar/main.css",
                      "~/Content/plugins/fontawesome-free/css/all.min.css",
                      "~/Content/css/adminlte.min.css",
                      "~/Content/dist/css/adminlte.min.css",
                      "~/Content/plugins/chart.js/Chart.min.css",
                      "~/Content/plugins/summernote/summernote-bs4.css",
                      "~/Content/plugins/datatables-bs4/dataTables.bootstrap4.min.css",
                      "~/Content/plugins/datatables-buttons/css/buttons.bootstrap4.min.css",
                      "~/Content/vendor/animate/animate.css",
                      "~/Content/fonts/Linearicons-Free-v1.0.0/icon-font.min.css",
                      "~/Content/fonts/font-awesome-4.7.0/css/font-awesome.min.css",
                      "~/Content/vendor/bootstrap/css/bootstrap.min.css",
                      "~/Content/vendor/css-hamburgers/hamburgers.min.css",
                      "~/Content/vendor/animsition/css/animsition.min.css",
                      "~/Content/vendor/select2/select2.min.css",
                      "~/Content/vendor/daterangepicker/daterangepicker.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/adminlte/js").Include(
                        "~/Content/js/adminlte.min.js",
                        "~/Content/plugins/fullcalendar/main.js",
                        "~/Content/plugins/chart.js/Chart.min.js",
                        "~/Content/dist/js/pages/dashboard3.js",
                        "~/Content/plugin/datatables-responsive/js/dataTables.responsive.min.js",
                        "~/Content/plugin/datatables-responsive/js/responsive.bootstrap4.min.js",
                        "~/Content/plugin/datatables-bs4/js/dataTables.bootstrap4.min.js",
                        "~/Content/vendor/bootstrap/js/bootstrap.min.js",
                        "~/Content/vendor/jquery/jquery-3.2.1.min.js",
                        "~/Content/vendor/animsition/js/animsition.min.js",
                        "~/Content/vendor/bootstrap/js/popper.js",
                        "~/Content/vendor/select2/select2.min.js",
                        "~/Content/vendor/daterangepicker/moment.min.js",
                        "~/Content/vendor/daterangepicker/daterangepicker.js",
                        "~/Content/vendor/countdowntime/countdowntime.js",
                        "~/Content/js/main.js",
                        "~/Content/plugins/datatables-bs4/summernote-bs4.js"));
        }
    }
}