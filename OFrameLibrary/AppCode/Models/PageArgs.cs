namespace OFrameLibrary.Models
{
    public class PagerArgs
    {
        public string CurrentPage { get; set; }
        public string PageSize { get; set; }
        public string PagerCount { get; set; }
        public string SortKey { get; set; }
        public string SortDirection { get; set; }
    }
}