using OFrameLibrary.Models;
using OFrameLibrary.Util;
using System;
using System.Web;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class PageCacheHelper
    {
        static readonly string fileName = AppConfig.PageCacheFile;
        const string uniqueKey = "_PageCacheHelper_";
        const string pageXPath = "pages/page";

        public static void AddCache(PageCache entity)
        {
            if (!IsPagePresent(entity.ID))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var newPage = xmlDoc.CreateElement("page");

                newPage.SetAttribute("id", entity.ID);
                newPage.SetAttribute("duration", entity.Minutes.ToString());
                newPage.SetAttribute("location", entity.Location.ToString());

                xmlDoc.SelectSingleNode(pageXPath).ParentNode.AppendChild(newPage);

                SaveXml(xmlDoc);
            }
        }

        public static void DeleteKeyword(string id)
        {
            if (IsPagePresent(id))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var pages = xmlDoc.SelectNodes(pageXPath);

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
            var entity = new PageCache();

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var pages = xmlDoc.SelectNodes(pageXPath);

            foreach (XmlNode page in pages)
            {
                if (id == page.Attributes["id"].Value)
                {
                    entity.ID = page.Attributes["id"].Value;
                    entity.Location = Utilities.ParseEnum<HttpCacheability>(page.Attributes["location"].Value);
                    entity.Minutes = int.Parse(page.Attributes["duration"].Value);
                    break;
                }
            }

            return entity;
        }

        public static PageCache GetCache(string id, PerformanceMode performanceMode)
        {
            var keyValue = new PageCache();
            string performanceKey = uniqueKey + id;

            var fnc = new Func<string, PageCache>(GetCacheFromSettings);

            object[] args = { id };

            Utilities.GetPerformance<PageCache>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static bool IsPagePresent(string id)
        {
            bool present = false;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var pages = xmlDoc.SelectNodes(pageXPath);

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
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var pages = xmlDoc.SelectNodes(pageXPath);

                foreach (XmlNode page in pages)
                {
                    if (entity.ID == page.Attributes["id"].Value)
                    {
                        page.Attributes["location"].Value = entity.Location.ToString();
                        page.Attributes["duration"].Value = entity.Minutes.ToString();

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        static void SaveXml(XmlDocument xmlDoc)
        {
            var xmlTextWriter = new XmlTextWriter(fileName, null);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlDoc.WriteContentTo(xmlTextWriter);
            xmlTextWriter.Close();
        }
    }
}