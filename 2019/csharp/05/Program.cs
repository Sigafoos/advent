using System;

namespace _05
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "";
            try {
                input = System.IO.File.ReadAllText(@"../../input/05.txt");
            }
            catch (System.IO.FileNotFoundException) {
                Console.WriteLine("file not found");
                System.Environment.Exit(1);
            }
            var raw = input.Split(",");
            var commands = new int[raw.Length];
            for (int i = 0; i < raw.Length; i++) {
                commands[i] = Int32.Parse(raw[i]);
            }

            var intcode = new Intcode(commands);
            intcode.Run();
        }
    }

    class Intcode {
        protected int[] commands;
        protected int position;
        public enum Mode { Position, Immediate };

        public Intcode(int[] commands) {
            this.commands = commands;
            this.position = 0;
        }

        public Intcode(int[] commands, int noun, int verb) {
            this.commands = (int[])commands.Clone();
            this.commands[1] = noun;
            this.commands[2] = verb;
            this.position = 0;
        }

        public void Run() {
            while (true) {
                var command = this.commands[this.position++];
                var instruction = command;
                var modes = new Mode[3]{ Mode.Position, Mode.Position, Mode.Position };
                int a;
                int b;
                int c;

                if (command > 100) {
                    var str = command.ToString();
                    instruction = Int32.Parse(str.Substring(str.Length - 2));
                    for (int i = 3; i <= str.Length; i++) {
                        modes[i-3] = (Mode)Int32.Parse(str.Substring(str.Length - i, 1));
                    }
                }

                switch (instruction) {
                    case 1:
                        a = this.Next(modes[0]);
                        b = this.Next(modes[1]);
                        c = this.Next(Mode.Position);
                        this.commands[c] = this.commands[a] + this.commands[b];
                        break;

                    case 2:
                        a = this.Next(modes[0]);
                        b = this.Next(modes[1]);
                        c = this.Next(Mode.Position);
                        this.commands[c] = this.commands[a] * this.commands[b];
                        break;

                    case 3:
                        a = this.Next(Mode.Position);
                        Console.Write("\n> ");
                        var input = Console.ReadLine();
                        this.commands[a] = Int32.Parse(input);
                        break;

                    case 4:
                        a = this.Next(Mode.Position);
                        Console.WriteLine(this.commands[a]);
                        break;

                    case 5:
                        a = this.Next(modes[0]);
                        b = this.Next(modes[1]);
                        if (this.commands[a] != 0) {
                            this.position = this.commands[b];
                        }
                        break;

                    case 6:
                        a = this.Next(modes[0]);
                        b = this.Next(modes[1]);
                        if (this.commands[a] == 0) {
                            this.position = this.commands[b];
                        }
                        break;

                    case 7:
                        a = this.Next(modes[0]);
                        b = this.Next(modes[1]);
                        c = this.Next(Mode.Position);

                        if (this.commands[a] < this.commands[b]) {
                            this.commands[c] = 1;
                        }
                        else {
                            this.commands[c] = 0;
                        }
                        break;


                    case 8:
                        a = this.Next(modes[0]);
                        b = this.Next(modes[1]);
                        c = this.Next(Mode.Position);

                        if (this.commands[a] == this.commands[b]) {
                            this.commands[c] = 1;
                        }
                        else {
                            this.commands[c] = 0;
                        }
                        break;

                    case 99:
                        return;

                    default:
                        throw new Exception($"unknown command {instruction}");
                }
            }
        }

        protected int Next(Mode mode) {
            if (mode == Mode.Position) {
                return this.commands[this.position++];
            }
            return this.position++;
        }

        public int At(int i) {
            return this.commands[i];
        }

        public override string ToString() {
            return string.Join(",", this.commands);
        }
    }
}
