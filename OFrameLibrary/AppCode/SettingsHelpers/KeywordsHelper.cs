using OFrameLibrary.Util;
using System;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class KeywordsHelper
    {
        const string uniqueKey = "_KeywordsHelper_";
        const string keyXPath = "keys/key";

        readonly static string fileName = AppConfig.KeywordsFile;

        static void SaveXml(XmlDocument xmlDoc)
        {
            var xmlTextWriter = new XmlTextWriter(fileName, null);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlDoc.WriteContentTo(xmlTextWriter);
            xmlTextWriter.Close();
        }

        public static void AddKeyword(string name, string value)
        {
            if (!IsKeywordPresent(name))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var newKey = xmlDoc.CreateElement("key");

                newKey.SetAttribute("name", name);
                newKey.SetAttribute("value", value);

                xmlDoc.SelectSingleNode(keyXPath).ParentNode.AppendChild(newKey);

                SaveXml(xmlDoc);
            }
        }

        public static void DeleteKeyword(string name)
        {
            if (IsKeywordPresent(name))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var keys = xmlDoc.SelectNodes(keyXPath);

                foreach (XmlNode key in keys)
                {
                    if (name == key.Attributes["name"].Value)
                    {
                        key.ParentNode.RemoveChild(key);

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        public static string GetKeywordValue(string name)
        {
            return GetKeywordValue(name, PerformanceMode.ApplicationState);
        }

        public static string GetKeywordValue(string name, PerformanceMode performanceMode)
        {
            var keyValue = string.Empty;

            var performanceKey = uniqueKey + name;

            Func<string, string> fnc = GetKeywordValueFromSettings;

            var args = new object[] { name };

            Utilities.GetPerformance<string>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static string GetKeywordValueFromSettings(string name)
        {
            var keyValue = string.Empty;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var keys = xmlDoc.SelectNodes(keyXPath);

            foreach (XmlNode key in keys)
            {
                if (name == key.Attributes["name"].Value)
                {
                    keyValue = key.Attributes["value"].Value;
                    break;
                }
            }

            return keyValue;
        }

        public static bool IsKeywordPresent(string name)
        {
            var present = false;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var keys = xmlDoc.SelectNodes(keyXPath);

            foreach (XmlNode key in keys)
            {
                if (name == key.Attributes["name"].Value)
                {
                    present = true;
                    break;
                }
            }

            return present;
        }

        public static void SetKeywordValue(string name, string value)
        {
            if (IsKeywordPresent(name))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var keys = xmlDoc.SelectNodes(keyXPath);

                foreach (XmlNode key in keys)
                {
                    if (name == key.Attributes["name"].Value)
                    {
                        key.Attributes["value"].Value = value;

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }
    }
}