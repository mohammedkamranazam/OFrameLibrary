using OFrameLibrary.Util;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace OFrameLibrary.Helpers
{
    public static class WebControlHelper
    {
        public static void AddSelect(this DropDownList dropDownList)
        {
            dropDownList.Items.Insert(0, new ListItem("Select", "-1"));
        }

        public static void ExportExcel(ControlCollection Controls, Object datasource, string filename)
        {
            var gridview = new GridView();
            gridview.DataSource = datasource;
            gridview.DataBind();
            gridview.AllowPaging = false;

            if (gridview.Rows.Count > 65535)
            {
                return;
            }

            filename = string.Format("{0}_{1}.xls", filename, Utilities.DateTimeNow());

            var response = HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            response.Charset = string.Empty;

            response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            response.ContentType = "application/vnd.xls";

            var stringWriter = new StringWriter();
            var htmlWriter = new HtmlTextWriter(stringWriter);

            ClearControls(gridview);

            var form = new HtmlForm();
            Controls.Add(form);
            form.Controls.Add(gridview);
            form.RenderControl(htmlWriter);

            response.Write(stringWriter.ToString());
            response.End();
        }

        public static string GetHTML(Control ctrl, string Script)
        {
            var frm = new HtmlForm();
            using (Page pg = new Page())
            {
                HttpContext.Current.Response.Clear();

                var stringWrite = new StringWriter();
                var htmlWrite = new HtmlTextWriter(stringWrite);

                if (ctrl is WebControl)
                {
                    var w = new Unit(100, UnitType.Percentage);

                    ((WebControl)ctrl).Width = w;
                }
                pg.EnableEventValidation = false;

                if (!string.IsNullOrWhiteSpace(Script))
                {
                    pg.ClientScript.RegisterStartupScript(pg.GetType(), "PrintJavaScript", Script);
                }
                pg.Controls.Add(frm);
                frm.Attributes.Add("runat", "server");
                frm.Controls.Add(ctrl);
                pg.DesignerInitialize();
                pg.RenderControl(htmlWrite);
                var strHTML = stringWrite.ToString();

                return strHTML;
            }
        }

        public static string GetTagsHTML(string tags, Page page)
        {
            var tagsList = tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var sb = new StringBuilder();

            foreach (var tag in tagsList)
            {
                sb.Append(string.Format("<strong><a href='{1}'>{0}</a></strong>, ", tag, page.ResolveClientUrl(string.Format("~/Search.aspx?Search={0}", tag))));
            }

            if (sb.Length > 0)
            {
                var lastCommaIndex = sb.ToString().LastIndexOf(',');

                sb = sb.Remove(lastCommaIndex, 1);
            }
            else
            {
                sb.Append("No tags");
            }

            return sb.ToString();
        }

        private static void ClearControls(Control control)
        {
            for (var i = control.Controls.Count - 1; i >= 0; i--)
            {
                ClearControls(control.Controls[i]);
            }

            if (!(control is TableCell))
            {
                if (control.GetType().GetProperty("SelectedItem") != null)
                {
                    var literal = new LiteralControl();
                    control.Parent.Controls.Add(literal);
                    try
                    {
                        literal.Text =
                            (string)control.GetType().GetProperty("SelectedItem").
                                GetValue(control, null);
                    }
                    catch
                    {
                    }

                    control.Parent.Controls.Remove(control);
                }
                else
                {
                    if (control.GetType().GetProperty("Text") != null)
                    {
                        var literal = new LiteralControl();
                        control.Parent.Controls.Add(literal);
                        literal.Text = (string)control.GetType().GetProperty("Text").GetValue(control, null);
                        control.Parent.Controls.Remove(control);
                    }
                }
            }
        }

        //public static Gender GetGender(this DropDownListAdv dropDownList)
        //{
        //    if (dropDownList.SelectedValue == Gender.Male.ToString())
        //    {
        //        return Gender.Male;
        //    }
        //    else
        //    {
        //        if (dropDownList.SelectedValue == Gender.Female.ToString())
        //        {
        //            return Gender.Female;
        //        }
        //        else
        //        {
        //            if (dropDownList.SelectedValue == Gender.Unspecified.ToString())
        //            {
        //                return Gender.Unspecified;
        //            }
        //            else
        //            {
        //                return Gender.Unspecified;
        //            }
        //        }
        //    }
        //}

        //public static string GetImageFileExtension(this AjaxFileUploadEventArgs ee)
        //{
        //    var extension = ".jpg";

        //    if (ee.ContentType.NullableContains("jpg"))
        //    {
        //        extension = ".jpg";
        //    }

        //    if (ee.ContentType.NullableContains("gif"))
        //    {
        //        extension = ".gif";
        //    }

        //    if (ee.ContentType.NullableContains("png"))
        //    {
        //        extension = ".png";
        //    }

        //    if (ee.ContentType.NullableContains("jpeg"))
        //    {
        //        extension = ".jpeg";
        //    }

        //    return extension;
        //}

        //public static int? GetNullableSelectedValue(this DropDownListAdv dropDownList)
        //{
        //    if (dropDownList.SelectedItem == null || dropDownList.SelectedValue == "-1")
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return dropDownList.GetSelectedValue();
        //    }
        //}
        //public static int GetSelectedValue(this DropDownListAdv dropDownList)
        //{
        //    return DataParser.IntParse(dropDownList.SelectedValue);
        //}
        //public static void SetNullableSelectedValue(this DropDownListAdv dropDownList, int? value)
        //{
        //    if (value == null)
        //    {
        //        dropDownList.SelectedValue = "-1";
        //    }
        //    else
        //    {
        //        dropDownList.SelectedValue = value.ToString();
        //    }
        //}
    }
}
