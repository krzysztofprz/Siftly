using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Siftly.Common;
using Siftly.Model;

namespace Siftly.Helpers.Queryable
{
    public sealed class PaginationHelper : Helper
    {
        private static MethodInfo _compareString =
            typeof(string).GetMethod(nameof(string.Compare), new[] { typeof(string), typeof(string) });

        public static IQueryable<T> Offset<T>(
            IQueryable<T> source,
            string orderBy,
            SortingDirection sortingDirection,
            int skip,
            int take)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(orderBy))
            {
                throw new ArgumentException("Invalid argument orderBy.", nameof(orderBy));
            }

            return SortingHelper
                .Sort(source, orderBy, sortingDirection)
                .Skip(skip)
                .Take(take);
        }

        public static IQueryable<T> Offset<T, S>(
            IQueryable<T> source,
            Expression<Func<T, S>> func,
            SortingDirection sortingDirection,
            int skip,
            int take)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return SortingHelper
                .Sort(source, func, sortingDirection)
                .Skip(skip)
                .Take(take);
        }

        public static IQueryable<T> Keyset<T>(
            IQueryable<T> source,
            string orderBy,
            object value,
            SortingDirection sortingDirection,
            int take)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var param = Expression.Parameter(typeof(T), Arg);
            var property = GetNestedProperty(param, orderBy);

            var constant = Expression.Constant(value, property.Type);

            Expression compare = GetCompare(property, constant);

            var argument = property.Type == typeof(string)
                ? Expression.Constant(0)
                : constant;

            var comparison = sortingDirection == SortingDirection.Ascending
                ? Expression.GreaterThan(compare, argument)
                : Expression.LessThan(compare, argument);

            var lambda = Expression.Lambda<Func<T, bool>>(comparison, param);

            return SortingHelper
                .Sort(source, orderBy, sortingDirection)
                .Where(lambda)
                .Take(take);
        }

        public static IQueryable<T> Keyset<T, S>(
            IQueryable<T> source,
            Expression<Func<T, S>> func,
            S value,
            SortingDirection sortingDirection,
            int take)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var param = Expression.Parameter(typeof(T), Arg);
            var property = GetNestedProperty(param, func.Body.ToString().Substring(2));

            if (property == null)
            {
                throw new ArgumentException($"Provided func: {func} of {nameof(func)} parameter property is not a property of {typeof(T).Name} type.");
            }

            var constant = Expression.Constant(value, typeof(S));

            Expression compare = GetCompare(property, constant);

            var argument = property.Type == typeof(string)
                ? Expression.Constant(0)
                : constant;

            var comparison = sortingDirection == SortingDirection.Ascending
                ? Expression.GreaterThan(compare, argument)
                : Expression.LessThan(compare, argument);

            var lambda = Expression.Lambda<Func<T, bool>>(comparison, param);

            return SortingHelper
                .Sort(source, func, sortingDirection)
                .Where(lambda)
                .Take(take);
        }

        // public static IQueryable<T> Keyset<T, S>(
        //     IQueryable<T> source,
        //     string orderBy,
        //     S value,
        //     SortingDirection sortingDirection,
        //     int take)
        // {
        //     if (source == null)
        //     {
        //         throw new ArgumentNullException(nameof(source));
        //     }
        //
        //     if (value == null)
        //     {
        //         throw new ArgumentNullException(nameof(value));
        //     }
        //
        //     var param = Expression.Parameter(typeof(T), Arg);
        //     var property = GetNestedProperty(param, orderBy);
        //     var constant = Expression.Constant(value, typeof(S));
        //
        //     Expression compare = sortingDirection == SortingDirection.Ascending
        //         ? Expression.GreaterThan(property, Expression.Convert(constant, property.Type))
        //         : Expression.LessThan(property, Expression.Convert(constant, property.Type));
        //
        //     var lambda = Expression.Lambda<Func<T, bool>>(compare, param);
        //
        //     return SortingHelper
        //         .Sort(source, orderBy, sortingDirection)
        //         .Where(lambda)
        //         .Take(take);
        // }

        // public static IQueryable<T> Keyset<T, S>(
        //     IQueryable<T> source,
        //     Expression<Func<T, S>> func,
        //     SortingDirection sortingDirection,
        //     int take)
        // {
        //     var sorted = SortingHelper
        //         .Sort(source, func, sortingDirection);
        //     
        //     var filtered = FilteringHelper.Filter(sorted, )
        //
        // }

        private static Expression GetCompare(Expression property, ConstantExpression constant)
        {
            return property.Type == typeof(string)
                ? Expression.Call(_compareString, property, constant)
                : property;
        }
    }
}