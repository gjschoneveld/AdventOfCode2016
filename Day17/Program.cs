using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Day17
{
    class Position
    {
        public string passcode;

        public string path = "";

        private const int minX = 0;
        private const int minY = 0;
        private const int maxX = 3;
        private const int maxY = 3;

        public int x = minX;
        public int y = minY;

        public Position[] Next()
        {
            string hash = CalculateMD5Hash(passcode + path);

            var result = new List<Position>();

            // up
            if (IsDoorOpen(hash[0]))
            {
                result.Add(new Position
                {
                    passcode = passcode,
                    path = path + "U",
                    x = x,
                    y = y - 1
                });
            }

            // down
            if (IsDoorOpen(hash[1]))
            {
                result.Add(new Position
                {
                    passcode = passcode,
                    path = path + "D",
                    x = x,
                    y = y + 1
                });
            }

            // left
            if (IsDoorOpen(hash[2]))
            {
                result.Add(new Position
                {
                    passcode = passcode,
                    path = path + "L",
                    x = x - 1,
                    y = y
                });
            }

            // right
            if (IsDoorOpen(hash[3]))
            {
                result.Add(new Position
                {
                    passcode = passcode,
                    path = path + "R",
                    x = x + 1,
                    y = y
                });
            }

            var valid = result.Where(p => p.IsValid());

            return valid.ToArray();
        }

        public bool IsFinished()
        {
            return x == maxX && y == maxY;
        }

        private bool IsValid()
        {
            bool validX = minX <= x && x <= maxX;
            bool validY = minY <= y && y <= maxY;
            return validX && validY;
        }

        private bool IsDoorOpen(char x)
        {
            return 'b' <= x && x <= 'f';
        }

        private static MD5 md5 = MD5.Create();

        private string CalculateMD5Hash(string x)
        {
            // step 1, calculate MD5 hash from input
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(x);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }

    class Program
    {
        static string FindPath(string passcode, bool shortest)
        {
            var queue = new Queue<Position>();
            queue.Enqueue(new Position
            {
                passcode = passcode
            });

            var path = "";
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current.IsFinished())
                {
                    path = current.path;
                    if (shortest)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                var next = current.Next();
                foreach (var item in next)
                {
                    queue.Enqueue(item);
                }
            }

            return path;
        }

        static void Main(string[] args)
        {
            string passcode = "gdjjyniy";

            string path1 = FindPath(passcode, true);
            Console.WriteLine("Answer 1: {0}", path1);

            string path2 = FindPath(passcode, false);
            Console.WriteLine("Answer 2: {0}", path2.Length);

            Console.ReadKey();
        }
    }
}
