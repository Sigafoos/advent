using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Advent.Common;

Grid<int> heightMap = await Grid<int>.FromFile("input.txt", x => int.Parse(x.ToString()));

int part1 = heightMap
    .Where(v =>
        heightMap
            .OrthogonalNeighborsOf(v.Position)
            .All(x => x > v.Value))
    .Select(x => x.Value + 1)
    .Sum();

Console.WriteLine($"Part 1: {part1}");