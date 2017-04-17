using OFrameLibrary.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace OFrameLibrary.Helpers
{
    public static class SendGridEmailHelper
    {
        public static SendGridSettings Settings
        {
            get
            {
                return new SendGridSettings
                {
                    SenderEmail = AppConfig.WebsiteMainEmail,
                    SenderName = AppConfig.SiteName,
                    SendGridApiKey = AppConfig.SendGridAPIKey.Replace(" ", "")
                };
            }
        }

        public static void SendMail(EmailMessage message)
        {
            try
            {
                var sendGridApiClient = new SendGridAPIClient(Settings.SendGridApiKey);

                sendGridApiClient.client.mail.send.post(requestBody: GetMail(message).Get());
            }
            catch (Exception) { throw; }
        }

        public static async Task SendMailAsync(EmailMessage message)
        {
            try
            {
                var sendGridApiClient = new SendGridAPIClient(Settings.SendGridApiKey);

                await sendGridApiClient.client.mail.send.post(requestBody: GetMail(message).Get());
            }
            catch (Exception) { throw; }
        }

        public static void SendMailToMultiple(EmailMessage message)
        {
            try
            {
                foreach (var to in message.Tos)
                {
                    message.To = to;
                    SendMail(message);
                }
            }
            catch (Exception) { throw; }
        }

        public static async Task SendMailToMultipleAsync(EmailMessage message)
        {
            try
            {
                foreach (var to in message.Tos)
                {
                    message.To = to;
                    await SendMailAsync(message);
                }
            }
            catch (Exception) { throw; }
        }

        static Mail GetMail(EmailMessage message)
        {
            return new Mail(
                                new Email(Settings.SenderEmail, Settings.SenderName),
                                message.Subject,
                                new Email(message.To),
                                new Content
                                {
                                    Type = "text/html",
                                    Value = message.Body
                                })
            {
                MailSettings = new MailSettings
                {
                    FooterSettings = new FooterSettings
                    {
                        Enable = !(string.IsNullOrWhiteSpace(message.FooterTemplate)),
                        Html = message.FooterTemplate
                    }
                }
            };
        }
    }
}
