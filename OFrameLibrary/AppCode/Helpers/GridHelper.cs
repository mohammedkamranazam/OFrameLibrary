using OFrameLibrary.Models;
using System;
using System.Linq;
using System.Linq.Dynamic;

namespace OFrameLibrary.Helpers
{
    public static class GridHelper
    {
        public static void BuildPager(this GridModel gm)
        {
            gm.Pager.TotalPages = (int)Math.Ceiling(Decimal.Divide(gm.Count, gm.Pager.PageSize));

            var startPage = gm.Pager.CurrentPage - 5;
            var endPage = gm.Pager.CurrentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > gm.Pager.TotalPages)
            {
                endPage = gm.Pager.TotalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            gm.Pager.StartPage = startPage;
            gm.Pager.EndPage = endPage;

            if (gm.Pager.CurrentPage > gm.Pager.TotalPages)
            {
                gm.Pager.CurrentPage = gm.Pager.TotalPages;
            }

            if (gm.Pager.CurrentPage < 1)
            {
                gm.Pager.CurrentPage = 1;
            }

            for (var x = gm.Pager.StartPage; x <= gm.Pager.EndPage; x++)
            {
                gm.Pager.Pages.Add(new GridPage
                {
                    SortKey = gm.SortKey,
                    SortDirection = gm.SortDirection,
                    PageNumber = x,
                    Text = x.ToString(),
                    CssClass = ((x == gm.Pager.CurrentPage) ? gm.Pager.LinkSelectedCssClass : string.Empty)
                });
            }

            if (gm.Pager.CurrentPage != 1)
            {
                gm.Pager.Pages.Insert(0, new GridPage
                {
                    SortKey = gm.SortKey,
                    SortDirection = gm.SortDirection,
                    PageNumber = 1,
                    Text = gm.Pager.FirstLinkText,
                    CssClass = gm.Pager.FirstLinkCssClass
                });
            }

            if (gm.Pager.CurrentPage != gm.Pager.TotalPages)
            {
                gm.Pager.Pages.Insert(gm.Pager.Pages.Count, new GridPage
                {
                    SortKey = gm.SortKey,
                    SortDirection = gm.SortDirection,
                    PageNumber = gm.Pager.TotalPages,
                    Text = gm.Pager.LastLinkText,
                    CssClass = gm.Pager.LastLinkCssClass
                });
            }

            if (gm.Pager.TotalPages > 1)
            {
                if (gm.Pager.CurrentPage > 1)
                {
                    gm.Pager.Pages.Insert(1, new GridPage
                    {
                        SortKey = gm.SortKey,
                        SortDirection = gm.SortDirection,
                        PageNumber = gm.Pager.CurrentPage - 1,
                        Text = gm.Pager.PreviousLinkText,
                        CssClass = gm.Pager.PreviousLinkCssClass
                    });
                }

                if (gm.Pager.CurrentPage < gm.Pager.TotalPages)
                {
                    gm.Pager.Pages.Insert(gm.Pager.Pages.Count - 1, new GridPage
                    {
                        SortKey = gm.SortKey,
                        SortDirection = gm.SortDirection,
                        PageNumber = gm.Pager.CurrentPage + 1,
                        Text = gm.Pager.NextLinkText,
                        CssClass = gm.Pager.NextLinkCssClass
                    });
                }
            }
        }

        public static IQueryable<T> UpdateGridModelList<T>(this IQueryable<T> entitySet, GridModel gm)
        {
            gm.Count = entitySet.Count();

            gm.BuildPager();

            var sortDirection = gm.SortDirection;

            if (gm.SortDirection == "DESC")
            {
                gm.SortDirection = "ASC";
            }
            else if (gm.SortDirection == "ASC")
            {
                gm.SortDirection = "DESC";
            }

            return entitySet.OrderBy(gm.SortKey + " " + sortDirection).Skip((gm.Pager.CurrentPage - 1) * gm.Pager.PageSize).Take(gm.Pager.PageSize);
        }
    }
}
