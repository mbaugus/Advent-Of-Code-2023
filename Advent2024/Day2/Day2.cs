using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Advent2024
{
    internal class Day2
    {
        public static void Solve()
        {
            using (StreamReader reader = new StreamReader(@".\Day2\input.txt"))
            {
                Game[] games = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None).Select(line =>
                {
                    var gameSplit = line.Split(':').ToArray();
                    var minigamesSplit = gameSplit[1].Split(';').Select(g => g.Trim()).ToArray();
                    List<BagContents> bagreveals = new List<BagContents>();
                    foreach (var s in minigamesSplit)
                    {
                        var colors = s.Split(',').Select(ss => ss.Trim()).ToArray();
                        bagreveals.Add(new BagContents()
                        {
                            Red = Convert.ToInt32(colors.Where(c => c.Contains("red")).SingleOrDefault()?.Split(' ').ToArray()[0]),
                            Green = Convert.ToInt32(colors.Where(c => c.Contains("green")).SingleOrDefault()?.Split(' ').ToArray()[0]),
                            Blue = Convert.ToInt32(colors.Where(c => c.Contains("blue")).SingleOrDefault()?.Split(' ').ToArray()[0])

                        });
                    }

                    return new Game(Convert.ToInt32(gameSplit[0].Replace("Game ", "")), bagreveals);
                }).ToArray();

                Console.WriteLine($"Day 2: Part 1: {Solve_Part1(games)}");
                Console.WriteLine($"Day 2: Part 2: {Solve_Part2(games)}");
            }
        }

        private class Game
        {
            public Game(int gameID, IEnumerable<BagContents> revealedContents)
            {
                GameID = gameID;
                Reveals = revealedContents.ToArray();
            }

            public int MaxGreen => Reveals.Select(p => p.Green).Where(p => p > 0).Max();
            public int MaxRed => Reveals.Select(p => p.Red).Where(p => p > 0).Max();
            public int MaxBlue => Reveals.Select(p => p.Blue).Where(p => p > 0).Max();
            public int GameID { get; private set; }
            public BagContents[] Reveals { get; private set; }
        }

        private class BagContents
        {
            public int Red { get; set; }
            public int Green { get; set; }
            public int Blue { get; set; }     
        }

        private static int Solve_Part1(Game[] games)
        {
            return games.Where(game => game.MaxRed <= 12 &&  game.MaxGreen <= 13 &&  game.MaxBlue <= 14).Select(p => p.GameID).Sum();

        }

        private static int Solve_Part2(Game[] games)
        {
            return games.Select(game => game.MaxRed * game.MaxGreen * game.MaxBlue).Sum();
        }
    }
}
