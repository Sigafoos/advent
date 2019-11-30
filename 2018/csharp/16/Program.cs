using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace _16
{
	class Program
	{
		static void Main(string[] args)
		{
			try {
				var file = System.IO.File.ReadAllLines(@"../../input/16-sample.txt");

				var part1 = 0;
				int[] before = new int[4];
				Device after = new Device(0);
				int[] instructions = new int[4];
				Regex re = new Regex(@"(\d{1,2}),? (\d{1,2}),? (\d{1,2}),? (\d{1,2})");
				var values = new Dictionary<string, OpCode>();
				values.Add("addr", new OpCode("addr"));
				values.Add("addi", new OpCode("addi"));
				values.Add("mulr", new OpCode("mulr"));
				values.Add("muli", new OpCode("muli"));
				values.Add("banr", new OpCode("banr"));
				values.Add("bani", new OpCode("bani"));
				values.Add("borr", new OpCode("borr"));
				values.Add("bori", new OpCode("bori"));
				values.Add("setr", new OpCode("setr"));
				values.Add("seti", new OpCode("seti"));
				values.Add("gtir", new OpCode("gtir"));
				values.Add("gtri", new OpCode("gtri"));
				values.Add("gtrr", new OpCode("gtrr"));
				values.Add("eqir", new OpCode("eqir"));
				values.Add("eqri", new OpCode("eqri"));
				values.Add("eqrr", new OpCode("eqrr"));

				foreach (string line in file) {
					var match = re.Match(line).Groups;
					int[] opcodes;
					if (match.Count == 5) {
						opcodes = new int[4]{ Int32.Parse(match[1].Value), Int32.Parse(match[2].Value), Int32.Parse(match[3].Value), Int32.Parse(match[4].Value) };
					} else {
						var possibilities = 0;

						var device = new Device(before);
						device.Addr(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["addr"].CanBe(instructions[0]);
						} else {
							values["addr"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Addi(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["addi"].CanBe(instructions[0]);
						} else {
							values["addi"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Mulr(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["mulr"].CanBe(instructions[0]);
						} else {
							values["mulr"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Muli(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["muli"].CanBe(instructions[0]);
						} else {
							values["muli"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Banr(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["banr"].CanBe(instructions[0]);
						} else {
							values["banr"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Bani(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["bani"].CanBe(instructions[0]);
						} else {
							values["bani"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Borr(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["borr"].CanBe(instructions[0]);
						} else {
							values["borr"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Bori(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["bori"].CanBe(instructions[0]);
						} else {
							values["bori"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Setr(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["setr"].CanBe(instructions[0]);
						} else {
							values["setr"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Seti(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["seti"].CanBe(instructions[0]);
						} else {
							values["seti"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Gtir(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["gtir"].CanBe(instructions[0]);
						} else {
							values["gtir"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Gtri(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["gtri"].CanBe(instructions[0]);
						} else {
							values["gtri"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Gtrr(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["gtrr"].CanBe(instructions[0]);
						} else {
							values["gtrr"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Eqir(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["eqir"].CanBe(instructions[0]);
						} else {
							values["eqir"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Eqri(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["eqri"].CanBe(instructions[0]);
						} else {
							values["eqri"].CannotBe(instructions[0]);
						}
						device = new Device(before);
						device.Eqrr(instructions[1], instructions[2], instructions[3]);
						if (device.Is(after.Registers())) {
							possibilities++;
							values["eqrr"].CanBe(instructions[0]);
						} else {
							values["eqrr"].CannotBe(instructions[0]);
						}

						before = new int[4];
						after = new Device(0);
						if (possibilities > 2) part1++;

						if (possibilities == 0) {
							Console.WriteLine($"ERROR!\nBefore: [{string.Join(" ", before)}]\n{string.Join(" ", instructions)}\nAfter: {after}");
						}
						continue;
					}

					if (line.StartsWith("Before")) {
						before = opcodes;
					} else if (line.StartsWith("After")) {
						after = new Device(opcodes);
					} else {
						instructions = opcodes;
					}
				}

				Console.WriteLine($"Part 1: {part1}");

				Console.WriteLine("\n       0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15");
				foreach (KeyValuePair<string, OpCode> value in values) {
					Console.WriteLine(value.Value);
				}
			}
			catch (System.IO.FileNotFoundException) {
				Console.WriteLine("file not found!");
				System.Environment.Exit(1);
			}
		}
	}

	class Device {
		protected int[] registers;

		public Device(int registers) {
			this.registers = new int[registers];
		}

		public Device(int[] registers) {
			this.registers = new int[registers.Length];
			for (int i = 0; i < registers.Length; i++) {
				this.registers[i] = registers[i];
			}
		}

		public bool Is(int[] b) {
			if (this.registers.Length != b.Length) {
				return false;
			}

			for (int i = 0; i < this.registers.Length; i++) {
				if (this.registers[i] != b[i]) {
					return false;
				}
			}
			return true;
		}

		public void Addr(int a, int b, int c) {
			this.registers[c] = this.registers[a] + this.registers[b];
		}

		public void Addi(int a, int b, int c) {
			this.registers[c] = this.registers[a] + b;
		}

		public void Mulr(int a, int b, int c) {
			this.registers[c] = this.registers[a] * this.registers[b];
		}

		public void Muli(int a, int b, int c) {
			this.registers[c] = this.registers[a] * b;
		}

		public void Banr(int a, int b, int c) {
			this.registers[c] = this.registers[a] & this.registers[b];
		}

		public void Bani(int a, int b, int c) {
			this.registers[c] = this.registers[a] & b;
		}

		public void Borr(int a, int b, int c) {
			this.registers[c] = this.registers[a] | this.registers[b];
		}

		public void Bori(int a, int b, int c) {
			this.registers[c] = this.registers[a] | b;
		}

		public void Setr(int a, int b, int c) {
			this.registers[c] = this.registers[a];
		}

		public void Seti(int a, int b, int c) {
			this.registers[c] = a;
		}

		public void Gtir(int a, int b, int c) {
			this.registers[c] = (a > this.registers[b]) ? 1 : 0;
		}

		public void Gtri(int a, int b, int c) {
			this.registers[c] = (this.registers[a] > b) ? 1 : 0;
		}

		public void Gtrr(int a, int b, int c) {
			this.registers[c] = (this.registers[a] > this.registers[b]) ? 1 : 0;
		}

		public void Eqir(int a, int b, int c) {
			this.registers[c] = (a == this.registers[b]) ? 1 : 0;
		}

		public void Eqri(int a, int b, int c) {
			this.registers[c] = (this.registers[a] == b) ? 1 : 0;
		}

		public void Eqrr(int a, int b, int c) {
			this.registers[c] = (this.registers[a] == this.registers[b]) ? 1 : 0;
		}

		public int[] Registers() {
			return this.registers;
		}

		public override string ToString() {
			return $"[{string.Join(" ", this.registers)}]";
		}
	}

	class OpCode {
		public string Name;

		private Dictionary<int, bool> can;

		private Dictionary<int, bool> cannot;

		public OpCode(string name) {
			this.Name = name;
			this.can = new Dictionary<int, bool>();
			this.cannot = new Dictionary<int, bool>();
		}

		public void CanBe(int i) {
			if (this.can.ContainsKey(i)) return;

			if (!this.cannot.ContainsKey(i)) {
				this.can.Add(i, true);
			}
		}

		public void CannotBe(int i) {
			if (this.cannot.ContainsKey(i)) return;

			this.cannot.Add(i, true);
			if (this.can.ContainsKey(i)) {
				this.can.Remove(i);
			}
		}

		public override string ToString() {
			var str = $"{this.Name}: ";
			for (int i = 0; i < 16; i++) {
				str += " ";
				if (this.can.ContainsKey(i)) {
					str += "X";
				} else if (!this.cannot.ContainsKey(i)) {
					str += "?";
				} else {
					str += " ";
				}
				str += " ";
			}
			return str;
		}
	}
}
