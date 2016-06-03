using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.Threading.Tasks;
using Twilio;

namespace OFrameLibrary.Helpers
{
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            switch (AppConfig.SMSServiceName)
            {
                case "TWILIO":
                    // Twilio Begin
                    var Twilio = new TwilioRestClient(AppConfig.SMSAccountIdentification, AppConfig.SMSAccountPassword);

                    var result = Twilio.SendMessage(AppConfig.SMSAccountFrom, message.Destination, message.Body);
                    //Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
                    Trace.TraceInformation(result.Status);
                    //Twilio doesn't currently have an async API, so return success.                    
                    // Twilio End
                    break;
                case "ASPSMS":
                    //// ASPSMS Begin
                    //var soapSms = new ASPSMSX2.ASPSMSX2SoapClient("ASPSMSX2Soap");
                    //soapSms.SendSimpleTextSMS(
                    //  System.Configuration.ConfigurationManager.AppSettings["SMSAccountIdentification"],
                    //  System.Configuration.ConfigurationManager.AppSettings["SMSAccountPassword"],
                    //  message.Destination,
                    //  System.Configuration.ConfigurationManager.AppSettings["SMSAccountFrom"],
                    //  message.Body);
                    //soapSms.Close();
                    //return Task.FromResult(0);
                    //// ASPSMS End
                    break;
            }

            return Task.FromResult(0);
        }
    }
}