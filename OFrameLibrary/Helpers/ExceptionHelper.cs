using System;

namespace OFrameLibrary.Helpers
{
    public static class ExceptionHelper
    {
        public static string GetExceptionMessage(Exception ex)
        {
            var message = "<br /><strong>Error occurred while performing this operation</strong><br /><br />";
            message += string.Format("<strong>Error Message:</strong> {0}<br /><br />", ex.Message);
            message += string.Format("<strong>Error Details:</strong> {0}<br /><br />", ((ex.InnerException == null) ? string.Empty : ((ex.InnerException.InnerException == null) ? ex.InnerException.ToString() : ex.InnerException.InnerException.ToString())));
            message += string.Format("<strong>Stack Trace:</strong> {0}<br /><br />", ex.StackTrace);
            message += string.Format("<strong>Method Name:</strong> {0}<br /><br />", (ex.TargetSite != null ? ex.TargetSite.Name : string.Empty));
            message += string.Format("<strong>Source:</strong> {0}<br /><br />", ex.Source);
            return message;
        }
    }
}