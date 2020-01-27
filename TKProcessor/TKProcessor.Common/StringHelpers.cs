using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TKProcessor.Common
{
    public static class StringHelpers
    {
        public static string Enclose(this string target, string encloseWith)
        {
            if (encloseWith.Length % 2 == 1)
                throw new Exception("Missing closing character");

            var prefix = encloseWith.Substring(0, encloseWith.Length / 2);
            var suffix = encloseWith.Substring(encloseWith.Length / 2);

            return prefix + target + suffix;
        }

        public static string FindFrom(this string target, IEnumerable<string> collection, bool ignoreCase = true)
        {
            return collection.FirstOrDefault(i => string.Compare(i, target, ignoreCase) == 0);
        }

        public static string FindFrom(this string target, params string[] collection)
        {
            return collection.FirstOrDefault(i => string.Compare(i, target, true) == 0);
        }

        public static bool In(this string target, IEnumerable<string> collection, bool ignoreCase = true)
        {
            return collection.Any(i => string.Compare(i, target, ignoreCase) == 0);
        }

        public static bool In(this string target, params string[] collection)
        {
            return collection.Any(i => string.Compare(i, target, true) == 0);
        }

        public static bool InsensitiveContains(this string target, string toFind)
        {
            return (CultureInfo.CurrentCulture.CompareInfo.IndexOf(target, toFind, CompareOptions.IgnoreCase) >= 0);
        }
    }
}
