using System;
using System.Collections.Generic;
using System.IO;

namespace _01
{
    class Program
    {
        public static List<int> Example = new List<int>{
            1721,
            979,
            366,
            299,
            675,
            1456
        };

        static void Main(string[] args)
        {
            var entries = new List<int>();
            using (StreamReader reader = File.OpenText("../../input/01.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    entries.Add(Int32.Parse(line));
                }
            }

            int part1 = Part1(entries);
            Console.WriteLine($"Part 1: {part1}");
            int part2 = Part2(entries);
            Console.WriteLine($"Part 2: {part2}");
        }

        public static int Part1(List<int> entries)
        {
            for (int i = 0; i < entries.Count - 1; i++)
            {
                for (int j = i + 1; j < entries.Count; j++)
                {
                    if (entries[i] + entries[j] == 2020)
                        return entries[i]*entries[j];
                }
            }
            throw new ArgumentException("somehow it never worked");
        }

        public static int Part2(List<int> entries)
        {
            for (int i = 0; i < entries.Count - 2; i++)
            {
                for (int j = i + 1; j < entries.Count - 1; j++)
                {
                    for (int k = j + 1; k < entries.Count; k++)
                    {
                        if (entries[i] + entries[j] + entries[k] == 2020)
                            return entries[i]*entries[j]*entries[k];
                    }
                }
            }
            throw new ArgumentException("somehow it never worked");
        }

    }
}
