using OFrameLibrary.Util;
using System.Collections.Generic;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class ThemeStylesheetsHelper
    {
        const string xPath = "stylesheets/stylesheet";

        public static string[] GetPathsFromSettings(string themeName)
        {
            var fileName = string.Format("~/Themes/{0}/CSS.xml", themeName).MapPath();

            var paths = new List<string>();

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var pathNodes = xmlDoc.SelectNodes(xPath);

            foreach (XmlNode pathNode in pathNodes)
            {
                paths.Add(pathNode.Attributes["path"].Value);
            }

            return paths.ToArray();
        }
    }
}