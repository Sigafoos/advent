using System;
using System.Collections.Generic;

namespace _03
{
	class Program
	{
		static void Main(string[] args)
		{
			var wire1 = new List<Coordinate>();
			var wire2 = new List<Coordinate>();
			try {
				var input = System.IO.File.ReadAllLines(@"../../input/03.txt");
				var current = new Coordinate(0, 0);
				// hi I'm here
				/*
				foreach (string command in input[0].Split(",")) {
					wire1.Add(command);
				}
				foreach (string command in input[1].Split(",")) {
					wire2.Add(command);
				}
				*/
			}
			catch (System.IO.FileNotFoundException) {
				Console.WriteLine("file not found");
				System.Environment.Exit(1);
			}

			var p = new Coordinate(3, 3);

			Console.WriteLine(p.Manhattan());
		}
	}

	class Coordinate {
		public int X { get; set; }
		public int Y { get; set; }

		public Coordinate(int x, int y) {
			this.X = x;
			this.Y = y;
		}

		public int Manhattan() {
			return Math.Abs(this.X) + Math.Abs(this.Y);
		}
	}
}
