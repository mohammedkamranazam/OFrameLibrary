using OFrameLibrary.Helpers;
using OFrameLibrary.Models;
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Web;

namespace OFrameLibrary.Util
{
    public static class ErrorLogger
    {
        public static void LogError(Exception ex, DbEntityValidationException e = null)
        {
            var datetime = Utilities.DateTimeNow();
            var dateTime = $"{datetime.ToLongDateString()}, at {datetime.ToShortTimeString()}";

            var errorMessage = "Exception generated on " + dateTime;

            var context = HttpContext.Current;
            errorMessage += "<br /><br /> Page location: " + context.Request.RawUrl;
            errorMessage += "<br /><br /> Client IP Address: " + Utilities.GetIPAddress(context);
            errorMessage += "<br /><br /> Inner Exception: " + ExceptionHelper.GetExceptionMessage(ex);
            errorMessage += "<br /><br /> Entity Exception: " + ExceptionHelper.GetEntityExceptionMessage(e);
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
                MailHelper.Send(new EmailMessage
                {
                    Body = body,
                    To = to,
                    Subject = subject
                });
            }
            catch (Exception mailException)
            {
                errorMessage += "--- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ";
                errorMessage += "<br /><br /> <strong>Mail Exception</strong>";
                errorMessage += "<br /><br /> Inner Exception: " + mailException.InnerException;
                errorMessage += "<br /><br /> Message: " + mailException.Message;
                errorMessage += "<br /><br /> Source: " + mailException.Source;
                errorMessage += "<br /><br /> Method: " + mailException.TargetSite;
                errorMessage += "<br /><br /> Stack Trace: <br /><br />" + mailException.StackTrace;
                errorMessage += "<hr /><br />";

                errorMessage.Replace("<br />", "\n");
                errorMessage.Replace("<hr />", "# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # ");

                var mydocpath = LocalStorages.Storage_Logs.MapPath();

                var fileName = $"{Utilities.DateTimeNow().ToString("dd-MM-yyyy")}.txt";

                // Write the text asynchronously to a new file named "WriteTextAsync.txt".
                using (var outputFile = new StreamWriter(mydocpath + fileName, true))
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
