using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    public static class Day5_Part2
    {
        public static List<List<Mapping>> mappings = new List<List<Mapping>>();

        public struct Range
        {
            public Range(long start, long len)
            {
                Start = start;
                Len = len;
            }
            public long Start;
            public long Len;
        }

        public struct Mapping
        {
            public Mapping(Range range, long dest)
            {
                Range = range;
                Dest = dest;
            }
            public Range Range;
            public long Dest;
        }

        public static void Solve()
        {
            using (StreamReader reader = new StreamReader(@".\Day5\input.txt"))
            {
                var seeds = reader.ReadLine().Split(':')[1].Trim().Split(' ').Select(p => Convert.ToInt64(p)).ToArray();
                List<long[][]> maplist = new List<long[][]>();

                var maps = reader.ReadToEnd().Split(':').Select(line => line.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
                foreach (var map in maps)
                {
                    long[][] data = map.Where(m => Char.IsDigit(m[0])).Select(m => m.Split(' ').Select(num => Convert.ToInt64(num)).ToArray()).ToArray();
                    if (data.Length > 0) 
                    {
                        mappings.Add(data.Select(p => new Mapping(new Range(p[0], p[1]), p[2])).ToList());
                    }
                }

                long min = long.MaxValue;
                // seeds, maplist[0], maplist[1], maplist[2], maplist[3], maplist[4], maplist[5], maplist[6]);
                for (int i = 0; i < seeds.Count(); i += 2)
                {
                    min = Math.Min(min, CalcRecursively(0, new Range(seeds[i], seeds[i]+1)));
                }

                Console.WriteLine(min);
            }
        }



        public static long CalcRecursively(int level, Range input)
        {
            if (level == mappings.Count)
                return input.Start;

            foreach (var mapping in mappings[level])
            {
                long input_end = input.Start + input.Len;
                long test_end = mapping.Range.Start + mapping.Range.Len;

                long overlap_start = Math.Max(input.Start, mapping.Range.Start);
                long overlap_end = Math.Min(input_end, test_end);
                long overlap_len = overlap_end - overlap_start;
                if (overlap_len <= 0)
                    continue;

                Range overlap = new Range(overlap_start - mapping.Range.Start + mapping.Dest, overlap_len);
                long min = CalcRecursively(level + 1, overlap);

                if(input.Start < overlap_start)
                {
                    var preoverlap = new Range(input.Start, mapping.Range.Start - input.Start);
                    min = Math.Min(min, CalcRecursively(level, preoverlap));
                }

                if(input_end > overlap_end)
                {
                    var postoverlap = new Range(test_end, input_end - test_end);
                    min = Math.Min(min, CalcRecursively(level, postoverlap));
                }

                return min;
            }

            return CalcRecursively(level + 1, input);
        }
    }

}
