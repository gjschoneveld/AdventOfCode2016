using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Day24
{
    class Positon
    {
        public int x;
        public int y;
    }

    class State : IEquatable<State>
    {
        public int x;
        public int y;

        public bool[] visited;

        public char[,] maze;

        public State[] Next()
        {
            List<State> result = new List<State>();

            Positon[] neighbours = {
                new Positon { x = x, y = y - 1 },
                new Positon { x = x - 1, y = y },
                new Positon { x = x + 1, y = y },
                new Positon { x = x, y = y + 1 }
            };

            foreach (var nb in neighbours)
            {
                if (maze[nb.y, nb.x] == '#')
                {
                    continue;
                }

                var newVisited = visited;
                if (maze[nb.y, nb.x] != '0' && char.IsDigit(maze[nb.y, nb.x]))
                {
                    newVisited = visited.Clone() as bool[];
                    newVisited[maze[nb.y, nb.x] - '0' - 1] = true;
                }

                result.Add(new State
                {
                    x = nb.x,
                    y = nb.y,
                    visited = newVisited,
                    maze = maze
                });
            }

            return result.ToArray();
        }

        public override int GetHashCode()
        {
            var hash = (x << 16) ^ y;

            for (int i = 0; i < visited.Length; i++)
            {
                hash <<= 1;
                if (visited[i])
                {
                    hash |= 1;
                }
            }

            return hash;
        }

        public override bool Equals(object obj)
        {
            var other = obj as State;
            if (other == null)
            {
                return false;
            }
            
            return Equals(other);
        }

        public bool Equals(State other)
        {
            if (other == null)
            {
                return false;
            }

            bool equalX = x == other.x;
            bool equalY = y == other.y;
            bool equalVisited = visited.SequenceEqual(other.visited);

            return equalX && equalY && equalVisited;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var maze = new char[input.Length, input[0].Length];
            int max = 0;
            int startX = 0;
            int startY = 0;

            for (int r = 0; r < input.Length; r++)
            {
                for (int c = 0; c < input[r].Length; c++)
                {
                    maze[r, c] = input[r][c];

                    if (maze[r, c] == '0')
                    {
                        startX = c;
                        startY = r;
                    }
                    else if (char.IsDigit(maze[r, c]))
                    {
                        max = Math.Max(max, maze[r, c] - '0');
                    }
                }
            }

            State start = new State
            {
                x = startX,
                y = startY,
                visited = new bool[max],
                maze = maze
            };

            State[] current = { start };
            var seen = new HashSet<State>(current);

            int steps = 0;
            while (!current.Any(s => s.visited.All(v => v)))
            {
                var next = current.SelectMany(s => s.Next()).Distinct().ToArray();
                var newNext = next.Where(s => !seen.Contains(s)).ToArray();

                seen.UnionWith(newNext);

                current = newNext;
                steps++;
            }

            Console.WriteLine("Answer 1: {0}", steps);


            current = new State[] { start };
            seen = new HashSet<State>(current);

            steps = 0;
            while (!current.Any(s => s.x == startX && s.y == startY && s.visited.All(v => v)))
            {
                var next = current.SelectMany(s => s.Next()).Distinct().ToArray();
                var newNext = next.Where(s => !seen.Contains(s)).ToArray();

                seen.UnionWith(newNext);

                current = newNext;
                steps++;
            }

            Console.WriteLine("Answer 2: {0}", steps);

            Console.ReadKey();
        }
    }
}
