using System.Web;
using System.Web.Optimization;

namespace AgroOrganic
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/Libs/jquery-1.10.2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/Libs/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/leaflet").Include(
                     "~/Scripts/assets/Leaflet.Editable.js"));
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/Libs/angular.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/map").Include(
                     "~/Scripts/assets/underscore.js",
                     "~/Scripts/Pages/calculators.js",
                     "~/Scripts/Pages/mapAng.js"));
        }
    }
}
