using OFrameLibrary.ILL;
using OFrameLibrary.SettingsHelpers;
using OFrameLibrary.Util;
using System;
using System.IO;

namespace OFrameLibrary.Helpers
{
    public static class EmailTagsHelper
    {
        public static string CleanUpPlaceHolders(string body, int lastCount)
        {
            var maxCount = KeywordsHelper.GetKeywordValue("MaxPlaceHoldersCount").IntParse();

            for (var xCount = lastCount + 1; xCount < maxCount; xCount++)
            {
                var placeHolder = string.Format("[PLACEHOLDER{0}]", xCount);

                body = body.Replace(placeHolder, string.Empty);
            }

            return body;
        }

        public static string GenerateEmailBody(EmailPlaceHolder emailPlaceHolder, string body)
        {
            var count = 0;

            Array.ForEach(emailPlaceHolder.GetType().GetProperties(), propertyInfo =>
            {
                count++;
                if (propertyInfo.CanRead)
                {
                    var property = propertyInfo.GetValue(emailPlaceHolder, null);
                    var value = string.Empty;
                    if (property != null)
                    {
                        value = property.ToString();
                    }
                    var placeHolder = string.Format("[PLACEHOLDER{0}]", count);
                    body = body.Replace(placeHolder, value);
                }
            });

            return body;
        }

        public static string GetEmailTemplateFromDataBase(string templateName)
        {
            return string.Empty;
        }

        public static string GetEmailTemplateFromFile(string templatePath)
        {
            var body = string.Empty;

            using (var reader = new StreamReader(templatePath))
            {
                body = reader.ReadToEnd();
            }

            return body;
        }
    }
}
