using System;
using System.Collections.Generic;
using System.Text;

namespace Dister.Net.Helpers
{
    internal static class IntHelpers
    {
        /// <summary>
        /// Creates hex <see cref="string"/> represenation <see cref="int"/> value
        /// </summary>
        /// <param name="i"><see cref="int"/> value</param>
        /// <returns>Hex represenation of <see cref="int"/> value</returns>
        internal static string ToHex(this int i) => i.ToString("X8");
        /// <summary>
        /// Creates <see cref="int"/> value from its hex representation
        /// </summary>
        /// <param name="s">Hex representation</param>
        /// <returns><see cref="int"/> value</returns>
        internal static int FromHex(this string s) => int.Parse(s, System.Globalization.NumberStyles.HexNumber);
    }
}
