using System;
using System.Collections.Generic;
using Advent.Common;

List<int> depths = await FileLoader<int>.Parse("input.txt", int.Parse);

int increases = 0;
int previous = depths[0];

for (int i = 1; i < depths.Count; i++)
{
    int current = depths[i];
    if (current > previous)
        increases++;
    previous = current;
}
Console.WriteLine($"Part 1: {increases}");

increases = 0;
previous = depths[0] + depths[1] + depths[2];
for (int i = 1; i < depths.Count-2; i++)
{
    int current = depths[i] + depths[i + 1] + depths[i + 2];
    if (current > previous)
        increases++;
    previous = current;
}

Console.WriteLine($"Part 2: {increases}");