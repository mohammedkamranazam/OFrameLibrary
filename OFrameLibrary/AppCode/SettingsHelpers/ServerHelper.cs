using OFrameLibrary.Helpers;
using OFrameLibrary.Models;
using OFrameLibrary.Util;
using System;
using System.Xml;

namespace OFrameLibrary.SettingsHelpers
{
    public static class ServerHelper
    {
        const string expressionXPath = "servers/server";
        const string uniqueKey = "_ServerHelper_";

        static readonly string fileName = AppConfig.RemoteServersFile;

        public static void AddServer(ServerSettings server)
        {
            if (!Exists(server.Name))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var newExpression = xmlDoc.CreateElement("server");

                using (var smc = new SymCryptography())
                {
                    newExpression.SetAttribute("domain", server.Domain);
                    newExpression.SetAttribute("ip", server.IP);
                    newExpression.SetAttribute("isHttp", server.IsHttp.ToString());
                    newExpression.SetAttribute("name", server.Name);
                    newExpression.SetAttribute("password", smc.Encrypt(server.Password));
                    newExpression.SetAttribute("path", server.Path);
                    newExpression.SetAttribute("rootDirectory", server.RootDirectory);
                    newExpression.SetAttribute("username", server.Username);

                    xmlDoc.SelectSingleNode(expressionXPath).ParentNode.AppendChild(newExpression);

                    SaveXml(xmlDoc);
                }
            }
        }

        public static void Delete(string name)
        {
            if (Exists(name))
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

        public static bool Exists(string name)
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

        public static ServerSettings GetDefaultServer()
        {
            return GetServer(GetDefaultServerName());
        }

        public static string GetDefaultServerName()
        {
            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var rootNode = xmlDoc.SelectSingleNode("servers");

            return rootNode.Attributes["default"].Value;
        }

        public static ServerSettings GetServer(string name)
        {
            return GetServer(name, AppConfig.PerformanceMode);
        }

        public static ServerSettings GetServer(string name, PerformanceMode performanceMode)
        {
            var keyValue = new ServerSettings();

            var performanceKey = uniqueKey + name;

            Func<string, ServerSettings> fnc = GetServerFromSettings;

            var args = new object[] { name };

            PerformanceHelper.GetPerformance(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static ServerSettings GetServerFromSettings(string name)
        {
            var server = new ServerSettings();

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode expression in xmlDoc.SelectNodes(expressionXPath))
            {
                if (name == expression.Attributes["name"].Value)
                {
                    using (var smc = new SymCryptography())
                    {
                        server.Domain = expression.Attributes["domain"].Value;
                        server.IP = expression.Attributes["ip"].Value;
                        server.IsHttp = expression.Attributes["isHttp"].Value.BoolParse();
                        server.Name = expression.Attributes["name"].Value;
                        server.Password = smc.Decrypt(expression.Attributes["password"].Value);
                        server.Path = expression.Attributes["path"].Value;
                        server.RootDirectory = expression.Attributes["rootDirectory"].Value;
                        server.Username = expression.Attributes["username"].Value;
                        break;
                    }
                }
            }

            return server;
        }

        public static void SetDefaultServerName(string name)
        {
            if (Exists(name))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var rootNode = xmlDoc.SelectSingleNode("servers");

                rootNode.Attributes["default"].Value = name;

                SaveXml(xmlDoc);
            }
        }

        public static void SetServer(ServerSettings server)
        {
            if (Exists(server.Name))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                foreach (XmlNode expression in xmlDoc.SelectNodes(expressionXPath))
                {
                    if (server.Name == expression.Attributes["name"].Value)
                    {
                        using (var smc = new SymCryptography())
                        {
                            expression.Attributes["domain"].Value = server.Domain;
                            expression.Attributes["ip"].Value = server.IP;
                            expression.Attributes["isHttp"].Value = server.IsHttp.ToString();
                            expression.Attributes["name"].Value = server.Name;
                            expression.Attributes["password"].Value = smc.Encrypt(server.Password);
                            expression.Attributes["path"].Value = server.Path;
                            expression.Attributes["rootDirectory"].Value = server.RootDirectory;
                            expression.Attributes["username"].Value = server.Username;

                            SaveXml(xmlDoc);

                            break;
                        }
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
