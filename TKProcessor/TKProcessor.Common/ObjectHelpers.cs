using System;

namespace TKProcessor.Common
{
    public static class ObjectHelpers
    {
        public static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public static int? GetNullableInt(string s)
        {
            if (int.TryParse(s, out int i))
                return i;

            return null;
        }

        public static decimal? GetNullableDecimal(string s)
        {
            if (decimal.TryParse(s, out decimal i))
                return i;

            return null;
        }
    }
}
