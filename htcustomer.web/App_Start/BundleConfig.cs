using System.Web;
using System.Web.Optimization;

namespace htcustomer.web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            StyleBundle baseCss = new StyleBundle("~/bundles/base-css");
            baseCss.Include("~/node_modules/bootstrap/dist/css/bootstrap.css");
            baseCss.Include("~/node_modules/bootstrap-vue/dist/bootstrap-vue.css");
                
            // inclue bootstrap , jquery, ...
            ScriptBundle baseScript = new ScriptBundle("~/bundles/base-script");
            baseScript.Include("~/Scripts/jquery-3.0.0.js");
            baseScript.Include("~/node_modules/bootstrap/dist/js/bootstrap.bundle.js");
            baseScript.Include("~/node_modules/bootstrap-vue/dist/bootstrap-vue.js");

            ScriptBundle scriptBundle = new ScriptBundle("~/bundles/vue/home");
            //use Include() method to add all the script files with their paths 
            scriptBundle.Include("~/Scripts/bundle/home.js");
            //Add the bundle into BundleCollection
            bundles.Add(scriptBundle);
            bundles.Add(baseCss);
            bundles.Add(baseScript);

            BundleTable.EnableOptimizations = true;
        }
    }
}