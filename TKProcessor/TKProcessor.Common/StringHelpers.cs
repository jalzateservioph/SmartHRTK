using System;

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
    }
}
