using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.Common
{
    public static class Extensions
    {
        public static T MostCommon<T>(this IEnumerable<T> e) =>
            e
                .GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .SelectMany(g => g)
                .First();

        public static int ToInt(this BitArray values)
        {
            int[] destination = new int[2];
            values.CopyTo(destination, 0);
            return destination[0];
        }
    }
}