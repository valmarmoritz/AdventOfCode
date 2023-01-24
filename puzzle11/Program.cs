using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace puzzle11
{
    class Program
    {
        static void Main(string[] args)
        {
            bool learning = false;
            int part = 2;
            int rounds = 0;

            // part 1: 20
            // part 2= 10000
            if (part == 1) { rounds = 20; }
            else if (part == 2) { rounds = 10000; }

            List<Monkey> monkeys = new List<Monkey>();

            ReadInput(monkeys, learning);


            int lcm = 1;

            foreach (Monkey m in monkeys)
            {
                lcm *= m.TestFactor;
            }

            Console.Write("Doing Monkey Stuff." /*+ " Round:"*/);
            for (int i = 0; i < rounds; i++)
            {
                //Console.Write(" " + (i+1));
                DoMonkeyStuff(monkeys, part, lcm);
            }
            Console.WriteLine();
            Console.WriteLine();


            long highest1 = 0;
            long highest2 = 0;

            foreach (Monkey m in monkeys)
            {
                Console.WriteLine("Monkey " + monkeys.IndexOf(m) + " inspected items " + m.InspectedItemsCount + " times.");
                if (m.InspectedItemsCount >= highest1)
                {
                    highest2 = highest1;
                    highest1 = m.InspectedItemsCount;
                }
                else if (m.InspectedItemsCount >= highest2)
                {
                    highest2 = m.InspectedItemsCount;
                }
            }

            long MonkeyBusiness = highest1 * highest2;
            Console.WriteLine("Level of monkey business: " + MonkeyBusiness);
        }

        private static void DoMonkeyStuff(List<Monkey> _monkeys, int _part, int _lcm)
        {
            foreach (Monkey m in _monkeys)
            {
                while (m.Items.Count > 0)
                {
                    m.InspectedItemsCount += 1;

                    m.Operation(m.Items[0]);

                    if (_part == 1)
                    {
                        m.Items[0].AddItemHistoryElement(new ItemHistory()
                        {
                            operation = "/",
                            factor = 3,
                        });
                    }
                    else if (_part == 2)
                    {
                        m.Items[0].AddItemHistoryElement(new ItemHistory()
                        {
                            operation = "%",
                            factor = _lcm,
                        });
                    }

                    int _to_throw_to = m.Test(m.Items[0]) ? m.TrueThrowTo : m.FalseThrowTo;

                    _monkeys[_to_throw_to].Items.Add(m.Items[0]);

                    m.Items.RemoveAt(0);
                }
            }
        }
        private static void ReadInput(List<Monkey> _monkeys, bool learning)
        {
            string fileName = "";

            // Read a text file line by line.
            if (learning)
            {
                fileName = @"puzzle11_learning.txt";
            }
            else
            {
                fileName = @"puzzle11.txt";
            }
            string[] lines = System.IO.File.ReadAllLines(fileName);

            int ln = 0;
            int _count_of_monkeys = 0;
            string[] _operation_string;
            string _operation_function = "";
            int _operation_factor = 0;
            string _test_function = "";
            int _test_factor = 0;
            int _true_throw_to = 0;
            int _false_throw_to = 0;

            foreach (string line in lines)
            {
                ln += 1;

                switch (ln)
                {
                    case 1:
                        _count_of_monkeys += 1;
                        _monkeys.Add(new Monkey());
                        break;
                    case 2:
                        foreach (int b in line?.Split(":")?[1].Split(",")?.Select(int.Parse)?.ToList())
                        {
                            var x = _monkeys.Last().Items;

                            x.Add(new Item());
                            x.Last().AddItemHistoryElement(new ItemHistory()
                                {
                                    operation = "+",
                                    factor = b
                                }
                            );
                        }
                        break;
                    case 3:
                        _operation_string = line?.Split(" ");
                        _operation_function = _operation_string[_operation_string.Length - 2];
                        if (_operation_string.Last() == "old")
                        {
                            _operation_function = "**";
                            _operation_factor = 1;
                        }
                        else
                        {
                            int.TryParse(_operation_string.Last(), out _operation_factor);
                        }
                        _monkeys.Last().OperationFunction = _operation_function;
                        _monkeys.Last().OperationFactor = _operation_factor;
                        break;
                    case 4:
                        _test_function = "divisible by";
                        int.TryParse(line?.Split(" ").Last(), out _test_factor);
                        _monkeys.Last().TestFunction = _test_function;
                        _monkeys.Last().TestFactor = _test_factor;
                        break;
                    case 5:
                        int.TryParse(line?.Split(" ").Last(), out _true_throw_to);
                        _monkeys.Last().TrueThrowTo = _true_throw_to;
                        break;
                    case 6:
                        int.TryParse(line?.Split(" ").Last(), out _false_throw_to);
                        _monkeys.Last().FalseThrowTo = _false_throw_to;
                        break;
                    case 7:
                        ln = 0;
                        break;
                    default:
                        break;
                }
            }

            // what was read from input
            //Console.WriteLine(_count_of_monkeys + " monkeys added:");
            //Console.WriteLine();

            //foreach (var m in _monkeys)
            //{
            //    Console.WriteLine("Monkey " + _monkeys.IndexOf(m));
            //    Console.Write("starting items: ");
            //    foreach (var i in m.Items) Console.Write(i.WorryLevelCalc() + " ");
            //    Console.WriteLine();
            //    Console.WriteLine("operation: old " + m.OperationFunction + " " + m.OperationFactor);
            //    Console.WriteLine("test: " + m.TestFunction + " " + m.TestFactor);
            //    Console.WriteLine("if true, throw to " + m.TrueThrowTo);
            //    Console.WriteLine("if false, throw to " + m.FalseThrowTo);
            //    Console.WriteLine();
            //}
        }
    }
}
