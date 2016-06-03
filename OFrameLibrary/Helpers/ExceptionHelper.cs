using System;

namespace OFrameLibrary.Helpers
{
    public static class ExceptionHelper
    {
        public static string GetExceptionMessage(Exception ex)
        {
            var message = "<br /><strong>Error occurred while performing this operation</strong><br /><br />";
            message += String.Format("<strong>Error Message:</strong> {0}<br /><br />", ex.Message);
            message += String.Format("<strong>Error Details:</strong> {0}<br /><br />", ((ex.InnerException == null) ? string.Empty : ((ex.InnerException.InnerException == null) ? ex.InnerException.ToString() : ex.InnerException.InnerException.ToString())));
            message += String.Format("<strong>Stack Trace:</strong> {0}<br /><br />", ex.StackTrace);
            message += String.Format("<strong>Method Name:</strong> {0}<br /><br />", (ex.TargetSite != null ? ex.TargetSite.Name : string.Empty));
            message += String.Format("<strong>Source:</strong> {0}<br /><br />", ex.Source);
            return message;
        }
    }
}