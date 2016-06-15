using OFrameLibrary.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OFrameLibrary.Helpers
{
    public static class ThemeHelper
    {
        public static IEnumerable<string> GetThemeDirectories()
        {
            return Directory.EnumerateDirectories("~/Content/Themes/".MapPath(), "*", SearchOption.TopDirectoryOnly).Select(c => new DirectoryInfo(c).Name);
        }

        public static string GetTheme(string theme)
        {
            var layoutPath = "~/Views/Layouts/FallBack.cshtml";

            if (File.Exists(layoutPath.MapPath()))
            {
                layoutPath = string.Format("~/Content/Themes/{0}/Layout.cshtml", theme);
            }

            return layoutPath;
        }
    }
}