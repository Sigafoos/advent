using System;

namespace _01
{
	class Program
	{
		static int fuel(int mass) {
			return (int)Math.Truncate((double)mass / 3.0)-2;
		}

		static int fuel_second_check(int mass) {
			int total = 0;
			for (int required = fuel(mass); required > 0; required = fuel(mass)) {
				total += required;
				mass = required;
			}
			return total;
		}

		static void Main(string[] args)
		{
			try {
				var part1 = 0;
				var part2 = 0;
				foreach (string line in System.IO.File.ReadAllLines(@"../../input/01.txt")) {
					var weight = Int32.Parse(line);
					part1 += fuel(weight);
					part2 += fuel_second_check(weight);
				}
				Console.WriteLine($"Part 1: {part1}");
				Console.WriteLine($"Part 2: {part2}");
			}
			catch (System.IO.FileNotFoundException) {
				Console.WriteLine("file not found");
				System.Environment.Exit(1);
			}
		}
	}
}
