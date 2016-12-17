using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day16
{
    class Program
    {
        static string Unfold(string x)
        {
            return x + "0" + new string(x.Reverse().Select(c => c == '0' ? '1' : '0').ToArray());
        }

        static string GenerateData(string x, int length)
        {
            while (x.Length < length)
            {
                x = Unfold(x);
            }

            return new string(x.Take(length).ToArray());
        }

        static string Checksum(string x)
        {
            while (x.Length % 2 == 0)
            {
                StringBuilder cs = new StringBuilder(x.Length / 2);

                for (int i = 0; i < x.Length; i += 2)
                {
                    cs.Append(x[i] == x[i + 1] ? '1' : '0');
                }

                x = cs.ToString();
            }

            return x;
        }

        static void Main(string[] args)
        {
            var data1 = GenerateData("01111001100111011", 272);
            var cs1 = Checksum(data1);

            Console.WriteLine("Answer 1: {0}", cs1);


            var data2 = GenerateData("01111001100111011", 35651584);
            var cs2 = Checksum(data2);

            Console.WriteLine("Answer 2: {0}", cs2);

            Console.ReadKey();
        }
    }
}
