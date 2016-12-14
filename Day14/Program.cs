using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Day14
{
    class Program
    {
        private static MD5 md5 = MD5.Create();

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }

        static string[] SplitIntoRuns(string x)
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

        static string[] GetHashRuns(Dictionary<string, string[]> cache, string input, bool stretched)
        {
            if (!cache.ContainsKey(input))
            {
                string hash = CalculateMD5Hash(input);

                if (stretched)
                {
                    for (int i = 0; i < 2016; i++)
                    {
                        hash = CalculateMD5Hash(hash);
                    }
                }

                var runs = SplitIntoRuns(hash);

                cache.Add(input, runs);
            }

            return cache[input];
        }

        static int FindIndex(string salt, int key, bool stretched)
        {
            var cache = new Dictionary<string, string[]>();

            int index = -1;
            int keyCount = 0;
            while (keyCount < key)
            {
                index++;

                var hashRuns = GetHashRuns(cache, salt + index, stretched);
                var run = hashRuns.FirstOrDefault(r => r.Length >= 3);
                if (run == null)
                {
                    continue;
                }

                var followupIndices = Enumerable.Range(index + 1, 1000);
                var followupRuns = followupIndices.Select(x => GetHashRuns(cache, salt + x, stretched));

                bool foundNeededFollowup = followupRuns.Any(runs => runs.Any(r => r.Length >= 5 && r[0] == run[0]));
                if (foundNeededFollowup)
                {
                    Console.WriteLine("{0}:\t{1}", keyCount, index);
                    keyCount++;
                }
            }

            return index;
        }

        static void Main(string[] args)
        {
            string salt = "ngcjuoqr";

            int key = 64;

            var result1 = FindIndex(salt, key, false);

            Console.WriteLine("Answer 1: {0}", result1);


            var result2 = FindIndex(salt, key, true);

            Console.WriteLine("Answer 2: {0}", result2);

            Console.ReadKey();
        }
    }
}
