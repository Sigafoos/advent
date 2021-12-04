using System.Collections;
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

        public List<T> Row(int row) => _rows[row].ToList();
        /// <summary>
        /// Return the column of the grid as a list
        /// </summary>
        /// <param name="column">The id of the column</param>
        /// <returns></returns>
        public List<T> Column(int column) => _rows.Values.Select(r => r.At(column)).ToList();
        
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

    public class Row<T> : IEnumerable<T>
    {

        public int Count => _entries.Count;
        public bool IsReadOnly => false;
        private readonly Dictionary<int, T> _entries = new();

        public delegate T ProcessFunc(char c);

        public T At(int i) => _entries[i];

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

        public IEnumerator<T> GetEnumerator()
        {
            return _entries.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}