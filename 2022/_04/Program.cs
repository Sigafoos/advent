using Advent.Common;

List<List<Assignment>>? pairs = await FileLoader<List<Assignment>>.Parse("input.txt", line =>
    line.Split(",").Select(elf => new Assignment(elf)).ToList());

int part1 = pairs.Count(p => p[0].CompletelyOverlaps(p[1]));
Console.WriteLine($"Part 1: {part1}");

int part2 = pairs.Count(p => p[0].Intersects(p[1]));
Console.WriteLine($"Part 2: {part2}");

class Assignment 
{
    public int Start { get; init; }
    public int End { get; init; }

    public bool CompletelyOverlaps(Assignment other) =>
        (Start <= other.Start && End >= other.End) ||
        (Start >= other.Start && End <= other.End);

    public bool Intersects(Assignment other) =>
        (Start >= other.Start && Start <= other.End) ||
        (End >= other.Start && End <= other.End) ||
        (other.Start >= Start && other.Start <= End) ||
        (other.End >= Start && other.End <= End);
    
    public Assignment(string input)
    {
        List<int> assignment = input.Split("-").Select(int.Parse).ToList();
        Start = assignment[0];
        End = assignment[1];
    }
}