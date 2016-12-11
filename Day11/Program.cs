using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day11
{
    class State
    {
        public const int lowestFloor = 1;
        public const int highestFloor = 4;

        public State previous;

        public int elevatorFloor = lowestFloor;
        public int[] generatorFloors;
        public int[] microcontrollerFloors;

        public override int GetHashCode()
        {
            int hash = elevatorFloor;

            for (int i = 0; i < generatorFloors.Length; i++)
            {
                hash ^= generatorFloors[i];
                hash ^= microcontrollerFloors[i];
            }

            return hash;
        }

        public override bool Equals(object obj)
        {
            State other = obj as State;

            if (other == null)
            {
                return false;
            }

            if (elevatorFloor != other.elevatorFloor)
            {
                return false;
            }

            var pairs = generatorFloors.Zip(microcontrollerFloors, (g, m) => new Tuple<int, int>(g, m));
            var sortedPairs = pairs.OrderBy(p => p.Item1).ThenBy(p => p.Item2);

            var otherPairs = other.generatorFloors.Zip(other.microcontrollerFloors, (g, m) => new Tuple<int, int>(g, m));
            var otherSortedPairs = otherPairs.OrderBy(p => p.Item1).ThenBy(p => p.Item2);

            return sortedPairs.SequenceEqual(otherSortedPairs);
        }

        private State Clone()
        {
            var result = new State
            {
                previous = this,

                elevatorFloor = elevatorFloor,
                generatorFloors = generatorFloors.Clone() as int[],
                microcontrollerFloors = microcontrollerFloors.Clone() as int[]
            };

            return result;
        }

        public bool IsFinished()
        {
            bool elevatorAtTop = elevatorFloor == highestFloor;
            bool generatorsAtTop = generatorFloors.All(f => f == highestFloor);
            bool microcontrollersAtTop = microcontrollerFloors.All(f => f == highestFloor);

            return elevatorAtTop && generatorsAtTop && microcontrollersAtTop;
        }

        public bool IsValid()
        {
            // invalid if we have at least one single microcontroller with a generator (paired or not)

            for (int i = 0; i < microcontrollerFloors.Length; i++)
            {
                bool paired = generatorFloors[i] == microcontrollerFloors[i];
                bool hasGenerator = generatorFloors.Any(f => microcontrollerFloors[i] == f);

                if (!paired && hasGenerator)
                {
                    return false;
                }
            }

            return true;
        }

        private int[] TakeOne(int[] floors, int f)
        {
            var result = new List<int>();

            for (int i = 0; i < floors.Length; i++)
            {
                if (floors[i] == f)
                {
                    result.Add(i);
                }
            }

            return result.ToArray();
        }

        private int[][] TakeTwo(int[] floors, int f)
        {
            var result = new List<int[]>();

            var items = TakeOne(floors, f);
            for (int i = 0; i < items.Length; i++)
            {
                for (int j = i + 1; j < items.Length; j++)
                {
                    result.Add(new int[] { items[i], items[j] });
                }
            }

            return result.ToArray();
        }

        private bool IsValidFloor(int f)
        {
            return lowestFloor <= f && f <= highestFloor;
        }

        public State[] Next()
        {
            List<State> result = new List<State>();

            // move elevator up or down
            var nextFloors = new int[] { elevatorFloor - 1, elevatorFloor + 1 }.Where(f => IsValidFloor(f));
            foreach (var next in nextFloors)
            {
                // take one generator
                var singleGenerators = TakeOne(generatorFloors, elevatorFloor);
                foreach (var item in singleGenerators)
                {
                    var x = this.Clone();
                    x.elevatorFloor = next;
                    x.generatorFloors[item] = next;
                    result.Add(x);
                }

                // take one microcontroller
                var singleMicrocontrollers = TakeOne(microcontrollerFloors, elevatorFloor);
                foreach (var item in singleMicrocontrollers)
                {
                    var x = this.Clone();
                    x.elevatorFloor = next;
                    x.microcontrollerFloors[item] = next;
                    result.Add(x);
                }

                // take two generators
                var doubleGenerators = TakeTwo(generatorFloors, elevatorFloor);
                foreach (var items in doubleGenerators)
                {
                    var x = this.Clone();
                    x.elevatorFloor = next;
                    x.generatorFloors[items[0]] = next;
                    x.generatorFloors[items[1]] = next;
                    result.Add(x);
                }

                // take two microcontrollers
                var doubleMicrocontrollers = TakeTwo(microcontrollerFloors, elevatorFloor);
                foreach (var items in doubleMicrocontrollers)
                {
                    var x = this.Clone();
                    x.elevatorFloor = next;
                    x.microcontrollerFloors[items[0]] = next;
                    x.microcontrollerFloors[items[1]] = next;
                    result.Add(x);
                }

                //// take one of each (different types in elevator allowed)
                //foreach (var gitem in singleGenerators)
                //{
                //    foreach (var mitem in singleMicrocontrollers)
                //    {
                //        var x = this.Clone();
                //        x.elevatorFloor = next;
                //        x.generatorFloors[gitem] = next;
                //        x.microcontrollerFloors[mitem] = next;
                //        result.Add(x);
                //    }
                //}

                // take one of each (different types in elevator not allowed)
                foreach (var item in singleGenerators.Intersect(singleMicrocontrollers))
                {
                    var x = this.Clone();
                    x.elevatorFloor = next;
                    x.generatorFloors[item] = next;
                    x.microcontrollerFloors[item] = next;
                    result.Add(x);
                }
            }

            return result.ToArray();
        }

        public void Print()
        {
            for (int f = State.highestFloor; f >= State.lowestFloor; f--)
            {
                Console.Write("F{0} ", f);

                if (elevatorFloor == f)
                {
                    Console.Write("E  ");
                }
                else
                {
                    Console.Write(".  ");
                }

                for (int i = 0; i < generatorFloors.Length; i++)
                {
                    if (generatorFloors[i] == f)
                    {
                        Console.Write("{0}G ", (char)(i + 'A'));
                    }
                    else
                    {
                        Console.Write(".  ");
                    }
                    if (microcontrollerFloors[i] == f)
                    {
                        Console.Write("{0}M ", (char)(i + 'A'));
                    }
                    else
                    {
                        Console.Write(".  ");
                    }
                }

                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    class Program
    {
        static List<State> FindPath(State start)
        {
            var seen = new HashSet<State> { start };
            var current = new List<State> { start };

            int steps = 0;

            while (!current.Any(s => s.IsFinished()))
            {
                Console.Write(".");

                var rawNext = current.SelectMany(s => s.Next()).Distinct().ToList();
                var validNext = rawNext.Where(s => s.IsValid()).ToList();

                var newNext = validNext.Where(s => !seen.Contains(s)).ToList();

                foreach (var n in newNext)
                {
                    seen.Add(n);
                }

                current = newNext;
                steps++;
            }
            Console.WriteLine();

            var state = current.First(s => s.IsFinished());
            var path = new List<State> { state };
            while (state.previous != null)
            {
                state = state.previous;
                path.Add(state);
            }
            path.Reverse();

            return path;
        }

        static void PrintPath(List<State> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                Console.WriteLine("{0}:", i);
                path[i].Print();
            }
        }

        static void Main(string[] args)
        {
            //// A = hydrogen, B = lithium
            //State startDemo = new State
            //{
            //    generatorFloors = new int[] { 2, 3 },
            //    microcontrollerFloors = new int[] { 1, 1 },
            //};

            //var pathDemo = FindPath(startDemo);

            //Console.WriteLine("A = hydrogen, B = lithium");
            //PrintPath(pathDemo);


            // A = thulium, B = plutonium, C = strontium, D = promethium, E = ruthenium
            State start1 = new State
            {
                generatorFloors = new int[] { 1, 1, 1, 3, 3 },
                microcontrollerFloors = new int[] { 1, 2, 2, 3, 3 },
            };

            var path1 = FindPath(start1);

            //Console.WriteLine("A = thulium, B = plutonium, C = strontium, D = promethium, E = ruthenium");
            //PrintPath(path1);

            Console.WriteLine("Answer 1: {0}", path1.Count - 1);


            // A = thulium, B = plutonium, C = strontium, D = promethium, E = ruthenium, F = elerium, G = dilithium
            State start2 = new State
            {
                generatorFloors = new int[] { 1, 1, 1, 3, 3, 1, 1 },
                microcontrollerFloors = new int[] { 1, 2, 2, 3, 3, 1, 1 },
            };

            var path2 = FindPath(start2);

            //Console.WriteLine("A = thulium, B = plutonium, C = strontium, D = promethium, E = ruthenium, F = elerium, G = dilithium");
            //PrintPath(path2);

            Console.WriteLine("Answer 2: {0}", path2.Count - 1);

            Console.ReadKey();
        }
    }
}
