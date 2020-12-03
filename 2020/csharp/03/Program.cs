using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _03
{
    class Map
    {
        private int _x = 0;
        private int _y = 0;
        private List<Row> _rows = new List<Row>();

        public void Add(Row row)
        {
            _rows.Add(row);
        }

        public int Traverse(int x, int y)
        {
            int trees = 0;
            while (TryMove(x, y))
                if (IsTree())
                    trees++;
            return trees;
        }

        public bool TryMove(int x, int y)
        {
            if (_y + y >= _rows.Count)
                return false;
            _x += x;
            _y += y;
            return true;
        }

        public bool IsTree()
        {
            return _rows[_y].At(_x) == '#';
        }

        public void Reset()
        {
            _x = 0;
            _y = 0;
        }
    }

    class Row
    {
        private string _text;

        public Row(string text)
        {
            _text = text;
        }

        public char At(int position)
        {
            return _text[position % _text.Length];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var map = new Map();
            using (StreamReader reader = File.OpenText("../../input/03.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    map.Add(new Row(line));
                }
            }

            List<(int, int)> routes = new List<(int, int)>{
                (1, 1),
                (3, 1),
                (5, 1),
                (7, 1),
                (1, 2)
            };
            Dictionary<(int, int), int> attempts = new Dictionary<(int, int), int>();

            foreach ((int, int) route in routes)
            {
                var trees = map.Traverse(route.Item1, route.Item2);
                attempts.Add(route, trees);
                map.Reset();
            }

            var product = attempts.Values.Aggregate((Int64)1, (total, attempt) => total * attempt);
            Console.WriteLine($"Part 1: {attempts[(3, 1)]}");
            Console.WriteLine($"Part 2: {product}");
        }
    }
}
