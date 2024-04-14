using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace NutriPro.Application.Extensions
{
    public static class ExpressionExtension
    {
        public static string MemberName<T, V>(this Expression<Func<T, V>> expression)
        {
            if (!(expression.Body is MemberExpression memberExpression))
            {
                throw new InvalidOperationException("Expression must be a member expression");
            }

            return memberExpression.Member.Name;
        }

        public static T GetAttribute<T>(this ICustomAttributeProvider provider) where T : Attribute
        {
            object[] customAttributes = provider.GetCustomAttributes(typeof(T), inherit: true);
            return (customAttributes.Length != 0) ? (customAttributes[0] as T) : null;
        }

        public static bool IsRequired<T, V>(this Expression<Func<T, V>> expression)
        {
            if (!(expression.Body is MemberExpression memberExpression))
            {
                throw new InvalidOperationException("Expression must be a member expression");
            }

            return memberExpression.Member.GetAttribute<RequiredAttribute>() != null;
        }
    }
}
