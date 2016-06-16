using System.Collections.Generic;

namespace OFrameLibrary.Models
{
    public class GridPager
    {
        /// <summary>
        /// Total Number of Pages calculated from Page Size
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// The Current Page Number
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Number of Records to Display in a single page of grid view
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Number of Pager buttons to show
        /// </summary>
        public int PagerCount { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public List<GridPage> Pages { get; set; }

        public string FirstLinkText { get; set; } = "First";

        public string LastLinkText { get; set; } = "Last";

        public string PreviousLinkText { get; set; } = "Previous";

        public string NextLinkText { get; set; } = "Next";

        public string FirstLinkCssClass { get; set; } = "First";

        public string LastLinkCssClass { get; set; } = "Last";

        public string PreviousLinkCssClass { get; set; } = "Previous";

        public string NextLinkCssClass { get; set; } = "Next";

        public string LinkSelectedCssClass { get; set; } = "Selected";
    }
}