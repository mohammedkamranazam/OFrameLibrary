﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFrameLibrary.Models
{
    public class GridPage
    {
        public string SortKey { get; set; }

        public string SortDirection { get; set; }

        public int PageNumber { get; set; }

        public string Text { get; set; }

        public string CssClass { get; set; }
    }
}