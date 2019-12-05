using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace _04
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("you need to pass in your puzzle input; ie '1234-5678'");
                System.Environment.Exit(1);
            }

            var range = args[0].Split("-");
            var matches = new List<int>();
            var matchesTwo = new List<int>();

            // The instructions don't say if it's inclusive or exclusive, but it doesn't seem to matter.
            for (int i = Int32.Parse(range[0]); i < Int32.Parse(range[1]); i++) {
                if (Valid(i, false)) {
                    matches.Add(i);
                }
                if (Valid(i, true)) {
                    matchesTwo.Add(i);
                }
            }

            Console.WriteLine($"Part 1: {matches.Count}");
            Console.WriteLine($"Part 2: {matchesTwo.Count}");
        }

        static bool Valid(int i, bool two) {
            var password = i.ToString();

            // There should be a way to do this in one go with regex. I couldn't make it work.
            if (two) {
                password = new Regex(@"(.)\1{2,}").Replace(password, "");
            }

            if (!new Regex(@"(.)\1").IsMatch(password)) {
                return false;
            }

            // all passwords are six digits
            for (int j = 0; j < 5; j++) {
                var current = (i % (int)Math.Pow(10, 6-j)) / (int)Math.Pow(10, 5-j);
                var next = (i % (int)Math.Pow(10, 5-j)) / (int)Math.Pow(10, 4-j);
                if (current > next) {
                    return false;
                }
            }

            return true;
        }
    }
}
