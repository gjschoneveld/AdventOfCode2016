using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day8
{
    static class FieldExtensions
    {
        public static void Apply(this bool[,] field, Command cmd)
        {
            cmd.Apply(field);
        }
    }

    abstract class Command
    {
        public abstract void Apply(bool[,] field);

        public static Command Parse(string x)
        {
            var parts = x.Split(' ');

            if (parts[0] == "rect")
            {
                var size = parts[1].Split('x').Select(s => int.Parse(s));
                int width = size.First();
                int height = size.Last();

                return new RectangleCommand
                {
                    width = width,
                    height = height
                };
            }


            int amount = int.Parse(parts[4]);
            int position = int.Parse(parts[2].Split('=')[1]);

            if (parts[1] == "row")
            {
                return new RowRotateCommand
                {
                    y = position,
                    amount = amount
                };
            }
            else
            {
                return new ColumnRotateCommand
                {
                    x = position,
                    amount = amount
                };
            }
        }
    }

    class RectangleCommand : Command
    {
        public int width;
        public int height;

        public override void Apply(bool[,] field)
        {
            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < width; c++)
                {
                    field[r, c] = true;
                }
            }
        }
    }

    abstract class RotateCommand : Command
    {
        public int amount;

        protected bool[] Rotate(bool[] x)
        {
            bool[] result = new bool[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                int pos = (i + amount) % x.Length;
                result[pos] = x[i];
            }

            return result;
        }
    }

    class ColumnRotateCommand : RotateCommand
    {
        public int x;

        private bool[] ExtractColumn(bool[,] field)
        {
            bool[] result = new bool[field.GetLength(0)];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = field[i, x];
            }

            return result;
        }

        private void PlaceColumn(bool[,] field, bool[] column)
        {
            for (int i = 0; i < column.Length; i++)
            {
                field[i, x] = column[i];
            }
        }

        public override void Apply(bool[,] field)
        {
            var column = ExtractColumn(field);
            var rotated = Rotate(column);
            PlaceColumn(field, rotated);
        }
    }

    class RowRotateCommand : RotateCommand
    {
        public int y;

        private bool[] ExtractRow(bool[,] field)
        {
            bool[] result = new bool[field.GetLength(1)];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = field[y, i];
            }

            return result;
        }

        private void PlaceRow(bool[,] field, bool[] row)
        {
            for (int i = 0; i < row.Length; i++)
            {
                field[y, i] = row[i];
            }
        }

        public override void Apply(bool[,] field)
        {
            var row = ExtractRow(field);
            var rotated = Rotate(row);
            PlaceRow(field, rotated);
        }
    }

    class Program
    {
        static void Print(bool[,] field)
        {
            for (int r = 0; r < field.GetLength(0); r++)
            {
                for (int c = 0; c < field.GetLength(1); c++)
                {
                    Console.Write(field[r, c] ? '#' : ' ');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            string[] input = {
                "rect 1x1",
                "rotate row y=0 by 5",
                "rect 1x1",
                "rotate row y=0 by 5",
                "rect 1x1",
                "rotate row y=0 by 5",
                "rect 1x1",
                "rotate row y=0 by 5",
                "rect 1x1",
                "rotate row y=0 by 2",
                "rect 1x1",
                "rotate row y=0 by 2",
                "rect 1x1",
                "rotate row y=0 by 3",
                "rect 1x1",
                "rotate row y=0 by 3",
                "rect 2x1",
                "rotate row y=0 by 2",
                "rect 1x1",
                "rotate row y=0 by 3",
                "rect 2x1",
                "rotate row y=0 by 2",
                "rect 1x1",
                "rotate row y=0 by 3",
                "rect 2x1",
                "rotate row y=0 by 5",
                "rect 4x1",
                "rotate row y=0 by 5",
                "rotate column x=0 by 1",
                "rect 4x1",
                "rotate row y=0 by 10",
                "rotate column x=5 by 2",
                "rotate column x=0 by 1",
                "rect 9x1",
                "rotate row y=2 by 5",
                "rotate row y=0 by 5",
                "rotate column x=0 by 1",
                "rect 4x1",
                "rotate row y=2 by 5",
                "rotate row y=0 by 5",
                "rotate column x=0 by 1",
                "rect 4x1",
                "rotate column x=40 by 1",
                "rotate column x=27 by 1",
                "rotate column x=22 by 1",
                "rotate column x=17 by 1",
                "rotate column x=12 by 1",
                "rotate column x=7 by 1",
                "rotate column x=2 by 1",
                "rotate row y=2 by 5",
                "rotate row y=1 by 3",
                "rotate row y=0 by 5",
                "rect 1x3",
                "rotate row y=2 by 10",
                "rotate row y=1 by 7",
                "rotate row y=0 by 2",
                "rotate column x=3 by 2",
                "rotate column x=2 by 1",
                "rotate column x=0 by 1",
                "rect 4x1",
                "rotate row y=2 by 5",
                "rotate row y=1 by 3",
                "rotate row y=0 by 3",
                "rect 1x3",
                "rotate column x=45 by 1",
                "rotate row y=2 by 7",
                "rotate row y=1 by 10",
                "rotate row y=0 by 2",
                "rotate column x=3 by 1",
                "rotate column x=2 by 2",
                "rotate column x=0 by 1",
                "rect 4x1",
                "rotate row y=2 by 13",
                "rotate row y=0 by 5",
                "rotate column x=3 by 1",
                "rotate column x=0 by 1",
                "rect 4x1",
                "rotate row y=3 by 10",
                "rotate row y=2 by 10",
                "rotate row y=0 by 5",
                "rotate column x=3 by 1",
                "rotate column x=2 by 1",
                "rotate column x=0 by 1",
                "rect 4x1",
                "rotate row y=3 by 8",
                "rotate row y=0 by 5",
                "rotate column x=3 by 1",
                "rotate column x=2 by 1",
                "rotate column x=0 by 1",
                "rect 4x1",
                "rotate row y=3 by 17",
                "rotate row y=2 by 20",
                "rotate row y=0 by 15",
                "rotate column x=13 by 1",
                "rotate column x=12 by 3",
                "rotate column x=10 by 1",
                "rotate column x=8 by 1",
                "rotate column x=7 by 2",
                "rotate column x=6 by 1",
                "rotate column x=5 by 1",
                "rotate column x=3 by 1",
                "rotate column x=2 by 2",
                "rotate column x=0 by 1",
                "rect 14x1",
                "rotate row y=1 by 47",
                "rotate column x=9 by 1",
                "rotate column x=4 by 1",
                "rotate row y=3 by 3",
                "rotate row y=2 by 10",
                "rotate row y=1 by 8",
                "rotate row y=0 by 5",
                "rotate column x=2 by 2",
                "rotate column x=0 by 2",
                "rect 3x2",
                "rotate row y=3 by 12",
                "rotate row y=2 by 10",
                "rotate row y=0 by 10",
                "rotate column x=8 by 1",
                "rotate column x=7 by 3",
                "rotate column x=5 by 1",
                "rotate column x=3 by 1",
                "rotate column x=2 by 1",
                "rotate column x=1 by 1",
                "rotate column x=0 by 1",
                "rect 9x1",
                "rotate row y=0 by 20",
                "rotate column x=46 by 1",
                "rotate row y=4 by 17",
                "rotate row y=3 by 10",
                "rotate row y=2 by 10",
                "rotate row y=1 by 5",
                "rotate column x=8 by 1",
                "rotate column x=7 by 1",
                "rotate column x=6 by 1",
                "rotate column x=5 by 1",
                "rotate column x=3 by 1",
                "rotate column x=2 by 2",
                "rotate column x=1 by 1",
                "rotate column x=0 by 1",
                "rect 9x1",
                "rotate column x=32 by 4",
                "rotate row y=4 by 33",
                "rotate row y=3 by 5",
                "rotate row y=2 by 15",
                "rotate row y=0 by 15",
                "rotate column x=13 by 1",
                "rotate column x=12 by 3",
                "rotate column x=10 by 1",
                "rotate column x=8 by 1",
                "rotate column x=7 by 2",
                "rotate column x=6 by 1",
                "rotate column x=5 by 1",
                "rotate column x=3 by 1",
                "rotate column x=2 by 1",
                "rotate column x=1 by 1",
                "rotate column x=0 by 1",
                "rect 14x1",
                "rotate column x=39 by 3",
                "rotate column x=35 by 4",
                "rotate column x=20 by 4",
                "rotate column x=19 by 3",
                "rotate column x=10 by 4",
                "rotate column x=9 by 3",
                "rotate column x=8 by 3",
                "rotate column x=5 by 4",
                "rotate column x=4 by 3",
                "rotate row y=5 by 5",
                "rotate row y=4 by 5",
                "rotate row y=3 by 33",
                "rotate row y=1 by 30",
                "rotate column x=48 by 1",
                "rotate column x=47 by 5",
                "rotate column x=46 by 5",
                "rotate column x=45 by 1",
                "rotate column x=43 by 1",
                "rotate column x=38 by 3",
                "rotate column x=37 by 3",
                "rotate column x=36 by 5",
                "rotate column x=35 by 1",
                "rotate column x=33 by 1",
                "rotate column x=32 by 5",
                "rotate column x=31 by 5",
                "rotate column x=30 by 1",
                "rotate column x=23 by 4",
                "rotate column x=22 by 3",
                "rotate column x=21 by 3",
                "rotate column x=20 by 1",
                "rotate column x=12 by 2",
                "rotate column x=11 by 2",
                "rotate column x=3 by 5",
                "rotate column x=2 by 5",
                "rotate column x=1 by 3",
                "rotate column x=0 by 4"
             };

            var commands = input.Select(x => Command.Parse(x)).ToList();

            int fieldWidth = 50;
            int fieldHeight = 6;
            bool[,] field = new bool[fieldHeight, fieldWidth];

            foreach (var cmd in commands)
            {
                field.Apply(cmd);
            }

            int result1 = field.Cast<bool>().Count(cell => cell);

            Console.WriteLine("Answer 1: {0}", result1);

            Console.WriteLine("Answer 2:");
            Print(field);

            Console.ReadKey();
        }
    }
}
