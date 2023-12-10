using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class Day1
    {
        public static void Solve()
        {
            using (StreamReader reader = new StreamReader(@".\Day1\input.txt"))
            {
                string[] input = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                Console.WriteLine($"Day 1: Part 1: {Solve_Part1(input)}");
                Console.WriteLine($"Day 1: Part 2: {Solve_Part2(input)}");
            }
        }

        public static int Solve_Part1(string[] lines)
        {
            return lines.Select(line => 
                Convert.ToInt32(
                    String.Concat(
                        line.First(c => Char.IsDigit(c)), 
                        line.Last(c => Char.IsDigit(c))
                ))).Sum();
        }

        public static int Solve_Part2(string[] lines)
        {
            Dictionary<string, string> coded = new Dictionary<string, string>()
            {
                { "1" ,"1"}, { "2", "2"}, {"3", "3" }, { "4" ,"4"}, { "5", "5"}, {"6", "6" },{ "7" ,"7"}, { "8", "8"}, {"9", "9" },
                { "one", "1"}, { "two", "2"} ,{"three", "3" }, { "four" ,"4"}, { "five", "5"}, {"six", "6" },{ "seven" ,"7"}, { "eight", "8"}, {"nine", "9" },
            };

            int total = 0;
            foreach (string line in lines)
            {
                string first = null, last = null;
                // checks every permutation going from start and end until it has a number from both sides
                for (int b = 0, e = line.Length - 1; b < line.Length; b++, e--)
                {
                    for (int i = b, x = e; i < line.Length; i++, x--)
                    {
                        if (first == null)  coded.TryGetValue(line.Substring(b, i - b + 1), out first);
                        if (last == null)   coded.TryGetValue(line.Substring(x, e - x + 1), out last);
                    }
                }

                total += Convert.ToInt32(first + last);
            }

            return total;

        }

    }
}
