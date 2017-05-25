using OFrameLibrary.Util;
using System.Collections.Generic;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class MvcThemeScriptsHelper
    {
        const string scriptXPath = "scripts/script";

        public static string[] GetPathsFromSettings(string themeName)
        {
            var fileName = $"~/Content/Themes/{themeName}/Scripts.xml".MapPath();

            var paths = new List<string>();

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode pathNode in xmlDoc.SelectNodes(scriptXPath))
            {
                paths.Add(pathNode.Attributes["path"].Value);
            }

            return paths.ToArray();
        }
    }
}
