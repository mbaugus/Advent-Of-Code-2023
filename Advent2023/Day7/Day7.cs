using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Advent2023.Day7;

namespace Advent2023
{
    public static class Day7
    {
        public static List<Game> Games = new List<Game>();

        public static void Solve()
        {
            using (StreamReader reader = new StreamReader(@".\Day7\input.txt"))
            {
                string[] lines = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                foreach (string line in lines)
                {
                    var split = line.Split(' ');
                    Games.Add(new Game(split[0], Convert.ToInt64(split[1])));
                }
            }

            Console.WriteLine(Solve_Part1());
            Console.WriteLine(Solve_Part2());
        }

        public static long Solve_Part1 () 
        {
            long totalwinnings = 0;

            Games.Sort(new GameSorter());

            for(int i = 0; i < Games.Count; i++)
            {
                totalwinnings += (i + 1) * Games[i].Bid;
            }

            return totalwinnings;
        }

        public static long Solve_Part2()
        {
            CardStrength['J'] = 1;
            long totalwinnings = 0;
            char[] PossibleCardsForJoker = new char[] { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };
            List<Game> GamesWithJokerRule = new List<Game>();

            foreach (var game in Games)
            {
                if (game.Cards.Contains('J'))
                {
                    List<int> JackIndexList = new List<int>();
                    for(int i = 0; i < game.Cards.Length; i++)
                    {
                        if (game.Cards[i] == 'J') 
                            JackIndexList.Add(i);
                    }

                    List<Game> possibleHands = new List<Game>();
                    char[] newHand = new char[game.Cards.Length];
                    game.Cards.ToArray().CopyTo(newHand, 0);

                    for(int i = 0; i < JackIndexList.Count; i++)
                    {
                        newHand[JackIndexList[i]] = '2';
                    }
                    PermutateGameCards(game, newHand, PossibleCardsForJoker, JackIndexList.ToArray(), 0, possibleHands);
                    possibleHands.Sort(new GameSorter(true));
                    var bestHand = possibleHands.Last();
                    GamesWithJokerRule.Add(bestHand);
                }
                else
                {
                    GamesWithJokerRule.Add(game);
                }
            }

            Games = GamesWithJokerRule;

            Games.Sort(new GameSorter());

            for (int i = 0; i < Games.Count; i++)
            {
                long winnings = (i + 1) * Games[i].Bid;
                totalwinnings += winnings;
            }

            return totalwinnings;
        }

        // recursive
        public static void PermutateGameCards(Game currentGame, char[] input, char[] possible , int[] indexes, int currentIndex, List<Game> newGameCards)
        {
            if (currentIndex > indexes.Length - 1) return;

            foreach (char c in possible)
            {
                input[indexes[currentIndex]] = c;
                newGameCards.Add(new Game(new string(input), currentGame.Bid, currentGame.Cards));
                
                PermutateGameCards(currentGame, input, possible, indexes, currentIndex + 1, newGameCards);
            }

            return;
        }

        public class Game
        {
            public string Cards;
            public long Bid;
            public readonly HandType Hand;
            public string OriginalCards;

            public override int GetHashCode()
            {
                return Cards.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return this.GetHashCode() == obj.GetHashCode();
            }

            public Game(string cards, long bid, string originalCards = null)
            {
                OriginalCards = originalCards is null ? cards : originalCards;
                Cards = cards;
                Bid = bid;

                Dictionary<char, int> thecards = new Dictionary<char, int>();
                foreach (char c in Cards)
                {
                    if (thecards.ContainsKey(c))
                    {
                        thecards[c]++;
                    }
                    else
                    {
                        thecards[c] = 1;
                    }
                }
                if (thecards.Values.Any(c => c == 5))
                    Hand = HandType.Five;
                else if (thecards.Values.Any(c => c == 4))
                    Hand = HandType.Four;
                else if (thecards.Values.Any(c => c == 3) && thecards.Values.Any(c => c == 2))
                    Hand = HandType.Full;
                else if (thecards.Values.Any(c => c == 3))
                    Hand = HandType.Three;
                else if (thecards.Values.Count(c => c == 2) == 2)
                    Hand = HandType.TwoPair;
                else if (thecards.Values.Count(c => c == 2) == 1)
                    Hand = HandType.OnePair;
                else
                    Hand = HandType.High;
            }

            public enum HandType
            {
                Five = 50,
                Four = 40,
                Full = 35,
                Three = 30,
                TwoPair = 20,
                OnePair = 10,
                High = 5
            }
        }


        public static readonly Dictionary<char, int> CardStrength = new Dictionary<char, int>()
        {
            {'A', 14  },{'K', 13 }, {'Q',12 }, {'J', 11 },{'T', 10 },{ '9', 9 },{ '8', 8 },{ '7', 7 },{ '6', 6 },{ '5', 5 },{ '4', 4 },{ '3', 3 },{ '2', 2 }
        };

        public class GameSorter : IComparer<Game>
        {
            public bool OnlyJacksInComparison;

            public GameSorter(bool onlyJacksInComparison = false)
            {
                OnlyJacksInComparison = onlyJacksInComparison;
            }

            public int Compare(Game x, Game y)
            {
                if (x.Hand == y.Hand)
                {
                    for (int i = 0; i < 5; i++)
                    {

                        char xcard = !OnlyJacksInComparison && x.OriginalCards[i] == 'J' ? 'J' : x.Cards[i];
                        char ycard = !OnlyJacksInComparison && y.OriginalCards[i] == 'J' ? 'J' : y.Cards[i];

                        int xStr = CardStrength[xcard];
                        int yStr = CardStrength[ycard];

                        if (xStr == yStr)
                        {
                            continue;
                        }
                        else if (xStr > yStr)
                            return 1;
                        else if (xStr < yStr)
                            return -1;
                        else return 0;
                    }
                }
                else if (x.Hand > y.Hand)
                    return 1;
                else if (x.Hand < y.Hand)
                    return -1;

                return 0;
            }
        }
    }
}
