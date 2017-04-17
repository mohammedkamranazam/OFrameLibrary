using OFrameLibrary.Helpers;
using OFrameLibrary.Util;
using System;
using System.IO;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class LocalStoragesHelper
    {
        const string localStoragesUniqueKey = "_LocalStoragesHelper_";
        const string localStoragesXPath = "storages/storage";

        static readonly string fileName = AppConfig.LocalStoragesFile;

        public static void AddStoragePath(string name, string path)
        {
            if (!StoragePathExists(name) && path.CheckPathFormat().MapPath().CreateDirectory())
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var newStorage = xmlDoc.CreateElement("storage");

                newStorage.SetAttribute("name", name);
                newStorage.SetAttribute("path", path);

                xmlDoc.SelectSingleNode(localStoragesXPath).ParentNode.AppendChild(newStorage);

                SaveXml(xmlDoc);
            }
        }

        public static void DeleteStoragePath(string name)
        {
            if (StoragePathExists(name))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                foreach (XmlNode storage in xmlDoc.SelectNodes(localStoragesXPath))
                {
                    if (name == storage.Attributes["name"].Value && storage.Attributes["path"].Value.CheckPathFormat().MapPath().DeleteDirectory())
                    {
                        storage.ParentNode.RemoveChild(storage);

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        public static string GetStoragePath(string name)
        {
            return GetStoragePath(name, AppConfig.PerformanceMode);
        }

        public static string GetStoragePath(string name, PerformanceMode performanceMode)
        {
            var keyValue = string.Empty;
            var performanceKey = localStoragesUniqueKey + name;

            Func<string, string> fnc = GetStoragePathFromSettings;

            var args = new object[] { name };

            PerformanceHelper.GetPerformance<string>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static string GetStoragePathFromSettings(string name)
        {
            var path = "~/";

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode storage in xmlDoc.SelectNodes(localStoragesXPath))
            {
                if (name == storage.Attributes["name"].Value)
                {
                    path = storage.Attributes["path"].Value;
                    break;
                }
            }

            path.MapPath().CreateDirectory();

            return path;
        }

        public static void SetStoragePath(string name, string path)
        {
            if (StoragePathExists(name))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                foreach (XmlNode storage in xmlDoc.SelectNodes(localStoragesXPath))
                {
                    if (name == storage.Attributes["name"].Value)
                    {
                        storage.Attributes["value"].Value = path;

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        public static bool StoragePathExists(string name)
        {
            var present = false;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode storage in xmlDoc.SelectNodes(localStoragesXPath))
            {
                if (name == storage.Attributes["name"].Value)
                {
                    present = true;
                    break;
                }
            }

            return present;
        }

        static string CheckPathFormat(this string path)
        {
            if (!path.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                path += "/";
            }

            return path;
        }

        static bool CreateDirectory(this string absPath)
        {
            var success = true;

            if (!Directory.Exists(absPath))
            {
                try
                {
                    Directory.CreateDirectory(absPath);
                }
                catch (Exception ex)
                {
                    success = false;
                    ErrorLogger.LogError(ex);
                }
            }

            return success;
        }

        static bool DeleteDirectory(this string absPath)
        {
            var success = false;

            try
            {
                Directory.Delete(absPath);
                success = true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
            }

            return success;
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
