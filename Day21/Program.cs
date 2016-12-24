using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day21
{
    abstract class Step
    {
        public abstract string Apply(string x);

        public abstract string Unscramble(string x);

        public static Step Parse(string x)
        {
            var parts = x.Split(' ');
            if (parts[0] == "swap" && parts[1] == "position")
            {
                return new SwapPositionStep
                {
                    x = int.Parse(parts[2]),
                    y = int.Parse(parts[5])
                };
            }
            else if (parts[0] == "swap" && parts[1] == "letter")
            {
                return new SwapLetterStep
                {
                    x = parts[2][0],
                    y = parts[5][0]
                };
            }
            else if (parts[0] == "rotate" && (parts[3] == "step" || parts[3] == "steps"))
            {
                var steps = int.Parse(parts[2]);
                if (parts[1] == "left")
                {
                    steps = -steps;
                }

                return new RotateConstantStep
                {
                    steps = steps
                };
            }
            else if (parts[0] == "rotate" && parts[1] == "based")
            {
                return new RotateBasedOnLetterStep
                {
                    letter = parts[6][0]
                };
            }
            else if (parts[0] == "reverse")
            {
                return new ReverseStep
                {
                    start = int.Parse(parts[2]),
                    end = int.Parse(parts[4])
                };
            }
            else if (parts[0] == "move")
            {
                return new MoveStep
                {
                    from = int.Parse(parts[2]),
                    to = int.Parse(parts[5])
                };
            }

            return null;
        }
    }

    class SwapPositionStep : Step
    {
        public int x;
        public int y;

        public override string Apply(string x)
        {
            var chars = x.ToCharArray();

            var tmp = chars[this.x];
            chars[this.x] = chars[this.y];
            chars[this.y] = tmp;

            return new string(chars);
        }

        public override string Unscramble(string x)
        {
            return Apply(x);
        }
    }

    class SwapLetterStep : Step
    {
        public char x;
        public char y;

        public override string Apply(string x)
        {
            var chars = x.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == this.x)
                {
                    chars[i] = this.y;
                }
                else if (chars[i] == this.y)
                {
                    chars[i] = this.x;
                }
            }

            return new string(chars);
        }

        public override string Unscramble(string x)
        {
            return Apply(x);
        }
    }

    abstract class RotateStep : Step
    {
        protected string Rotate(string x, int steps)
        {
            while (steps < 0)
            {
                steps += x.Length;
            }
            steps = steps % x.Length;

            return x.Substring(x.Length - steps) + x.Substring(0, x.Length - steps);
        }
    }

    class RotateConstantStep : RotateStep
    {
        public int steps;

        public override string Apply(string x)
        {
            return Rotate(x, steps);
        }

        public override string Unscramble(string x)
        {
            return Rotate(x, -steps);
        }
    }

    class RotateBasedOnLetterStep : RotateStep
    {
        public char letter;

        public override string Apply(string x)
        {
            int position = x.IndexOf(letter);

            int steps = position + 1;
            if (position >= 4)
            {
                steps++;
            }

            return Rotate(x, steps);
        }

        public override string Unscramble(string x)
        {
            for (int steps = 0; steps < x.Length; steps++)
            {
                var candidate = Rotate(x, steps);
                if (Apply(candidate) == x)
                {
                    return candidate;
                }
            }

            return null;
        }
    }

    class ReverseStep : Step
    {
        public int start;
        public int end;

        public override string Apply(string x)
        {
            var sub = x.Substring(start, end - start + 1);
            var reversed = new string(sub.Reverse().ToArray());

            return x.Substring(0, start) + reversed + x.Substring(end + 1);
        }

        public override string Unscramble(string x)
        {
            return Apply(x);
        }
    }

    class MoveStep : Step
    {
        public int from;
        public int to;

        public override string Apply(string x)
        {
            var chars = x.ToList();
            var c = chars[from];
            chars.RemoveAt(from);
            chars.Insert(to, c);

            return new string(chars.ToArray());
        }

        public override string Unscramble(string x)
        {
            var chars = x.ToList();
            var c = chars[to];
            chars.RemoveAt(to);
            chars.Insert(from, c);

            return new string(chars.ToArray());
        }
    }

    class Program
    {
        static void Main(string[] args)
        { 
            string[] input = {
                "rotate based on position of letter d",
                "move position 1 to position 6",
                "swap position 3 with position 6",
                "rotate based on position of letter c",
                "swap position 0 with position 1",
                "rotate right 5 steps",
                "rotate left 3 steps",
                "rotate based on position of letter b",
                "swap position 0 with position 2",
                "rotate based on position of letter g",
                "rotate left 0 steps",
                "reverse positions 0 through 3",
                "rotate based on position of letter a",
                "rotate based on position of letter h",
                "rotate based on position of letter a",
                "rotate based on position of letter g",
                "rotate left 5 steps",
                "move position 3 to position 7",
                "rotate right 5 steps",
                "rotate based on position of letter f",
                "rotate right 7 steps",
                "rotate based on position of letter a",
                "rotate right 6 steps",
                "rotate based on position of letter a",
                "swap letter c with letter f",
                "reverse positions 2 through 6",
                "rotate left 1 step",
                "reverse positions 3 through 5",
                "rotate based on position of letter f",
                "swap position 6 with position 5",
                "swap letter h with letter e",
                "move position 1 to position 3",
                "swap letter c with letter h",
                "reverse positions 4 through 7",
                "swap letter f with letter h",
                "rotate based on position of letter f",
                "rotate based on position of letter g",
                "reverse positions 3 through 4",
                "rotate left 7 steps",
                "swap letter h with letter a",
                "rotate based on position of letter e",
                "rotate based on position of letter f",
                "rotate based on position of letter g",
                "move position 5 to position 0",
                "rotate based on position of letter c",
                "reverse positions 3 through 6",
                "rotate right 4 steps",
                "move position 1 to position 2",
                "reverse positions 3 through 6",
                "swap letter g with letter a",
                "rotate based on position of letter d",
                "rotate based on position of letter a",
                "swap position 0 with position 7",
                "rotate left 7 steps",
                "rotate right 2 steps",
                "rotate right 6 steps",
                "rotate based on position of letter b",
                "rotate right 2 steps",
                "swap position 7 with position 4",
                "rotate left 4 steps",
                "rotate left 3 steps",
                "swap position 2 with position 7",
                "move position 5 to position 4",
                "rotate right 3 steps",
                "rotate based on position of letter g",
                "move position 1 to position 2",
                "swap position 7 with position 0",
                "move position 4 to position 6",
                "move position 3 to position 0",
                "rotate based on position of letter f",
                "swap letter g with letter d",
                "swap position 1 with position 5",
                "reverse positions 0 through 2",
                "swap position 7 with position 3",
                "rotate based on position of letter g",
                "swap letter c with letter a",
                "rotate based on position of letter g",
                "reverse positions 3 through 5",
                "move position 6 to position 3",
                "swap letter b with letter e",
                "reverse positions 5 through 6",
                "move position 6 to position 7",
                "swap letter a with letter e",
                "swap position 6 with position 2",
                "move position 4 to position 5",
                "rotate left 5 steps",
                "swap letter a with letter d",
                "swap letter e with letter g",
                "swap position 3 with position 7",
                "reverse positions 0 through 5",
                "swap position 5 with position 7",
                "swap position 1 with position 7",
                "swap position 1 with position 7",
                "rotate right 7 steps",
                "swap letter f with letter a",
                "reverse positions 0 through 7",
                "rotate based on position of letter d",
                "reverse positions 2 through 4",
                "swap position 7 with position 1",
                "swap letter a with letter h"
            };

            var steps = input.Select(i => Step.Parse(i)).ToList();

            var password = "abcdefgh";
            foreach (var step in steps)
            {
                password = step.Apply(password);
            }

            Console.WriteLine("Answer 1: {0}", password);


            var unscrambled = "fbgdceah";
            foreach (var step in steps.Reverse<Step>())
            {
                unscrambled = step.Unscramble(unscrambled);
            }

            Console.WriteLine("Answer 2: {0}", unscrambled);

            Console.ReadKey();
        }
    }
}
