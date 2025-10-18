using System;
using System.Linq;
using System.Linq.Expressions;
using Siftly.Helpers.Queryable;
using Siftly.Model;

namespace Siftly.Extensions
{
    public static class SortingHelperExtensions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string sortBy, SortingDirection sortingDirection = SortingDirection.Ascending)
        {
            return SortingHelper.Sort(source, sortBy, sortingDirection);
        }

        public static IQueryable<T> Sort<T, S>(this IQueryable<T> source, Expression<Func<T, S>> func, SortingDirection sortingDirection = SortingDirection.Ascending)
        {
            return SortingHelper.Sort(source, func, sortingDirection);
        }
    }
}