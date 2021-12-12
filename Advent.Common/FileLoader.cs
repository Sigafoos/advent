using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.Common
{
    public static class FileLoader<T>
    {
        public delegate T ProcessFunction(string input);
        
        public static async Task<List<T>> Parse(string filename, ProcessFunction processFunction)
        {
            List<T> entries = new();
            
            using StreamReader reader = File.OpenText(filename);
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                entries.Add(processFunction(line));
            }

            return entries;
        }

        public static async Task<List<T>> ParseSingleLine(string filename, string separator,
            ProcessFunction processFunction)
        {
            using StreamReader reader = File.OpenText(filename);
            string line = await reader.ReadLineAsync() ?? throw new ArgumentException($"{filename} has no lines");

            return line.Trim().Split(separator).Select(entry => processFunction(entry)).ToList();
        }
    }
}