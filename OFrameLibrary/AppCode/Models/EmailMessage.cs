using System.Collections.Generic;
using System.Net.Mail;

namespace OFrameLibrary.Models
{
    public class EmailMessage
    {
        public List<Attachment> Attachments { get; set; }

        public string Body { get; set; }

        public string FooterTemplate { get; set; }

        public string From { get; set; } = AppConfig.WebsiteMainEmail;

        public string Subject { get; set; }

        public string To { get; set; }

        public List<string> Tos { get; set; }
    }
}
