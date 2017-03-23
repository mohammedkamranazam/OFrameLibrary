using OFrameLibrary.Models;
using OFrameLibrary.Util;
using System;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class SEOHelper
    {
        private const string pageXPath = "pages/page";
        private const string uniqueKey = "_PageSEO_";

        private static readonly string fileName = AppConfig.SEOFile;

        public static void AddPage(string id)
        {
            if (!PageExists(id))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var page = xmlDoc.CreateElement("page");
                page.SetAttribute("id", id);
                xmlDoc.SelectSingleNode(pageXPath).ParentNode.AppendChild(page);

                SaveXml(xmlDoc);
            }
        }

        public static void AddPageSEO(SEO entity)
        {
            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var newTitleMeta = xmlDoc.CreateElement("meta");
            newTitleMeta.SetAttribute("name", "title");
            newTitleMeta.SetAttribute("content", entity.Title);

            var newDescriptionMeta = xmlDoc.CreateElement("meta");
            newDescriptionMeta.SetAttribute("name", "description");
            newDescriptionMeta.SetAttribute("content", entity.Description);

            var newKeywordMeta = xmlDoc.CreateElement("meta");
            newKeywordMeta.SetAttribute("name", "keywords");
            newKeywordMeta.SetAttribute("content", entity.Keywords);

            var pages = xmlDoc.SelectNodes(pageXPath);

            foreach (XmlNode page in pages)
            {
                if (page.Attributes["id"].Value == entity.ID)
                {
                    page.AppendChild(newTitleMeta);
                    page.AppendChild(newDescriptionMeta);
                    page.AppendChild(newKeywordMeta);

                    SaveXml(xmlDoc);

                    break;
                }
            }
        }

        public static void DeletePage(string id)
        {
            if (PageExists(id))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var pages = xmlDoc.SelectNodes(pageXPath);

                foreach (XmlNode page in pages)
                {
                    if (page.Attributes["id"].Value == id)
                    {
                        page.ParentNode.RemoveChild(page);

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        public static SEO GetPageSEO(string id)
        {
            return GetPageSEO(id, AppConfig.PerformanceMode);
        }

        public static SEO GetPageSEO(string id, PerformanceMode performanceMode)
        {
            var keyValue = new SEO();
            var performanceKey = string.Format("{0}_{1}", uniqueKey, id);

            Func<string, SEO> fnc = GetPageSEOFromSettings;

            var args = new object[] { id };

            Utilities.GetPerformance<SEO>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static SEO GetPageSEOFromSettings(string id)
        {
            var entity = new SEO();
            entity.ID = id;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var pages = xmlDoc.SelectNodes(pageXPath);

            foreach (XmlNode page in pages)
            {
                if (page.Attributes["id"].Value == id)
                {
                    var metas = page.ChildNodes;

                    foreach (XmlNode meta in metas)
                    {
                        if ("keywords" == meta.Attributes["name"].Value)
                        {
                            entity.Keywords = meta.Attributes["content"].Value;
                        }

                        if ("title" == meta.Attributes["name"].Value)
                        {
                            entity.Title = meta.Attributes["content"].Value;
                        }

                        if ("description" == meta.Attributes["name"].Value)
                        {
                            entity.Description = meta.Attributes["content"].Value;
                        }
                    }

                    break;
                }
            }

            return entity;
        }

        public static bool PageExists(string id)
        {
            var present = false;

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

        public static void SetPageSEO(SEO entity)
        {
            if (PageExists(entity.ID))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var pages = xmlDoc.SelectNodes(pageXPath);

                foreach (XmlNode page in pages)
                {
                    if (page.Attributes["id"].Value == entity.ID)
                    {
                        var metas = page.ChildNodes;

                        foreach (XmlNode meta in metas)
                        {
                            if ("keywords" == meta.Attributes["name"].Value)
                            {
                                meta.Attributes["content"].Value = entity.Keywords;
                            }

                            if ("title" == meta.Attributes["name"].Value)
                            {
                                meta.Attributes["content"].Value = entity.Title;
                            }

                            if ("description" == meta.Attributes["name"].Value)
                            {
                                meta.Attributes["content"].Value = entity.Description;
                            }
                        }

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        private static void SaveXml(XmlDocument xmlDoc)
        {
            var xmlTextWriter = new XmlTextWriter(fileName, null);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlDoc.WriteContentTo(xmlTextWriter);
            xmlTextWriter.Close();
        }
    }
}
