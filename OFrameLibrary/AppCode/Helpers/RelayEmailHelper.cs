using OFrameLibrary.Models;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OFrameLibrary.Helpers
{
    public static class RelayEmailHelper
    {
        public static void SendUsingRelay(EmailMessage message)
        {
            var msg = MailHelper.GetMessage(message);

            using (var smtp = new SmtpClient())
            {
                smtp.Send(msg);
            }

            msg.Dispose();
        }

        public static Task SendUsingRelayAsync(EmailMessage message, object token)
        {
            var msg = MailHelper.GetMessage(message);

            using (var smtp = new SmtpClient())
            {
                smtp.SendCompleted += MailHelper.smtp_SendCompleted;

                return Task.Run(() => smtp.SendAsync(msg, token));
            }
        }

        public static void SendUsingRelayWithAttachments(EmailMessage message)
        {
            var msg = MailHelper.GetMessage(message);

            MailHelper.GetAttachments(message.Attachments, msg);

            using (var smtp = new SmtpClient())
            {
                smtp.Send(msg);
            }

            msg.Dispose();
        }

        public static Task SendUsingRelayWithAttachmentsAsync(EmailMessage message, object token)
        {
            var msg = MailHelper.GetMessage(message);

            MailHelper.GetAttachments(message.Attachments, msg);

            using (var smtp = new SmtpClient())
            {
                smtp.SendCompleted += MailHelper.smtp_SendCompleted;

                return Task.Run(() => smtp.SendAsync(msg, token));
            }
        }
    }
}
