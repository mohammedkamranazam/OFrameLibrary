using OFrameLibrary.ILL;
using OFrameLibrary.SettingsHelpers;
using OFrameLibrary.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace OFrameLibrary.Helpers
{
    public static class MailHelper
    {
        private static string CleanUpPlaceHolders(string body, int lastCount)
        {
            var maxCount = DataParser.IntParse(KeywordsHelper.GetKeywordValue("MaxPlaceHoldersCount"));

            for (var xCount = lastCount + 1; xCount < maxCount; xCount++)
            {
                var placeHolder = string.Format("[PLACEHOLDER{0}]", xCount);

                body = body.Replace(placeHolder, string.Empty);
            }

            return body;
        }

        private static void GetAttachments(List<Attachment> attachments, MailMessage msg)
        {
            attachments.ForEach(msg.Attachments.Add);
        }

        private static MailMessage GetMessage(string from, string to, string subject, string message)
        {
            var msg = new MailMessage();

            msg.From = new MailAddress(from);

            msg.To.Add(to);

            msg.Subject = subject;

            msg.Body = message;

            msg.IsBodyHtml = true;

            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

            msg.Priority = MailPriority.Normal;

            return msg;
        }

        private static void SendUsingRelay(string from, string to, string subject, string message)
        {
            var msg = GetMessage(from, to, subject, message);

            using (var smtp = new SmtpClient())
            {
                smtp.Send(msg);
            }

            msg.Dispose();
        }

        private static void SendUsingRelayAsync(string from, string to, string subject, string message, object token)
        {
            var msg = GetMessage(from, to, subject, message);

            var smtp = new SmtpClient();

            smtp.SendCompleted += smtp_SendCompleted;

            token = msg;

            smtp.SendAsync(msg, token);

            msg.Dispose();
        }

        private static void SendUsingRelayWithAttachments(string from, string to, string subject, string message, List<Attachment> attachments)
        {
            var msg = GetMessage(from, to, subject, message);

            GetAttachments(attachments, msg);

            using (var smtp = new SmtpClient())
            {
                smtp.Send(msg);
            }

            msg.Dispose();
        }

        private static void SendUsingRelayWithAttachmentsAsync(string from, string to, string subject, string message, List<Attachment> attachments, object token)
        {
            var msg = GetMessage(from, to, subject, message);

            GetAttachments(attachments, msg);

            var smtp = new SmtpClient();

            smtp.SendCompleted += smtp_SendCompleted;

            token = msg;

            smtp.SendAsync(msg, token);

            msg.Dispose();
        }

        private static void SendUsingSmtp(string from, string to, string subject, string message)
        {
            var msg = GetMessage(from, to, subject, message);

            using (var smtp = new SmtpClient(AppConfig.MailServer))
            {
                smtp.EnableSsl = AppConfig.EnableSsl;

                smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword);

                smtp.Port = AppConfig.MailServerPort;

                smtp.Send(msg);
            }

            msg.Dispose();
        }

        private static void SendUsingSmtpAsync(string from, string to, string subject, string message, object token)
        {
            var msg = GetMessage(from, to, subject, message);

            var smtp = new SmtpClient(AppConfig.MailServer);

            smtp.EnableSsl = AppConfig.EnableSsl;

            smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword);

            smtp.Port = AppConfig.MailServerPort;

            smtp.SendCompleted += smtp_SendCompleted;

            token = msg;

            smtp.SendAsync(msg, token);

            msg.Dispose();
        }

        private static void SendUsingSmtpWithAttachments(string from, string to, string subject, string message, List<Attachment> attachments)
        {
            var msg = GetMessage(from, to, subject, message);

            GetAttachments(attachments, msg);

            using (var smtp = new SmtpClient(AppConfig.MailServer))
            {
                smtp.EnableSsl = AppConfig.EnableSsl;

                smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword);

                smtp.Port = AppConfig.MailServerPort;

                smtp.Send(msg);
            }

            msg.Dispose();
        }

        private static void SendUsingSmtpWithAttachmentsAsync(string from, string to, string subject, string message, List<Attachment> attachments, object token)
        {
            var msg = GetMessage(from, to, subject, message);

            GetAttachments(attachments, msg);

            var smtp = new SmtpClient(AppConfig.MailServer);

            smtp.EnableSsl = AppConfig.EnableSsl;

            smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword);

            smtp.Port = AppConfig.MailServerPort;

            smtp.SendCompleted += smtp_SendCompleted;

            token = msg;

            smtp.SendAsync(msg, token);

            msg.Dispose();
        }

        private static void smtp_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //var StatusMessage = e.UserState as StatusMessageJQuery;

            //if (e.Error != null)
            //{
            //    StatusMessage.MessageType = StatusMessageType.Error;
            //    StatusMessage.Message = ExceptionHelper.GetExceptionMessage(e.Error);
            //}
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
            var body = string.Empty;

            return body;
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

        public static void Send(string from, string to, string subject, string message)
        {
            if (AppConfig.MailSmtp)
            {
                SendUsingSmtp(from, to, subject, message);
            }
            else
            {
                SendUsingRelay(from, to, subject, message);
            }
        }

        public static void SendAsync(string from, string to, string subject, string message, object token)
        {
            if (AppConfig.MailSmtp)
            {
                SendUsingSmtpAsync(from, to, subject, message, token);
            }
            else
            {
                SendUsingRelayAsync(from, to, subject, message, token);
            }
        }

        public static void SendWithAttachments(string from, string to, string subject, string message, List<Attachment> attachments)
        {
            if (AppConfig.MailSmtp)
            {
                SendUsingSmtpWithAttachments(from, to, subject, message, attachments);
            }
            else
            {
                SendUsingRelayWithAttachments(from, to, subject, message, attachments);
            }
        }

        public static void SendWithAttachmentsAsync(string from, string to, string subject, string message, List<Attachment> attachments, object token)
        {
            if (AppConfig.MailSmtp)
            {
                SendUsingSmtpWithAttachmentsAsync(from, to, subject, message, attachments, token);
            }
            else
            {
                SendUsingRelayWithAttachmentsAsync(from, to, subject, message, attachments, token);
            }
        }
    }
}