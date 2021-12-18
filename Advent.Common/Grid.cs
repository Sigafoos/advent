using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.Common
{
    public class Grid<T> : IEnumerable<GridObject<T>> where T : new()
    {
        public int RowCount => _rows.Count;
        public int ColumnCount => _rows.FirstOrDefault(r => r.Key == 0).Value.Count;
        private readonly Dictionary<int, Row<T>> _rows;

        public T At(int x, int y) => _rows[y].At(x);
        public T At(Coordinate position) => At(position.X, position.Y);

        public IEnumerable<List<T>> Rows => _rows.Values.Select(r => r.ToList());
        public List<T> Row(int row) => _rows[row].ToList();

        public List<T> OrthogonalNeighborsOf(Coordinate position)
        {
            List<T> neighbors = _rows[position.Y].NeighborsOf(position.X);
            if (position.Y != 0)
                neighbors.Add(At(position.Up));
            if (position.Y < _rows.Count - 1)
                neighbors.Add(At(position.Down));
            return neighbors;
        }
        
        /// <summary>
        /// Return the column of the grid as a list
        /// </summary>
        /// <param name="column">The id of the column</param>
        /// <returns></returns>
        public List<T> Column(int column) => _rows.Values.Select(r => r.At(column)).ToList();

        public IEnumerable<List<T>> Columns => Enumerable.Range(0, ColumnCount).Select(Column);

        public IEnumerable<T> Range(Coordinate start, Coordinate end)
        {
            int skipX, takeX, skipY, takeY;
            if (start.Y < end.Y)
            {
                skipY = start.Y;
                takeY = end.Y - start.Y;
            }
            else
            {
                skipY = end.Y;
                takeY = start.Y - end.Y;
            }

            if (start.X < end.X)
            {
                skipX = start.X;
                takeX = end.X - start.X;
            }
            else
            {
                skipX = end.X;
                takeX = start.X - end.X;
            }
            return _rows.Skip(skipY).Take(takeY + 1).SelectMany(row => row.Value.ToList().Skip(skipX).Take(takeX + 1));
        }

        public IEnumerable<T> Line(Coordinate start, Coordinate end)
        {
            if (start.X == end.X || start.Y == end.Y)
                return Range(start, end);

            Coordinate left, right;
            if (start.X < end.X)
            {
                left = start;
                right = end;
            }
            else
            {
                left = end;
                right = start;
            }
            
            int diffX = left.X - right.X;
            int diffY = left.Y - right.Y;

            // TODO support this I guess
            if (Math.Abs(diffX) != Math.Abs(diffY))
                throw new ArgumentException("not a 45 degree angle");

            List<T> diagonal = new();

            int y = left.Y;
            int step = diffY < 0 ? 1 : -1;
            for (int x = left.X; x <= right.X; x++)
            {
                diagonal.Add(At(x, y));
                y += step;
            }

            return diagonal;
        }

        public bool Contains(T x) => _rows.Values.Any(r => r.Contains(x));

        public IEnumerator<GridObject<T>> GetEnumerator()
        {
            for (int y = 0; y < RowCount; y++)
            {
                for (int x = 0; x < ColumnCount; x++)
                {
                    yield return new GridObject<T>
                    {
                        Position = new Coordinate(x, y),
                        Value = At(x, y)
                    };
                }
            }
        }

        public override string ToString()
        {
            return string.Join("\n", _rows.Values.Select(r => r.ToString()));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Grid(IEnumerable<IEnumerable<T>> raw)
        {
            _rows = new Dictionary<int, Row<T>>(
                raw.Select((row, i) => new KeyValuePair<int, Row<T>>(i, new Row<T>(row))));
        }

        public Grid(int columns, int rows)
        {
            _rows = new Dictionary<int, Row<T>>(
                Enumerable.Range(0, columns)
                    .Select(i => new KeyValuePair<int, Row<T>>(i, new Row<T>(Enumerable.Range(0, rows).Select(_ => new T())))));
        }

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

        public bool Contains(T x) => _entries.ContainsValue(x);

        public List<T> ToList() => _entries.Values.ToList();
        
        public List<T> NeighborsOf(int i)
        {
            List<T> neighbors = new();
            if (i != 0)
                neighbors.Add(_entries[i - 1]);
            if (i < Count - 1)
                neighbors.Add(_entries[i + 1]);
            return neighbors;
        }

        public override string ToString() => string.Join("", _entries.Values.Select(e => e?.ToString() ?? ""));

        public Row(string row, ProcessFunc processFunc)
        {
            int i = 0;
            foreach (char entry in row)
            {
                _entries[i] = processFunc(entry);
                i++;
            }
        }

        public Row(IEnumerable<T> raw)
        {
            _entries = new Dictionary<int, T>(raw.Select((x, i) => new KeyValuePair<int, T>(i, x)));
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

    public class GridObject<T>
    {
        public T Value { get; init; }
        public Coordinate Position { get; init; }
    }
}