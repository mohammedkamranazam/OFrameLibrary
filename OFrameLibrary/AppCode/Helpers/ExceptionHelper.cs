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
            var message = "<br /><strong>Error occurred while performing this operation</strong><br /><br />";
            message += $"<strong>Error Message:</strong> {ex.Message}<br /><br />";
            message += $"<strong>Error Details:</strong> {((ex.InnerException == null) ? string.Empty : ((ex.InnerException.InnerException == null) ? ex.InnerException.ToString() : ex.InnerException.InnerException.ToString()))}<br /><br />";
            message += $"<strong>Stack Trace:</strong> {ex.StackTrace}<br /><br />";
            message += $"<strong>Method Name:</strong> {(ex.TargetSite != null ? ex.TargetSite.Name : string.Empty)}<br /><br />";
            message += $"<strong>Source:</strong> {ex.Source}<br /><br />";
            return message;
        }

        public static string GetEntityExceptionMessage(DbEntityValidationException e)
        {
            if (e == null)
            {
                return string.Empty;
            }

            var message = new List<string>();
            foreach (var eve in e.EntityValidationErrors)
            {
                message.Add($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                foreach (var ve in eve.ValidationErrors)
                {
                    message.Add($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                }
            }
            return string.Join("<br /><br />", message.ToArray());
        }
    }
}
