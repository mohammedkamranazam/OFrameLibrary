using OFrameLibrary.AppCode.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFrameLibrary.AppCode.Helpers
{
    public class SendGridEmailHelper
    {
        public static bool SendMail(SendGridSettings settings, String To, string Subject, string Message, string footerTemplate = "")
        {
            try
            {

                SendGridAPIClient sg = new SendGridAPIClient(settings.SendGridApiKey.Replace(" ", ""));

                Email from = new Email(settings.SenderEmail, settings.SenderName);
                string subject = Subject;
                Email to = new Email(To);

                Content content = new Content();
                content = new Content();
                content.Type = "text/html";
                content.Value = Message;

                Mail mail = new Mail(from, subject, to, content);

                MailSettings mailSettings = new MailSettings();
                if (footerTemplate != "")
                {
                    FooterSettings footerSettings = new FooterSettings();
                    footerSettings.Enable = true;
                    footerSettings.Html = footerTemplate;
                    mailSettings.FooterSettings = footerSettings;
                    mail.MailSettings = mailSettings;
                }
                var response = sg.client.mail.send.post(requestBody: mail.Get());
                return true;
            }
            catch (Exception ex) { throw (ex); }
        }

        public static async Task<bool> SendMailAsync(SendGridSettings settings, String To, string Subject, string Message, string footerTemplate = "")
        {
            try
            {
                SendGridAPIClient sg = new SendGridAPIClient(settings.SendGridApiKey.Replace(" ", ""));

                Email from = new Email(settings.SenderEmail, settings.SenderName);
                string subject = Subject;
                Email to = new Email(To);

                Content content = new Content();
                content = new Content();
                content.Type = "text/html";
                content.Value = Message;

                Mail mail = new Mail(from, subject, to, content);

                MailSettings mailSettings = new MailSettings();
                if (footerTemplate != "")
                {
                    FooterSettings footerSettings = new FooterSettings();
                    footerSettings.Enable = true;
                    footerSettings.Html = footerTemplate;
                    mailSettings.FooterSettings = footerSettings;
                    mail.MailSettings = mailSettings;
                }
                var response = await sg.client.mail.send.post(requestBody: mail.Get());
                return true;
            }
            catch (Exception ex) { throw (ex); }
        }



        public static bool SendMailToMultiple(SendGridSettings settings, List<string> To, string Subject, string Message)
        {
            try
            {
                foreach (string recipiant in To) SendMail(settings, recipiant, Subject, Message);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public static async Task<bool> SendMailToMultipleAsync(SendGridSettings settings, List<string> To, string Subject, string Message)
        {
            try
            {
                foreach (string recipiant in To) await SendMailAsync(settings, recipiant, Subject, Message);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }
    }

}
