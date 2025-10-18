using System;
using System.Linq;
using System.Linq.Expressions;
using Siftly.Helpers.Queryable;

namespace Siftly.Extensions
{
    public static class FilteringHelperExtensions
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> source, string filterBy, object filterValue)
        {
            return FilteringHelper.Filter(source, filterBy, filterValue);
        }

        public static IQueryable<T> Filter<T, S>(this IQueryable<T> source, string filterBy, S filterValue)
        {
            return FilteringHelper.Filter(source, filterBy, filterValue);
        }

        public static IQueryable<T> Filter<T, S>(this IQueryable<T> source, Expression<Func<T, S>> func, S filterValue)
        {
            return FilteringHelper.Filter(source, func, filterValue);
        }
    }
}