using Microsoft.AspNet.Identity;
using OFrameLibrary.ILL;
using OFrameLibrary.SettingsHelpers;
using OFrameLibrary.Util;
using SendGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OFrameLibrary.Helpers
{
    public static class MailHelper
    {
        public static void Send(IdentityMessage message)
        {
            switch (AppConfig.EmailServiceName)
            {
                case "SENDGRID":
                    SendUsingSendGrid(message);
                    break;

                case "SMTP":
                    SendUsingSmtp(message);
                    break;

                case "RELAY":
                    SendUsingRelay(message);
                    break;
            }
        }

        public static Task SendAsync(IdentityMessage message)
        {
            switch (AppConfig.EmailServiceName)
            {
                case "SENDGRID":
                    return SendUsingSendGridAsync(message);

                case "SMTP":
                    return SendUsingSmtpAsync(message, null);

                case "RELAY":
                    return SendUsingRelayAsync(message, null);
            }

            return Task.FromResult(0);
        }

        public static void SendUsingSendGrid(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new MailAddress(AppConfig.WebsiteMainEmail, AppConfig.MailLabel);
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(AppConfig.SendGridUsername, AppConfig.SendGridPassword);

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                transportWeb.DeliverAsync(myMessage);
            }
        }

        public static Task SendUsingSendGridAsync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new MailAddress(AppConfig.WebsiteMainEmail, AppConfig.MailLabel);
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(AppConfig.SendGridUsername, AppConfig.SendGridPassword);

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                return transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                return Task.FromResult(0);
            }
        }

        public static string CleanUpPlaceHolders(string body, int lastCount)
        {
            var maxCount = DataParser.IntParse(KeywordsHelper.GetKeywordValue("MaxPlaceHoldersCount"));

            for (var xCount = lastCount + 1; xCount < maxCount; xCount++)
            {
                var placeHolder = string.Format("[PLACEHOLDER{0}]", xCount);

                body = body.Replace(placeHolder, string.Empty);
            }

            return body;
        }

        public static void GetAttachments(List<Attachment> attachments, MailMessage msg)
        {
            attachments.ForEach(msg.Attachments.Add);
        }

        public static MailMessage GetMessage(IdentityMessage message)
        {
            var msg = new MailMessage();

            msg.From = new MailAddress(AppConfig.WebsiteMainEmail);

            msg.To.Add(message.Destination);

            msg.Subject = message.Subject;

            msg.Body = message.Body;

            msg.IsBodyHtml = true;

            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

            msg.Priority = MailPriority.Normal;

            return msg;
        }

        public static void SendUsingRelay(IdentityMessage message)
        {
            var msg = GetMessage(message);

            using (var smtp = new SmtpClient())
            {
                smtp.Send(msg);
            }

            msg.Dispose();
        }

        public static Task SendUsingRelayAsync(IdentityMessage message, object token)
        {
            var msg = GetMessage(message);

            var smtp = new SmtpClient();

            smtp.SendCompleted += smtp_SendCompleted;

            token = msg;

            smtp.SendAsync(msg, token);

            msg.Dispose();

            return Task.FromResult(0);
        }

        public static void SendUsingRelayWithAttachments(IdentityMessage message, List<Attachment> attachments)
        {
            var msg = GetMessage(message);

            GetAttachments(attachments, msg);

            using (var smtp = new SmtpClient())
            {
                smtp.Send(msg);
            }

            msg.Dispose();
        }

        public static Task SendUsingRelayWithAttachmentsAsync(IdentityMessage message, List<Attachment> attachments, object token)
        {
            var msg = GetMessage(message);

            GetAttachments(attachments, msg);

            var smtp = new SmtpClient();

            smtp.SendCompleted += smtp_SendCompleted;

            token = msg;

            smtp.SendAsync(msg, token);

            msg.Dispose();

            return Task.FromResult(0);
        }

        public static void SendUsingSmtp(IdentityMessage message)
        {
            var msg = GetMessage(message);

            using (var smtp = new SmtpClient(AppConfig.MailServer))
            {
                smtp.EnableSsl = AppConfig.EnableSsl;

                smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword);

                smtp.Port = AppConfig.MailServerPort;

                smtp.Send(msg);
            }

            msg.Dispose();
        }

        public static Task SendUsingSmtpAsync(IdentityMessage message, object token)
        {
            var msg = GetMessage(message);

            var smtp = new SmtpClient(AppConfig.MailServer);

            smtp.EnableSsl = AppConfig.EnableSsl;

            smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword);

            smtp.Port = AppConfig.MailServerPort;

            smtp.SendCompleted += smtp_SendCompleted;

            token = msg;

            smtp.SendAsync(msg, token);

            msg.Dispose();

            return Task.FromResult(0);
        }

        public static void SendUsingSmtpWithAttachments(IdentityMessage message, List<Attachment> attachments)
        {
            var msg = GetMessage(message);

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

        public static Task SendUsingSmtpWithAttachmentsAsync(IdentityMessage message, List<Attachment> attachments, object token)
        {
            var msg = GetMessage(message);

            GetAttachments(attachments, msg);

            var smtp = new SmtpClient(AppConfig.MailServer);

            smtp.EnableSsl = AppConfig.EnableSsl;

            smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword);

            smtp.Port = AppConfig.MailServerPort;

            smtp.SendCompleted += smtp_SendCompleted;

            token = msg;

            smtp.SendAsync(msg, token);

            msg.Dispose();

            return Task.FromResult(0);
        }

        public static void smtp_SendCompleted(object sender, AsyncCompletedEventArgs e)
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
    }
}