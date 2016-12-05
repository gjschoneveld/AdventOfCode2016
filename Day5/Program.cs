using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Day5
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

        static void Main(string[] args)
        {
            string id = "cxdnnyjw";

            const int passwordLength = 8;

            string password1 = "";

            int index = 0;
            while (password1.Length < passwordLength)
            {
                string candidate = id + index;
                string hash = CalculateMD5Hash(candidate);
                if (hash.StartsWith("00000"))
                {
                    password1 += hash[5];
                    Console.WriteLine(password1);
                }

                index++;
            }

            Console.WriteLine("Answer 1: {0}", password1);


            const char defaultCharacter = '_';

            string password2 = new string(defaultCharacter, passwordLength);

            index = 0;
            while (password2.Contains(defaultCharacter))
            {
                string candidate = id + index;
                string hash = CalculateMD5Hash(candidate);

                int passwordIndex = hash[5] - '0';
                bool validIndex = 0 <= passwordIndex && passwordIndex < password2.Length;
                bool characterMissing = validIndex && password2[passwordIndex] == defaultCharacter;
                if (hash.StartsWith("00000") && characterMissing)
                {
                    var characters = password2.ToCharArray();
                    characters[passwordIndex] = hash[6];
                    password2 = new string(characters);

                    Console.WriteLine(password2);
                }

                index++;
            }

            Console.WriteLine("Answer 2: {0}", password2);

            Console.ReadKey();
        }
    }
}
