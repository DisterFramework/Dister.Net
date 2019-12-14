using System;
using System.Collections.Generic;
using System.Text;

namespace Dister.Net.Helpers
{
    internal static class IntHelpers
    {
        internal static string ToHex(this int i) => i.ToString("X8");
        internal static int FromHex(this string s) => int.Parse(s, System.Globalization.NumberStyles.HexNumber);
    }
}
