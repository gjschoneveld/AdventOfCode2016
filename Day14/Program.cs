using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Day14
{
    class Solver
    {
        public string salt;
        public bool stretched;

        private Dictionary<string, string[]> cache = new Dictionary<string, string[]>();

        private MD5 md5 = MD5.Create();

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

        private string CalculateHash(string x)
        {
            string hash = CalculateMD5Hash(x);

            if (stretched)
            {
                for (int i = 0; i < 2016; i++)
                {
                    hash = CalculateMD5Hash(hash);
                }
            }

            return hash;
        }

        private string[] SplitIntoRuns(string x)
        {
            var result = new List<string>();

            int index = 0;
            while (index < x.Length)
            {
                int end = index;
                while (end < x.Length && x[index] == x[end])
                {
                    end++;
                }

                int length = end - index;
                var run = x.Substring(index, length);
                result.Add(run);

                index += length;
            }

            return result.ToArray();
        }

        private string[] GetHashRuns(string x)
        {
            if (!cache.ContainsKey(x))
            {
                string hash = CalculateHash(x);
                var runs = SplitIntoRuns(hash);
                cache.Add(x, runs);
            }

            return cache[x];
        }

        private bool IsKey(int index)
        {
            var hashRuns = GetHashRuns(salt + index);
            var run = hashRuns.FirstOrDefault(r => r.Length >= 3);
            if (run == null)
            {
                return false;
            }

            var followupIndices = Enumerable.Range(index + 1, 1000);
            var followupRuns = followupIndices.Select(i => GetHashRuns(salt + i));

            bool foundNeededFollowup = followupRuns.Any(runs => runs.Any(r => r.Length >= 5 && r[0] == run[0]));
            return foundNeededFollowup;
        }

        public int FindIndex(int key)
        {
            int index = -1;
            int keyCount = 0;
            while (keyCount < key)
            {
                index++;

                if (IsKey(index))
                {
                    Console.WriteLine("{0}:\t{1}", keyCount, index);
                    keyCount++;
                }
            }

            return index;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string salt = "ngcjuoqr";

            int key = 64;


            var solver1 = new Solver
            {
                salt = salt
            };

            var result1 = solver1.FindIndex(key);

            Console.WriteLine("Answer 1: {0}", result1);


            var solver2 = new Solver
            {
                salt = salt,
                stretched = true
            };

            var result2 = solver2.FindIndex(key);

            Console.WriteLine("Answer 2: {0}", result2);

            Console.ReadKey();
        }
    }
}
