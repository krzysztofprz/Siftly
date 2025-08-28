using Siftly.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Siftly
{
    public static class QueryableHelper
    {
        private const string Arg = "x";

        private static readonly MethodInfo OrderBy = typeof(Queryable).GetMethods().Single(x => x.Name == nameof(OrderBy) && x.GetParameters().Length == 2);
        private static readonly MethodInfo OrderByDescending = typeof(Queryable).GetMethods().Single(x => x.Name == nameof(OrderByDescending) && x.GetParameters().Length == 2);

        /// <summary>
        /// Filters the source <see cref="IQueryable{T}"/> by the specified property and value.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="filterBy">The property name to filter by (case-insensitive).</param>
        /// <param name="value">The value to compare for equality.</param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> containing elements where the specified property equals the given value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="filterBy"/> is null or empty, or if the property does not exist on <typeparamref name="T"/>.
        /// </exception>
        public static IQueryable<T> Filter<T>(IQueryable<T> source, string filterBy, object value)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!source.Any())
            {
                return source;
            }

            if (string.IsNullOrEmpty(filterBy))
            {
                throw new ArgumentException("Invalid argument value.", nameof(filterBy));
            }

            var prop = typeof(T).GetProperty(filterBy, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (prop == null)
            {
                throw new ArgumentException($"Provided value: {filterBy} of {nameof(filterBy)} parameter is not a property of {typeof(T).Name} type.");
            }

            var param = Expression.Parameter(typeof(T), Arg);
            var property = Expression.Property(param, filterBy);
            var constant = Expression.Constant(value);
            var equals = Expression.Equal(property, Expression.Convert(constant, property.Type));
            var lambda = Expression.Lambda<Func<T, bool>>(equals, param);

            return source.Where(lambda);
        }

        /// <summary>
        /// Filters the source <see cref="IQueryable{T}"/> by the specified property and strongly-typed value.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <typeparam name="S">The type of the value to compare.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="filterBy">The property name to filter by (case-insensitive).</param>
        /// <param name="value">The strongly-typed value to compare for equality.</param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> containing elements where the specified property equals the given value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="filterBy"/> is null or empty, if the property does not exist on <typeparamref name="T"/>,
        /// or if the property type does not match <typeparamref name="S"/>.
        /// </exception>
        public static IQueryable<T> Filter<T, S>(IQueryable<T> source, string filterBy, S value)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!source.Any())
            {
                return source;
            }

            if (string.IsNullOrEmpty(filterBy))
            {
                throw new ArgumentException("Invalid argument value.", nameof(filterBy));
            }

            var prop = typeof(T).GetProperty(filterBy, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (prop == null)
            {
                throw new ArgumentException($"Provided value: {filterBy} of {nameof(filterBy)} parameter is not a property of {typeof(T).Name} type.");
            }

            if (prop.PropertyType != typeof(S))
            {
                throw new ArgumentException($"FilterBy {filterBy} parameter type does not match {nameof(value)} type of {typeof(S)}.");
            }

            var param = Expression.Parameter(typeof(T), Arg);
            var property = Expression.Property(param, filterBy);
            var constant = Expression.Constant(value, typeof(S));
            var equals = Expression.Equal(property, Expression.Convert(constant, property.Type));
            var lambda = Expression.Lambda<Func<T, bool>>(equals, param);

            return source.Where(lambda);
        }

        /// <summary>
        /// Filters the source <see cref="IQueryable{T}"/> using the specified predicate expression.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="predicate">The predicate expression to filter by.</param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> containing elements that satisfy the predicate.
        /// </returns>
        public static IQueryable<T> Filter<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            return source.Where(predicate);
        }

        /// <summary>
        /// Sorts the source <see cref="IQueryable{T}"/> by the specified property in the given direction.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="sortBy">The property name to sort by (case-insensitive).</param>
        /// <param name="sortingDirection">The direction to sort (ascending or descending). Defaults to ascending.</param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> with the elements sorted by the specified property and direction.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="sortBy"/> is null or empty, or if the property does not exist on <typeparamref name="T"/>.
        /// </exception>
        public static IQueryable<T> Sort<T>(IQueryable<T> source, string sortBy, SortingDirection sortingDirection = SortingDirection.Ascending)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!source.Any())
            {
                return source;
            }

            if (string.IsNullOrEmpty(sortBy))
            {
                throw new ArgumentException("Invalid argument value.", nameof(sortBy));
            }

            var prop = typeof(T).GetProperty(sortBy, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (prop == null)
            {
                throw new ArgumentException($"Provided value: {sortBy} of {nameof(sortBy)} parameter is not a property of {typeof(T).Name} type.");
            }

            var param = Expression.Parameter(typeof(T), Arg);
            var property = Expression.Property(param, sortBy);
            var lambda = Expression.Lambda(property, param);

            var method = (sortingDirection == SortingDirection.Ascending
                    ? OrderBy
                    : OrderByDescending)
                .MakeGenericMethod(typeof(T), property.Type);

            return (IQueryable<T>)method.Invoke(null, new object[] { source, lambda });
        }

        /// <summary>
        /// Sorts the source <see cref="IQueryable{T}"/> using the specified key selector expression and direction.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <typeparam name="S">The type of the key to sort by.</typeparam>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="func">The key selector expression.</param>
        /// <param name="sortingDirection">The direction to sort (ascending or descending). Defaults to ascending.</param>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> with the elements sorted by the specified key and direction.
        /// </returns>
        public static IQueryable<T> Sort<T, S>(IQueryable<T> source, Expression<Func<T, S>> func, SortingDirection sortingDirection = SortingDirection.Ascending)
        {
            return sortingDirection == SortingDirection.Ascending
                ? source.OrderBy(func)
                : source.OrderByDescending(func);
        }
    }
}