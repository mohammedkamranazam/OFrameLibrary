using OFrameLibrary.Models;
using OFrameLibrary.Util;
using System;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class PageCacheHelper
    {
        private static string fileName = AppConfig.PageCacheFile;
        private const string uniqueKey = "_PageCacheHelper_";
        private const string xPath = "pages/page";

        public static void AddCache(PageCache entity)
        {
            if (!IsPagePresent(entity.ID))
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                XmlElement newPage = xmlDoc.CreateElement("page");

                newPage.SetAttribute("id", entity.ID);
                newPage.SetAttribute("duration", entity.Minutes.ToString());
                newPage.SetAttribute("location", entity.Location);

                xmlDoc.SelectSingleNode(xPath).ParentNode.AppendChild(newPage);

                SaveXml(xmlDoc);
            }
        }

        public static void DeleteKeyword(string id)
        {
            if (IsPagePresent(id))
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                XmlNodeList pages = xmlDoc.SelectNodes(xPath);

                foreach (XmlNode page in pages)
                {
                    if (id == page.Attributes["id"].Value)
                    {
                        page.ParentNode.RemoveChild(page);

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        public static PageCache GetCache(string id)
        {
            return GetCache(id, AppConfig.PerformanceMode);
        }

        public static PageCache GetCacheFromSettings(string id)
        {
            PageCache entity = new PageCache();

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            XmlNodeList pages = xmlDoc.SelectNodes(xPath);

            foreach (XmlNode page in pages)
            {
                if (id == page.Attributes["id"].Value)
                {
                    entity.ID = page.Attributes["id"].Value;
                    entity.Location = page.Attributes["location"].Value;
                    entity.Minutes = int.Parse(page.Attributes["duration"].Value);
                    break;
                }
            }

            return entity;
        }

        public static PageCache GetCache(string id, PerformanceMode performanceMode)
        {
            PageCache keyValue = new PageCache();
            string performanceKey = uniqueKey + id;

            Func<string, PageCache> fnc = new Func<string, PageCache>(GetCacheFromSettings);

            object[] args = { id };

            Utilities.GetPerformance<PageCache>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static bool IsPagePresent(string id)
        {
            bool present = false;

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            XmlNodeList pages = xmlDoc.SelectNodes(xPath);

            foreach (XmlNode page in pages)
            {
                if (id == page.Attributes["id"].Value)
                {
                    present = true;
                    break;
                }
            }

            return present;
        }

        public static void SetKeywordValue(PageCache entity)
        {
            if (IsPagePresent(entity.ID))
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                XmlNodeList pages = xmlDoc.SelectNodes(xPath);

                foreach (XmlNode page in pages)
                {
                    if (entity.ID == page.Attributes["id"].Value)
                    {
                        page.Attributes["location"].Value = entity.Location;
                        page.Attributes["duration"].Value = entity.Minutes.ToString();

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        private static void SaveXml(XmlDocument xmlDoc)
        {
            XmlTextWriter xmlTextWriter = new XmlTextWriter(fileName, null);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlDoc.WriteContentTo(xmlTextWriter);
            xmlTextWriter.Close();
        }
    }
}