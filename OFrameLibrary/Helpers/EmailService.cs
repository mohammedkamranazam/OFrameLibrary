using Microsoft.AspNet.Identity;
using SendGrid;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace OFrameLibrary.Helpers
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return MailHelper.SendAsync(message);
        }
    }
}