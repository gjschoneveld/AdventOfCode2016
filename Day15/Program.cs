using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day15
{
    class Disc
    {
        public int positions;
        public int start;

        public int PositionAtTime(int time)
        {
            return (start + time) % positions;
        }
    }

    class Program
    {
        static bool FallsThrough(List<Disc> discs, int time)
        {
            for (int i = 0; i < discs.Count; i++)
            {
                if (discs[i].PositionAtTime(time + i + 1) != 0)
                {
                    return false;
                }
            }

            return true;
        }

        static int FindBestTime(List<Disc> discs)
        {
            int time = 0;
            while (!FallsThrough(discs, time))
            {
                time++;
            }

            return time;
        }

        static void Main(string[] args)
        {
            var discs = new List<Disc> {
                new Disc { positions = 17, start = 1 },
                new Disc { positions = 7, start = 0 },
                new Disc { positions = 19, start = 2 },
                new Disc { positions = 5, start = 0 },
                new Disc { positions = 3, start = 0 },
                new Disc { positions = 13, start = 5 }
            };

            var time1 = FindBestTime(discs);

            Console.WriteLine("Answer 1: {0}", time1);


            discs.Add(new Disc { positions = 11, start = 0 });

            var time2 = FindBestTime(discs);

            Console.WriteLine("Answer 2: {0}", time2); 
            
            Console.ReadKey();
        }
    }
}
