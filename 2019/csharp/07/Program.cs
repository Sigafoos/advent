using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace _07
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "";
            try {
                input = System.IO.File.ReadAllText(@"../../input/07.txt");
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

            var outputs = new List<int>();
            //var signals = new List<int>(new int[5] {0, 1, 2, 3, 4});
            var signals = new List<int>(new int[5] {5, 6, 7, 8, 9});
            foreach (List<int> perm in Permutation.Permutate(signals)) {
                var output = new List<int>();
                var a = new Intcode(commands, perm[0]);
                var b = new Intcode(commands, perm[1]);
                var c = new Intcode(commands, perm[2]);
                var d = new Intcode(commands, perm[3]);
                var e = new Intcode(commands, perm[4]);

                var cZ = Channel.CreateUnbounded<int>();
                var cA = Channel.CreateUnbounded<int>();
                var cB = Channel.CreateUnbounded<int>();
                var cC = Channel.CreateUnbounded<int>();
                var cD = Channel.CreateUnbounded<int>();
                var cE = Channel.CreateUnbounded<int>();

                a.input = cZ.Reader;
                a.output = cA.Writer;
                b.input = cA.Reader;
                b.output = cB.Writer;
                c.input = cB.Reader;
                c.output = cC.Writer;
                d.input = cC.Reader;
                d.output = cD.Writer;
                e.input = cD.Reader;
                e.output = cE.Writer;

                Task.Run(async () => await cZ.Writer.WriteAsync(0));
                Task.Run(() => a.Run());
                Task.Run(() => b.Run());
                Task.Run(() => c.Run());
                Task.Run(() => d.Run());
                Task.Run(() => e.Run());

                Task.Run(async () => {
                        while (await cE.Reader.WaitToReadAsync())
                        await foreach (int item in cE.Reader.ReadAllAsync()) {
                            output.Add(item);
                            await cZ.Writer.WriteAsync(item);
                        }
                        }).Wait();

                //outputs.Add(Int32.Parse(string.Join("", output)));
                outputs.Add(output[output.Count-1]);
            }

            outputs.Sort();
            Console.WriteLine($"Part 2: {outputs[outputs.Count-1]}");
        }
    }

    class Intcode {
        protected int[] commands;
        protected int position = 0;
        public enum Mode { Position, Immediate };
        protected int phase = -1;
        public ChannelReader<int> input = Channel.CreateUnbounded<int>().Reader;
        public ChannelWriter<int> output = Channel.CreateUnbounded<int>().Writer;

        public Intcode(int[] commands) {
            this.commands = (int[])commands.Clone();
        }

        public Intcode(int[] commands, int noun, int verb) {
            this.commands = (int[])commands.Clone();
            this.commands[1] = noun;
            this.commands[2] = verb;
        }

        public Intcode(int[] commands, int phase) {
            this.commands = (int[])commands.Clone();
            this.phase = phase;
        }

        public async void Run() {
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
                        if (this.phase != -1) {
                            this.commands[a] = this.phase;
                            this.phase = -1;
                        }
                        else {
                            this.commands[a] = await this.input.ReadAsync();
                        }
                        break;

                    case 4:
                        a = this.Next(Mode.Position);
                        await this.output.WriteAsync(this.commands[a]);
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
                        this.output.Complete();
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

    // https://stackoverflow.com/a/23718676
    static class Permutation {
        public static IEnumerable<List<T>> Permutate<T>(List<T> input)
        {
            if (input.Count == 2) // this are permutations of array of size 2
            {
                yield return new List<T>(input);
                yield return new List<T> {input[1], input[0]}; 
            }
            else
            {
                foreach(T elem in input) // going through array
                {
                    var rlist = new List<T>(input); // creating subarray = array
                    rlist.Remove(elem); // removing element
                    foreach(List<T> retlist in Permutate(rlist))
                    {
                        retlist.Insert(0,elem); // inserting the element at pos 0
                        yield return retlist;
                    }

                }
            }
}}
}
