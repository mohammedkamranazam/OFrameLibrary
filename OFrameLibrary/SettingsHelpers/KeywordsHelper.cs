using OFrameLibrary.Util;
using System;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class KeywordsHelper
    {
        private const string uniqueKey = "_KeywordsHelper_";
        private const string xPath = "keys/key";

        private readonly static string fileName = AppConfig.KeywordsFile;

        private static void SaveXml(XmlDocument xmlDoc)
        {
            var xmlTextWriter = new XmlTextWriter(fileName, null);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlDoc.WriteContentTo(xmlTextWriter);
            xmlTextWriter.Close();
        }

        public static void AddKeyword(string keyName, string keyValue)
        {
            if (!IsKeywordPresent(keyName))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var newKey = xmlDoc.CreateElement("key");

                newKey.SetAttribute("name", keyName);
                newKey.SetAttribute("value", keyValue);

                xmlDoc.SelectSingleNode(xPath).ParentNode.AppendChild(newKey);

                SaveXml(xmlDoc);
            }
        }

        public static void DeleteKeyword(string keyName)
        {
            if (IsKeywordPresent(keyName))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var keys = xmlDoc.SelectNodes(xPath);

                foreach (XmlNode key in keys)
                {
                    if (keyName == key.Attributes["name"].Value)
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

        public static string GetKeywordValueFromSettings(string keyName)
        {
            var KeyValue = string.Format("KEY_[{0}]_UNDEFINED", keyName);

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var keys = xmlDoc.SelectNodes(xPath);

            foreach (XmlNode key in keys)
            {
                if (keyName == key.Attributes["name"].Value)
                {
                    KeyValue = key.Attributes["value"].Value;
                    break;
                }
            }

            return KeyValue;
        }

        public static bool IsKeywordPresent(string keyName)
        {
            var present = false;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var keys = xmlDoc.SelectNodes(xPath);

            foreach (XmlNode key in keys)
            {
                if (keyName == key.Attributes["name"].Value)
                {
                    present = true;
                    break;
                }
            }

            return present;
        }

        public static void SetKeywordValue(string keyName, string keyValue)
        {
            if (IsKeywordPresent(keyName))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var keys = xmlDoc.SelectNodes(xPath);

                foreach (XmlNode key in keys)
                {
                    if (keyName == key.Attributes["name"].Value)
                    {
                        key.Attributes["value"].Value = keyValue;

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }
    }
}