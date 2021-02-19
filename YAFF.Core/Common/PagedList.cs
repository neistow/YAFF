using System.Collections;
using System.Collections.Generic;

namespace YAFF.Core.Common
{
    public class PagedList<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _entities;
        public int Page { get; }
        public int PageSize { get; }
        public int TotalPages { get; }

        public PagedList(IEnumerable<T> entities, int page, int pageSize, int totalPages)
        {
            _entities = entities;
            Page = page;
            PageSize = pageSize;
            TotalPages = totalPages;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}