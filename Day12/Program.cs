using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day12
{
    class Value
    {
        public int value;
    }

    abstract class Instruction
    {
        public abstract int Execute();

        private static Value ParseArgument(string x, Dictionary<string, Value> regfile)
        {
            if (char.IsLetter(x, 0))
            {
                return regfile[x];
            }

            return new Value
            {
                value = int.Parse(x)
            };
        }

        public static Instruction Parse(string x, Dictionary<string, Value> regfile)
        {
            var parts = x.Split(' ');
            var opcode = parts[0];

            switch (opcode)
            {
                case "cpy":
                    return new Copy
                    {
                        source = ParseArgument(parts[1], regfile),
                        destination = ParseArgument(parts[2], regfile)
                    };
                case "inc":
                    return new Increment
                    {
                        argument = ParseArgument(parts[1], regfile)
                    };
                case "dec":
                    return new Decrement
                    {
                        argument = ParseArgument(parts[1], regfile)
                    };
                case "jnz":
                    return new JumpNotZero
                    {
                        test = ParseArgument(parts[1], regfile),
                        offset = ParseArgument(parts[2], regfile)
                    };
            }

            return null;
        }
    }

    class Copy : Instruction
    {
        public Value source;
        public Value destination;

        public override int Execute()
        {
            destination.value = source.value;
            return 1;
        }
    }

    class Increment : Instruction
    {
        public Value argument;

        public override int Execute()
        {
            argument.value++;
            return 1;
        }
    }

    class Decrement : Instruction
    {
        public Value argument;

        public override int Execute()
        {
            argument.value--;
            return 1;
        }
    }

    class JumpNotZero : Instruction
    {
        public Value test;
        public Value offset;

        public override int Execute()
        {
            if (test.value != 0)
            {
                return offset.value;
            }

            return 1;
        }
    }

    class Program
    {
        static void Run(Instruction[] program)
        {
            int programCounter = 0;
            while (programCounter < program.Length)
            {
                var offset = program[programCounter].Execute();
                programCounter += offset;
            }
        }

        static void Main(string[] args)
        {
            string[] input = {
                "cpy 1 a",
                "cpy 1 b",
                "cpy 26 d",
                "jnz c 2",
                "jnz 1 5",
                "cpy 7 c",
                "inc d",
                "dec c",
                "jnz c -2",
                "cpy a c",
                "inc a",
                "dec b",
                "jnz b -2",
                "cpy c b",
                "dec d",
                "jnz d -6",
                "cpy 16 c",
                "cpy 12 d",
                "inc a",
                "dec d",
                "jnz d -2",
                "dec c",
                "jnz c -5"     
            };

            var registerFile = new Dictionary<string, Value>
            {
                { "a", new Value() }, 
                { "b", new Value() }, 
                { "c", new Value() }, 
                { "d", new Value() } 
            };

            Instruction[] program = input.Select(x => Instruction.Parse(x, registerFile)).ToArray();


            Run(program);

            int a1 = registerFile["a"].value;

            Console.WriteLine("Answer 1: {0}", a1);


            registerFile["a"].value = 0;
            registerFile["b"].value = 0;
            registerFile["c"].value = 1;
            registerFile["d"].value = 0;

            Run(program);

            int a2 = registerFile["a"].value;

            Console.WriteLine("Answer 2: {0}", a2);

            Console.ReadKey();
        }
    }
}
