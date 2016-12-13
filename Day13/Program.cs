using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day13
{
    class Settings
    {
        public int favoriteNumber;
        public Position end;
    }

    class Position : IEquatable<Position>
    {
        public Position previous;

        public int x;
        public int y;

        public Settings settings;

        public override int GetHashCode()
        {
            return x ^ y;
        }

        public override bool Equals(object obj)
        {
            Position other = obj as Position;

            if (other == null)
            {
                return false;
            }

            return x == other.x && y == other.y;
        }

        public bool Equals(Position other)
        {
            return Equals((object)other);
        }

        public bool IsFinished()
        {
            return this.Equals(settings.end);
        }

        public bool IsValid()
        {
            if (x < 0 || y < 0)
            {
                return false;
            }

            int number = x * x + 3 * x + 2 * x * y + y + y * y + settings.favoriteNumber;
            int count = Convert.ToString(number, 2).Count(c => c == '1');
            bool isEven = count % 2 == 0;

            return isEven;
        }

        public Position[] Next()
        {
            var results = new Position[] {
                new Position { previous = this, settings = settings, x = x - 1, y = y },
                new Position { previous = this, settings = settings, x = x + 1, y = y },
                new Position { previous = this, settings = settings, x = x, y = y - 1 },
                new Position { previous = this, settings = settings, x = x, y = y + 1 },
            };

            var validResults = results.Where(s => s.IsValid());

            return validResults.ToArray();
        }
    }

    class Program
    {
        static List<Position> FindPath(Position start, Position end, int favoriteNumber)
        {
            start.settings = new Settings
            {
                favoriteNumber = favoriteNumber,
                end = end
            };

            var seen = new HashSet<Position> { start };

            var current = new List<Position> { start };
            while (!current.Any(s => s.IsFinished()))
            {
                var next = current.SelectMany(s => s.Next()).Distinct().ToList();

                var newNext = next.Where(s => !seen.Contains(s)).ToList();

                seen.UnionWith(newNext);

                current = newNext;
            }

            var state = current.First(s => s.IsFinished());
            var path = new List<Position> { state };
            while (state.previous != null)
            {
                state = state.previous;
                path.Add(state);
            }
            path.Reverse();

            return path;
        }

        static HashSet<Position> FindReachable(Position start, int steps, int favoriteNumber)
        {
            start.settings = new Settings
            {
                favoriteNumber = favoriteNumber,
            };

            var seen = new HashSet<Position> { start };

            var current = new List<Position> { start };
            for (int i = 0; i < steps; i++)
            {
                var next = current.SelectMany(s => s.Next()).Distinct().ToList();

                var newNext = next.Where(s => !seen.Contains(s)).ToList();

                seen.UnionWith(newNext);

                current = newNext;
            }

            return seen;
        }

        static void Main(string[] args)
        {
            int favoriteNumber = 1358;
            var start = new Position { x = 1, y = 1 };
            var end = new Position { x = 31, y = 39 };

            var path = FindPath(start, end, favoriteNumber);

            Console.WriteLine("Answer 1: {0}", path.Count - 1);


            int steps = 50;
            var seen = FindReachable(start, steps, favoriteNumber);

            Console.WriteLine("Answer 2: {0}", seen.Count);

            Console.ReadKey();
        }
    }
}
