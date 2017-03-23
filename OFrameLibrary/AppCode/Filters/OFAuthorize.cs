using System.Web.Mvc;

namespace OFrameLibrary.Filters
{
    public class OFAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { action = "Login", controller = "Authentication", area = "" }));
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
