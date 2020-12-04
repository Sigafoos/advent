using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _04
{
    class Passport
    {
        public string? BirthYear { get; private set; }
        public string? IssueYear { get; private set; }
        public string? ExpirationYear { get; private set; }
        public string? Height { get; private set; }
        public string? HairColor { get; private set; }
        public string? EyeColor { get; private set; }
        public string? PassportId { get; private set; }
        public string? CountryId { get; private set; }

        private readonly Regex _pattern = new Regex(@"([a-z]+):([#\w]+)");
        private readonly List<string> _eyeColors = new List<string>{ "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

        public Passport(string input)
        {
            var matches = _pattern.Matches(input);
            foreach (Match? match in matches)
            {
                string key = match?.Groups[1].Value ?? throw new ArgumentException($"bad input {input}");
                string val = match?.Groups[2].Value ?? throw new ArgumentException($"bad input {input}");

                switch (key)
                {
                    case "byr":
                        BirthYear = val;
                        break;
                    case "iyr":
                        IssueYear = val;
                        break;
                    case "eyr":
                        ExpirationYear = val;
                        break;
                    case "hgt":
                        Height = val;
                        break;
                    case "hcl":
                        HairColor = val;
                        break;
                    case "ecl":
                        EyeColor = val;
                        break;
                    case "pid":
                        PassportId = val;
                        break;
                    case "cid":
                        CountryId = val;
                        break;
                    default:
                        throw new ArgumentException($"unknown key '{key}");
                }
            }
        }

        public bool IsValid(bool strict = false)
        {
            if (BirthYear == default || (strict && !(Int32.TryParse(BirthYear, out int byr) && byr >= 1920 && byr <= 2002)))
                return false;
            if (IssueYear == default || (strict && !(Int32.TryParse(IssueYear, out int iyr) && iyr >= 2010 && iyr <= 2020)))
                return false;
            if (ExpirationYear == default || (strict && !(Int32.TryParse(ExpirationYear, out int eyr) && eyr >= 2020 && eyr <= 2030)))
                return false;
            if (Height == default)
                return false;
            if (strict)
            {
                var match = Regex.Match(Height, @"^(\d+)(in|cm)$");
                int height;
                if (match == Match.Empty || !Int32.TryParse(match.Groups[1].Value, out height))
                    return false;
                if (match.Groups[2].Value == "in")
                {
                    if (height < 59 || height > 76)
                        return false;
                }
                else
                {
                    if (height < 150 || height > 193)
                        return false;
                }
            }
            if (HairColor == default || (strict && !Regex.IsMatch(HairColor, @"^#[0-9a-f]{6}$")))
                return false;
            if (EyeColor == default || (strict && !_eyeColors.Any(c => c == EyeColor)))
                return false;
            if (PassportId == default || (strict && !Regex.IsMatch(PassportId, @"^\d{9}$")))
                return false;
            return true;
        }

        public override string ToString()
        {
            return $"BirthYear: {BirthYear}\nIssueYear: {IssueYear}\nExpirationYear: {ExpirationYear}\nHeight: {Height}\nHairColor: {HairColor}\nEyeColor: {EyeColor}\nPassportId: {PassportId}\nCountryId: {CountryId}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var passports = new List<Passport>();
            string input = File.ReadAllText("../../input/04.txt");
            foreach (string passport in input.Split("\n\n"))
            {
                passports.Add(new Passport(passport));
            }

            int part1 = passports.Where(p => p.IsValid()).Count();
            Console.WriteLine($"Part 1: {part1}");
            int part2 = passports.Where(p => p.IsValid(strict: true)).Count();
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}
