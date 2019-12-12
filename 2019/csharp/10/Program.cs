using System;
using System.Collections.Generic;

namespace _10
{
    class Program
    {
        static void Main(string[] args)
        {
            var belt = new Dictionary<string, Asteroid>();
            var width = 0;
            var height = 0;
            try {
                var y = 0;
                var lines = System.IO.File.ReadAllLines(@"../../input/10.txt");
                width = lines[0].Length;
                height = lines.Length;
                foreach (string line in lines) {
                    for (int x = 0; x < line.Length; x++) {
                        if (line[x] == '#') {
                            belt.Add($"{x},{y}", new Asteroid(x, y));
                        }
                    }
                    y++;
                }
            }
            catch (System.IO.FileNotFoundException) {
                Console.WriteLine("file not found");
                System.Environment.Exit(1);
            }

            var best = 0;
            foreach (KeyValuePair<string, Asteroid> current in belt) {
                // "northwest"
                for (int x = current.Value.X-1; x >= 0; x--) {
                    for (int y = current.Value.Y; y >= 0; y--) {
                        var key = $"{x},{y}";
                        if (belt.ContainsKey(key)) {
                            current.Value.Inspect(new Asteroid(x, y));
                        }
                    }
                }

                // "northeast"
                for (int x = current.Value.X; x < width; x++) {
                    for (int y = current.Value.Y-1; y >= 0; y--) {
                        var key = $"{x},{y}";
                        if (belt.ContainsKey(key)) {
                            current.Value.Inspect(new Asteroid(x, y));
                        }
                    }
                }

                // "southeast
                for (int x = current.Value.X+1; x < width; x++) {
                    for (int y = current.Value.Y; y < height; y++) {
                        var key = $"{x},{y}";
                        if (belt.ContainsKey(key)) {
                            current.Value.Inspect(new Asteroid(x, y));
                        }
                    }
                }

                // "southwest"
                for (int x = current.Value.X; x >= 0; x--) {
                    for (int y = current.Value.Y+1; y < height; y++) {
                        var key = $"{x},{y}";
                        if (belt.ContainsKey(key)) {
                            current.Value.Inspect(new Asteroid(x, y));
                        }
                    }
                }

                var saw = current.Value.Seen();
                if (saw > best) {
                    best = saw;
                }
            }
            Console.WriteLine($"Part 1: {best}");
        }
    }

    class Asteroid {
        public int X {get;}
        public int Y {get;}
        protected List<double> tangents;

        public Asteroid(int x, int y) {
            this.X = x;
            this.Y = y;
            this.tangents = new List<double>();
        }

        public void Inspect(Asteroid asteroid) {
            if (this.X == asteroid.X && this.Y == asteroid.Y) {
                return;
            }
            int x = asteroid.X - this.X;
            int y = asteroid.Y - this.Y;

            var rads = Math.Atan2(y, x);
            if (!this.tangents.Contains(rads)) {
                this.tangents.Add(rads);
            }
        }

        public int Seen() {
            return this.tangents.Count;
        }

        public override string ToString() {
            return $"{this.X},{this.Y}";
        }
    }
}
