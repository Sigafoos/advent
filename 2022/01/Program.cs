using StreamReader reader = File.OpenText("input.txt");
string raw = await reader.ReadToEndAsync();

var x = raw.Trim().Split("\n\n").Select(elf => elf.Split("\n"));

List<List<int>> elves = raw.Trim().Split("\n\n")
    .Select(elf =>
        elf.Split("\n")
            .Select(int.Parse)
            .ToList()
        ).ToList();

List<List<int>> topElves = elves.OrderByDescending(elf => elf.Sum()).Take(3).ToList();

int part1 = topElves.First().Sum();
Console.WriteLine($"Part 1: {part1}");

int part2 = topElves.Select(elf => elf.Sum()).Sum();
Console.WriteLine($"Part 2: {part2}");