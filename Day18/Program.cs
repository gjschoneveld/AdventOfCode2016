using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day18
{
    class Program
    {
        const char safeSymbol = '.';
        const char trapSymbol = '^';

        static bool IsSafe(string row, int index)
        {
            if (index < 0 || row.Length <= index)
            {
                return true;
            }

            return row[index] == safeSymbol;
        }

        static string NextRow(string row)
        {
            StringBuilder next = new StringBuilder(row.Length);

            for (int i = 0; i < row.Length; i++)
            {
                bool left = IsSafe(row, i - 1);
                bool right = IsSafe(row, i + 1);

                bool trap = left ^ right;
                if (trap)
                {
                    next.Append(trapSymbol);
                }
                else
                {
                    next.Append(safeSymbol);
                }
            }

            return next.ToString();
        }

        static int CountSafe(string start, int rows)
        {
            var row = start;
            int count = start.Count(t => t == safeSymbol);

            for (int i = 1; i < rows; i++)
            {
                row = NextRow(row);
                count += row.Count(t => t == safeSymbol);
            }

            return count;
        }

        static void Main(string[] args)
        {
            var start = "^..^^.^^^..^^.^...^^^^^....^.^..^^^.^.^.^^...^.^.^.^.^^.....^.^^.^.^.^.^.^.^^..^^^^^...^.....^....^.";

            int rows1 = 40;
            int result1 = CountSafe(start, rows1);

            Console.WriteLine("Answer 1: {0}", result1);


            int rows2 = 400000;
            int result2 = CountSafe(start, rows2);

            Console.WriteLine("Answer 2: {0}", result2);

            Console.ReadKey();
        }
    }
}
