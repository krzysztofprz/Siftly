using System;
using System.Linq;
using System.Linq.Expressions;
using Siftly.Extensions;

namespace Siftly.Helpers.Queryable
{
    public static class FilteringHelper
    {
        /// <summary>
        /// Filters the source <see cref="IQueryable{T}"/> by the specified property and filterValue.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="filterBy">The property name to filter by (case-insensitive).</param>
        /// <param name="filterValue">The filterValue to compare for equality.</param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> containing elements where the specified property equals the given filterValue.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="filterBy"/> is null or empty, or if the property does not exist on <typeparamref name="T"/>.
        /// </exception>
        public static IQueryable<T> Filter<T>(
            IQueryable<T> source,
            string filterBy,
            object filterValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(filterBy))
            {
                throw new ArgumentException("Invalid argument filterValue.", nameof(filterBy));
            }

            var param = Expression.Parameter(typeof(T), ExpressionExtensions.Arg);
            var property = param.GetNestedProperty(filterBy);

            if (property == null)
            {
                throw new ArgumentException(
                    $"Provided filterValue: {filterBy} of {nameof(filterBy)} parameter is not a property of {typeof(T).Name} type.");
            }

            var equal = filterValue == null
                ? Expression.Equal(property, Expression.Constant(null, property.Type))
                : Expression.Equal(property, Expression.Convert(Expression.Constant(filterValue), property.Type));

            var lambda = Expression.Lambda<Func<T, bool>>(equal, param);
            return source.Where(lambda);
        }

        /// <summary>
        /// Filters the source <see cref="IQueryable{T}"/> by the specified property and strongly-typed filterValue.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <typeparam name="S">The type of the filterValue to compare.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="filterBy">The property name to filter by (case-insensitive).</param>
        /// <param name="filterValue">The strongly-typed filterValue to compare for equality.</param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> containing elements where the specified property equals the given filterValue.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="filterBy"/> is null or empty, if the property does not exist on <typeparamref name="T"/>,
        /// or if the property type does not match <typeparamref name="S"/>.
        /// </exception>
        public static IQueryable<T> Filter<T, S>(
            IQueryable<T> source,
            string filterBy,
            S filterValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(filterBy))
            {
                throw new ArgumentException("Invalid argument filterValue.", nameof(filterBy));
            }

            var param = Expression.Parameter(typeof(T), ExpressionExtensions.Arg);
            var property = param.GetNestedProperty(filterBy);

            if (property == null)
            {
                throw new ArgumentException(
                    $"Provided filterValue: {filterBy} of {nameof(filterBy)} parameter is not a property of {typeof(T).Name} type.");
            }

            if (property.Type != typeof(S))
            {
                throw new ArgumentException(
                    $"FilterBy {filterBy} parameter type does not match {nameof(filterValue)} type of {typeof(S)}.");
            }

            var equal = filterValue == null
                ? Expression.Equal(property, Expression.Constant(null, property.Type))
                : Expression.Equal(property, Expression.Convert(Expression.Constant(filterValue), typeof(S)));

            var lambda = Expression.Lambda<Func<T, bool>>(equal, param);
            return source.Where(lambda);
        }
    }
}