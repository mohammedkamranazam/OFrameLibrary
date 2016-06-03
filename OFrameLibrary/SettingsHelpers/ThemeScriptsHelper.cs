using System.Collections.Generic;
using System.Web;
using System.Xml;
using OFrameLibrary.Util;

namespace OFrameLibrary.SettingsHelpers
{
    public static class ThemeScriptsHelper
    {
        private const string xPath = "scripts/script";

        public static string[] GetPathsFromSettings(string themeName)
        {
            var fileName = string.Format("~/Themes/{0}/Scripts.xml", themeName).MapPath();

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