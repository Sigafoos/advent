using System.Text;
using System.Xml.Linq;
using Advent.Common;

List<List<int>> dupes = await FileLoader<List<int>>.Parse("input.txt", FindDuplicates);
int part1 = dupes.SelectMany(x => x).Sum();
Console.WriteLine($"Part 1: {part1}");

List<byte[]> sacks = await FileLoader<byte[]>.Parse("input.txt", Encoding.ASCII.GetBytes);
List<int> part2 = new();
for (int i = 0; i < sacks.Count; i += 3)
{
    part2.Add(sacks[i].Intersect(sacks[i+1]).Intersect(sacks[i+2]).Single().ToPriority());
}
// 9383 too high
Console.WriteLine($"Part 2: {part2.Sum()}");

List<int> FindDuplicates(string input)
{
    byte[] bytes = Encoding.ASCII.GetBytes(input);
    int rucksackSize = bytes.Length / 2;
    List<byte[]?> split = new() { bytes[..rucksackSize], bytes[rucksackSize..] };

    return split[0].Intersect(split[1]).Select(d => d.ToPriority()).ToList();
}

static class Helpers
{
    public static int ToPriority(this byte letter) => letter < 91 ? letter - 38 : letter - 96;
}