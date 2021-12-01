using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.Common
{
    public class Grid<T>
    {
        public int RowCount => _rows.Count;
        public int ColumnCount => _rows.FirstOrDefault(r => r.Key == 0).Value.Count;
        private readonly Dictionary<int, Row<T>> _rows;

        public static async Task<Grid<T>> FromFile(string filename, Row<T>.ProcessFunc processFunc)
        {
            Dictionary<int, Row<T>> rows = new();
            using StreamReader reader = File.OpenText(filename);
            string? line;
            int i = 0;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                rows[i] = new Row<T>(line, processFunc);
                i++;
            }

            return new Grid<T>(rows);
        }

        private Grid(Dictionary<int, Row<T>> rows)
        {
            _rows = rows;
        }
    }

    public class Row<T>
    {
        public int Count => _entries.Count;
        private readonly Dictionary<int, T> _entries = new();

        public delegate T ProcessFunc(char c);

        public List<T> NeighborsOf(int i)
        {
            List<T> neighbors = new();
            if (i != 0)
                neighbors.Add(_entries[i - 1]);
            if (i < Count - 1)
                neighbors.Add(_entries[i + 1]);
            return neighbors;
        }

        public Row(string row, ProcessFunc processFunc)
        {
            //Count = row.Length;
            int i = 0;
            foreach (char entry in row)
            {
                _entries[i] = processFunc(entry);
                i++;
            }
        }
    }
}