namespace OFrameLibrary.Models
{
    public class GridModel
    {
        /// <summary>
        /// Total records In The Database
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// The actual list of records returned to the view for rendering
        /// </summary>
        public object ListData { get; set; }

        /// <summary>
        /// The pager data
        /// </summary>
        public GridPager Pager { get; set; }

        /// <summary>
        /// The ASC or DESC sort directions
        /// </summary>
        public string SortDirection { get; set; }

        /// <summary>
        /// The column to sort
        /// </summary>
        public string SortKey { get; set; }
    }
}
