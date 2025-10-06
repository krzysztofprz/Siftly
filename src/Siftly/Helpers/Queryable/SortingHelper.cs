using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Siftly.Extensions;
using Siftly.Model;

namespace Siftly.Helpers.Queryable
{
    public static class SortingHelper
    {
        private static readonly MethodInfo OrderBy = typeof(System.Linq.Queryable).GetMethods()
            .Single(x => x.Name == nameof(OrderBy) && x.GetParameters().Length == 2);

        private static readonly MethodInfo OrderByDescending = typeof(System.Linq.Queryable).GetMethods()
            .Single(x => x.Name == nameof(OrderByDescending) && x.GetParameters().Length == 2);

        /// <summary>
        /// Sorts the source <see cref="IQueryable"/> by the specified property in the given direction.
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
        public static IQueryable<T> Sort<T>(
            IQueryable<T> source,
            string sortBy,
            SortingDirection sortingDirection = SortingDirection.Ascending)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(sortBy))
            {
                throw new ArgumentException("Invalid argument value.", nameof(sortBy));
            }

            var param = Expression.Parameter(typeof(T), ExpressionExtensions.Arg);
            var property = param.GetNestedProperty(sortBy);

            if (property == null)
            {
                throw new ArgumentException(
                    $"Provided value: {sortBy} of {nameof(sortBy)} parameter is not a property of {typeof(T).Name} type.");
            }

            var lambda = Expression.Lambda(property, param);

            var method = (sortingDirection == SortingDirection.Ascending
                    ? OrderBy
                    : OrderByDescending)
                .MakeGenericMethod(typeof(T), property.Type);

            var call = Expression.Call(method, source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(call);
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
        public static IQueryable<T> Sort<T, S>(
            IQueryable<T> source,
            Expression<Func<T, S>> func,
            SortingDirection sortingDirection = SortingDirection.Ascending)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            if (func.Parameters.Count != 1)
            {
                throw new ArgumentException($"Invalid number of {func} parameters. Expected 1 parameter.");
            }

            var parameter = func.Parameters[0];
            Expression body = NullSafe(func.Body);

            var expression = Expression.Lambda<Func<T, S>>(body, parameter);

            return sortingDirection == SortingDirection.Ascending
                ? source.OrderBy(expression)
                : source.OrderByDescending(expression);
        }

        private static Expression NullSafe(Expression expression)
        {
            switch (expression)
            {
                case ParameterExpression _:
                    return expression;
                case MemberExpression memberExpr:
                {
                    var parent = NullSafe(memberExpr.Expression);

                    if (!memberExpr.Type.IsValueType || Nullable.GetUnderlyingType(memberExpr.Type) != null)
                    {
                        return Expression.Condition(
                            Expression.Equal(parent, Expression.Constant(null, parent.Type)),
                            Expression.Default(memberExpr.Type),
                            Expression.MakeMemberAccess(parent, memberExpr.Member)
                        );
                    }

                    return Expression.MakeMemberAccess(parent, memberExpr.Member);
                }
                default:
                    return expression;
            }
        }
    }
}