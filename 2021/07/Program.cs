using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

List<int> positions = (await FileLoader<int>.ParseSingleLine("input.txt", ",", int.Parse)).OrderBy(x => x).ToList();

KeyValuePair<int, int> best = new(-1, int.MaxValue);
// seems like this way will trip the "higher than our best" conditional quicker
for (int i = positions.Last() - positions.First(); i >= positions.First(); i--)
{
   int total = 0;
   foreach (int x in positions)
   {
      total += Math.Abs(x - i);
      if (total > best.Value)
         break;
   }

   if (total < best.Value)
       best = new KeyValuePair<int, int>(i, total);
}

Console.WriteLine($"Part 1: {best.Value}");

best = new KeyValuePair<int, int>(-1, int.MaxValue);
Dictionary<int, int> sums = new();
// seems like this way will trip the "higher than our best" conditional quicker
for (int i = positions.Last() - positions.First(); i >= positions.First(); i--)
{
   int total = 0;
   foreach (int distance in positions.Select(x => Math.Abs(x - i)))
   {
      if (!sums.TryGetValue(distance, out int sum))
      {
         sum = Enumerable.Range(0, distance+1).Sum();
         sums[distance] = sum;
      }

      total += sum;
      if (total > best.Value)
         break;
   }

   if (total < best.Value)
       best = new KeyValuePair<int, int>(i, total);
}

Console.WriteLine($"Part 2: {best.Value}");

