using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Advent.Common;
using Microsoft.VisualBasic;

List<Instruction> instructions = await FileLoader<Instruction>.Parse("input.txt", ParseLine);

int x = 0;
int y = 0;

foreach (Instruction instruction in instructions)
{
    switch (instruction.Direction)
    {
        case "forward":
            x += instruction.Amount;
            break;
        case "down":
            y += instruction.Amount;
            break;
        case "up":
            y -= instruction.Amount;
            break;
    }
}

Console.WriteLine($"Part 1: {x*y}");

// I could do them at the same time, but this is AoC. runtime be damned!
int aim = 0;
x = 0;
y = 0;

foreach (Instruction instruction in instructions)
{
    switch (instruction.Direction)
    {
        case "forward":
            x += instruction.Amount;
            y += instruction.Amount * aim;
            break;
        case "down":
            aim += instruction.Amount;
            break;
        case "up":
            aim -= instruction.Amount;
            break;
    }    
}
Console.WriteLine($"Part 2: {x*y}");

Instruction ParseLine(string line)
{
    string[] s = Strings.Split(line);
    return new Instruction()
    {
        Direction = s[0],
        Amount = int.Parse(s[1])
    };
}

class Instruction
{
    public string Direction { get; init; } = "";
    public int Amount { get; init; }
}
