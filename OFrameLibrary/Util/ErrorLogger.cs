using Microsoft.AspNet.Identity;
using OFrameLibrary.Helpers;
using System;
using System.Diagnostics;
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
            errorMessage += "<br /><br /> Inner Exception: " + ExceptionHelper.GetExceptionMessage(ex);
            errorMessage += "<br /><br /> Message: " + ex.Message;
            errorMessage += "<br /><br /> Source: " + ex.Source;
            errorMessage += "<br /><br /> Method: " + ex.TargetSite;
            errorMessage += "<br /><br /> Stack Trace: <br /><br />" + ex.StackTrace;

            EventLog myLog = new EventLog();
            myLog.Source = AppConfig.EventLogSourceName;
            myLog.WriteEntry(errorMessage);
        }

        public static void LogError(string error)
        {
            var ex = new Exception(error);
            LogError(ex);
        }
    }
}