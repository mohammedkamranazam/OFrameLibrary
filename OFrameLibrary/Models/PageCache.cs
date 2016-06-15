using System.Web;

namespace OFrameLibrary.Models
{
    public class PageCache
    {
        public int Minutes
        {
            get;
            set;
        }

        public HttpCacheability Location
        {
            get;
            set;
        }

        public string ID
        {
            get;
            set;
        }
    }
}