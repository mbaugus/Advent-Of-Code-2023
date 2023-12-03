using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Advent2024.Day3;

namespace Advent2024
{
    internal static class Day3
    {
        public static void Solve()
        {
            using (StreamReader reader = new StreamReader(@".\Day3\input.txt"))
            {
                List<SchematicLocation> schematicPoints = new List<SchematicLocation>();

                string line = null;
                int y = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    Partnumber CapturingPartnumber = null;
                    int captureStart = -1;
                    for(int x = 0; x < line.Length; x++)
                    {
                        char symbol = line[x];
                        SchematicLocation point = new SchematicLocation(schematicPoints.Count, symbol.ToString(), null);

                        if (Char.IsDigit(symbol))
                        {
                            if(captureStart == -1)
                            {
                                CapturingPartnumber = new Partnumber() { Value = symbol.ToString()};
                                captureStart = x;
                            }
                            else
                            {
                                CapturingPartnumber.Value += symbol;
                            }

                            point.PN = CapturingPartnumber;
                        }
                        else 
                        {
                            if(captureStart != -1)
                            {
                                captureStart = -1;
                                CapturingPartnumber = null;
                            }
                        }
                        schematicPoints.Add(point);
                    }
                    y++;
                }

               
                Schematic schematic = new Schematic(schematicPoints, y);
                Console.WriteLine($"Day 3: Part 1: {Solve_Part1(schematic)}");
                Console.WriteLine($"Day 3: Part 2: {Solve_Part2(schematic)}");
            }
        }

        public class Schematic
        {
            public readonly int RowSize;
            public readonly SchematicLocation[] Map;

            public Schematic(IEnumerable<SchematicLocation> schematicMap, int rowSize)
            {
                Map = schematicMap.ToArray();
                RowSize = rowSize;
            }

            private bool ValidIndex(int index) => index <= Map.Length - 1 && index > 0;

            public SchematicLocation[] GetSurrounding(int index)
            {
                if (!ValidIndex(index)) return new SchematicLocation[0];
                int[] ordinals = new int[]
                {
                    index + 1,
                    index - 1,
                    index + RowSize,
                    index - RowSize,
                    index - RowSize - 1,
                    index - RowSize + 1,
                    index + RowSize - 1,
                    index + RowSize + 1
                };

                return ordinals.Where(o => ValidIndex(o)).Select(i => Map[i]).ToArray();
            }
        }

        public class SchematicLocation
        {
            public SchematicLocation(int index, string symbol, Partnumber partnumberLink = null)
            {
                Symbol = symbol;
                PN = partnumberLink;
                Index = index;
            }
            public readonly int Index;
            public readonly string Symbol;
            public Partnumber PN { get; set; }
            public bool HasPN => PN != null;
            public bool IsIndicator => !Char.IsDigit(Symbol[0]) && Symbol[0] != '.';
        }

        public class Partnumber
        {
            public Partnumber()
            {
                ID = ++LastID;
                LastID = ID;
            }
            public override string ToString() => Value.ToString();
            public override int GetHashCode() => ID.GetHashCode();
            public readonly int ID;
            public string Value { get; set; } = String.Empty;
            public int ToInt() => Convert.ToInt32(Value);
            private static int LastID = 0;
        }

        public static int Solve_Part1(Schematic schematic)
        {
            List<Partnumber> finalpartnumbers = new List<Partnumber>();

            foreach (var location in schematic.Map.Where(p => p.HasPN))
            {
                var surroundingIndicators = schematic.GetSurrounding(location.Index).Where(p => p.IsIndicator).ToArray();
                if (surroundingIndicators.Length > 0 && !finalpartnumbers.Contains(location.PN))
                    finalpartnumbers.Add(location.PN);
            }

            return finalpartnumbers.Select(p => Convert.ToInt32(p.Value)).Sum();
        }

        public static int Solve_Part2(Schematic schematic)
        {
            List<int> gearRatios = new List<int>();

            foreach (var gear in schematic.Map.Where(p => p.Symbol == "*"))
            {
                var surroundingPNs = schematic.GetSurrounding(gear.Index)
                                              .Where(p => p.HasPN)
                                              .Select(p => p.PN)
                                              .Distinct().ToArray();

                if (surroundingPNs.Length == 2)
                   gearRatios.Add(surroundingPNs[0].ToInt() * surroundingPNs[1].ToInt());
            }

            return gearRatios.Sum();
        }
    }


}

