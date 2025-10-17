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

        internal static Expression NullSafe(Expression expression)
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