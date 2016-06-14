using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFrameLibrary.Models
{
    public class GridModel<T>
    {
        /// <summary>
        /// Total records In The Database
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// The column to sort
        /// </summary>
        public string SortKey { get; set; }

        /// <summary>
        /// The ASC or DESC sort directions
        /// </summary>
        public string SortDirection { get; set; }

        /// <summary>
        /// The actual list of records returned to the view for rendering
        /// </summary>
        public List<T> ListData { get; set; }

        /// <summary>
        /// The pager data
        /// </summary>
        public GridPager Pager { get; set; }
    }
}
