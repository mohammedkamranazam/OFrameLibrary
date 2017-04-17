using OFrameLibrary.Helpers;
using OFrameLibrary.Util;
using System;
using System.Globalization;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class ValidationHelper
    {
        const string expressionUniqueKey = "_ValidationExpressionHelper_";
        const string expressionXPath = "validationSetting/validationExpressions/validationExpression";
        const string messageUniqueKey = "_ValidationMessageHelper_";
        const string messageXPath = "validationSetting/validationMessages/language";

        static readonly string fileName = AppConfig.ValidationSettingsFile;

        public static void AddExpression(string name, string value)
        {
            if (!ExpressionExists(name))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var newExpression = xmlDoc.CreateElement("validationExpression");

                newExpression.SetAttribute("name", name);
                newExpression.SetAttribute("value", value);

                xmlDoc.SelectSingleNode(expressionXPath).ParentNode.AppendChild(newExpression);

                SaveXml(xmlDoc);
            }
        }

        public static void AddLanguage(string locale)
        {
            if (!LanguageExists(locale))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var newLanguage = xmlDoc.CreateElement("language");

                newLanguage.SetAttribute("locale", locale);

                xmlDoc.SelectSingleNode(messageXPath).ParentNode.AppendChild(newLanguage);

                SaveXml(xmlDoc);
            }
        }

        public static void AddMessage(string name, string locale, string value)
        {
            if (!MessageExistsFromSettings(name, locale) && LanguageExists(locale))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var newMessage = xmlDoc.CreateElement("validationMessage");

                newMessage.SetAttribute("name", name);
                newMessage.SetAttribute("value", value);

                foreach (XmlNode language in xmlDoc.SelectNodes(messageXPath))
                {
                    if (language.Attributes["locale"].Value == locale)
                    {
                        language.AppendChild(newMessage);

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        public static void DeleteExpression(string name)
        {
            if (ExpressionExists(name))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                foreach (XmlNode expression in xmlDoc.SelectNodes(expressionXPath))
                {
                    if (name == expression.Attributes["name"].Value)
                    {
                        expression.ParentNode.RemoveChild(expression);

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        public static void DeleteMessage(string name, string locale)
        {
            if (MessageExistsFromSettings(name, locale))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                foreach (XmlNode language in xmlDoc.SelectNodes(messageXPath))
                {
                    if (language.Attributes["locale"].Value == locale)
                    {
                        foreach (XmlNode message in language.ChildNodes)
                        {
                            if (name == message.Attributes["name"].Value)
                            {
                                message.ParentNode.RemoveChild(message);

                                SaveXml(xmlDoc);

                                break;
                            }
                        }

                        break;
                    }
                }
            }
        }

        public static bool ExpressionExists(string name)
        {
            var present = false;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode expression in xmlDoc.SelectNodes(expressionXPath))
            {
                if (name == expression.Attributes["name"].Value)
                {
                    present = true;
                    break;
                }
            }

            return present;
        }

        public static string GetExpression(string name)
        {
            return GetExpression(name, AppConfig.PerformanceMode);
        }

        public static string GetExpression(string name, PerformanceMode performanceMode)
        {
            var keyValue = string.Empty;
            var performanceKey = expressionUniqueKey + name;

            Func<string, string> fnc = GetExpressionFromSettings;

            var args = new object[] { name };

            PerformanceHelper.GetPerformance<string>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static string GetExpressionFromSettings(string name)
        {
            var validationExpression = string.Empty;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode expression in xmlDoc.SelectNodes(expressionXPath))
            {
                if (name == expression.Attributes["name"].Value)
                {
                    validationExpression = expression.Attributes["value"].Value;
                    break;
                }
            }

            return validationExpression;
        }

        public static string GetMessage(string name)
        {
            var userSetLocale = CookiesHelper.GetCookie(Constants.Keys.CurrentCultureCookieKey);
            var defaultLocale = AppConfig.DefaultLocale;
            var currentLocale = CultureInfo.CurrentCulture.Name;

            if (!string.IsNullOrWhiteSpace(userSetLocale) && MessageExistsFromSettings(name, userSetLocale))
            {
                return GetMessage(name, userSetLocale);
            }
            else
            {
                if (MessageExistsFromSettings(name, currentLocale))
                {
                    return GetMessage(name, currentLocale);
                }
                else
                {
                    if (MessageExistsFromSettings(name, defaultLocale))
                    {
                        return GetMessage(name, defaultLocale);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
        }

        public static string GetMessage(string name, string locale)
        {
            return GetMessage(name, locale, AppConfig.PerformanceMode);
        }

        public static string GetMessage(string name, string locale, PerformanceMode performanceMode)
        {
            var keyValue = string.Empty;
            var performanceKey = string.Format("{0}{1}_{2}", messageUniqueKey, name, locale);

            Func<string, string, string> fnc = GetMessageFromSettings;

            var args = new object[] { name, locale };

            PerformanceHelper.GetPerformance<string>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static string GetMessageFromSettings(string name, string locale)
        {
            var errorMessage = string.Empty;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode language in xmlDoc.SelectNodes(messageXPath))
            {
                if (language.Attributes["locale"].Value == locale)
                {
                    foreach (XmlNode message in language.ChildNodes)
                    {
                        if (name == message.Attributes["name"].Value)
                        {
                            errorMessage = message.Attributes["value"].Value;
                            break;
                        }
                    }

                    break;
                }
            }

            return errorMessage;
        }

        public static bool LanguageExists(string locale)
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

        public static bool MessageExists(string name, string locale)
        {
            return MessageExists(name, locale, AppConfig.PerformanceMode);
        }

        public static bool MessageExists(string name, string locale, PerformanceMode performanceMode)
        {
            var keyValue = false;
            var performanceKey = string.Format("{0}{1}_{2}_MessageExistance", messageUniqueKey, name, locale);

            Func<string, string, bool> fnc = MessageExistsFromSettings;

            var args = new object[] { name, locale };

            PerformanceHelper.GetPerformance<bool>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static bool MessageExistsFromSettings(string name, string locale)
        {
            var present = false;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode language in xmlDoc.SelectNodes(messageXPath))
            {
                if (language.Attributes["locale"].Value == locale)
                {
                    foreach (XmlNode message in language.ChildNodes)
                    {
                        if (name == message.Attributes["name"].Value)
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

        public static void SetExpression(string name, string value)
        {
            if (ExpressionExists(name))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                foreach (XmlNode expression in xmlDoc.SelectNodes(expressionXPath))
                {
                    if (name == expression.Attributes["name"].Value)
                    {
                        expression.Attributes["value"].Value = value;

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        public static void SetMessage(string name, string locale, string value)
        {
            if (MessageExistsFromSettings(name, locale))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                foreach (XmlNode language in xmlDoc.SelectNodes(messageXPath))
                {
                    if (language.Attributes["locale"].Value == locale)
                    {
                        foreach (XmlNode message in language.ChildNodes)
                        {
                            if (name == message.Attributes["name"].Value)
                            {
                                message.Attributes["value"].Value = value;

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
