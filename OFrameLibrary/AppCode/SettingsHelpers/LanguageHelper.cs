using OFrameLibrary.Helpers;
using OFrameLibrary.Models;
using OFrameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class LanguageHelper
    {
        const string languagesUniqueKey = "_LanguagesDataSource_";
        const string messageXPath = "languages/language";
        const string uniqueKey = "_LanguageHelper_";

        static readonly string fileName = AppConfig.LanguagesFile;

        public static void AddKey(string name, string locale, string value)
        {
            if (!KeyExists(name, locale) && LanguageExists(locale))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var newKey = xmlDoc.CreateElement("key");

                newKey.SetAttribute("name", name);
                newKey.SetAttribute("value", value);

                foreach (XmlNode language in xmlDoc.SelectNodes(messageXPath))
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

                foreach (XmlNode language in xmlDoc.SelectNodes(messageXPath))
                {
                    if (language.Attributes["locale"].Value == locale)
                    {
                        foreach (XmlNode key in language.ChildNodes)
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
                        return $"KEY_[{name}]_UNDEFINED";
                    }
                }
            }
        }

        public static string GetKey(string name, string locale)
        {
            return KeyExists(name, locale) ? GetKey(name, locale, AppConfig.PerformanceMode) : $"KEY_[{name}]_UNDEFINED";
        }

        public static string GetKey(string name, string locale, PerformanceMode performanceMode)
        {
            var keyValue = $"KEY_[{name}]_UNDEFINED";

            var performanceKey = $"{uniqueKey}{name}_{locale}";

            Func<string, string, string> fnc = GetKeyFromSettings;

            var args = new object[] { name, locale };

            PerformanceHelper.GetPerformance(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static string GetKeyFromSettings(string name, string locale)
        {
            var keyValue = $"KEY_[{name}]_UNDEFINED";

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode language in xmlDoc.SelectNodes(messageXPath))
            {
                if (language.Attributes["locale"].Value == locale)
                {
                    foreach (XmlNode key in language.ChildNodes)
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

            PerformanceHelper.GetPerformance(performanceMode, languagesUniqueKey, out keyValue, fnc, args);

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
                lang = new Language
                {
                    Direction = language.Attributes["direction"].Value,
                    Locale = language.Attributes["locale"].Value,
                    Name = language.Attributes["name"].Value
                };
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
            var performanceKey = $"{uniqueKey}_{locale}_LocaleDirection";

            Func<string, string> fnc = GetLocaleDirectionFromSettings;

            var args = new object[] { locale };

            PerformanceHelper.GetPerformance(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static string GetLocaleDirectionFromSettings(string locale)
        {
            var directionValue = "ltr";

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode language in xmlDoc.SelectNodes(messageXPath))
            {
                if (language.Attributes["locale"].Value == locale)
                {
                    directionValue = language.Attributes["direction"].Value;

                    break;
                }
            }

            return directionValue;
        }

        public static string GetLocaleHash(string locale)
        {
            return $"{locale}#{GetLocaleName(locale)};";
        }

        public static string GetLocaleName(string locale)
        {
            return GetLanguages().FirstOrDefault(c => c.Locale == locale)?.Name;
        }

        public static string GetLocalesHash(List<Translator> translations)
        {
            var locales = translations.Select(c => c.Locale).ToList();

            locales = locales.Select(c => GetLocaleHash(c)).ToList();

            return locales.ToArray<string>().Join("");
        }

        public static string GetTranslation(List<Translator> translations, string locale)
        {
            var transText = translations.FirstOrDefault(d => d.Locale == locale);

            if (transText != null)
            {
                return transText.Text;
            }
            else
            {
                return $"No Translation Found For Language: {GetLocaleName(locale)}";
            }
        }

        public static bool KeyExists(string name, string locale)
        {
            return KeyExists(name, locale, AppConfig.PerformanceMode);
        }

        public static bool KeyExists(string name, string locale, PerformanceMode performanceMode)
        {
            var keyValue = false;
            var performanceKey = $"{uniqueKey}{name}_{locale}_KeyExistance";

            Func<string, string, bool> fnc = KeyExistsFromSettings;

            var args = new object[] { name, locale };

            PerformanceHelper.GetPerformance(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static bool KeyExistsFromSettings(string name, string locale)
        {
            var present = false;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode language in xmlDoc.SelectNodes(messageXPath))
            {
                if (language.Attributes["locale"].Value == locale)
                {
                    foreach (XmlNode key in language.ChildNodes)
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
            var performanceKey = $"{uniqueKey}_{locale}_LanguageExistance";

            Func<string, bool> fnc = LanguageExistsFromSettings;

            var args = new object[] { locale };

            PerformanceHelper.GetPerformance(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static bool LanguageExistsFromSettings(string locale)
        {
            var present = false;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode language in xmlDoc.SelectNodes(messageXPath))
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

                foreach (XmlNode language in xmlDoc.SelectNodes(messageXPath))
                {
                    if (language.Attributes["locale"].Value == locale)
                    {
                        foreach (XmlNode key in language.ChildNodes)
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
