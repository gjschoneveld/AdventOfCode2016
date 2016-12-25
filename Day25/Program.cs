using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day25
{
    class Computer
    {
        public string[] instructionMemory;
        private int programCounter;
        private Dictionary<string, int> registerFile = new Dictionary<string, int>
        {
            { "a", 0 },
            { "b", 0 },
            { "c", 0 },
            { "d", 0 }
        };

        public int Read(string x)
        {
            if (char.IsLetter(x, 0))
            {
                return registerFile[x];
            }

            return int.Parse(x);
        }

        public void Write(string reg, int val)
        {
            if (registerFile.ContainsKey(reg))
            {
                registerFile[reg] = val;
            }
        }

        public bool Run()
        {
            var states = new List<Dictionary<string, int>>();
            int expected = 0;

            while (0 <= programCounter && programCounter < instructionMemory.Length)
            {
                var instruction = instructionMemory[programCounter];

                var parts = instruction.Split(' ');
                var opcode = parts[0];

                int offset = 1;
                switch (opcode)
                {
                    case "cpy":
                        Write(parts[2], Read(parts[1]));
                        break;
                    case "inc":
                        Write(parts[1], Read(parts[1]) + 1);
                        break;
                    case "dec":
                        Write(parts[1], Read(parts[1]) - 1);
                        break;
                    case "jnz":
                        if (Read(parts[1]) != 0)
                        {
                            offset = Read(parts[2]);
                        }
                        break;
                    case "out":
                        var x = Read(parts[1]);
                        if (x != expected)
                        {
                            return false;
                        }

                        expected = 1 - x;

                        if (x == 0)
                        {
                            foreach (var state in states)
                            {
                                bool seen = true;
                                foreach (var kv in registerFile)
                                {
                                    if (kv.Value != state[kv.Key])
                                    {
                                        seen = false;
                                    }
                                }

                                if (seen)
                                {
                                    return true;
                                }
                            }

                            states.Add(registerFile.ToDictionary(r => r.Key, r => r.Value));
                        }

                        break;
                }

                programCounter += offset;
            }

            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] input = {
                "cpy a d",
                "cpy 15 c",
                "cpy 170 b",
                "inc d",
                "dec b",
                "jnz b -2",
                "dec c",
                "jnz c -5",
                "cpy d a",
                "jnz 0 0",
                "cpy a b",
                "cpy 0 a",
                "cpy 2 c",
                "jnz b 2",
                "jnz 1 6",
                "dec b",
                "dec c",
                "jnz c -4",
                "inc a",
                "jnz 1 -7",
                "cpy 2 b",
                "jnz c 2",
                "jnz 1 4",
                "dec b",
                "dec c",
                "jnz 1 -4",
                "jnz 0 0",
                "out b",
                "jnz a -19",
                "jnz 1 -21"
            };

            int a = -1;
            bool done = false;
            while (!done)
            {
                a++;

                var comp = new Computer
                {
                    instructionMemory = input
                };

                comp.Write("a", a);
                done = comp.Run();
            }
            
            Console.WriteLine("Answer: {0}", a);

            Console.ReadKey();
        }
    }
}

