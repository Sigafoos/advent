using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _02
{
    class Password
    {
        private int _first;
        private int _second;
        private char _character;
        private char[] _password;

        public Password(string input)
        {
            var regex = new Regex(@"^(\d+)-(\d+) (\w): (\w+)$");
            Match match = regex.Match(input); // semantic satiation on "match"
            // ToString is lazy
            _first = Int32.Parse(match.Groups[1].ToString());
            _second = Int32.Parse(match.Groups[2].ToString());
            _character = char.Parse(match.Groups[3].ToString());
            _password = match.Groups[4].ToString().ToCharArray();
        }

        public bool ValidatesPart1()
        {
            Dictionary<char, int> processed = _password
                .GroupBy(item => item)
                .ToDictionary(item => item.Key, item => item.Count());

            var exists = processed.TryGetValue(_character, out int occurrences);

            return exists && occurrences >= _first && occurrences <= _second;
        }

        public bool ValidatesPart2()
        {
            char first = _password[_first-1];
            char second = _password[_second-1];

            return (first != second) && (first == _character || second == _character);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var entries = new List<Password>();
            using (StreamReader reader = File.OpenText("../../input/02.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    entries.Add(new Password(line));
                }
            }

            int part1 = entries.Where(p => p.ValidatesPart1()).Count();
            Console.WriteLine($"Part 1: {part1}");
            int part2 = entries.Where(p => p.ValidatesPart2()).Count();
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}
