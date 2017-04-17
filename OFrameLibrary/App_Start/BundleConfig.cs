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
                var cssBundle = new StyleBundle($"~/Theme_{directory}");

                cssBundle.Include(MvcThemeStylesheetsHelper.GetPathsFromSettings(directory));

                bundles.Add(cssBundle);

                var scriptBundle = new ScriptBundle($"~/Script_{directory}");

                scriptBundle.Include(MvcThemeScriptsHelper.GetPathsFromSettings(directory));

                bundles.Add(scriptBundle);
            }

            BundleTable.EnableOptimizations = AppConfig.EnableBundleOptimization;
        }
    }
}
