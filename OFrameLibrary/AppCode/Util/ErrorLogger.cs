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
        public static void LogError(Exception ex = null, DbEntityValidationException e = null)
        {
            var datetime = Utilities.DateTimeNow();
            var context = HttpContext.Current;

            var errorMessage = $"<h3>Exception Generated On {datetime.ToLongDateString()} @ {datetime.ToShortTimeString()}</h3>";
            errorMessage += $"<br /><br /><strong>Page Location:</strong> {context?.Request.RawUrl}";
            errorMessage += $"<br /><br /><strong>Client IP Address:</strong> {Utilities.GetIPAddress(context)}";
            errorMessage += ExceptionHelper.GetExceptionMessage(ex);
            errorMessage += ExceptionHelper.GetEntityExceptionMessage(e);

            try
            {
                MailHelper.Send(new EmailMessage
                {
                    From = AppConfig.WebsiteMainEmail,
                    Body = errorMessage,
                    To = AppConfig.ErrorAdminEmail,
                    Subject = AppConfig.SiteName + ": Error Log"
                });
            }
            catch (Exception mex)
            {
                errorMessage += $"<br /><hr /><br /><h3>Mail Exception</h3>{ExceptionHelper.GetExceptionMessage(mex)}<hr /><br />";

                //errorMessage.Replace("<br />", "\n");
                //errorMessage.Replace("<hr />", "# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # ");

                // Write the text asynchronously to a new file named "WriteTextAsync.txt".
                using (var outputFile = new StreamWriter($"{LocalStorages.Storage_Logs.MapPath()}{Utilities.DateTimeNow().ToString("dd-MM-yyyy")}.html", true))
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
