using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

List<int> adapters = await FileLoader<int>.Parse("../../../../../input/10.txt", int.Parse);

adapters.Add(0); // wall
adapters = adapters.OrderBy(x => x).ToList();
adapters.Add(adapters.Max() + 3); // my device

Dictionary<int, int> differences = new(new KeyValuePair<int, int>[] { new (1, 0), new (3, 0)});

for (int i = 0; i < adapters.Count - 1; i++)
{
    switch (adapters[i + 1] - adapters[i])
    {
        case 3:
            differences[3]++;
            break;
        case 1:
            differences[1]++;
            break;
    }
}

Console.WriteLine($"Part 1: {differences[3]*differences[1]}");
