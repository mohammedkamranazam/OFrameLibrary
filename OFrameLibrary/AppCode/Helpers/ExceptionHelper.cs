using OFrameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace OFrameLibrary.Helpers
{
    public static class ExceptionHelper
    {
        public static string GetExceptionMessage(Exception ex)
        {
            if (ex == null)
            {
                return "NA";
            }

            var message = $"<br /><br /><strong>Error Message:</strong> {ex.Message}";
            message += $"<br /><br /><strong>Error Details:</strong> {((ex.InnerException == null) ? string.Empty : ((ex.InnerException.InnerException == null) ? ex.InnerException.ToString() : ex.InnerException.InnerException.ToString()))}";
            message += $"<br /><br /><strong>Method Name:</strong> {(ex.TargetSite != null ? ex.TargetSite.Name : string.Empty)}";
            message += $"<br /><br /><strong>Source:</strong> {ex.Source}";
            message += $"<br /><br /><strong>Stack Trace:</strong><br />{ex.StackTrace}";
            return message;
        }

        public static string GetEntityExceptionMessage(DbEntityValidationException e)
        {
            if (e == null)
            {
                return "NA";
            }

            var message = new List<string>();
            var exs = GetExceptionMessage(e);

            foreach (var eve in e.EntityValidationErrors)
            {
                message.Add($"Entity of type <strong>{eve.Entry.Entity.GetType().Name}</strong> in state <strong>{eve.Entry.State}</strong> has the following validation errors:");

                foreach (var ve in eve.ValidationErrors)
                {
                    message.Add($"<br /></br />" +
                        $"<strong>Property:</strong> {ve.PropertyName}, " +
                        $"<strong>Value:</strong> {eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName)}, " +
                        $"<strong>Error:</strong> {ve.ErrorMessage}");
                }
            }

            return exs + string.Join("<br /><br />", message.ToArray());
        }
    }
}
