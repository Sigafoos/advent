using System.Diagnostics.CodeAnalysis;
using Advent.Common;

List<Round> rounds = await FileLoader<Round>.Parse("input.txt", Round.ParsePart1);

Round.Validate();
/*
// uncomment me for debug
foreach (string round in rounds.Select(x => x.ToString()))
    Console.WriteLine(round);
*/
int part1 = rounds.Select(g => g.Result).Sum();
Console.WriteLine($"Part 1: {part1}");

List<Round> rounds2 = await FileLoader<Round>.Parse("input.txt", Round.ParsePart2);
int part2 = rounds2.Select(g => g.Result).Sum();
Console.WriteLine($"Part 2: {part2}"); // 11260 too low

public class Round
{
    public required Throw Opponent { get; init; }
    public Throw You { get; init; }
    public char RawYou { get; init; }

    [SetsRequiredMembers] // why is this needed? grr.
    public Round(Throw opponent, Throw you)
    {
        Opponent = opponent;
        You = you;
    }

    public static Round ParsePart1(string line) => new(_parseThrow(line[0]), _parseThrow(line[2]));
    
    public static Round ParsePart2(string line)
    {
        Throw opponent = _parseThrow(line[0]);
        Throw you = line[2] switch
        {
            'X' => _parseThrow(((int)opponent + 1) % 3 + 1),
            'Y' => opponent,
            'Z' => _parseThrow((int)opponent % 3 + 1),
            _ => throw new ArgumentException(line[2].ToString())
        };
        return new Round(opponent, you);
    }
    public int Result => _gameResult + (int)You;

    private string _outcome
    {
        get
        {
            switch (_gameResult)
            {
                case 0:
                    return "LOSS";
                case 3:
                    return "TIE";
                case 6:
                    return "WIN";
                default:
                    throw new ArgumentException(_gameResult.ToString());
            }
        }
    }

public override string ToString()
    {
        return $""" 
Opponent: {Opponent.ToString()}
You:      {You.ToString()} ({(int)You})
Result:   {_outcome} ({_gameResult})
Score:    {Result}
-----
""";
    }

    private int _gameResult
    {
        get 
        {
            if (Opponent == You)
                return 3;
            if (Opponent == Throw.Scissors && You == Throw.Rock)
                return 6;
            if (Opponent == Throw.Rock && You == Throw.Scissors)
                return 0;
            return Opponent < You ? 6 : 0;
        }
    }

    private static Throw _parseThrow(char letter) => letter switch
    {
        'A' or 'X' => Throw.Rock,
        'B' or 'Y' => Throw.Paper,
        'C' or 'Z' => Throw.Scissors,
        _ => throw new ArgumentException(letter.ToString())
    };

    private static Throw _parseThrow(int value) => value switch
    {
        1 => Throw.Rock,
        2 => Throw.Paper,
        3 => Throw.Scissors,
        _ => throw new ArgumentException(value.ToString())
    };
    

    public enum Throw
    {
        Rock = 1,
        Paper,
        Scissors
    }

    public static void Validate()
    {
        Dictionary<Round, int> rounds = new(new List<KeyValuePair<Round, int>> {
            new (new Round(Throw.Rock, Throw.Rock), 4),
            new (new Round(Throw.Rock, Throw.Paper), 8),
            new (new Round(Throw.Rock, Throw.Scissors), 3),
            new (new Round(Throw.Paper, Throw.Rock), 1),
            new (new Round(Throw.Paper, Throw.Paper), 5),
            new (new Round(Throw.Paper, Throw.Scissors), 9),
            new (new Round(Throw.Scissors, Throw.Rock), 7),
            new (new Round(Throw.Scissors, Throw.Paper), 2),
            new (new Round(Throw.Scissors, Throw.Scissors), 6)
        });

        int failures = 0;
        foreach (KeyValuePair<Round, int> scenario in rounds.Where(scenario => scenario.Key.Result != scenario.Value))
        {
            Console.WriteLine(scenario.Key.ToString());
            failures++;
        }
        
        Console.WriteLine($"{rounds.Count - failures}/{rounds.Count} tests passed");
    }
}