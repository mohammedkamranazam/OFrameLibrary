using OFrameLibrary.Util;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace OFrameLibrary.Helpers
{
    public static class MembershipHelper
    {
        public static string GetAnonymousID()
        {
            var cookieValue = CookiesHelper.GetCookie(Constants.Keys.AnonymousUserIDCookieKey);

            if (!string.IsNullOrWhiteSpace(cookieValue))
            {
                return cookieValue;
                ;
            }
            else
            {
                var anonymousID = HttpContext.Current.Request.AnonymousID;

                CookiesHelper.SetCookie(Constants.Keys.AnonymousUserIDCookieKey, anonymousID, Utilities.DateTimeNow().AddDays(1));

                return anonymousID;
            }
        }

        //public static void PopulateRolelist(DropDownListAdv RolesDropDownList, string[] hiddenRoles)
        //{
        //    PopulateRolelist(RolesDropDownList.DropDownList, hiddenRoles);
        //}

        public static void PopulateRolelist(DropDownList RolesDropDownList, string[] hiddenRoles)
        {
            RolesDropDownList.DataSource = Roles.GetAllRoles();
            RolesDropDownList.DataBind();
            RolesDropDownList.AddSelect();

            Array.ForEach(hiddenRoles, role =>
            {
                if (RolesDropDownList.Items.FindByValue(role) != null)
                {
                    RolesDropDownList.Items.Remove(role);
                }
            });
        }

        //public static void PopulateRoleList(DropDownListAdv RolesDropDownList, bool hideSuperAdmin)
        //{
        //    PopulateRoleList(RolesDropDownList.DropDownList, hideSuperAdmin);
        //}

        //public static void PopulateRoleList(DropDownList RolesDropDownList, bool hideSuperAdmin)
        //{
        //    RolesDropDownList.DataSource = Roles.GetAllRoles();
        //    RolesDropDownList.DataBind();
        //    RolesDropDownList.AddSelect();

        //    if (hideSuperAdmin)
        //    {
        //        var item = RolesDropDownList.Items.FindByValue(UserRoles.SuperAdminRole);

        //        if (item != null)
        //        {
        //            RolesDropDownList.Items.Remove(item);
        //        }
        //    }
        //}
    }
}