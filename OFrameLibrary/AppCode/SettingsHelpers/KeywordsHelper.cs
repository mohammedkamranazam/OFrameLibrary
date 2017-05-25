using OFrameLibrary.Helpers;
using OFrameLibrary.Util;
using System;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class KeywordsHelper
    {
        const string keyXPath = "keys/key";
        const string uniqueKey = "_KeywordsHelper_";
        static readonly string fileName = AppConfig.KeywordsFile;

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

                foreach (XmlNode key in xmlDoc.SelectNodes(keyXPath))
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

            PerformanceHelper.GetPerformance<string>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static string GetKeywordValueFromSettings(string name)
        {
            var keyValue = string.Empty;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode key in xmlDoc.SelectNodes(keyXPath))
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

            foreach (XmlNode key in xmlDoc.SelectNodes(keyXPath))
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

                foreach (XmlNode key in xmlDoc.SelectNodes(keyXPath))
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

        static void SaveXml(XmlDocument xmlDoc)
        {
            var xmlTextWriter = new XmlTextWriter(fileName, null)
            {
                Formatting = Formatting.Indented
            };
            xmlDoc.WriteContentTo(xmlTextWriter);
            xmlTextWriter.Close();
        }
    }
}
