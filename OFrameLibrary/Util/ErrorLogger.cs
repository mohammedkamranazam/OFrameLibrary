using OFrameLibrary.Helpers;
using System;
using System.Web;

namespace OFrameLibrary.Util
{
    public static class ErrorLogger
    {
        public static void LogError(Exception ex)
        {
            var datetime = Utilities.DateTimeNow();
            var dateTime = String.Format("{0}, at {1}", datetime.ToLongDateString(), datetime.ToShortTimeString());

            var errorMessage = "Exception generated on " + dateTime;

            var context = HttpContext.Current;
            errorMessage += "<br /><br /> Page location: " + context.Request.RawUrl;
            errorMessage += "<br /><br /> Client IP Address: " + Utilities.GetIPAddress(context);
            errorMessage += "<br /><br /> Inner Exception: " + ((ex.InnerException == null) ? string.Empty : ((ex.InnerException.InnerException == null) ? ex.InnerException.ToString() : ex.InnerException.InnerException.ToString()));
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
                MailHelper.Send(from, to, subject, body);
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

                //using (var logContext = new OWDAROEntities())
                //{
                //    var log = new OW_ActivityLogs();
                //    log.ActivityLogID = Guid.NewGuid();
                //    log.ActivityMessage = errorMessage;
                //    log.ActivityOn = datetime;
                //    log.ActivityType = "Combined Error (Process + Mail)";

                //    try
                //    {
                //        ActivityLogsBL.Add(log);
                //    }
                //    catch
                //    {
                //    }
                //}
            }
        }

        public static void LogError(string error)
        {
            var ex = new Exception(error);
            LogError(ex);
        }
    }
}