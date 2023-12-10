using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Advent2023.Day6;

namespace Advent2023
{
    internal class Day6
    {
        public static void Solve()
        {
            Console.WriteLine(Solve_Part1());
            Console.WriteLine(Solve_Part2());
        }

        public struct Race { public int Time; public int Distance; }

            

        public static int Solve_Part1()
        {
            Race[] RaceInput = new Race[]
            {
                new Race() { Time = 60, Distance = 601 },
                new Race() { Time = 80, Distance = 1163 },
                new Race() { Time = 86, Distance = 1559 },
                new Race() { Time = 76, Distance = 1300 },
                new Race() {Time = 38, Distance = 650 }
            };

            Func<Race, int> BoatRace = new Func<Race, int>((race) =>
            {
                int winWays = 0;
                for (int i = 0; i < race.Time; i++)
                {
                    int dist = (race.Time - i) * i;
                    if (dist > race.Distance)
                        winWays++;
                }
                return winWays;
            });

            int answer = 1;
            foreach (var race in RaceInput)
            {
                answer *= BoatRace(race);
            }
            return answer;
        }

        public static int Solve_Part2()
        {
            long Distance = 601116315591300;
            long Time = 60808676;

            int winWays = 0;
            for (long i = 0; i < Time; i++)
            {
                long dist = (Time - i) * i;
                if (dist > Distance)
                    winWays++;

            }

            return winWays;
        }
    }
}
