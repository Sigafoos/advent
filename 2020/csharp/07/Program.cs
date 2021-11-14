using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Common;

Regex lineRegex = new(@"^(?<type>\w+ \w+) bags contain (?<rules>.*)\.$", RegexOptions.Compiled);
Regex ruleRegex = new(@"^(?<number>\d+) (?<type>\w+ \w+) bags?", RegexOptions.Compiled);

Dictionary<string, string> unparsedRules = new((await FileLoader<string>.Parse("../../../../../input/07.txt", line => line))
    .Select(ParseLine));

Dictionary<string, Bag> bags = new();
foreach (var (key, value) in unparsedRules)
{
    if (!bags.TryGetValue(key, out Bag? bag))
    {
        bag = new Bag(key);
        bags.Add(key, bag);
    }

    foreach (string rule in value.Split(", "))
    {
        Match parsed = ruleRegex.Match(rule);
        if (!parsed.Success)
            continue;
        
        if (!bags.TryGetValue(parsed.Groups["type"].Value, out Bag? referencedBag))
        {
            referencedBag = new Bag(parsed.Groups["type"].Value);
            bags.Add(parsed.Groups["type"].Value, referencedBag);
        }

        bag.Rules.Add(new Rule(referencedBag, int.Parse(parsed.Groups["number"].Value)));
    }
}

int partOne = bags.Values.Count(b => b.ContainsGold.Value);
Console.WriteLine($"Part 1: {partOne}");

int partTwo = bags["shiny gold"].BagCount.Value;
Console.WriteLine($"Part 2: {partTwo}"); // 14178 too high

KeyValuePair<string, string> ParseLine(string line)
{
    Match parsed = lineRegex.Match(line) ?? throw new ArgumentException(line);

    return new KeyValuePair<string, string>(
        parsed.Groups["type"].Value,
        parsed.Groups["rules"].Value
    );
}

internal class Bag
{
    public string Type { get; init; }
    public List<Rule> Rules { get; init; } = new();
    public Lazy<bool> ContainsGold { get; private set; }
    
    public Lazy<int> BagCount { get; private set; }

    public Bag(string type)
    {
        Type = type;
        ContainsGold = new Lazy<bool>(() => Rules.Any(r => r.Bag.Type == "shiny gold" || r.Bag.ContainsGold.Value));
        BagCount = new Lazy<int>(() => Rules.Select(r => r.Number + r.Number * r.Bag.BagCount.Value).Sum());
    }
}

internal record Rule(Bag Bag, int Number);