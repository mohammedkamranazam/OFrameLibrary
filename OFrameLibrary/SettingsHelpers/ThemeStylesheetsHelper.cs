using System.Collections.Generic;
using System.Web;
using System.Xml;
using OFrameLibrary.Util;

namespace OFrameLibrary.SettingsHelpers
{
    public static class ThemeStylesheetsHelper
    {
        private const string xPath = "stylesheets/stylesheet";

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