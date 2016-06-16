using System;
using System.Web;

namespace OFrameLibrary.Models
{
    public class TagCloudItem
    {
        private string _text;
        private int _scaleValue;
        private string _hovertitle;

        public string Text
        {
            get
            {
                return HttpContext.Current.Server.HtmlEncode(this._text);
            }
            set
            {
                this._text = value;
            }
        }

        public string HoverTitle
        {
            get
            {
                return HttpContext.Current.Server.HtmlEncode(this._hovertitle);
            }
            set
            {
                this._hovertitle = value;
            }
        }

        public string Url
        {
            get;
            set;
        }

        public Decimal Count
        {
            get;
            set;
        }

        public int ScaleValue
        {
            get
            {
                return this._scaleValue;
            }
        }

        public TagCloudItem(string text, string url, int count)
        {
            this._text = text;
            this.Url = url;
            this.Count = (Decimal)count;
        }

        public TagCloudItem(string text, string hoverTitle, string url, int count)
        {
            this._text = text;
            this.Url = url;
            this.Count = (Decimal)count;
            this._hovertitle = hoverTitle;
        }

        public void SetScaleValue(int scaleValue)
        {
            this._scaleValue = scaleValue;
        }
    }
}