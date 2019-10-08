using System;
using System.Collections.Generic;
using System.Linq;

namespace Day11 {
	class Item : IComparable {
		public string element { get; private set; }
		public string type { get; private set; }

		public Item(string item) {
			this.element = item.Substring(0, item.Length-1);
			this.type = item.Substring(item.Length-1);
		}

		public int CompareTo(object obj) {
			if (obj == null) return 1;
			Item to = obj as Item;
			return this.element.CompareTo(to.element);
		}

		public override string ToString() {
			return this.element + this.type;
		}
	}

	class Floor {
		// these will always be requested as ie HM and HG, but are split internally for validity checking
		private List<Item> _chips;
		private List<Item> _generators;

		public void Take(string i) {
			Item item = new Item(i);
			if (item.type == "M") {
				Item toRemove = this._chips.Single(r => r.element == item.element);
				this._chips.Remove(toRemove);
			} else if (item.type == "G") {
				Item toRemove = this._generators.Single(r => r.element == item.element);
				this._generators.Remove(toRemove);
			} else {
				throw new System.ArgumentException($"{item} is neither a microchip nor a generator");
			}
		}

		public void Add(string i) {
			Item item = new Item(i);
			if (item.type == "M") {
				this._chips.Add(item);
				this._chips.Sort();
			} else if (item.type == "G") {
				this._generators.Add(item);
				this._generators.Sort();
			} else {
				throw new System.ArgumentException($"{item} is neither a microchip nor a generator");
			}
		}

		private bool HasGenerator(string element) {
			foreach (Item generator in this._generators) {
				if (generator.element == element) {
					return true;
				}
			}
			return false;
		}

		public bool Valid() {
			foreach (Item chip in this._chips) {
				if (this.HasGenerator(chip.element)) {
					continue;
				}
				if (this._generators.Count > 0) {
					return false;
				}
			}
			return true;
		}

		public Floor(List<string> items) {
			this._generators = new List<Item>();
			this._chips = new List<Item>();
			foreach (string item in items) {
				if (!item.Equals("")) {
					this.Add(item);
				}
			}
		}

		public override string ToString() {
			string map = "";
			if (this._generators.Count > 0) {
				map += String.Join(" ", this._generators) + " ";
			}
			if (this._chips.Count > 0) {
				map += String.Join(" ", this._chips);
			}
			return map.Trim();
		}
	}

	class Facility {
		private Floor[] _floors;
		private List<string> _elevator;
		public int position {get; private set;}

		public void Move(int direction) {
			this.position += direction;
			Floor current = this._floors[this.position];
			foreach (string item in this._elevator) {
				current.Add(item);
			}
			this._floors[this.position] = current;
			this._elevator.Clear();
		}

		public void Load(string item) {
			if (this._elevator.Count >= 2) {
				throw new System.ArgumentException("elevator is full");
			}
			this._elevator.Add(item);
			this._floors[this.position].Take(item);
		}

		public bool Valid() {
			for (int i = 0; i < this._floors.Length; i++) {
				if (!this._floors[i].Valid()) {
					return false;
				}
			}
			return true;
		}

		public Floor Floor() {
			return this._floors[this.position];
		}

		public bool Done() {
			// this doesn't take the elevator into account, but shouldn't matter
			for (int i = 0; i < 3; i++) {
				if (!this._floors[i].ToString().Equals("")) {
					return false;
				}
			}
			return true;
		}

		public Facility Clone() {
			Floor[] floors = new Floor[4];
			for (int i = 0; i < 4; i++) {
				string[] items = this._floors[i].ToString().Split(" ");
				floors[i] = new Floor(new List<string>(items));
			}
			List<string> elevator = new List<string>(this._elevator.ToArray());

			return new Facility(floors, this.position, elevator);
		}

		public Facility(Floor[] floors, int position = 0, List<string> elevator = null) {
			this.position = position;
			this._floors = floors;
			if (elevator != null) {
				this._elevator = elevator;
			} else {
				this._elevator = new List<string>();
			}
		}

		public override string ToString() {
			string map = "";
			for (int i = this._floors.Length - 1; i >= 0; i--) {
				map += $"F{i+1} ";
				if (i == this.position) {
					map += "E ";
				} else {
					map += "| ";
				}
				map += $"{this._floors[i]}\n";
			}
			return map;
		}
	}

	public class Day11 {
		private static int Run(Facility facility) {
			Dictionary<string, bool> known = new Dictionary<string, bool>();
			known.Add(facility.ToString(), true);

			List<Facility> scenarios = new List<Facility>{facility};
			for (int i = 1; i < 2000; i++) {
				Console.WriteLine($"iteration {i} ({scenarios.Count} options)");
				List<Facility> next = new List<Facility>();
				foreach (Facility scenario in scenarios) {
					string[] items = scenario.Floor().ToString().Split(" ");
					List<List<int>> combinations = Combinations.GetCombinations(items.Length, 1, 2);
					foreach (List<int> combo in combinations) {
						Facility option = scenario.Clone();
						foreach (int item in combo) {
							option.Load(items[item]);
						}
						for (int dir = -1; dir < 2; dir += 2) {
							if (option.position + dir < 0 || option.position + dir > 3) {
								continue;
							}

							Facility moved = option.Clone();
							moved.Move(dir);

							if (known.ContainsKey(moved.ToString())) {
								continue;
							}
							if (moved.Done()) {
								return i;
							}
							if (moved.Valid()) {
								known.Add(moved.ToString(), true);
								next.Add(moved);
							}
						}
					}
				}
				scenarios = next;
			}
			return -1;
		}

		static void Main() {
			Floor[] floors = new Floor[4]{
				new Floor(new List<string>{ "PmG", "PmM" }),
				new Floor(new List<string>{ "CoG", "CuG", "RuG", "PuG" }),
				new Floor(new List<string>{ "CoM", "CuM", "RuM", "PuM" }),
				new Floor(new List<string>())
			};

			Facility facility = new Facility(floors);
			int steps = Run(facility);
			Console.WriteLine($"Part 1: {steps}");

			floors = new Floor[4]{
				new Floor(new List<string>{ "PmG", "PmM", "ElG", "ElM", "Li2G", "Li2M" }),
				new Floor(new List<string>{ "CoG", "CuG", "RuG", "PuG" }),
				new Floor(new List<string>{ "CoM", "CuM", "RuM", "PuM" }),
				new Floor(new List<string>())
			};

			facility = new Facility(floors);
			steps = Run(facility);
			Console.WriteLine($"Part 2: {steps}");
		}
	}
}
