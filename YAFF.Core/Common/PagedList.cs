using System.Collections.Generic;

namespace YAFF.Core.Common
{
    public class PagedList<T>
    {
        public IEnumerable<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalPages { get; }

        public PagedList(IEnumerable<T> items, int page, int pageSize, int totalPages)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalPages = totalPages;
        }
    }
}