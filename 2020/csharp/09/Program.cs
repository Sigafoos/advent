using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

List<Int64> input = await FileLoader<Int64>.Parse("../../../../../input/09.txt", line => Int64.Parse(line));

Xmas cypher = new(25, input);

cypher.DiscoverInvalidNumber();
Console.WriteLine($"Part 1: {cypher.InvalidNumber}");

Console.WriteLine($"Part 2: {cypher.EncryptionWeakness()}");

internal class Xmas
{
    public int Preamble { get; }
    public List<Int64> Cypher { get; }

    public Int64? InvalidNumber { get; private set; }

    public Xmas(int preamble, List<Int64> cypher)
    {
        Preamble = preamble;
        Cypher = cypher;
    }

    public void DiscoverInvalidNumber()
    {
        for (int i = Preamble; i < Cypher.Count; i++)
        {
            if (_validAt(i)) continue;

            InvalidNumber = Cypher[i];
            return;
        }

        throw new InvalidOperationException("no valid number found");
    }

    public Int64 EncryptionWeakness()
    {
        if (InvalidNumber == null)
            throw new InvalidOperationException("must discover invalid number first");
        
        for (int i = 0; i < Cypher.Count-1; i++)
        {
            Int64 remaining = InvalidNumber!.Value - Cypher[i];
            for (int j = i + 1; j < Cypher.Count; j++)
            {
                remaining -= Cypher[j];
                if (remaining == 0)
                {
                    List<Int64> values = Cypher.Skip(i).Take(j - i).OrderBy(x => x).ToList();
                    return values.Last() + values.First();
                }
                if (remaining < 0)
                {
                    break;
                }
            }
        }

        throw new InvalidOperationException("nothing matched");
    }

    private bool _validAt(int position)
    {
        if (position < Preamble)
            throw new ArgumentException();
        
        Int64 value = Cypher[position];
        List<Int64> checks = Cypher.Skip(position - Preamble).Take(Preamble).OrderBy(i => i).ToList();

        for (int i = 0; i < checks.Count - 1; i++)
        {
            for (int j = i + 1; j < checks.Count; j++)
            {
                if (checks[i] + checks[j] == value)
                    return true;
                
                // we sorted numerically, so every check from here on out will fail
                if (checks[i] + checks[j] > value)
                    break;
            }
        }

        return false;
    }
}
