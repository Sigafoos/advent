using System;
using System.Collections.Generic;

public class Combinations {
	public static List<List<int>> GetCombinations(int items, int min, int max) {
		List<List<int>> combinations = new List<List<int>>();

		for (int i = 0; i < Math.Pow(2.0, (double)items); i++) {
			int count = BitCount(i);
			if (count < min || count > max) {
				continue;
			}
			List<int> c = new List<int>();
			for (int j = 0; j < items; j++) {
				int pow = (int)Math.Pow(2, j);
				if ((i & pow) == pow) {
					c.Add(j);
				}
			}
			combinations.Add(c);
		}
		return combinations;
	}

	private static int BitCount(int n) {
		int count = 0;
		while (n != 0) {
			count++;
			n &= (n - 1);
		}
		return count;
	}
}
