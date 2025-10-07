using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Siftly.Extensions
{
    internal static class ExpressionExtensions
    {
        internal const string Arg = "x";

        private static readonly ConcurrentDictionary<(Type, string), PropertyInfo> PropertyCache = new();

        internal static Expression GetNestedProperty(this Expression param, string propertyPath)
        {
            Expression expression = param;

            foreach (var member in propertyPath.Split('.'))
            {
                var propertyInfo = PropertyCache.GetOrAdd(
                    (expression.Type, member),
                    key => key.Item1.GetProperty(
                        key.Item2,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase));

                if (propertyInfo == null)
                {
                    return null;
                }

                if (!expression.Type.IsValueType || Nullable.GetUnderlyingType(expression.Type) != null)
                {
                    Expression nullValue;

                    var propAccess = Expression.Property(expression, propertyInfo);

                    if (!propertyInfo.PropertyType.IsValueType ||
                        Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
                    {
                        nullValue = Expression.Constant(null, propertyInfo.PropertyType);
                    }
                    else
                    {
                        nullValue = Expression.Default(propertyInfo.PropertyType);
                    }

                    expression = Expression.Condition(
                        Expression.Equal(expression, Expression.Constant(null, expression.Type)),
                        nullValue,
                        propAccess);

                    continue;
                }

                expression = Expression.Property(expression, propertyInfo);
            }

            return expression;
        }
    }
}