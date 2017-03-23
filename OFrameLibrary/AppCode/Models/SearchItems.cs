namespace OFrameLibrary.Models
{
    public class SearchItems
    {
        public SearchItems(string title, string description, string url)
        {
            this.Title = title;
            this.Description = description;
            this.URL = url;
        }

        public string Description
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string URL
        {
            get;
            set;
        }
    }
}
