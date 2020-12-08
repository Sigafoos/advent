using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _06
{
    class Program
    {
        class Group
        {
            public IEnumerable<char[]> Answers { get; private set; }

            public Group(string responses)
            {
                Answers = responses.Split('\n').Select(r => r.ToCharArray());
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
                    .Count();
            }
        }

        static void Main(string[] args)
        {
            var groups = new List<Group>();
            string input = File.ReadAllText("../../input/06.txt");
            foreach (string g in input.Split("\n\n"))
            {
                groups.Add(new Group(g));
            }

            Console.WriteLine($"Part 1: {groups.Select(g => g.CountDistinct()).Sum()}");
            Console.WriteLine($"Part 2: {groups.Select(g => g.CountConsensus()).Sum()}"); // 3244 too low
        }
    }
}
