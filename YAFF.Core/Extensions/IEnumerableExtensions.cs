﻿using System;
using System.Collections.Generic;
using YAFF.Core.Common;

namespace YAFF.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> enumerable, int page, int pageSize,
            int totalRecordsCount)
        {
            return new PagedList<T>(enumerable, page, pageSize, (int) Math.Ceiling(totalRecordsCount / (double) pageSize));
        }
    }
}