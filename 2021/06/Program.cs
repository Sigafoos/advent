using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Advent.Common;

List<int> school = await FileLoader<int>.ParseSingleLine("example.txt", ",", int.Parse);

/*
int part1 = school.Select(f => CalculateSpawn(18 - f)).Sum();
Console.WriteLine($"Part 1: {part1}");
*/

const int days = 18;

List<int> spawns = Enumerable.Range(0, days + 1).Select(x => (int)Math.Ceiling((decimal)x / 6)).ToList();
Console.WriteLine(CalculateSpawn(18-3));

int CalculateSpawn(int daysAfterFirstSpawn)
{
    if (daysAfterFirstSpawn < 0)
        return 1; // yourself
    // doesnt quite work. also need to handle the 8 + 2 (probably by subtracting two from the starting group?
    return 1 + Enumerable.Range(0, daysAfterFirstSpawn / 6).Select(x => CalculateSpawn(daysAfterFirstSpawn - x * 6)).Sum();
}

internal class School
{
    private List<int> _school;
    private int _days;
    private List<int> _spawnRate;

    public School(List<int> school, int days)
    {
        _school = school;
        _days = days;
        _spawnRate = Enumerable.Range(0, days + 1).Select(x => x.Spawns()).ToList();
    }

    // how many fish is this fish responsible for after $n days
    public int Calculate(int days) =>
        Enumerable.Range(days.Spawns(), 0).Select(x => x * Calculate(days - x * 8)).Sum();

}

internal static class ExtensionMethods
{
    public static int Spawns(this int x) => (int)Math.Ceiling((decimal)x / 6);
}

/*
// for posterity
List<Fish> school = await FileLoader<Fish>.ParseSingleLine("input.txt", ",", i => new Fish(int.Parse(i)));

for (int i = 0; i < 80; i++)
{
    int births = school.Count(fish => fish.Spawns());
    for (int j = 0; j < births; j++)
        school.Add(new Fish());
}

Console.WriteLine($"Part 1: {school.Count}");

internal class Fish
{
    private int _spawnsIn;

    public Fish(int spawnsIn = 8)
    {
        _spawnsIn = spawnsIn;
    }

    public bool Spawns()
    {
        if (_spawnsIn == 0)
        {
            _spawnsIn = 6;
            return true;
        }

        _spawnsIn--;
        return false;
    }
    
}
*/