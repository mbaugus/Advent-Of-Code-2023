using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    public static class Day8
    {
        public class Node
        {
            public Node Left;
            public Node Right;
            public string NodeLine;
            public string ThisNode;
            public string LeftNode;
            public string RightNode;
            public bool EndsWithZ = false;
            public int LeftIndex = -1;
            public int RightIndex = -1;
        }

        public static int[] Directions;
        public static Node[] Nodes;

        public static void Solve()
        {
            List<Node> list = new List<Node>();
            using (StreamReader reader = new StreamReader(@".\Day8\input.txt"))
            {
                string[] lines = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];

                    if (i == 0)
                    {
                        Directions = line.Select(p => p == 'L' ? 0 : 1).ToArray();
                        continue;
                    }

                    Node n = new Node();
                    n.NodeLine = line;
                    var split = line.Split('=').ToArray();
                    n.ThisNode = split[0].Trim();
                    n.LeftNode = split[1].Replace("(", "").Replace(")", "").Split(new string[] { ", " }, StringSplitOptions.None).ToArray()[0].Trim();
                    n.RightNode = split[1].Replace("(", "").Replace(")", "").Split(new string[] { ", " }, StringSplitOptions.None).ToArray()[1].Trim();
                    list.Add(n);
                }

                foreach (Node n in list)
                {
                    string NodeName = n.ThisNode;
                    string Right = n.RightNode;
                    string Left = n.LeftNode;

                    var rightConnector = list.Single(p => p.ThisNode == Right);
                    var leftConnector = list.Single(p => p.ThisNode == Left);

                    for (int i = 0; i < list.Count; i++)
                    {
                        var node = list[i];
                        if (node.ThisNode == Right) n.RightIndex = i;
                        if (node.ThisNode == Left) n.LeftIndex = i;

                    }

                    n.Right = rightConnector;
                    n.Left = leftConnector;

                    if (NodeName.EndsWith("Z")) n.EndsWithZ = true;
                }
            }

            Nodes = list.ToArray();

            Console.WriteLine(Solve_Day1());
            Console.WriteLine(Solve_Day2());
        }

        public static long Solve_Day1()
        {
            long steps = 0;
            int directionNumber = 0;
            Node currentNode = Nodes.Single(p => p.ThisNode == "AAA");
            while (currentNode.ThisNode != "ZZZ")
            {
                int dir = Directions[directionNumber];
                currentNode = dir == 0 ? currentNode.Left : currentNode.Right;
                directionNumber++;
                steps++;
                if (directionNumber > Directions.Length - 1) directionNumber = 0;
            }

            return steps;
        }

        public static long Solve_Day2()
        {
           // long steps = 0;
            

            List<Node> StartNodes = Nodes.Where(p => p.ThisNode.EndsWith("A")).ToList();
            List<long> Steps = new List<long>();
            foreach (var node in StartNodes)
            {
                long steps = 0;
                var curretNode = node;
                int directionNumber = 0;

                while (!curretNode.EndsWithZ)
                {
                    curretNode = Directions[directionNumber] == 0 ? Nodes[curretNode.LeftIndex] : Nodes[curretNode.RightIndex];
                    directionNumber++;
                    steps++;
                    if (directionNumber > Directions.Length - 1) directionNumber = 0;
                }

                Steps.Add(steps);
            }
            return Steps.Aggregate(LCM);
        }

        static long LCM(long a, long b)
        {
            long l = a * b;
            long r;

            while(b != 0)
            {
                r = a % b;
                a = b;
                b = r;
            }

            return l / a;
        }

    }
}
