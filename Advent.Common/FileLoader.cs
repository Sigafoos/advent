using System;
using System.Collections.Generic;
using System.IO;
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
    }
}