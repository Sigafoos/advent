using System;
using System.Collections.Generic;

namespace _06
{
    class Program
    {
        static void Main(string[] args)
        {
            var orbits = new Dictionary<string, Object>();
            try {
                foreach (string line in System.IO.File.ReadAllLines(@"../../input/06.txt")) {
                    var parsed = line.Split(")");
                    Object child;
                    Object parent;
                    if (!orbits.TryGetValue(parsed[0], out parent)) {
                        parent = new Object(parsed[0]);
                        orbits.Add(parsed[0], parent);
                    }
                    if (orbits.TryGetValue(parsed[1], out child)) {
                        child.parent = parent;
                        orbits[parsed[1]] = child;
                    }
                    else {
                        child = new Object(parsed[1], parent);
                        orbits.Add(parsed[1], child);
                    }
                }
            }
            catch (System.IO.FileNotFoundException) {
                Console.WriteLine("file not found");
                System.Environment.Exit(1);
            }

            var count = 0;
            foreach(KeyValuePair<string, Object> orbit in orbits) {
                count += orbit.Value.Orbits();
            }
            Console.WriteLine($"Part 1: {count}");

            // using a dictionary of value: key instead of a list of key: value because of all the lookups
            var youMap = new Dictionary<string, int>();
            var sanMap = new Dictionary<string, int>();
            var meeting = "";
            var you = orbits["YOU"].parent;
            var san = orbits["SAN"].parent;
            var i = 0;

            while (meeting == "") {
                if (!youMap.ContainsKey(you.ID)) {
                    youMap.Add(you.ID, i);
                }
                if (!sanMap.ContainsKey(san.ID)) {
                    sanMap.Add(san.ID, i);
                }
                i++;

                if (youMap.ContainsKey(san.ID)) {
                    meeting = san.ID;
                }
                else if (sanMap.ContainsKey(you.ID)) {
                    meeting = you.ID;
                }

                if (you.parent != null) {
                    you = you.parent;
                }
                if (san.parent != null) {
                    san = san.parent;
                }
            }

            Console.WriteLine($"Part 2: {youMap[meeting] + sanMap[meeting]}");
        }
    }

    class Object {
        public string ID {get;}
        public Object parent {get; set;}
        protected int orbits;

        public Object(string id) {
            this.ID = id;
            this.orbits = -1;
        }

        public Object(string id, Object parent) {
            this.ID = id;
            this.parent = parent;
            this.orbits = -1;
        }

        public int Orbits() {
            if (this.orbits < 0) {
                if (this.parent == null) {
                    this.orbits = 0;
                }
                else {
                    this.orbits = this.parent.Orbits() + 1;
                }
            }
            return this.orbits;
        }
    }
}
