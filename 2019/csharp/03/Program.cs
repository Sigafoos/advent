using System;
using System.Collections.Generic;
using System.Linq;

namespace _03
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                var input = System.IO.File.ReadAllLines(@"../../input/03.txt");
                var one = new Wire(input[0]);
                var two = new Wire(input[1]);

                var intersections = one.coordinates.Intersect(two.coordinates, new CoordinateEqualityComparer());
                var distance = 99999;
                var steps = 99999;
                foreach (Coordinate intersection in intersections) {
                    var manhattan = intersection.Manhattan();
                    if (manhattan < distance) {
                        distance = manhattan;
                    }

                    // Intersection is a pointer to the value in one, which isn't present in two. So we
                    // need to get a bit more creative.
                    var localSteps = one.coordinates.IndexOf(intersection);
                    for (int i = 0; !(two.coordinates[i].X == intersection.X && two.coordinates[i].Y == intersection.Y); i++) {
                        localSteps++;
                    }
                    if (localSteps < steps) {
                        steps = localSteps;
                    }
                }

                Console.WriteLine($"Part 1: {distance}");
                Console.WriteLine($"Part 2: {steps}");
            }
            catch (System.IO.FileNotFoundException) {
                Console.WriteLine("file not found");
                System.Environment.Exit(1);
            }
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

        public override string ToString() {
            return $"({this.X}, {this.Y})";
        }
    }

    // sheeeeeeeee-it
    class Wire {
        public List<Coordinate> coordinates {get;}

        public Wire(string input) {
            var current = new Coordinate(0, 0);
            this.coordinates = new List<Coordinate>();
            this.coordinates.Add(current);

            foreach (string command in input.Split(",")) {
                var length = Int32.Parse(command.Substring(1));
                switch (command.Substring(0, 1)) {
                    case "L":
                        for (int i = 0; i < length; i++) {
                            this.coordinates.Add(new Coordinate(--current.X, current.Y));
                        }
                        break;

                    case "R":
                        for (int i = 0; i < length; i++) {
                            this.coordinates.Add(new Coordinate(++current.X, current.Y));
                        }
                        break;

                    case "U":
                        for (int i = 0; i < length; i++) {
                            this.coordinates.Add(new Coordinate(current.X, ++current.Y));
                        }
                        break;

                    case "D":
                        for (int i = 0; i < length; i++) {
                            this.coordinates.Add(new Coordinate(current.X, --current.Y));
                        }
                        break;

                    default:
                        throw new System.ArgumentException($"I don't know how to parse {command}");
                }
            }
        }
    }

    class CoordinateEqualityComparer : IEqualityComparer<Coordinate> {
        public bool Equals(Coordinate a, Coordinate b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public int GetHashCode(Coordinate c)
        {
            return c.ToString().GetHashCode();
        }
    }
}
