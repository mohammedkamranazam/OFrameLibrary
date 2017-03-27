using OFrameLibrary.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OFrameLibrary.Helpers
{
    public static class MailHelper
    {
        public static void GetAttachments(List<Attachment> attachments, MailMessage msg)
        {
            attachments.ForEach(msg.Attachments.Add);
        }

        public static MailMessage GetMessage(EmailMessage message)
        {
            var msg = new MailMessage();

            msg.From = new MailAddress(message.From);

            if (message.Tos.Count > 0)
            {
                foreach (var to in message.Tos)
                {
                    msg.To.Add(to);
                }
            }
            else
            {
                msg.To.Add(message.To);
            }

            msg.Subject = message.Subject;

            msg.Body = message.Body;

            msg.IsBodyHtml = true;

            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

            msg.Priority = MailPriority.Normal;

            return msg;
        }

        public static void Send(EmailMessage message)
        {
            switch (AppConfig.EmailServiceName)
            {
                case "SENDGRID":
                    if (message.Tos.Count > 0)
                    {
                        SendGridEmailHelper.SendMailToMultiple(message);
                    }
                    else
                    {
                        SendGridEmailHelper.SendMail(message);
                    }
                    break;

                case "SMTP":
                    SmtpEmailHelper.SendUsingSmtp(message);
                    break;

                case "RELAY":
                    RelayEmailHelper.SendUsingRelay(message);
                    break;
            }
        }

        public static async Task SendAsync(EmailMessage message, object token = null)
        {
            switch (AppConfig.EmailServiceName)
            {
                case "SENDGRID":
                    if (message.Tos.Count > 0)
                    {
                        await SendGridEmailHelper.SendMailToMultipleAsync(message);
                    }
                    else
                    {
                        await SendGridEmailHelper.SendMailAsync(message);
                    }
                    break;

                case "SMTP":
                    await SmtpEmailHelper.SendUsingSmtpAsync(message, token);
                    break;

                case "RELAY":
                    await RelayEmailHelper.SendUsingRelayAsync(message, token);
                    break;
            }
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
    }
}
