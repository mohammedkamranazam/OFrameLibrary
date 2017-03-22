using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFrameLibrary.AppCode.Models
{
    public class SendGridSettings
    {
        public string SenderEmail { get; set; }

        public string SenderName { get; set; }

        public string SendGridApiKey { get; set; }
    }
}
