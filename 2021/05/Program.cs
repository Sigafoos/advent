using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

List<VentCoordinate> coordinates = await FileLoader<VentCoordinate>.Parse("input.txt", Parse);

int x = coordinates.SelectMany(c => new[] { c.Start.X, c.End.X }).Max();
int y = coordinates.SelectMany(c => new[] { c.Start.Y, c.End.Y }).Max();

Grid<VentPosition> floor = new(x+1, y+1);

List<VentCoordinate> toProcess =
    coordinates.Where(vent => vent.Start.X == vent.End.X || vent.Start.Y == vent.End.Y).ToList();

foreach (VentPosition value in toProcess.SelectMany(vent => floor.Line(vent.Start, vent.End)))
    value.VentCount++;

int part1 = floor.Range(new Coordinate(0, 0), new Coordinate(x, y)).Count(vent => vent.VentCount > 1);

// TODO you're missing a row and a col!!
Console.WriteLine($"Part 1: {part1}");

Grid<VentPosition> floor2 = new(x+1, y+1);
foreach (VentPosition value in coordinates.SelectMany(vent => floor2.Line(vent.Start, vent.End)))
{
    value.VentCount++;
}

int part2 = floor2.Range(new Coordinate(0, 0), new Coordinate(x, y)).Count(vent => vent.VentCount > 1);
Console.WriteLine($"Part 2: {part2}");

VentCoordinate Parse(string line)
{
    string[] split = line.Split(" -> ");
    if (!Coordinate.TryParse(split[0], out Coordinate? start) || start == null)
        throw new Exception("start isn't valid");
    if (!Coordinate.TryParse(split[1], out Coordinate? end) || end == null)
        throw new Exception("end isn't valid");
    return new VentCoordinate(start, end);
}

internal record VentCoordinate(Coordinate Start, Coordinate End);

class VentPosition
{
    public int VentCount { get; set;  }

    public override string ToString() => VentCount == 0 ? "." : VentCount > 9 ? "^" : VentCount.ToString();
}