using System.Text.RegularExpressions;
using Advent.Common;

// lazy lazy lazy
Regex pattern = new(@"^Game (\d+): (.*)$", RegexOptions.Compiled);
List<Game> games = await FileLoader<Game>.Parse("input.txt", line =>
{
    Match match = pattern.Match(line);
    return new Game(int.Parse(match.Groups[1].Value),
        match.Groups[2].Value.Split("; ").Select(t => new Throw(t)).ToList());
});

Dictionary<string, int> cubes = new()
{
    { "red", 12 },
    { "green", 13 },
    { "blue", 14 }
};

int part1 = games.Where(g => g.IsValid(cubes)).Select(g => g.Id).Sum();
Console.WriteLine($"Part 1: {part1}");

int part2 = games.Select(g => g.Power()).Sum();
Console.WriteLine($"Part 2: {part2}");

internal class Game(int id, List<Throw> throws)
{
    public int Id { get; } = id;
    public List<Throw> Throws { get; } = throws;

    public bool IsValid(Dictionary<string, int> cubes) => Throws.TrueForAll(t => t.IsValid(cubes));

    public int Power()
    {
        Dictionary<string, int> max = new();
        foreach ((string color, int count) in Throws.SelectMany(t => t.Cubes))
        {
            if (!max.TryGetValue(color, out int highest) || highest < count)
                max[color] = count;
        }

        return max.Values.Aggregate(1, (a, b) => a * b);
    }
}
    

internal class Throw
{
    public readonly Dictionary<string, int> Cubes = new();
    
    public Throw(string raw)
    {
        foreach (string cubeList in raw.Split(", "))
        {
            string[] split = cubeList.Split(" ");
            Cubes.Add(split[1], int.Parse(split[0]));
        }
    }

    public bool IsValid(Dictionary<string, int> cubes)
    {
        foreach ((string color, int seen) in Cubes)
        {
            if (!cubes.TryGetValue(color, out int count))
                return false;
            if (count < seen)
                return false;
        }

        return true;
    }
}
