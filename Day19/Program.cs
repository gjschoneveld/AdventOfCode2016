using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day19
{
    class Elf
    {
        public int id;

        public Elf previous;
        public Elf next;
    }

    class Program
    {
        static void BuildLinkedList(Elf[] elves)
        {
            foreach (var elf in elves)
            {
                elf.previous = elves[(elf.id + elves.Length - 2) % elves.Length];
                elf.next = elves[elf.id % elves.Length];
            }
        }

        static void Remove(Elf elf)
        {
            elf.previous.next = elf.next;
            elf.next.previous = elf.previous;
        }

        static Elf Get(Elf elf, int steps)
        {
            while (steps > 0)
            {
                elf = elf.next;
                steps--;
            }

            return elf;
        }

        static void Main(string[] args)
        {
            int numberOfElves = 3014387;

            var elves = Enumerable.Range(1, numberOfElves).Select(id => new Elf { id = id }).ToArray();
            BuildLinkedList(elves);

            var current = elves.First();
            while (current != current.next)
            {
                // remove next
                Remove(current.next);

                // jump to new next
                current = current.next;
            }

            Console.WriteLine("Answer 1: {0}", current.id);


            BuildLinkedList(elves);
            current = elves.First();
            var count = elves.Length;
            while (current != current.next)
            {
                if (count % 10000 == 0)
                {
                    Console.WriteLine(count);
                }

                // remove at half way
                var elf = Get(current, count / 2);
                Remove(elf);
                count--;

                // jump to next
                current = current.next;
            }

            Console.WriteLine("Answer 2: {0}", current.id);

            Console.ReadKey();
        }
    }
}
