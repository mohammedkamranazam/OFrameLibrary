using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace OFrameLibrary.Helpers
{
    public static class PrintHelper
    {
        public static void PrintControl(Control ctrl, int printCount)
        {
            PrintControl(ctrl, string.Empty, printCount);
        }

        public static void PrintControl(Control ctrl, string Script, int printCount)
        {
            HtmlForm frm;
            Page pg;

            HttpContext.Current.Response.Clear();

            for (var x = 0; x < printCount; x++)
            {
                var stringWrite = new StringWriter();

                var htmlWrite = new HtmlTextWriter(stringWrite);

                if (ctrl is WebControl)
                {
                    var w = new Unit(100, UnitType.Percentage);

                    ((WebControl)ctrl).Width = w;
                }

                pg = new Page();
                pg.EnableEventValidation = false;

                if (!string.IsNullOrWhiteSpace(Script))
                {
                    pg.ClientScript.RegisterStartupScript(pg.GetType(), "PrintJavaScript", Script);
                }

                frm = new HtmlForm();
                pg.Controls.Add(frm);
                frm.Attributes.Add("runat", "server");
                frm.Controls.Add(ctrl);
                pg.DesignerInitialize();
                pg.RenderControl(htmlWrite);

                var strHTML = stringWrite.ToString();

                HttpContext.Current.Response.Write(strHTML);
            }

            HttpContext.Current.Response.Write("<script>window.print();</script>");
            HttpContext.Current.Response.End();
        }

        public static void PrintHtml(string html, int printCount)
        {
            PrintHtml(html, string.Empty, printCount);
        }

        public static void PrintHtml(string html, string Script, int printCount)
        {
            HtmlForm frm;
            Page pg;

            HttpContext.Current.Response.Clear();

            for (var x = 0; x < printCount; x++)
            {
                var stringWrite = new StringWriter();

                var htmlWrite = new HtmlTextWriter(stringWrite);

                pg = new Page();
                pg.EnableEventValidation = false;

                if (!string.IsNullOrWhiteSpace(Script))
                {
                    pg.ClientScript.RegisterStartupScript(pg.GetType(), "PrintJavaScript", Script);
                }

                frm = new HtmlForm();
                pg.Controls.Add(frm);
                frm.Attributes.Add("runat", "server");
                frm.InnerHtml = html;
                pg.DesignerInitialize();
                pg.RenderControl(htmlWrite);

                var strHTML = stringWrite.ToString();

                HttpContext.Current.Response.Write(strHTML);
            }

            HttpContext.Current.Response.Write("<script>window.print();</script>");
            HttpContext.Current.Response.End();
        }
    }
}
