namespace OFrameLibrary.Models
{
    public class JQXTreeItem
    {
        public long id { get; set; }
        public long parentid { get; set; }
        public string text { get; set; }
        public string value { get; set; }
        public bool selected { get; set; }
        public bool expanded { get; set; }
        public bool disabled { get; set; }
    }
}