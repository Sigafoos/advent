using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

Grid<char> diagnostic = await Grid<char>.FromFile("input.txt", x => x);

int columnCount = diagnostic.ColumnCount;
BitArray gamma = new(columnCount);
BitArray epsilon = new(columnCount);
for (int i = 1; i <= columnCount; i++)
{
    char value = diagnostic.Column(i - 1).MostCommon();
    gamma.Set(columnCount-i, value == '1');
    epsilon.Set(columnCount-i, value != '1');
}

Console.WriteLine($"Part 1: {gamma.ToInt()*epsilon.ToInt()}");

// I spent a good chunk of time trying to make this abstractable. is that even a word? just gonna do it.
List<List<char>> validO2 = new();
List<List<char>> validCO2 = new();
int? o2Rating = null;
int? cO2Rating = null;
for (int i = 0; i < diagnostic.RowCount; i++)
{
    List<char> row = diagnostic.Row(i);
    validO2.Add(row);
    validCO2.Add(row);
}

for (int i = 0; i < columnCount; i++)
{
    if (o2Rating == null)
    {
        char mostCommon = validO2.Select(r => r[i])
                .GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .ThenByDescending(g => g.First())
                .SelectMany(g => g)
                .First();
        validO2 = validO2.Where(r => r[i] == mostCommon).ToList();
        if (validO2.Count == 1)
        {
            var x = validO2.Single().Take(i + 1);
            o2Rating = new BitArray(validO2.Single().Select(x => x == '1').Reverse().ToArray()).ToInt();
        }
    }

    if (cO2Rating == null)
    {
        char leastCommon = validCO2.Select(r => r[i])
                .GroupBy(x => x)
                .OrderBy(g => g.Count())
                .ThenBy(g => g.First())
                .SelectMany(g => g)
                .First();
        validCO2 = validCO2.Where(r => r[i] == leastCommon).ToList();
        if (validCO2.Count == 1)
            cO2Rating = new BitArray(validCO2.Single().Select(x => x == '1').Reverse().ToArray()).ToInt();
    }

    if (o2Rating != null && cO2Rating != null)
        break;
}

if (o2Rating == null || cO2Rating == null)
    throw new Exception("you done did it now");

Console.WriteLine($"Part 2: {o2Rating*cO2Rating}");