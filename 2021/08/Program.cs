using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Advent.Common;

List<Output> outputs = await FileLoader<Output>.Parse("input.txt", line => new Output(line));
Dictionary<int, bool> uniques = new(new List<int> { 2, 3, 4, 7 }.Select(x => new KeyValuePair<int, bool>(x, true)));
int part1 = outputs.SelectMany(o => o.RawOutputs).Count(raw => uniques.ContainsKey(raw.Length));
Console.WriteLine($"Part 1: {part1}");

internal class Output
{
    public readonly List<string> Patterns = new();
    public readonly List<string> RawOutputs = new();
    public readonly List<Display> Outputs = new();

    public Output(string raw)
    {
        List<string> split = raw.Split(" | ").ToList();
        if (split.Count != 2)
            throw new ArgumentException($"invalid input string '{raw}'");
        Patterns = split[0].Split().ToList();
        RawOutputs = split[1].Split().ToList();
    }
    
    public Output() { }
}

internal class Display
{
    public readonly bool[] Segments;

    public Display(IEnumerable<int> segments)
    {
        Segments = new bool[7];
        foreach (int segment in segments)
            Segments[segment] = true;
    }

    public static implicit operator int(Display x) => SevenSegmentDisplayValues.Parse(x.Segments);

}

public static class SevenSegmentDisplayValues
{
    private static Dictionary<bool[], int> _values = new(new []
    {
        new KeyValuePair<bool[], int>(new[] { true, true, true, false, true, true, true }, 0),
        new KeyValuePair<bool[], int>(new[] { false, false, true, false, false, true, false }, 1),
        new KeyValuePair<bool[], int>(new[] { true, false, true, true, true, false, true }, 2),
        new KeyValuePair<bool[], int>(new[] { true, false, true, true, false, true, true }, 3),
        new KeyValuePair<bool[], int>(new[] { false, true, true, true, false, true, false }, 4),
        new KeyValuePair<bool[], int>(new[] { true, true, false, true, false, true, true }, 5),
        new KeyValuePair<bool[], int>(new[] { true, true, false, true, true, true, true }, 6),
        new KeyValuePair<bool[], int>(new[] { true, true, true, false, true, true, true }, 7),
        new KeyValuePair<bool[], int>(new[] { true, true, true, true, true, true, true }, 8),
        new KeyValuePair<bool[], int>(new[] { true, true, true, true, false, true, true }, 9)
    });

    public static int Parse(bool[] raw)
    {
        return _values[raw];
    }
}
