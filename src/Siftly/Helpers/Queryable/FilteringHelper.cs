using System;
using System.Linq;
using System.Linq.Expressions;
using Siftly.Extensions;

namespace Siftly.Helpers.Queryable
{
    /// <summary>
    /// Filters the source <see cref="IQueryable{T}"/> by the specified property and value.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source.</typeparam>
    /// <param name="source">The source queryable collection.</param>
    /// <param name="filterBy">The property name to filter by (case-insensitive, supports nested properties).</param>
    /// <param name="filterValue">The value to filter by.</param>
    /// <returns>
    /// An <see cref="IQueryable{T}"/> containing elements where the specified property equals the given value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="source"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="filterBy"/> is null or empty, or if the property does not exist on <typeparamref name="T"/>.
    /// </exception>
    public static class FilteringHelper
    {
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

            var lambda = BuildFilterLambda<T>(filterBy, filterValue, out _);

            return source.Where(lambda);
        }

        /// <summary>
        /// Filters the source <see cref="IQueryable{T}"/> by the specified property and value, with strong typing.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <typeparam name="S">The type of the filter value.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="filterBy">The property name to filter by (case-insensitive, supports nested properties).</param>
        /// <param name="filterValue">The value to filter by.</param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> containing elements where the specified property equals the given value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="filterBy"/> is null or empty, if the property does not exist on <typeparamref name="T"/>, or if the property type does not match <typeparamref name="S"/>.
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

            var lambda = BuildFilterLambda<T>(filterBy, filterValue, out var propertyType);

            if (propertyType != typeof(S))
            {
                throw new ArgumentException($"FilterBy {filterBy} parameter type does not match {nameof(filterValue)} type of {typeof(S)}.");
            }


            return source.Where(lambda);
        }

        private static Expression<Func<T, bool>> BuildFilterLambda<T>(
            string filterBy,
            object filterValue,
            out Type propertyType)
        {
            var param = Expression.Parameter(typeof(T), ExpressionExtensions.Arg);
            var property = param.GetNestedProperty(filterBy);

            if (property == null)
            {
                throw new ArgumentException($"Provided filterValue: {filterBy} of filterBy parameter is not a property of {typeof(T).Name} type.");
            }

            propertyType = property.Type;

            var equal = filterValue == null
                ? Expression.Equal(property, Expression.Constant(null, property.Type))
                : Expression.Equal(property, Expression.Convert(Expression.Constant(filterValue), property.Type));

            return Expression.Lambda<Func<T, bool>>(equal, param);
        }
    }
}