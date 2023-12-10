using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class Day4
    {
        public static void Solve()
        {
            using (StreamReader reader = new StreamReader(@".\Day4\input.txt"))
            {
                var input = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None)
                    .Select(p => p.Substring(p.IndexOf(":") + 1).Trim()).Select(p => p.Split('|')).ToArray();

                var l = input.Select(p => p[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                                              .Select(p => p.Select(n => Convert.ToInt32(n)).ToArray()).ToArray();

                var r = input.Select(p => p[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                                              .Select(p => p.Select(n => Convert.ToInt32(n)).ToArray()).ToArray();

                var games = l.Select((p, i) => new ScratchOff(p, r[i])).ToArray();

                Console.WriteLine(Solve_Part1(games));
                Console.WriteLine(Solve_Part2(games, games, 0));
            }
          
        }

        private class ScratchOff
        {
            public int[] WinningNumbers { get; set; }
            public int[] MyNumbers { get; set; }

            public ScratchOff(IEnumerable<int> winningNumbers, IEnumerable<int> myNumbers)
            {
                WinningNumbers = winningNumbers.ToArray();
                MyNumbers = myNumbers.ToArray();
            }

            public int Score() => MyNumbers.Where(p => WinningNumbers.Contains(p)).Count();
        }

        private static int Solve_Part1(ScratchOff[] cards)
        {
            return cards.Select(card => (int)Math.Pow(2, card.Score() - 1)).Where(score => score > 0).Sum();
        }
        private static int Solve_Part2(ScratchOff[] allCards, ScratchOff[] toCheck, int j)
        {
            int sum = 0;
            for (int i = 0; i < toCheck.Length; i++)
            {
                sum++;
                int score = allCards[j + i].Score();
                var copies = allCards.Skip(j + i + 1).Take(score).ToArray();
                sum += Solve_Part2(allCards, copies, j + i + 1);
            }
            return sum;
        }
    }
}
