using System;
using System.Linq.Expressions;

namespace YAFF.Core.Common
{
    public abstract class Specification<T> where T : class
    {
        public abstract Expression<Func<T, bool>> Expression { get; }
    }
}