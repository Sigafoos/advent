using System;

namespace _02
{
	class Program
	{
		static void Main(string[] args)
		{
			string input = "";
			try {
				input = System.IO.File.ReadAllText(@"../../input/02.txt");
			}
			catch (System.IO.FileNotFoundException) {
				Console.WriteLine("file not found");
				System.Environment.Exit(1);
			}
			var raw = input.Split(",");
			var commands = new int[raw.Length];
			for (int i = 0; i < raw.Length; i++) {
				commands[i] = Int32.Parse(raw[i]);
			}

			var intcode = new Intcode(commands, 12, 2);
			intcode.Run();
			Console.WriteLine($"Part 1: {intcode.At(0)}");

			for (int noun = 0; noun < 100; noun++) {
				for (int verb = 0; verb < 100; verb++) {
					intcode = new Intcode(commands, noun, verb);
					try {
						intcode.Run();
					}
					catch (System.IndexOutOfRangeException) {
						continue;
					}

					if (intcode.At(0) == 19690720) {
						var correctNoun = noun.ToString();
						if (correctNoun.Length == 1) correctNoun = "0" + correctNoun;
						var correctVerb = verb.ToString();
						if (correctVerb.Length == 1) correctVerb = "0" + correctVerb;
						Console.WriteLine($"Part 2: {correctNoun}{correctVerb}");
						return;
					}
				}
			}
		}
	}

	class Intcode {
		protected int[] commands;
		protected int position;

		public Intcode(int[] commands) {
			this.commands = commands;
			this.position = 0;
		}

		public Intcode(int[] commands, int noun, int verb) {
			// Perhaps there's a better way of deep-copying an array in C#.
			// I should look into that.
			this.commands = new int[commands.Length];
			for (int i = 0; i < commands.Length; i++) {
				this.commands[i] = commands[i];
			}
			this.commands[1] = noun;
			this.commands[2] = verb;
			this.position = 0;
		}

		public void Run() {
			while (true) {
				var command = this.commands[this.position++];
				int a;
				int b;
				int c;

				switch (command) {
					case 1:
						a = this.commands[this.position++];
						b = this.commands[this.position++];
						c = this.commands[this.position++];
						this.commands[c] = this.commands[a] + this.commands[b];
						break;

					case 2:
						a = this.commands[this.position++];
						b = this.commands[this.position++];
						c = this.commands[this.position++];
						this.commands[c] = this.commands[a] * this.commands[b];
						break;

					case 99:
						return;
				}
			}
		}

		public int At(int i) {
			return this.commands[i];
		}
	}
}
