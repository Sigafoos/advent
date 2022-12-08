using Advent.Common;

using StreamReader reader = File.OpenText("input.txt");
string input = await reader.ReadToEndAsync();

int headerLocation = HeaderLocation(input, 4);
Console.WriteLine($"Part 1: {headerLocation}");

int messageLocation = HeaderLocation(input, 14);
Console.WriteLine($"Part 2: {messageLocation}");

int HeaderLocation(string signal, int length)
{
    for (int i = 0; i < signal.Length - length; i++)
    {
        string packet = signal[i..(i + length)];
        if (packet.Distinct().Count() == length)
            return i + length;
    }

    throw new ArgumentException();
}
