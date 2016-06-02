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
        private string[] FontScale = new string[7]
        {
            "tag-link-1",
            "tag-link-2",
            "tag-link-3",
            "tag-link-4",
            "tag-link-5",
            "tag-link-6",
            "tag-link-7"
        };

        private Decimal minWeight = new Decimal(-1, -1, -1, false, (byte)0);
        private Decimal maxWeight = new Decimal(-1, -1, -1, true, (byte)0);
        private const string SPACER_MARKUP = " ";
        private const string TAG_LINK = "<a class=\"{0}\" title=\"{1}\" href=\"{2}\">{3}</a>{4}";
        private Decimal scaleUnitLength;

        private string TagCloudsHtml
        {
            get
            {
                string str = (string)this.ViewState["TagCloudsHtml"];
                return str == null ? string.Empty : str;
            }
            set
            {
                this.ViewState["TagCloudsHtml"] = (object)value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public void GenerateTagCloud(TagCloudItemCollection tags)
        {
            this.ProcessTagWeights(tags);
            this.ProcessTagCloud(tags);
            this.TagCloudsHtml = this.GetTagCloudHtml(tags);
        }

        protected void ProcessTagWeights(TagCloudItemCollection tags)
        {
            foreach (TagCloudItem tagCloudItem in (List<TagCloudItem>)tags)
            {
                if (tagCloudItem.Count < this.minWeight)
                    this.minWeight = tagCloudItem.Count;
                if (tagCloudItem.Count > this.maxWeight)
                    this.maxWeight = tagCloudItem.Count;
            }
            this.scaleUnitLength = (Convert.ToDecimal(this.maxWeight - this.minWeight) + 1) / Convert.ToDecimal(this.FontScale.Length);
        }

        protected string GetTagCloudHtml(TagCloudItemCollection tags)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div id=\"tag-cloud-div\">");
            foreach (TagCloudItem tagCloudItem in (List<TagCloudItem>)tags)
                stringBuilder.Append(string.Format("<a class=\"{0}\" title=\"{1}\" href=\"{2}\">{3}</a>{4}", (object)this.FontScale[tagCloudItem.ScaleValue], (object)tagCloudItem.HoverTitle, (object)tagCloudItem.Url, (object)tagCloudItem.Text, (object)" "));
            stringBuilder.Append("</div>");
            return ((object)stringBuilder).ToString();
        }

        protected void ProcessTagCloud(TagCloudItemCollection tags)
        {
            foreach (TagCloudItem tagCloudItem in (List<TagCloudItem>)tags)
            {
                int scaleValue = (int)Math.Truncate((tagCloudItem.Count - this.minWeight) / this.scaleUnitLength);
                tagCloudItem.SetScaleValue(scaleValue);
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.Write(this.TagCloudsHtml);
        }
    }
}