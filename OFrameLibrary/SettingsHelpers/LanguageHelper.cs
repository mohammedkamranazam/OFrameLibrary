using OFrameLibrary.Helpers;
using OFrameLibrary.Models;
using OFrameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class LanguageHelper
    {
        private const string languagesUniqueKey = "_LanguagesDataSource_";
        private const string messageXPath = "languages/language";
        private const string uniqueKey = "_LanguageHelper_";

        private readonly static string fileName = AppConfig.LanguagesFile;

        private static void SaveXml(XmlDocument xmlDoc)
        {
            var xmlTextWriter = new XmlTextWriter(fileName, null);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlDoc.WriteContentTo(xmlTextWriter);
            xmlTextWriter.Close();
        }

        public static void AddKey(string name, string locale, string value)
        {
            if (!KeyExists(name, locale) && LanguageExists(locale))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var newKey = xmlDoc.CreateElement("key");

                newKey.SetAttribute("name", name);
                newKey.SetAttribute("value", value);

                var languages = xmlDoc.SelectNodes(messageXPath);

                foreach (XmlNode language in languages)
                {
                    if (language.Attributes["locale"].Value == locale)
                    {
                        language.AppendChild(newKey);

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        public static void AddLanguage(string locale, string direction, string name)
        {
            if (!LanguageExists(locale))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var newLanguage = xmlDoc.CreateElement("language");

                newLanguage.SetAttribute("locale", locale);
                newLanguage.SetAttribute("direction", direction);
                newLanguage.SetAttribute("name", name);

                xmlDoc.SelectSingleNode(messageXPath).ParentNode.AppendChild(newLanguage);

                SaveXml(xmlDoc);
            }
        }

        public static void DeleteKey(string name, string locale)
        {
            if (KeyExists(name, locale))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var languages = xmlDoc.SelectNodes(messageXPath);

                foreach (XmlNode language in languages)
                {
                    if (language.Attributes["locale"].Value == locale)
                    {
                        var keys = language.ChildNodes;

                        foreach (XmlNode key in keys)
                        {
                            if (name == key.Attributes["name"].Value)
                            {
                                key.ParentNode.RemoveChild(key);

                                SaveXml(xmlDoc);

                                break;
                            }
                        }

                        break;
                    }
                }
            }
        }

        public static string GetKey(string name)
        {
            var userSetLocale = CookiesHelper.GetCookie(Constants.Keys.CurrentCultureCookieKey);
            var defaultLocale = AppConfig.DefaultLocale;
            var currentLocale = CultureInfo.CurrentCulture.Name;

            if (!string.IsNullOrWhiteSpace(userSetLocale) && KeyExists(name, userSetLocale))
            {
                return GetKey(name, userSetLocale, AppConfig.PerformanceMode);
            }
            else
            {
                if (KeyExists(name, currentLocale))
                {
                    return GetKey(name, currentLocale, AppConfig.PerformanceMode);
                }
                else
                {
                    if (KeyExists(name, defaultLocale))
                    {
                        return GetKey(name, defaultLocale, AppConfig.PerformanceMode);
                    }
                    else
                    {
                        return string.Format("KEY_[{0}]_UNDEFINED", name);
                    }
                }
            }
        }

        public static string GetKey(string name, string locale)
        {
            return KeyExists(name, locale) ? GetKey(name, locale, AppConfig.PerformanceMode) : string.Empty;
        }

        public static string GetKey(string name, string locale, PerformanceMode performanceMode)
        {
            var keyValue = string.Format("KEY_[{0}]_UNDEFINED", name);

            var performanceKey = string.Format("{0}{1}_{2}", uniqueKey, name, locale);

            Func<string, string, string> fnc = GetKeyFromSettings;

            var args = new object[] { name, locale };

            Utilities.GetPerformance<string>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static string GetKeyFromSettings(string name, string locale)
        {
            var keyValue = string.Format("KEY_[{0}]_UNDEFINED", name);

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var languages = xmlDoc.SelectNodes(messageXPath);

            foreach (XmlNode language in languages)
            {
                if (language.Attributes["locale"].Value == locale)
                {
                    var keys = language.ChildNodes;

                    foreach (XmlNode key in keys)
                    {
                        if (name == key.Attributes["name"].Value)
                        {
                            keyValue = key.Attributes["value"].Value;
                            break;
                        }
                    }

                    break;
                }
            }

            return keyValue;
        }

        public static List<Language> GetLanguages()
        {
            return GetLanguages(AppConfig.PerformanceMode);
        }

        public static List<Language> GetLanguages(PerformanceMode performanceMode)
        {
            var keyValue = new List<Language>();

            Func<List<Language>> fnc = GetLanguagesFromSettings;

            var args = (object[])null;

            Utilities.GetPerformance<List<Language>>(performanceMode, languagesUniqueKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static List<Language> GetLanguagesFromSettings()
        {
            var langs = new List<Language>();

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var languages = xmlDoc.SelectNodes(messageXPath);

            Language lang;

            foreach (XmlNode language in languages)
            {
                lang = new Language();

                lang.Direction = language.Attributes["direction"].Value;
                lang.Locale = language.Attributes["locale"].Value;
                lang.Name = language.Attributes["name"].Value;

                langs.Add(lang);
            }

            return langs;
        }

        public static string GetLocaleDirection(string locale)
        {
            return GetLocaleDirection(locale, AppConfig.PerformanceMode);
        }

        public static string GetLocaleDirection(string locale, PerformanceMode performanceMode)
        {
            var keyValue = "ltr";
            var performanceKey = string.Format("{0}_{1}_LocaleDirection", uniqueKey, locale);

            Func<string, string> fnc = GetLocaleDirectionFromSettings;

            var args = new object[] { locale };

            Utilities.GetPerformance<string>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }
        public static string GetLocaleDirectionFromSettings(string locale)
        {
            var directionValue = "ltr";

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var languages = xmlDoc.SelectNodes(messageXPath);

            foreach (XmlNode language in languages)
            {
                if (language.Attributes["locale"].Value == locale)
                {
                    directionValue = language.Attributes["direction"].Value;

                    break;
                }
            }

            return directionValue;
        }

        public static bool KeyExists(string name, string locale)
        {
            return KeyExists(name, locale, AppConfig.PerformanceMode);
        }

        public static bool KeyExists(string name, string locale, PerformanceMode performanceMode)
        {
            var keyValue = false;
            var performanceKey = string.Format("{0}{1}_{2}_KeyExistance", uniqueKey, name, locale);

            Func<string, string, bool> fnc = KeyExistsFromSettings;

            var args = new object[] { name, locale };

            Utilities.GetPerformance<bool>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static bool KeyExistsFromSettings(string name, string locale)
        {
            var present = false;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var languages = xmlDoc.SelectNodes(messageXPath);

            foreach (XmlNode language in languages)
            {
                if (language.Attributes["locale"].Value == locale)
                {
                    var keys = language.ChildNodes;

                    foreach (XmlNode key in keys)
                    {
                        if (name == key.Attributes["name"].Value)
                        {
                            present = true;
                            break;
                        }
                    }

                    break;
                }
            }

            return present;
        }

        public static bool LanguageExists(string locale)
        {
            return LanguageExists(locale, AppConfig.PerformanceMode);
        }

        public static bool LanguageExists(string locale, PerformanceMode performanceMode)
        {
            var keyValue = false;
            var performanceKey = string.Format("{0}_{1}_LanguageExistance", uniqueKey, locale);

            Func<string, bool> fnc = LanguageExistsFromSettings;

            var args = new object[] { locale };

            Utilities.GetPerformance<bool>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static bool LanguageExistsFromSettings(string locale)
        {
            var present = false;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var languages = xmlDoc.SelectNodes(messageXPath);

            foreach (XmlNode language in languages)
            {
                if (locale == language.Attributes["locale"].Value)
                {
                    present = true;
                    break;
                }
            }

            return present;
        }

        public static void SetKey(string name, string locale, string value)
        {
            if (KeyExists(name, locale))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var languages = xmlDoc.SelectNodes(messageXPath);

                foreach (XmlNode language in languages)
                {
                    if (language.Attributes["locale"].Value == locale)
                    {
                        var keys = language.ChildNodes;

                        foreach (XmlNode key in keys)
                        {
                            if (name == key.Attributes["name"].Value)
                            {
                                key.Attributes["value"].Value = value;

                                SaveXml(xmlDoc);

                                break;
                            }
                        }

                        break;
                    }
                }
            }
        }
    }
}