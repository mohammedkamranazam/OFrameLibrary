using OFrameLibrary.Helpers;
using OFrameLibrary.Models;
using OFrameLibrary.Util;
using System;
using System.Web;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class PageCacheHelper
    {
        const string pageXPath = "pages/page";
        const string uniqueKey = "_PageCacheHelper_";
        static readonly string fileName = AppConfig.PageCacheFile;

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

                foreach (XmlNode page in xmlDoc.SelectNodes(pageXPath))
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

        public static PageCache GetCache(string id, PerformanceMode performanceMode)
        {
            var keyValue = new PageCache();
            var performanceKey = uniqueKey + id;

            var fnc = new Func<string, PageCache>(GetCacheFromSettings);

            object[] args = { id };

            PerformanceHelper.GetPerformance<PageCache>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static PageCache GetCacheFromSettings(string id)
        {
            var entity = new PageCache();

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode page in xmlDoc.SelectNodes(pageXPath))
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

        public static bool IsPagePresent(string id)
        {
            var present = false;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode page in xmlDoc.SelectNodes(pageXPath))
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

                foreach (XmlNode page in xmlDoc.SelectNodes(pageXPath))
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

        public static void SetPageCache(PageCache entity)
        {
            switch (entity.Location)
            {
                case HttpCacheability.Public:
                case HttpCacheability.ServerAndPrivate:
                case HttpCacheability.Server:
                    var freshness = new TimeSpan(0, 0, 0, entity.Minutes);
                    var now = DateTime.Now;
                    HttpContext.Current.Response.Cache.SetExpires(now.Add(freshness));
                    HttpContext.Current.Response.Cache.SetMaxAge(freshness);
                    HttpContext.Current.Response.Cache.SetCacheability(entity.Location);
                    HttpContext.Current.Response.Cache.SetValidUntilExpires(true);
                    break;

                case HttpCacheability.Private:
                    HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddMinutes(entity.Minutes));
                    HttpContext.Current.Response.Cache.SetCacheability(entity.Location);
                    break;

                case HttpCacheability.NoCache:
                default:
                    HttpContext.Current.Response.Cache.SetCacheability(entity.Location);
                    break;
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
