using OFrameLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OFrameLibrary.OFrame
{
    [ToolboxData("<{0}:TagCloud runat=server></{0}:TagCloud>")]
    [DefaultProperty("BookmarkText")]
    public class TagCloud : WebControl
    {
        const string SPACER_MARKUP = " ";

        const string TAG_LINK = "<a class=\"{0}\" title=\"{1}\" href=\"{2}\">{3}</a>{4}";

        string[] FontScale = {
            "tag-link-1",
            "tag-link-2",
            "tag-link-3",
            "tag-link-4",
            "tag-link-5",
            "tag-link-6",
            "tag-link-7"
        };

        decimal maxWeight = new Decimal(-1, -1, -1, true, 0);
        decimal minWeight = new Decimal(-1, -1, -1, false, 0);
        decimal scaleUnitLength;

        string TagCloudsHtml
        {
            get
            {
                var str = (string)ViewState["TagCloudsHtml"];
                return str ?? string.Empty;
            }

            set
            {
                ViewState["TagCloudsHtml"] = value;
            }
        }

        public void GenerateTagCloud(TagCloudItemCollection tags)
        {
            ProcessTagWeights(tags);
            ProcessTagCloud(tags);
            TagCloudsHtml = GetTagCloudHtml(tags);
        }

        protected string GetTagCloudHtml(TagCloudItemCollection tags)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div id=\"tag-cloud-div\">");
            foreach (TagCloudItem tagCloudItem in tags)
            {
                stringBuilder.Append($"<a class=\"{FontScale[tagCloudItem.ScaleValue]}\" title=\"{tagCloudItem.HoverTitle}\" href=\"{tagCloudItem.Url}\">{tagCloudItem.Text}</a>{" "}");
            }

            stringBuilder.Append("</div>");
            return stringBuilder.ToString();
        }

        protected void ProcessTagCloud(TagCloudItemCollection tags)
        {
            foreach (TagCloudItem tagCloudItem in tags)
            {
                var scaleValue = (int)Math.Truncate((tagCloudItem.Count - minWeight) / scaleUnitLength);
                tagCloudItem.SetScaleValue(scaleValue);
            }
        }

        protected void ProcessTagWeights(TagCloudItemCollection tags)
        {
            foreach (TagCloudItem tagCloudItem in tags)
            {
                if (tagCloudItem.Count < minWeight)
                {
                    minWeight = tagCloudItem.Count;
                }

                if (tagCloudItem.Count > maxWeight)
                {
                    maxWeight = tagCloudItem.Count;
                }
            }
            scaleUnitLength = (Convert.ToDecimal(maxWeight - minWeight) + 1) / Convert.ToDecimal(FontScale.Length);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.Write(TagCloudsHtml);
        }
    }
}
