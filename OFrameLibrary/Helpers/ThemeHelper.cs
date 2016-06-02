using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OFrameLibrary.Helpers
{
    public static class ThemeHelper
    {
        public static IEnumerable<string> GetThemeDirectories()
        {
            return Directory.EnumerateDirectories(HttpContext.Current.Server.MapPath("~/Content/Themes/"), "*", SearchOption.TopDirectoryOnly).Select(c => new DirectoryInfo(c).Name);
        }

        public static string GetTheme(string theme)
        {
            var layoutPath = "~/Views/Layouts/FallBack.cshtml";

            if (File.Exists(HttpContext.Current.Server.MapPath(layoutPath)))
            {
                layoutPath = string.Format("~/Content/Themes/{0}/Layout.cshtml", theme);
            }

            return layoutPath;
        }
    }
}