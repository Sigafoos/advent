using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

string input = File.ReadAllText("../../../../../input/06.txt");
List<Group> groups = input.Split("\n\n").Select(g => new Group(g)).ToList();

Console.WriteLine($"Part 1: {groups.Select(g => g.CountDistinct()).Sum()}");
Console.WriteLine($"Part 2: {groups.Select(g => g.CountConsensus()).Sum()}"); // 3244 too low

internal class Group
{
    public List<char[]> Answers { get; private set; }

    public Group(string responses)
    {
        Answers = responses.Trim().Split('\n').Select(r => r.ToCharArray()).ToList();
    }

    public int CountDistinct()
    {
        return Answers
            .SelectMany(a => a)
            .Distinct()
            .Count();
    }

   public int CountConsensus()
   {
        return Answers
            .Aggregate(Answers.First(), (current, next) => current.Intersect(next).ToArray())
            .Length;
   }
}