using System;
using System.Linq;
using System.Linq.Expressions;
using Siftly.Model;

namespace Siftly.Helpers
{
    public sealed class PaginationHelper
    {
        private const string Arg = "x";

        public static IQueryable<T> Offset<T>(
            IQueryable<T> source,
            string orderBy,
            SortingDirection sortingDirection,
            int skip,
            int take)
        {
            return SortingHelper
                .Sort(source, orderBy, sortingDirection)
                .Skip(skip)
                .Take(take);
        }

        public static IQueryable<T> Keyset<T>(
            IQueryable<T> source,
            string orderBy,
            object value,
            SortingDirection sortingDirection,
            int skip,
            int take)
        {
            // var param = Expression.Parameter(typeof(T), Arg);
            // var property = Expression.Property(param, orderBy);
            // var constant = Expression.Constant(value);
            // var equals = Expression.Equal(property, Expression.Convert(constant, property.Type));
            // var lambda = Expression.Lambda<Func<T, bool>>(equals, param);

            return SortingHelper
                .Sort(source, orderBy, sortingDirection)
                // .Where(lambda)
                .Skip(skip)
                .Take(take);
        }

        public static IQueryable<T> Keyset<T, S>(
            IQueryable<T> source,
            string orderBy,
            S value,
            SortingDirection sortingDirection,
            int skip,
            int take)
        {
            var param = Expression.Parameter(typeof(T), Arg);
            var property = Expression.Property(param, orderBy);
            var constant = Expression.Constant(value, typeof(S));
            var equals = Expression.Equal(property, Expression.Convert(constant, property.Type));
            var lambda = Expression.Lambda<Func<T, bool>>(equals, param);

            return SortingHelper
                .Sort(source, orderBy, sortingDirection)
                .Where(lambda)
                .Skip(skip)
                .Take(take);
        }
    }
}