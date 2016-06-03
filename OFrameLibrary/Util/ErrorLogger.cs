using Microsoft.AspNet.Identity;
using OFrameLibrary.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace OFrameLibrary.Util
{
    public static class ErrorLogger
    {
        public static void LogError(Exception ex)
        {
            var datetime = Utilities.DateTimeNow();
            var dateTime = string.Format("{0}, at {1}", datetime.ToLongDateString(), datetime.ToShortTimeString());

            var errorMessage = "Exception generated on " + dateTime;

            var context = HttpContext.Current;
            errorMessage += "<br /><br /> Page location: " + context.Request.RawUrl;
            errorMessage += "<br /><br /> Client IP Address: " + Utilities.GetIPAddress(context);
            errorMessage += "<br /><br /> Inner Exception: " + ExceptionHelper.GetExceptionMessage(ex);
            errorMessage += "<br /><br /> Message: " + ex.Message;
            errorMessage += "<br /><br /> Source: " + ex.Source;
            errorMessage += "<br /><br /> Method: " + ex.TargetSite;
            errorMessage += "<br /><br /> Stack Trace: <br /><br />" + ex.StackTrace;

            var from = AppConfig.WebsiteMainEmail;
            var to = AppConfig.ErrorAdminEmail;
            var subject = AppConfig.SiteName + ": Error log";
            var body = errorMessage;

            try
            {
                MailHelper.Send(new IdentityMessage
                {
                    Body = body,
                    Destination = to,
                    Subject = subject
                });
            }
            catch (Exception mailException)
            {
                errorMessage += "<hr />";
                errorMessage += "<br /><br /> Mail Exception";
                errorMessage += "<br /><br /> Inner Exception: " + mailException.InnerException;
                errorMessage += "<br /><br /> Message: " + mailException.Message;
                errorMessage += "<br /><br /> Source: " + mailException.Source;
                errorMessage += "<br /><br /> Method: " + mailException.TargetSite;
                errorMessage += "<br /><br /> Stack Trace: <br /><br />" + mailException.StackTrace;

                string mydocpath = LocalStorages.Storage_Logs.MapPath();

                string fileName = string.Format("{0}.txt", Utilities.DateTimeNow().ToShortDateString());

                // Write the text asynchronously to a new file named "WriteTextAsync.txt".
                using (StreamWriter outputFile = new StreamWriter(mydocpath + fileName, true))
                {
                    outputFile.WriteLine(errorMessage);
                }
            }
        }

        public static void LogError(string error)
        {
            var ex = new Exception(error);
            LogError(ex);
        }
    }
}