using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Common;

using StreamReader reader = File.OpenText("input.txt");
List<int> numbers = (await reader.ReadLineAsync() ?? "").Split(",").Select(x => int.Parse(x)).ToList();

string rest = await reader.ReadToEndAsync();

List<BingoBoard> boards = rest.Trim().Split("\n\n")
    .Select(board => new BingoBoard(board))
    .ToList();

List<BingoBoard> winners = new();

foreach (int number in numbers)
{
    foreach (BingoBoard board in boards)
    {
        if (board.Score == null && board.Mark(number) && board.Winner)
        {
            winners.Add(board);
        }
    }
}

Console.WriteLine($"Part 1: {winners.First().Score}");
Console.WriteLine($"Part 2: {winners.Last().Score}");

class BingoBoard : Grid<BingoEntry>
{
    public bool Winner => Rows.Any(r => r.All(entry => entry.Marked)) || Columns.Any(c => c.All(entry => entry.Marked));
    
    public int? Score { get; private set; }

    // maybe could use linq!
    public bool Mark(int value)
    {
        BingoEntry? entry = Rows.SelectMany(r => r.ToList()).SingleOrDefault(e => e.Number == value);
        if (entry == null)
            return false;
        entry.Marked = true;

        if (Winner)
            Score = value * Rows.SelectMany(r => r.ToList()).Where(e => !e.Marked).Select(e => e.Number).Sum();
        return true;
    }
    
    public BingoBoard(string board) : 
        base(board.Split("\n")
                .Select(row => row.Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => new BingoEntry { Number = int.Parse(x) })))
        {}
}

class BingoEntry : IEquatable<BingoEntry>
{
    public int Number { get; init; }
    public bool Marked { get; set; }

    public bool Equals(BingoEntry? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Number == other.Number;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BingoEntry)obj);
    }

    public override int GetHashCode()
    {
        return Number;
    }
}