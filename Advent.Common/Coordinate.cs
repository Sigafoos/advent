using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace Advent.Common
{
    public record Coordinate(int X, int Y)
    {
        public override string ToString() => $"({X},{Y})";
        public static bool TryParse(string input, out Coordinate? coordinate)
        {
            coordinate = null;
            
            string[] split = input.Split(",");
            if (split.Length != 2)
                return false;
            if (!int.TryParse(split[0], out int x))
                return false;
            if (!int.TryParse(split[1], out int y))
                return false;
            coordinate = new Coordinate(x, y);
            return true;
        }
    }
}