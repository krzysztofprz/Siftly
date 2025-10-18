using System;
using System.Linq;
using System.Linq.Expressions;
using Siftly.Helpers.Queryable;
using Siftly.Model;

namespace Siftly.Extensions
{
    public static class PaginationHelperExtensions
    {
        public static IQueryable<T> Offset<T>(
            this IQueryable<T> source,
            string orderBy,
            SortingDirection sortingDirection,
            int skip,
            int take)
        {
            return PaginationHelper.Offset(source, orderBy, sortingDirection, skip, take);
        }

        public static IQueryable<T> Offset<T, S>(
            this IQueryable<T> source,
            Expression<Func<T, S>> func,
            SortingDirection sortingDirection,
            int skip,
            int take)
        {
            return PaginationHelper.Offset(source, func, sortingDirection, skip, take);
        }

        public static IQueryable<T> Keyset<T>(
            this IQueryable<T> source,
            string orderBy,
            object value,
            SortingDirection sortingDirection,
            int take)
        {
            return PaginationHelper.Keyset(source, orderBy, value, sortingDirection, take);
        }

        public static IQueryable<T> Keyset<T, S>(
            this IQueryable<T> source,
            Expression<Func<T, S>> func,
            S value,
            SortingDirection sortingDirection,
            int take)
        {
            return PaginationHelper.Keyset(source, func, value, sortingDirection, take);
        }
    }
}