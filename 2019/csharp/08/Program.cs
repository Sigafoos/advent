using System;
using System.Collections.Generic;
using System.Linq;

namespace _08
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "";
            try {
                input = System.IO.File.ReadAllText(@"../../input/08.txt");
            }
            catch (System.IO.FileNotFoundException) {
                Console.WriteLine("file not found");
                System.Environment.Exit(1);
            }
            var image = new Image(25, 6, input.Trim());
            Console.WriteLine($"Part 1: {image.Checksum()}");
            Console.WriteLine(image);
        }
    }

    class Image {
        protected int width;
        protected int height;
        protected List<List<int>> data;

        public Image(int width, int height, string data) {
            this.width = width;
            this.height = height;
            this.data = new List<List<int>>();

            var layer = new List<int>();
            for (int i = 0; i < data.Length; i++) {
                layer.Add(Int32.Parse(data[i].ToString()));
                if (layer.Count == width * height) {
                    this.data.Add(layer);
                    layer = new List<int>();
                }
            }
        }

        public int Checksum() {
            var best = new List<int>();
            foreach (var current in this.data) {
                if (best.Count == 0 || current.FindAll(i => i == 0).Count < best.FindAll(i => i == 0).Count) {
                    best = current;
                }
            }
            return best.FindAll(i => i == 1).Count * best.FindAll(i => i == 2).Count;
        }

        public override string ToString() {
            var raw = "";
            for (var pos = 0; pos < this.width * this.height; pos++) {
                for (var i = 0; i < this.data.Count; i++) {
                    if (this.data[i][pos] == 0) {
                        raw += " ";
                        break;
                    } else if (this.data[i][pos] == 1) {
                        raw += "X";
                        break;
                    }
                }
            }

            var data = "";
            for (int i = 0; i < raw.Length; i++) {
                data += raw[i].ToString();
                if ((i+1) % this.width == 0) {
                    data += "\n";
                }
            }
            return data;
        }
    }
}
