using System.Text.RegularExpressions;

using StreamReader reader = File.OpenText("input.txt");
string file = await reader.ReadToEndAsync();
/*
string file = """
                    [D]            
                    [T]            
                    [W]            
                    [C]            
                    [F]            
                    [M]            
                [P] [H]            
                [V] [J]            
                [G] [C]            
                [C] [H]     [N]    
                [N] [T]     [M]    
                [B] [L]     [L]    
                [L] [V]     [Z]    
                [R] [Z]     [C]    
                [L] [N]     [V]    
                [T] [H]     [F]    
                [F] [B]     [J]    
            [J] [C] [G] [S] [R]    
[L]         [P] [S] [D] [M] [Q] [P]
[B] [W]     [S] [Z] [W] [F] [W] [R]
 1   2   3   4   5   6   7   8   9

move 4 from 6 to 9
""";
*/
string[] contents = file.Split("\n\n");
Regex pattern = new(@"^move (\d+) from (\d+) to (\d+)$");

Ship ship = new(contents[0]);
foreach (string instruction in contents[1].Split("\n"))
{
    Match result = pattern.Match(instruction);
    List<int> ints = result.Groups.Values.ToArray()[1..].Select(g => int.Parse(g.Value)).ToList();
    ship.Move(ints[0], ints[1], ints[2]);
}

string part1 = string.Join("", ship.Stacks.Values.Select(v => v.Top));
Console.WriteLine($"Part 1: {part1}");

Ship ship2 = new(contents[0]);
foreach (string instruction in contents[1].Split("\n"))
{
    Match result = pattern.Match(instruction);
    List<int> ints = result.Groups.Values.ToArray()[1..].Select(g => int.Parse(g.Value)).ToList();
    ship2.Move(ints[0], ints[1], ints[2], batches: true);
}
string part2 = string.Join("", ship2.Stacks.Values.Select(v => v.Top));
Console.WriteLine($"Part 2: {part2}");

class Ship
{
    public Dictionary<int, Stack> Stacks { get; } = new();

    public Ship(string input)
    {
        string[] lines = input.Split("\n").Reverse().ToArray();
        List<int> stackIds = lines[0].Split(" ").Where(x => x != "").Select(int.Parse).ToList();
        foreach (int id in stackIds)
        {
            Stacks.Add(id, new Stack());
        }

        foreach (string line in lines[1..])
        {
            for (int i = 0; i < Stacks.Count; i ++)
            {
                char payload = line[i*4+1];
                if (payload != ' ')
                    Stacks[i+1].Add(new []{ payload.ToString()});
            }
        }
    }

    public void Move(int count, int from, int to, bool batches = false)
    {
        List<string> crates = new();
        for (int i = 0; i < count; i++)
        {
            string crate = Stacks[from].Remove();
            crates.Add(crate);
        }

        if (batches)
            crates.Reverse();

        Stacks[to].Add(crates);
    }

    public override string ToString()
    {
        string ids = string.Join("   ", Stacks.Keys);
        List<string> lines = new() { $" {ids}" };
        for (int i = 0; i < Stacks.Values.Select(s => s.Height).Max(); i++)
        {
            IEnumerable<string> row = Stacks.Values.Select(s => s.At(i) == null ? "   " : $"[{s.At(i)}]");
            lines.Add(string.Join(" ", row));
        }
        
        lines.Reverse();
        return string.Join("\n", lines);
    }
}

class Stack
{
    private List<string> _stack { get; set; } = new();

    public string Remove()
    {
        int final = _stack.Count - 1;
        string removed = _stack[final];
        _stack.RemoveAt(final);
        return removed;
    }

    public void Add(IEnumerable<string> crates) => _stack.AddRange(crates);

    public string Top => _stack.LastOrDefault() ?? "";

    public int Height => _stack.Count;

    public string? At(int i) => i >= _stack.Count ? null : _stack[i];

}