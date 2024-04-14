using System.Runtime.CompilerServices;

namespace NutriPro.Application.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsAnonymousType(this Type type)
        {
            bool flag = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), inherit: false).Length != 0;
            bool flag2 = type.FullName.Contains("AnonymousType");
            return flag && flag2;
        }

        public static bool IsNumericType(this Type t)
        {
            if (Nullable.GetUnderlyingType(t) != null)
            {
                t = Nullable.GetUnderlyingType(t);
            }

            TypeCode typeCode = Type.GetTypeCode(t);
            TypeCode typeCode2 = typeCode;
            if ((uint)(typeCode2 - 5) <= 10u)
            {
                return true;
            }

            return false;
        }
    }
}
