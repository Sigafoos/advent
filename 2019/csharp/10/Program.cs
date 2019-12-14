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

            var best = new Asteroid(0, 0);
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

                if (current.Value.Seen() > best.Seen()) {
                    best = current.Value;
                }
            }
            Console.WriteLine($"Part 1: {best.Seen()}");

            best.Sort();
            while (best.Destroyed.Count < 200 && best.Seen() > 0) {
                best.Sweep();
            }
            var final = best.Destroyed[best.Destroyed.Count-1];
            Console.WriteLine($"Part 2: {final.X * 100 + final.Y}");
        }
    }

    class Asteroid {
        public int X {get;}
        public int Y {get;}
        protected Dictionary<double, Queue<Asteroid>> tangents;
        public List<Asteroid> Destroyed {get; private set;}
        protected Queue<double> radar;

        public Asteroid(int x, int y) {
            this.X = x;
            this.Y = y;
            this.tangents = new Dictionary<double, Queue<Asteroid>>();
            this.Destroyed = new List<Asteroid>();
            this.radar = new Queue<double>();
        }

        public void Inspect(Asteroid asteroid) {
            if (this.X == asteroid.X && this.Y == asteroid.Y) {
                return;
            }
            int x = asteroid.X - this.X;
            int y = asteroid.Y - this.Y;

            var rads = Math.Atan2(y, x);
            if (!this.tangents.ContainsKey(rads)) {
                var queue = new Queue<Asteroid>();
                queue.Enqueue(asteroid);
                this.tangents.Add(rads, queue);
            }
            else {
                this.tangents[rads].Enqueue(asteroid);
            }
        }

        public int Seen() {
            return this.tangents.Count;
        }

        public void Sweep() {
            if (this.radar.Count == 0) {
                var keys = new double[this.tangents.Count];
                this.tangents.Keys.CopyTo(keys, 0);
                Array.Sort(keys);
                this.radar = new Queue<double>(keys);

                // I don't like this bit
                while (this.radar.Peek() < Math.Atan2(-1, 0)) {
                    this.radar.Enqueue(this.radar.Dequeue());
                }
            }
            var tangent = this.radar.Dequeue();
            this.Destroyed.Add(this.tangents[tangent].Dequeue());
            if (this.tangents[tangent].Count == 0) {
                this.tangents.Remove(tangent);
            }
        }

        public void Sort() {
            var tangents = new Dictionary<double, Queue<Asteroid>>();
            foreach (double key in this.tangents.Keys) {
                var queue = this.tangents[key].ToArray();
                Array.Sort(queue, new AsteroidComparer(this));
                tangents.Add(key, new Queue<Asteroid>(queue));
            }
            this.tangents = tangents;
        }

        public override string ToString() {
            return $"{this.X},{this.Y}";
        }
    }

    class AsteroidComparer : IComparer<Asteroid> {
        protected Asteroid asteroid;

        public AsteroidComparer(Asteroid asteroid) {
            this.asteroid = asteroid;
        }

        public int Compare(Asteroid a, Asteroid b) {
            return (Math.Abs(a.X - this.asteroid.X) + Math.Abs(a.Y - this.asteroid.Y)) - (Math.Abs(b.X - this.asteroid.X) + Math.Abs(b.Y - this.asteroid.Y));
        }
    }
}
