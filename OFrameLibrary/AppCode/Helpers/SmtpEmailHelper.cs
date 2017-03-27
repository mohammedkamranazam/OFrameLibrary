using OFrameLibrary.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OFrameLibrary.Helpers
{
    public static class SmtpEmailHelper
    {
        public static void SendUsingSmtp(EmailMessage message)
        {
            var msg = MailHelper.GetMessage(message);

            using (var smtp = new SmtpClient(AppConfig.MailServer))
            {
                smtp.EnableSsl = AppConfig.EnableSsl;

                smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword);

                smtp.Port = AppConfig.MailServerPort;

                smtp.Send(msg);
            }

            msg.Dispose();
        }

        public static Task SendUsingSmtpAsync(EmailMessage message, object token)
        {
            var msg = MailHelper.GetMessage(message);

            var smtp = new SmtpClient(AppConfig.MailServer);

            smtp.EnableSsl = AppConfig.EnableSsl;

            smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword);

            smtp.Port = AppConfig.MailServerPort;

            smtp.SendCompleted += MailHelper.smtp_SendCompleted;

            token = msg;

            return Task.Run(() => smtp.SendAsync(msg, token));
        }

        public static void SendUsingSmtpWithAttachments(EmailMessage message, List<Attachment> attachments)
        {
            var msg = MailHelper.GetMessage(message);

            MailHelper.GetAttachments(attachments, msg);

            using (var smtp = new SmtpClient(AppConfig.MailServer))
            {
                smtp.EnableSsl = AppConfig.EnableSsl;

                smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword);

                smtp.Port = AppConfig.MailServerPort;

                smtp.Send(msg);
            }

            msg.Dispose();
        }

        public static Task SendUsingSmtpWithAttachmentsAsync(EmailMessage message, List<Attachment> attachments, object token)
        {
            var msg = MailHelper.GetMessage(message);

            MailHelper.GetAttachments(attachments, msg);

            var smtp = new SmtpClient(AppConfig.MailServer);

            smtp.EnableSsl = AppConfig.EnableSsl;

            smtp.Credentials = new NetworkCredential(AppConfig.MailLogOnId, AppConfig.MailLogOnPassword);

            smtp.Port = AppConfig.MailServerPort;

            smtp.SendCompleted += MailHelper.smtp_SendCompleted;

            token = msg;

            return Task.Run(() => smtp.SendAsync(msg, token));
        }
    }
}
