namespace Hms.Common
{
    using System;
    using System.Collections.Generic;

    public static class EnumExtensions
    {
        public static IEnumerable<string> GetFlags(this Enum input)
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
            {
                if (input.HasFlag(value))
                {
                    yield return value.ToString();
                }
            }
        }
    }
}