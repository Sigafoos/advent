using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

List<Instruction> instructions = await FileLoader<Instruction>.Parse("../../../../../input/08.txt", line =>
{
    List<string> split = line.Split(' ').ToList();
    return new Instruction(split[0], int.Parse(split[1]));
});

GameConsole console = new(instructions);
_ = console.Execute();

Console.WriteLine($"Part 1: {console.Accumulator}");

bool succeeded = false;
int rewritable = console.Instructions.Count(i => i.Operation is "jmp" or "nop");
for (int i = 0; i < rewritable; i++)
{
    console.Reset();
    if (console.Execute(i))
    {
        succeeded = true;
        break;
    }
}

if (!succeeded)
    throw new Exception("never completed");

Console.WriteLine($"Part 2: {console.Accumulator}");

internal class GameConsole
{
    public List<Instruction> Instructions { get; }

    public int Accumulator { get; private set; }

    private int _index;

    private Dictionary<int, bool> _visited = new();

    private int _jmpsAndNopsSeen;

    public GameConsole(List<Instruction> instructions)
    {
        Instructions = instructions;
    }

    public void Reset()
    {
        Accumulator = 0;
        _index = 0;
        _visited = new Dictionary<int, bool>();
        _jmpsAndNopsSeen = 0;
    }

    public bool Execute(int? flip = null)
    {
        while (true)
        {
            if (_visited.ContainsKey(_index))
                return false;
            if (_index == Instructions.Count)
                return true;

            _visited.Add(_index, true);

            Instruction instruction = Instructions[_index];

            if (flip != null && instruction.Operation != "acc")
            {
                if (_jmpsAndNopsSeen == flip)
                {
                    string op = instruction.Operation == "jmp" ? "nop" : "jmp";
                    instruction = instruction with { Operation = op };
                }

                _jmpsAndNopsSeen++;
            }
            
            switch (instruction.Operation)
            {
                case "acc":
                    Accumulator += instruction.Argument;
                    _index++;
                    break;
                case "jmp":
                    _index += instruction.Argument;
                    break;
                case "nop":
                    _index++;
                    break;
                default:
                    throw new ArgumentException(instruction.Operation);
            }
        }
    }
}

internal record Instruction(string Operation, int Argument);
