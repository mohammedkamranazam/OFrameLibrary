using OFrameLibrary.Helpers;
using OFrameLibrary.SettingsHelpers;
using System.Web.Optimization;

namespace OFrameLibrary.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            foreach (var directory in ThemeHelper.GetThemeDirectories())
            {
                var cssBundle = new StyleBundle(string.Format("~/Theme_{0}", directory));

                cssBundle.Include(MvcThemeStylesheetsHelper.GetPathsFromSettings(directory));

                bundles.Add(cssBundle);

                var scriptBundle = new ScriptBundle(string.Format("~/Script_{0}", directory));

                scriptBundle.Include(MvcThemeScriptsHelper.GetPathsFromSettings(directory));

                bundles.Add(scriptBundle);
            }

            BundleTable.EnableOptimizations = AppConfig.EnableBundleOptimization;
        }
    }
}