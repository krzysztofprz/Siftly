using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Siftly.Model;

namespace Siftly.Helpers.Enumerable
{
    public sealed class EnumerableHelper
    {
        /// <summary>
        /// Filters the source <see cref="IEnumerable{T}"/> by the specified property and value.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="source">The source enumerable collection.</param>
        /// <param name="filterBy">The property name to filter by (case-insensitive).</param>
        /// <param name="value">The value to compare for equality.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing elements where the specified property equals the given value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="filterBy"/> is null or empty, or if the property does not exist on <typeparamref name="T"/>.
        /// </exception>
        public static IEnumerable<T> Filter<T>(IEnumerable<T> source, string filterBy, object value)
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

            return source.Where(x =>
            {
                var propValue = prop.GetValue(x);
                return Equals(propValue, value);
            });
        }

        /// <summary>
        /// Filters the source <see cref="IEnumerable{T}"/> by the specified property and strongly-typed value.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <typeparam name="S">The type of the value to compare.</typeparam>
        /// <param name="source">The source enumerable collection.</param>
        /// <param name="filterBy">The property name to filter by (case-insensitive).</param>
        /// <param name="value">The strongly-typed value to compare for equality.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing elements where the specified property equals the given value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="filterBy"/> is null or empty, or if the property does not exist on <typeparamref name="T"/>.
        /// </exception>
        public static IEnumerable<T> Filter<T, S>(IEnumerable<T> source, string filterBy, S value)
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

            return source.Where(x =>
            {
                var propValue = prop.GetValue(x);
                return Equals(propValue, value);
            });
        }

        /// <summary>
        /// Sorts the source <see cref="IEnumerable{T}"/> by the specified property in the given direction.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="source">The source enumerable collection.</param>
        /// <param name="sortBy">The property name to sort by (case-insensitive).</param>
        /// <param name="sortingDirection">The direction to sort (ascending or descending). Defaults to ascending.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> with the elements sorted by the specified property and direction.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="sortBy"/> is null or empty.
        /// </exception>
        public static IEnumerable<T> Sort<T>(IEnumerable<T> source, string sortBy, SortingDirection sortingDirection = SortingDirection.Ascending)
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

            Func<T, object> keySelector = x => prop.GetValue(x, null);

            return sortingDirection == SortingDirection.Ascending
                ? source.OrderBy(keySelector)
                : source.OrderByDescending(keySelector);
        }
    }
}