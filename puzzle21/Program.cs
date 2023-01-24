// See https://aka.ms/new-console-template for more information

using System.Data.SqlTypes;

bool learning = false;
long test_range_end = 10000000;
long test_range_start = test_range_end / 10;

LinkedList<Monkey> Monkeys = new LinkedList<Monkey>();

ReadInput(Monkeys, learning);

// perhaps we should sort the monkeys so that calculations are only done when inputs are there already
Monkeys = SortMonkeys(Monkeys, learning);

LinkedList<Monkey> SortMonkeys(LinkedList<Monkey> monkeys, bool learning)
{
    LinkedList<Monkey> sortedMonkeys = new LinkedList<Monkey>();

    // first, add all monkeys who just shout a number
    foreach (Monkey m in monkeys)
    {
        if (m.Operation == '=') sortedMonkeys.AddLast(m); continue;
    }

    // now go through the remaining monkeys and add those who already have all necessary inputs, until all monkeys are added
    while (sortedMonkeys.Count < monkeys.Count)
    {
        foreach (Monkey m in monkeys)
        {
            if (m.Operation != '=' && !sortedMonkeys.Any(x => x.Name == m.Name ))
            {
                bool is_first_known = sortedMonkeys.Any(x => x.Name == m.FirstInput );
                bool is_second_known = sortedMonkeys.Any(x => x.Name == m.SecondInput );
                if (is_first_known && is_second_known) sortedMonkeys.AddLast(m); continue;
            }
        }
    }

    return sortedMonkeys;
}

Dictionary<string, long> YelledNumbers = new Dictionary<string, long>();


while (!YelledNumbers.ContainsKey("root"))
{
    foreach (Monkey monkey in Monkeys)
    {
        monkey.Yell(YelledNumbers);
    }

    Console.WriteLine("Last yell: " + YelledNumbers.Last().Key + ": " + YelledNumbers.Last().Value);
}

if (YelledNumbers.ContainsKey("root"))
{
    Console.WriteLine();
    Console.WriteLine("Root has yelled: " + YelledNumbers["root"]);
    Console.WriteLine();
}

Console.Write("Press <ENTER> to continue...");
Console.ReadLine();


// part two: finding humn input that produces equal inputs for root
//bool found = false;
Console.WriteLine("Testing humn: input range from " + test_range_start.ToString("N0") + " to " + test_range_end.ToString("N0"));

Parallel.For(test_range_start, test_range_end, humn => TestHumnInputParallel(Monkeys, humn));

//for (long humn = test_range_start; humn <= test_range_end; humn++)
//{
//    if (humn % test_range_start == 0)
//    {
//        Console.Write(humn / test_range_start);
//    }
//    else 
//    { 
//        if (humn % (test_range_start / 10) == 0) Console.Write('.');
//    }

//    found = TestHumnInput(Monkeys, humn);

//    if (found)
//    {
//        Console.WriteLine();
//        Console.WriteLine("Found humn: input that results in root receiving equal inputs from both monkeys she is waiting on: " + humn);
//        break;
//    }
//}

//if (!found)
//{
//    Console.WriteLine();
//    Console.WriteLine("Did not find a solution with test range from " + test_range_start.ToString("N0") + " to " + test_range_end.ToString("N0"));
//}


//bool TestHumnInput(LinkedList<Monkey> monkeys, long humn)
//{
//    YelledNumbers = new Dictionary<string, long>();

//    Monkeys.FirstOrDefault(x => x.Name == "humn").Number = humn;

//    while (!YelledNumbers.ContainsKey("root"))
//    {
//        foreach (Monkey monkey in Monkeys)
//        {
//            monkey.Yell(YelledNumbers);
//        }
//    }

//    string checkFirst = Monkeys.FirstOrDefault(x => x.Name == "root").FirstInput;
//    string checkSecond = Monkeys.FirstOrDefault(x => x.Name == "root").SecondInput;

//    if (YelledNumbers[checkFirst] == YelledNumbers[checkSecond])
//    {
//        Console.WriteLine();
//        Console.WriteLine(checkFirst + " yelled: " + YelledNumbers[checkFirst]);
//        Console.WriteLine(checkSecond + " yelled: " + YelledNumbers[checkSecond]);
//        return true;
//    }

//    return false;
//}




void TestHumnInputParallel(LinkedList<Monkey> monkeys/*_orig*/, long humn)
{
    //LinkedList<Monkey> monkeys = new LinkedList<Monkey>();
    //foreach (Monkey monkey in monkeys_orig)
    //{
    //    monkeys.AddLast(new Monkey
    //    {
    //        Name = monkey.Name,
    //        Task= monkey.Task,
    //        Operation= monkey.Operation,
    //        Number= monkey.Number,
    //        FirstInput= monkey.FirstInput,
    //        SecondInput= monkey.SecondInput,
    //    });
    //}

    if (humn % (test_range_start / 10) == 0)
    {
        Console.Write(humn / test_range_start);
    }
    else
    {
        if (humn % (test_range_start / 100) == 0) Console.Write('.');
    }

    Dictionary<string, long> yelledNumbers = new Dictionary<string, long>();

    monkeys.FirstOrDefault(x => x.Name == "humn").Number = humn;

    while (!yelledNumbers.ContainsKey("root"))
    {
        foreach (Monkey monkey in monkeys)
        {
            monkey.Yell(yelledNumbers);
        }
    }

    string checkFirst = monkeys.FirstOrDefault(x => x.Name == "root").FirstInput;
    string checkSecond = monkeys.FirstOrDefault(x => x.Name == "root").SecondInput;

    if (yelledNumbers[checkFirst] == yelledNumbers[checkSecond])
    {
        Console.WriteLine();
        Console.WriteLine(checkFirst + " yelled: " + yelledNumbers[checkFirst]);
        Console.WriteLine(checkSecond + " yelled: " + yelledNumbers[checkSecond]);
        Console.WriteLine();
        Console.WriteLine("Found humn: input that results in root receiving equal inputs from both monkeys she is waiting on: " + humn);
        return;
    }

    return;
}



static void ReadInput(LinkedList<Monkey> monkeys, bool learning)
{
    string[] lines;
    // Read a text file line by line.
    if (learning) lines = System.IO.File.ReadAllLines(@"puzzle21_learning.txt");
    else lines = System.IO.File.ReadAllLines(@"puzzle21.txt");

    List<string> parts;
    List<string> words;

    foreach (string line in lines)
    {
        parts = line
            .Split(':')
            .ToList();

        monkeys.AddLast(new Monkey()
        {
            Name = parts[0],
            Task = parts[1].Trim(),
        });

        if (long.TryParse(parts[1], out long n))
        {
            monkeys.Last().Operation = '=';
            monkeys.Last().Number = n;
        }
        else
        {
            words = parts[1].Trim()
                .Split(' ')
                .ToList();

            if (words.Count == 3) 
            {
                monkeys.Last().Operation = char.Parse(words[1]);
                monkeys.Last().FirstInput = words[0];
                monkeys.Last().SecondInput = words[2];
            }
        }
    }
}