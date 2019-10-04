class Day01 {
	static void Main() {
		string instructions = System.IO.File.ReadAllText(@"../input/input1.txt");
		int floor = 0;
		int pos = 1;
		int basement = -1;
		foreach (char dir in instructions) {
			if (dir == '(') {
				floor++;
			} else if (dir == ')') {
				floor--;
			} else {
				throw new System.Exception("how did this even happen");
			}
			if (basement == -1 && floor < 0) {
				basement = pos;
			} else {
				pos++;
			}
		}
		System.Console.WriteLine("Part 1: {0}", floor);
		System.Console.WriteLine("Part 2: {0}", basement);
	}
}
