using StreamReader reader = File.OpenText("input.txt");
string input = await reader.ReadToEndAsync();

Terminal t = new(input);

//int part1 = BelowSize(t.Root, 100000); // found a better way!
int part1 = t.AllDirectories.Where(d => d.Size <= 100000).Select(d => d.Size).Sum();
Console.WriteLine($"Part 1: {part1}");

const int totalSize = 70000000;
const int neededSpace = 30000000;
int freeSpace = totalSize - t.Root.Size;
int additionalSpaceNeeded = neededSpace - freeSpace;

int part2 = t.AllDirectories
    .Where(d => d.Size >= additionalSpaceNeeded)
    .Select(d => d.Size)
    .Order()
    .First();
Console.WriteLine($"Part 2: {part2}"); // 37072768 too high

int BelowSize(Directory dir, int maxSize)
{
    int size = 0;
    if (dir.Size <= maxSize)
        size += dir.Size;
    size += dir.Subdirectories.Values.Sum(sub => BelowSize(sub, maxSize));
    return size;
}

class Terminal
{
    public Directory Root { get; } = new() { Name = "/" };
    public Directory Cwd { get; private set; }

    public List<Directory> AllDirectories { get; } = new();

    public Terminal(string input)
    {
        Cwd = Root;
        
        foreach (string line in input.Split("\n"))
        {
            if (line.StartsWith("$ "))
                _handleCommand(line);
            else
                _handleOutput(line);
        }
    }

    private void _handleCommand(string command)
    {
        if (command == "$ ls")
            return;
        if (command.StartsWith("$ cd"))
            ChangeDirectory(command[5..]);
        else
            throw new ArgumentException(command);
    }

    public void ChangeDirectory(string directory)
    {
        if (directory == "/")
        {
            Cwd = Root;
            return;
        }
        string[] destinations = directory.Split("/");
        foreach (string destination in destinations)
        {
            Cwd = destination switch
            {
                ".." => Cwd.Parent ?? Cwd,
                _ => Cwd.Subdirectories[directory]
            };
        }
    }

    private void _handleOutput(string output)
    {
        string[] split = output.Split(" ");
        if (split[0] == "dir")
            _createDirectory(split[1]);
        else
            _createFile(split);
    }

    private void _createDirectory(string name)
    {
        Directory dir = new() { Name = name, Parent = Cwd };
        Cwd.Subdirectories.Add(name, dir);
        AllDirectories.Add(dir);
    }

    private void _createFile(string[] info) =>
        Cwd.Files.Add(new DirectoryFile()
        {
            Size = int.Parse(info[0]),
            Name = info[1]
        });
}

class Directory
{
    public required string Name { get; init; }
    public Directory? Parent { get; init; }
    public Dictionary<string, Directory> Subdirectories { get; } = new();
    public List<DirectoryFile> Files { get; } = new();

    public int Size => Files.Select(f => f.Size).Sum() + Subdirectories.Values.Select(sd => sd.Size).Sum();
}

class DirectoryFile
{
    public required string Name { get; init; }
    public required int Size { get; init; }
    
}