using Microsoft.AspNet.Identity;
using OFrameLibrary.Models;
using System.Threading.Tasks;

namespace OFrameLibrary.Helpers
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return MailHelper.SendAsync(new EmailMessage
            {
                Body = message.Body,
                To = message.Destination,
                Subject = message.Subject
            });
        }
    }
}
