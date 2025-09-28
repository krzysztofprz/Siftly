using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Siftly.Common
{
    public abstract class Helper
    {
        protected const string Arg = "x";

        /// <summary>
        /// Gets an Expression representing a (possibly nested) property access.
        /// Example: "Address.City" -> x => x.Address.City
        /// </summary>
        protected static Expression GetNestedProperty(Expression param, string propertyPath)
        {
            Expression expression = param;

            foreach (var member in propertyPath.Split('.'))
            {
                var propertyInfo = expression.Type.GetProperty(
                    member,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (propertyInfo == null)
                {
                    return null;
                }

                if (!expression.Type.IsValueType || Nullable.GetUnderlyingType(expression.Type) != null)
                {
                    Expression nullValue;
                    
                    var propAccess = Expression.Property(expression, propertyInfo);

                    if (!propertyInfo.PropertyType.IsValueType || Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
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