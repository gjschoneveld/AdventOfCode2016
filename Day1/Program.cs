using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day1
{
    struct Location
    {
        public int x;
        public int y;

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Location))
            {
                return false;
            }

            Location other = (Location)obj;

            return x == other.x && y == other.y;
        }

        public static bool operator ==(Location a, Location b) {
            return a.Equals(b);
        }

        public static bool operator !=(Location a, Location b)
        {
            return !a.Equals(b);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string input = "R2, L5, L4, L5, R4, R1, L4, R5, R3, R1, L1, L1, R4, L4, L1, R4, L4, R4, L3, R5, R4, R1, R3, L1, L1, R1, L2, R5, L4, L3, R1, L2, L2, R192, L3, R5, R48, R5, L2, R76, R4, R2, R1, L1, L5, L1, R185, L5, L1, R5, L4, R1, R3, L4, L3, R1, L5, R4, L4, R4, R5, L3, L1, L2, L4, L3, L4, R2, R2, L3, L5, R2, R5, L1, R1, L3, L5, L3, R4, L4, R3, L1, R5, L3, R2, R4, R2, L1, R3, L1, L3, L5, R4, R5, R2, R2, L5, L3, L1, L1, L5, L2, L3, R3, R3, L3, L4, L5, R2, L1, R1, R3, R4, L2, R1, L1, R3, R3, L4, L2, R5, R5, L1, R4, L5, L5, R1, L5, R4, R2, L1, L4, R1, L1, L1, L5, R3, R4, L2, R1, R2, R1, R1, R3, L5, R1, R4";

            var commands = input.Split(',').Select(cmd => cmd.Trim());

            const int north = 0;
            const int east = 1;
            const int south = 2;
            const int west = 3;

            int direction = north;

            Location start = new Location();

            HashSet<Location> visited = new HashSet<Location>
            {
                start
            };

            Location firstVisitedTwice = start;

            Location current = start;

            foreach (var cmd in commands)
            {
                // set new direction
                switch (cmd[0])
                {
                    case 'L':
                        direction = (direction + 3) % 4;
                        break;
                    case 'R':
                        direction = (direction + 1) % 4;
                        break;
                }

                // move
                int steps = int.Parse(cmd.Substring(1));
                int dx = 0;
                int dy = 0;
                switch (direction)
                {
                    case north:
                        dy = -1;
                        break;
                    case east:
                        dx = 1;
                        break;
                    case south:
                        dy = 1;
                        break;
                    case west:
                        dx = -1;
                        break;
                }

                for (int i = 1; i <= steps; i++)
                {
                    current = new Location { x = current.x + dx, y = current.y + dy };
                    if (firstVisitedTwice == start && visited.Contains(current))
                    {
                        firstVisitedTwice = current;
                    }

                    if (!visited.Contains(current))
                    {
                        visited.Add(current);
                    }
                }
            }

            int distance1 = Math.Abs(current.x) + Math.Abs(current.y);
            Console.WriteLine("Answer 1: {0}", distance1);

            int distance2 = Math.Abs(firstVisitedTwice.x) + Math.Abs(firstVisitedTwice.y);
            Console.WriteLine("Answer 2: {0}", distance2);

            Console.ReadKey();
        }
    }
}
