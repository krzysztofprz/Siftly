using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Siftly.Extensions;
using Siftly.Model;

namespace Siftly.Helpers.Queryable
{
    public static class PaginationHelper
    {
        private static readonly MethodInfo CompareString =
            typeof(string).GetMethod(nameof(string.Compare), new[] { typeof(string), typeof(string) });

        /// <summary>
        /// Applies offset-based pagination to the source <see cref="IQueryable{T}"/> using the specified property for sorting.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="orderBy">The property name to sort by (case-insensitive, supports nested properties).</param>
        /// <param name="sortingDirection">The direction to sort (ascending or descending).</param>
        /// <param name="skip">The number of elements to skip.</param>
        /// <param name="take">The number of elements to take.</param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> containing the paginated results.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="orderBy"/> is null or empty.
        /// </exception>
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

        /// <summary>
        /// Applies offset-based pagination to the source <see cref="IQueryable{T}"/> using the specified key selector expression for sorting.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <typeparam name="S">The type of the key to sort by.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="func">The key selector expression.</param>
        /// <param name="sortingDirection">The direction to sort (ascending or descending).</param>
        /// <param name="skip">The number of elements to skip.</param>
        /// <param name="take">The number of elements to take.</param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> containing the paginated results.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="source"/> or <paramref name="func"/> is null.
        /// </exception>
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

        /// <summary>
        /// Applies keyset pagination to the source <see cref="IQueryable{T}"/> using the specified property for sorting.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="orderBy">The property name to sort by (case-insensitive, supports nested properties).</param>
        /// <param name="value">The value to use as the keyset cursor.</param>
        /// <param name="sortingDirection">The direction to sort (ascending or descending).</param>
        /// <param name="take">The number of elements to take.</param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> containing the paginated results after the specified keyset value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="source"/> or <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="orderBy"/> is not a valid property of <typeparamref name="T"/>.
        /// </exception>
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

            var param = Expression.Parameter(typeof(T), ExpressionExtensions.Arg);
            var property = param.GetNestedProperty(orderBy);

            if (property == null)
            {
                throw new ArgumentException($"Provided orderBy: {orderBy} is not a property of {typeof(T).Name} type.");
            }

            var lambda = BuildKeysetLambda<T>(property, value, sortingDirection, param);

            return SortingHelper
                .Sort(source, orderBy, sortingDirection)
                .Where(lambda)
                .Take(take);
        }

        /// <summary>
        /// Applies keyset pagination to the source <see cref="IQueryable{T}"/> using the specified key selector expression for sorting.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <typeparam name="S">The type of the key to sort by.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="func">The key selector expression.</param>
        /// <param name="value">The value to use as the keyset cursor.</param>
        /// <param name="sortingDirection">The direction to sort (ascending or descending).</param>
        /// <param name="take">The number of elements to take.</param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> containing the paginated results after the specified keyset value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="source"/>, <paramref name="func"/>, or <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="func"/> does not refer to a valid property of <typeparamref name="T"/>.
        /// </exception>
        public static IQueryable<T> Keyset<T, S>(
            IQueryable<T> source,
            Expression<Func<T, S>> func,
            S value,
            SortingDirection sortingDirection,
            int take)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            var param = Expression.Parameter(typeof(T), ExpressionExtensions.Arg);
            var property = param.GetNestedProperty(func.Body.ToString().Substring(2));

            if (property == null)
            {
                throw new ArgumentException(
                    $"Provided func: {func} of {nameof(func)} parameter property is not a property of {typeof(T).Name} type.");
            }

            var lambda = BuildKeysetLambda<T>(property, value, sortingDirection, param);

            return SortingHelper
                .Sort(source, func, sortingDirection)
                .Where(lambda)
                .Take(take);
        }

        private static Expression<Func<T, bool>> BuildKeysetLambda<T>(
            Expression property,
            object value,
            SortingDirection sortingDirection,
            ParameterExpression param)
        {
            var constant = Expression.Constant(value, property.Type);
            Expression compare = GetCompare(property, constant);

            var argument = property.Type == typeof(string)
                ? Expression.Constant(0)
                : constant;

            var comparison = sortingDirection == SortingDirection.Ascending
                ? Expression.GreaterThan(compare, argument)
                : Expression.LessThan(compare, argument);

            return Expression.Lambda<Func<T, bool>>(comparison, param);
        }

        private static Expression GetCompare(Expression property, ConstantExpression constant)
        {
            return property.Type == typeof(string)
                ? Expression.Call(CompareString, property, constant)
                : property;
        }
    }
}