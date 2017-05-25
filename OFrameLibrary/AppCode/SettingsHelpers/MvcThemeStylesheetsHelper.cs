using OFrameLibrary.Util;
using System.Collections.Generic;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class MvcThemeStylesheetsHelper
    {
        const string stylesheetXPath = "stylesheets/stylesheet";

        public static string[] GetPathsFromSettings(string themeName)
        {
            var fileName = $"~/Content/Themes/{themeName}/CSS.xml".MapPath();

            var paths = new List<string>();

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode pathNode in xmlDoc.SelectNodes(stylesheetXPath))
            {
                paths.Add(pathNode.Attributes["path"].Value);
            }

            return paths.ToArray();
        }
    }
}
