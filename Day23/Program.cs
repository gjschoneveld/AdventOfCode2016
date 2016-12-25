using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day23
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

        public void Run()
        {
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
                    case "tgl":
                        var location = programCounter + Read(parts[1]);
                        if (location < 0 || instructionMemory.Length <= location)
                        {
                            break;
                        }

                        var target = instructionMemory[location];
                        var targetParts = target.Split(' ');

                        if (targetParts.Length == 2)
                        {
                            if (targetParts[0] == "inc")
                            {
                                targetParts[0] = "dec";
                            }
                            else
                            {
                                targetParts[0] = "inc";
                            }
                        }
                        else if (targetParts.Length == 3)
                        {
                            if (targetParts[0] == "jnz")
                            {
                                targetParts[0] = "cpy";
                            }
                            else
                            {
                                targetParts[0] = "jnz";
                            }
                        }

                        instructionMemory[location] = string.Join(" ", targetParts);
                        break;
                }

                programCounter += offset;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*
            string[] input = {
                "cpy 2 a",
                "tgl a",
                "tgl a",
                "tgl a",
                "cpy 1 a",
                "dec a",
                "dec a"
            };
             */

            string[] input = {
                "cpy a b",
                "dec b",
                "cpy a d",
                "cpy 0 a",
                "cpy b c",
                "inc a",
                "dec c",
                "jnz c -2",
                "dec d",
                "jnz d -5",
                "dec b",
                "cpy b c",
                "cpy c d",
                "dec d",
                "inc c",
                "jnz d -2",
                "tgl c",
                "cpy -16 c",
                "jnz 1 c",
                "cpy 96 c",
                "jnz 91 d",
                "inc a",
                "inc d",
                "jnz d -2",
                "inc c",
                "jnz c -5"
            };

            var comp1 = new Computer
            {
                instructionMemory = input.Clone() as string[]
            };

            comp1.Write("a", 7);

            comp1.Run();

            var a1 = comp1.Read("a");

            Console.WriteLine("Answer 1: {0}", a1);


            var comp2 = new Computer
            {
                instructionMemory = input.Clone() as string[]
            };

            comp2.Write("a", 12);

            comp2.Run();

            var a2 = comp2.Read("a");

            Console.WriteLine("Answer 2: {0}", a2);

            Console.ReadKey();
        }
    }
}
